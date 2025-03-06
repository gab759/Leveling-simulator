using UnityEngine;
using DG.Tweening;

public class ValveHandle : MonoBehaviour
{
    public enum ValveType { Upper, Lower }
    public ValveType valveType;
    public WaterFillController waterController;

    private bool isRotating = false;
    private bool isClockwise = false;
    private bool isOpen = false; // Controla si la válvula está abierta

    public float rotationDuration = 3.5f; // Tiempo de rotación

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectTouchOrClick();
        }
    }

    private void DetectTouchOrClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform && !isRotating)
            {
                RotateHandle();
            }
        }
    }

    private void RotateHandle()
    {
        isRotating = true;
        float rotationAmount = isClockwise ? -360 : 360;
        float newRotationX = transform.eulerAngles.x + rotationAmount;

        Debug.Log($"{valveType} comenzando a girar {(isClockwise ? "horario" : "antihorario")}");

        transform.DORotate(new Vector3(newRotationX, transform.eulerAngles.y, transform.eulerAngles.z),
                           rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (valveType == ValveType.Upper)
                {
                    waterController.SetFillSpeed(isClockwise ? 1 : 0);
                    Debug.Log($"{valveType} está llenando el tanque.");
                }
                else if (valveType == ValveType.Lower)
                {
                    waterController.SetDrainSpeed(isClockwise ? 1 : 0);
                    Debug.Log($"{valveType} está vaciando el tanque.");
                }
            })
            .OnComplete(() =>
            {
                isRotating = false;
                isClockwise = !isClockwise;
                isOpen = !isOpen; // Alterna el estado de la válvula

                Debug.Log($"{valveType} {(isOpen ? "abierta" : "cerrada")}");

                // Llamar a CheckValves() inmediatamente después de cambiar el estado
                waterController.CheckValves();
            });
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}