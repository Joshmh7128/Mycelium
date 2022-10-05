using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PA_FungusManager : MonoBehaviour
{

    #region Singleton
    public static PA_FungusManager instance;
    private void Awake() {
        if (instance) {
            Debug.Log("Warning! More than one instance of PA_FungusManager found!");
            return;
        } else instance = this;
    }
    #endregion

    //Instance refs
    PA_NodeManager nodeManager;

    //Scene UI
    [SerializeField] Slider storageSlider, productionSlider, consumptionSlider;
    [SerializeField] Text nutrientTotalText, storageText, productionText, consumptionText;

    //Tracked resource distribution
    public float nutrientTotal;
    public float nutrientStorage;
    public float nutrientProduction;
    public float nutrientConsumption;

    void Start() {
        if (PA_NodeManager.instance) nodeManager = PA_NodeManager.instance;
    }

    private void Update() {
        ApplyResourceDistribution();
        UpdateSliderDisplay();
    }

    void ApplyResourceDistribution() {

        nutrientTotal += (nutrientProduction - nutrientConsumption) * Time.deltaTime;
        if (nutrientTotal > nutrientStorage) nutrientTotal = nutrientStorage;
    }

    void UpdateSliderDisplay() {
        storageSlider.maxValue = nutrientStorage;
        storageText.text = "/" + ((int)nutrientStorage).ToString();
        storageSlider.value = nutrientTotal;
        nutrientTotalText.text = ((int)nutrientTotal).ToString();

        productionSlider.maxValue = nutrientProduction + nutrientConsumption;
        productionSlider.value = nutrientProduction;
        productionText.text = "+" + ((int)(nutrientProduction * 10));

        consumptionSlider.maxValue = nutrientProduction + nutrientConsumption;
        consumptionSlider.value = nutrientConsumption;
        consumptionText.text = "-" + ((int)(nutrientConsumption * 10));
    }
}

