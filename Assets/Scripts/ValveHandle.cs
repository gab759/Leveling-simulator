using UnityEngine;
using DG.Tweening;

public class ValveHandle : MonoBehaviour
{
    public enum ValveType { Upper1, Lower1, Upper2, Lower2, Main }
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
            .OnComplete(() =>
            {
                isRotating = false;
                isClockwise = !isClockwise;
                isOpen = !isOpen; // Alterna el estado de la válvula

                Debug.Log($"{valveType} {(isOpen ? "abierta" : "cerrada")}");

                // Actualizar el estado en WaterFillController
                if (valveType == ValveType.Main)
                {
                    waterController.SetMainValve(isOpen);
                }
                else
                {
                    int tankIndex = (valveType == ValveType.Upper1 || valveType == ValveType.Lower1) ? 1 : 2;
                    bool isFilling = (valveType == ValveType.Upper1 || valveType == ValveType.Upper2);
                    waterController.UpdateTankState(tankIndex, isFilling, isOpen);
                }
            });
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
