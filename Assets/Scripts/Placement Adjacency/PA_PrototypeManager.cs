using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PA_PrototypeManager : MonoBehaviour
{
    PA_NodeManager nodeManager;
    PA_FungusManager fungusManager;

    [SerializeField] Sprite unlockSprite;

    public Button[] purchaseButtons;
    Image[] padlocks, icons;
    Text[] nameText;

    [SerializeField] TMP_Text objectiveText;

    Color[] disabledColors = { new Color(255, 255, 255, 1), new Color(0, 0, 0, .5f) };

    private void Start() {
        if (PA_NodeManager.instance) nodeManager = PA_NodeManager.instance;
        if (PA_FungusManager.instance) fungusManager = PA_FungusManager.instance;

        padlocks = new Image[purchaseButtons.Length]; 
        icons = new Image[purchaseButtons.Length]; 
        nameText = new Text[purchaseButtons.Length]; 
        for (int i = purchaseButtons.Length - 1; i >= 0; i--) {
            padlocks[i] = purchaseButtons[i].GetComponentsInChildren<Image>()[2];
            icons[i] = purchaseButtons[i].GetComponentsInChildren<Image>()[1];
            nameText[i] = purchaseButtons[i].GetComponentInChildren<Text>();
            
            if (i != 0)
                purchaseButtons[i].gameObject.SetActive(false);
        }
        UpdateUI(0, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6)) {
            UnlockAll();
        }
    }

    public IEnumerator UnlockNode(int index) {
        padlocks[index].sprite = unlockSprite;
        objectiveText.fontStyle = FontStyles.Strikethrough;
        yield return new WaitForSecondsRealtime(1);
        UpdateUI(index, true);
        if (purchaseButtons.Length > index + 1) 
            UpdateUI(index + 1, false);
    }


    public void UpdateUI(int index, bool state) {
        purchaseButtons[index].gameObject.SetActive(true);
        purchaseButtons[index].interactable = state;
        icons[index].color = disabledColors[(state) ? 0 : 1];
        padlocks[index].gameObject.SetActive(!state);
        nameText[index].gameObject.SetActive(state);

        objectiveText.fontStyle = FontStyles.Normal;
        if (!state) {  
            switch (index) {              
                case 0:
                    objectiveText.text = "Unlocks when 10 plants die";
                    StartCoroutine(ObjectiveOne());
                    break;
                case 1:
                    objectiveText.text = "Unlocks when you produce +30";
                    StartCoroutine(ObjectiveTwo());
                    break;
                case 2:
                    objectiveText.text = "Unlocks when you consume -30";
                    StartCoroutine(ObjectiveThree());
                    break;
                case 3:
                    objectiveText.text = "";
                    break;

            }
        } else if (index == purchaseButtons.Length - 1)
            objectiveText.text = "";
    }

    IEnumerator ObjectiveOne() {
        int deadPlants = 0;
        while(deadPlants < 10) {
            yield return new WaitForSecondsRealtime(1 / 10);
            int newCount = 0;
            foreach(PA_AdjacencyNode node in nodeManager.plantNodes) {
                if (node.stage == PA_AdjacencyNode.NodeStage.Decaying)
                    newCount++;
            }
            deadPlants = newCount;
        }
        StartCoroutine(UnlockNode(0));
    }

    IEnumerator ObjectiveTwo() {
        while(fungusManager.nutrientProduction < 3) 
            yield return new WaitForSecondsRealtime(1 / 10);        
        StartCoroutine(UnlockNode(1));
    }

    IEnumerator ObjectiveThree() {
        while(fungusManager.nutrientConsumption < 3)
            yield return new WaitForSecondsRealtime(1 / 10);
        StartCoroutine(UnlockNode(2));
    }

    void UnlockAll() {
        StopAllCoroutines();
        for (int i = 0; i < purchaseButtons.Length; i++) {
            UpdateUI(i, true);        
        }
    }
}
