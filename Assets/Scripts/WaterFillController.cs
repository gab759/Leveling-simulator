using UnityEngine;
using TMPro;

public class WaterFillController : MonoBehaviour
{
    public Material waterMaterial1;
    public Material waterMaterial2;

    private float fillAmount1 = -0.9f;
    private float fillAmount2 = -0.9f;
    public float fillSpeed = 0.2f;

    private bool upperValve1Open = false;
    private bool lowerValve1Open = false;
    private bool upperValve2Open = false;
    private bool lowerValve2Open = false;
    private bool mainValveOpen = false;

    public TextMeshProUGUI percentageText1;
    public TextMeshProUGUI percentageText2;

    private void Awake()
    {
        UpdateWaterLevels();
    }

    public void SetMainValve(bool open)
    {
        mainValveOpen = open;
        Debug.Log("Válvula principal " + (open ? "Abierta" : "Cerrada"));
    }

    public void UpdateTankState(int tankIndex, bool isFilling, bool isOpen)
    {
        if (tankIndex == 1)
        {
            if (isFilling)
                upperValve1Open = isOpen;
            else
                lowerValve1Open = isOpen;
        }
        else if (tankIndex == 2)
        {
            if (isFilling)
                upperValve2Open = isOpen;
            else
                lowerValve2Open = isOpen;
        }

        DebugValveStatus();
    }

    private void Update()
    {
        if (!mainValveOpen)
        {
            Debug.Log("Válvula principal cerrada, el agua no cambia.");
            return;
        }

        if (upperValve1Open && !lowerValve1Open)
        {
            fillAmount1 = Mathf.Clamp(fillAmount1 + fillSpeed * Time.deltaTime, -0.9f, 0.9f);
        }
        else if (lowerValve1Open && !upperValve1Open)
        {
            fillAmount1 = Mathf.Clamp(fillAmount1 - fillSpeed * Time.deltaTime, -0.9f, 0.9f);
        }

        if (upperValve2Open && !lowerValve2Open)
        {
            fillAmount2 = Mathf.Clamp(fillAmount2 + fillSpeed * Time.deltaTime, -0.9f, 0.9f);
        }
        else if (lowerValve2Open && !upperValve2Open)
        {
            fillAmount2 = Mathf.Clamp(fillAmount2 - fillSpeed * Time.deltaTime, -0.9f, 0.9f);
        }

        UpdateWaterLevels();
    }

    private void UpdateWaterLevels()
    {
        waterMaterial1.SetFloat("_Fill", fillAmount1);
        waterMaterial2.SetFloat("_Fill", fillAmount2);
        percentageText1.text = Mathf.RoundToInt(((fillAmount1 + 0.9f) / 1.8f) * 100) + "%";
        percentageText2.text = Mathf.RoundToInt(((fillAmount2 + 0.9f) / 1.8f) * 100) + "%";
    }

    private void DebugValveStatus()
    {
        Debug.Log($"Estado de válvulas -> Principal: {(mainValveOpen ? "Abierta" : "Cerrada")}, " +
                  $"Superior 1: {(upperValve1Open ? "Abierta" : "Cerrada")}, Inferior 1: {(lowerValve1Open ? "Abierta" : "Cerrada")}, " +
                  $"Superior 2: {(upperValve2Open ? "Abierta" : "Cerrada")}, Inferior 2: {(lowerValve2Open ? "Abierta" : "Cerrada")}");
    }
}
