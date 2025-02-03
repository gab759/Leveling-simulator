using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class WaterFillController : MonoBehaviour
{
    public Material waterMaterial;
    public float fillSpeed = 0.5f;
    private float fillAmount = -0.9f;
    private bool increasing = false;
    private bool decreasing = false;
    public TextMeshProUGUI percentageText; // TextMeshPro en lugar de Text

    private void Awake()
    {
        waterMaterial.SetFloat("_Fill", fillAmount);
        UpdatePercentageText(); // Inicializa el porcentaje al inicio
    }

    void Update()
    {
        if (increasing)
        {
            fillAmount = Mathf.Clamp(fillAmount + fillSpeed * Time.deltaTime, -0.9f, 0.9f);
            waterMaterial.SetFloat("_Fill", fillAmount);
            UpdatePercentageText();
        }
        else if (decreasing)
        {
            fillAmount = Mathf.Clamp(fillAmount - fillSpeed * Time.deltaTime, -0.9f, 0.9f);
            waterMaterial.SetFloat("_Fill", fillAmount);
            UpdatePercentageText();
        }
    }

    public void StartIncreasing() { increasing = true; }
    public void StopIncreasing() { increasing = false; }
    public void StartDecreasing() { decreasing = true; }
    public void StopDecreasing() { decreasing = false; }

    private void UpdatePercentageText()
    {
        int percentage = Mathf.RoundToInt(((fillAmount + 0.9f) / 1.8f) * 100);
        percentageText.text = percentage + "%";
    }
}