using UnityEngine;
using DG.Tweening; // Importa DOTween

public class ValveHandle : MonoBehaviour
{
    private bool isRotating = false;

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

        // Mantiene Y y Z, solo suma -360° en X
        float newRotationX = transform.eulerAngles.x + 360;

        transform.DORotate(new Vector3(newRotationX, transform.eulerAngles.y, transform.eulerAngles.z),
                           3.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear) // Usa Linear para evitar overshoot
            .OnComplete(() => isRotating = false);
    }
}