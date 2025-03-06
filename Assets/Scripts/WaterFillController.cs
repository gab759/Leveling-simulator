using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaterFillController : MonoBehaviour
{
    public Material waterMaterial;
    public float maxFillSpeed = 0.2f; // Velocidad reducida de llenado/vaciado
    private float fillAmount = -0.9f;
    public TextMeshProUGUI percentageText;

    private float currentFillSpeed = 0f;
    private float currentDrainSpeed = 0f;

    private bool upperValveOpen = false;
    private bool lowerValveOpen = false;

    private void Awake()
    {
        waterMaterial.SetFloat("_Fill", fillAmount);
        UpdatePercentageText();
    }

    void Update()
    {
        // Comprobamos si ambas válvulas están abiertas
        if (upperValveOpen && lowerValveOpen)
        {
            Debug.Log("Ambas válvulas abiertas -> El agua NO cambia.");
            return;
        }

        // Si la válvula superior está abierta, llenar
        if (upperValveOpen && !lowerValveOpen)
        {
            SetFillSpeed(1);
        }
        else
        {
            SetFillSpeed(0);
        }

        // Si la válvula inferior está abierta, vaciar
        if (lowerValveOpen && !upperValveOpen)
        {
            SetDrainSpeed(1);
        }
        else
        {
            SetDrainSpeed(0);
        }

        // Actualizar el nivel de agua gradualmente
        float previousFill = fillAmount;
        fillAmount = Mathf.Clamp(fillAmount + (currentFillSpeed - currentDrainSpeed) * Time.deltaTime, -0.9f, 0.9f);
        waterMaterial.SetFloat("_Fill", fillAmount);
        UpdatePercentageText();

        if (previousFill != fillAmount)
        {
            Debug.Log($"Nivel del agua cambiado a {fillAmount:F2}");
        }
    }

    public void SetFillSpeed(float speed)
    {
        currentFillSpeed = maxFillSpeed * speed;
        Debug.Log($"Velocidad de llenado ajustada a {currentFillSpeed}");
    }

    public void SetDrainSpeed(float speed)
    {
        currentDrainSpeed = maxFillSpeed * speed;
        Debug.Log($"Velocidad de vaciado ajustada a {currentDrainSpeed}");
    }

    public void CheckValves()
    {
        ValveHandle[] valves = FindObjectsOfType<ValveHandle>();
        upperValveOpen = false;
        lowerValveOpen = false;

        foreach (var valve in valves)
        {
            if (valve.valveType == ValveHandle.ValveType.Upper)
                upperValveOpen = valve.IsOpen();
            else if (valve.valveType == ValveHandle.ValveType.Lower)
                lowerValveOpen = valve.IsOpen();
        }

        Debug.Log($"Estado de válvulas -> Superior: {(upperValveOpen ? "Abierta" : "Cerrada")}, Inferior: {(lowerValveOpen ? "Abierta" : "Cerrada")}");
    }

    private void UpdatePercentageText()
    {
        int percentage = Mathf.RoundToInt(((fillAmount + 0.9f) / 1.8f) * 100);
        percentageText.text = percentage + "%";
    }
}