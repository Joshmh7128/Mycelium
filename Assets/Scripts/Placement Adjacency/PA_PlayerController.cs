using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PA_PlayerController : MonoBehaviour
{
    #region Singleton
    public static PA_PlayerController instance;
    private void Awake() { 
        if (PA_PlayerController.instance != null) {
            Debug.Log("Warning! More than one instance of PA_PlayerController found!");
            return;
        } else instance = this;
    }
    #endregion

    //Class refs
    PA_NodeManager nodeManager;
    PA_FungusManager fungusManager;
    bool prevDisplayToggle;
    LayerCameraController cameraController;

    //Layer masks
    LayerMask placementMask = 1 << 8;
    LayerMask mouseOverMask = 1 << 9;

    //Mouse input refs
    [SerializeField] Vector3 mousePos;
    PA_AdjacencyNode mousedNode;
    PA_AdjacencyNode selectedNode;

    //Placement state refs
    PA_PlayerNode purchasedNode;

    //Purchasables
    public GameObject[] playerNodePrefabs;

    //Scene refs
    [SerializeField] Slider costSlider;
    [SerializeField] Text costText;



    void Start() {
        if (PA_NodeManager.instance) nodeManager = PA_NodeManager.instance;
        if (PA_FungusManager.instance) fungusManager = PA_FungusManager.instance;
        if (LayerCameraController.instance) cameraController = LayerCameraController.instance;
    }

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) { 
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementMask))
            {
                mousePos = hit.point;

            }
            //if cursor is hovering over a node
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseOverMask)) {
                //if mouse leaves one node directly onto another, disable the first's radius display
                if (hit.transform.GetComponentInParent<PA_AdjacencyNode>() != mousedNode) ToggleMouseOver(false);
                //set mousedNode to node beneath cursor
                mousedNode = hit.transform.GetComponentInParent<PA_AdjacencyNode>();
                ToggleMouseOver(true); //toggle radius display
                //left click input
                if (Input.GetMouseButtonDown(0)) {
                    //center camera on point
                    cameraController.CenterPoint(hit.collider.gameObject.transform.parent.position);
                    //display stats for node
                }
            }
            else {
                ToggleMouseOver(false);
            }

            if (purchasedNode) {
                purchasedNode.transform.position = mousePos;

                costSlider.gameObject.SetActive(true);
                costText.gameObject.SetActive(true);
                costSlider.value = purchasedNode.cost;
                costSlider.maxValue = fungusManager.nutrientTotal;
                costText.text = "-" + purchasedNode.cost;

                if (fungusManager.nutrientTotal > purchasedNode.cost) {
                    if (Input.GetMouseButtonDown(0) && purchasedNode.validPlacement) {
                        purchasedNode.placing = false;
                        PlaceNode();
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        purchasedNode.placing = false;
                        CancelPlace();
                    } 
                } else
                    CancelPlace();
            }
        }

    }

    void ToggleMouseOver(bool state) {
        //enable mouse over
        if (state) { 
            mousedNode.ToggleRadiusDisplay(state);
            //left control shows all nodes in this node's network
            if (Input.GetKey(KeyCode.LeftShift)) {
                foreach(PA_AdjacencyNode node in mousedNode.adjacentNodes) { 
                    if (node)
                        node.ToggleRadiusDisplay(state);   
                }
            //release of left control disables network view
            } else if (Input.GetKeyUp(KeyCode.LeftShift)) { 
                foreach (PA_AdjacencyNode node in mousedNode.adjacentNodes) {              
                    if (node.kingdom == PA_Taxonomy.Kingdom.Plant)
                        node.ToggleRadiusDisplay(nodeManager.plantDisplayActive);
                    if (node.kingdom == PA_Taxonomy.Kingdom.Fungi)
                        node.ToggleRadiusDisplay(nodeManager.fungusDisplayActive);
                }
            }
        }
        //disable mouse over
        else {
            if (mousedNode) {
                if (mousedNode.kingdom == PA_Taxonomy.Kingdom.Plant)
                    mousedNode.ToggleRadiusDisplay(nodeManager.plantDisplayActive);
                if (mousedNode.kingdom == PA_Taxonomy.Kingdom.Fungi)
                    mousedNode.ToggleRadiusDisplay(nodeManager.fungusDisplayActive);

                foreach (PA_AdjacencyNode node in mousedNode.adjacentNodes) {
                    if (node.kingdom == PA_Taxonomy.Kingdom.Plant)
                        node.ToggleRadiusDisplay(nodeManager.plantDisplayActive);
                    if (node.kingdom == PA_Taxonomy.Kingdom.Fungi)
                        node.ToggleRadiusDisplay(nodeManager.fungusDisplayActive);

                } 
                mousedNode = null;
            }
        }
    }

    public void PlaceNode() {
        fungusManager.nutrientTotal -= purchasedNode.cost;
        nodeManager.fungusNodes.Add(purchasedNode);
        nodeManager.adjacencyNodes.Add(purchasedNode); // also add to the master list so we can access via adjacency check
        purchasedNode.ConfirmPlace();
        nodeManager.SetFungusRadiusDisplay(prevDisplayToggle);

        if (fungusManager.nutrientTotal > purchasedNode.cost) {
            PA_PlayerNode tempNode = purchasedNode;
            purchasedNode = null;
            BuyNode(tempNode);
        } else
            purchasedNode = null;

        costSlider.gameObject.SetActive(false);
        costText.gameObject.SetActive(false);
        
    }

    public void BuyNode(int index) {
        if (fungusManager.nutrientTotal >= playerNodePrefabs[index].GetComponent<PA_PlayerNode>().cost) {
            if (purchasedNode) CancelPlace();
           
            purchasedNode = Instantiate(playerNodePrefabs[index], mousePos, 
                            Quaternion.identity, nodeManager.transform).GetComponent<PA_PlayerNode>();

            StartCoroutine(DelayCall());
        }
    }

    public void BuyNode(PA_PlayerNode node) {
        if (fungusManager.nutrientTotal >= node.cost) {            
            if (purchasedNode) CancelPlace();
           
            purchasedNode = Instantiate(playerNodePrefabs[(int)node.buttonIndex], mousePos, 
                            Quaternion.identity, nodeManager.transform).GetComponent<PA_PlayerNode>();

            StartCoroutine(DelayCall());
        }
    } 

    IEnumerator DelayCall() {
        yield return new WaitForFixedUpdate();
        purchasedNode.Place();
        prevDisplayToggle = nodeManager.fungusDisplayActive;
        nodeManager.SetFungusRadiusDisplay(true);
    }

    public void CancelPlace() {
        Destroy(purchasedNode.gameObject);
        nodeManager.SetFungusRadiusDisplay(prevDisplayToggle);
        purchasedNode = null;
        costSlider.gameObject.SetActive(false);
        costText.gameObject.SetActive(false);
    }

    public void HarvestNutrients() { 
        foreach (PA_AdjacencyNode node in nodeManager.fungusNodes) {
            fungusManager.nutrientTotal += node.growthRate;
        }
        if (fungusManager.nutrientTotal < 0) fungusManager.nutrientTotal = 0;
    }
}
