using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    LayerCameraController cameraController;

    //Layer masks
    LayerMask placementMask = 1 << 8;
    LayerMask mouseOverMask = 1 << 9;

    //Mouse input refs
    [SerializeField] Vector3 mousePos;
    PA_AdjacencyNode mousedNode;

    //Placement state refs
    PA_Placeable purchasedNode;
    public bool placementValid;

    //Purchasables
    public GameObject myceliumPrefab;

    //Currency
    public float carbonStores;

    void Start() {
        if (PA_NodeManager.instance) nodeManager = PA_NodeManager.instance;
        if (LayerCameraController.instance) cameraController = LayerCameraController.instance;
    }

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementMask))
        {
            mousePos = hit.point;
            if (hit.collider.gameObject.layer == 8) { 
                placementValid = true;
            } else placementValid = false;
        }
             
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseOverMask))
        {
            if (hit.transform.GetComponentInParent<PA_AdjacencyNode>() != mousedNode) ToggleMouseOver(false);
            mousedNode = hit.transform.GetComponentInParent<PA_AdjacencyNode>();
            ToggleMouseOver(true);
            if (Input.GetMouseButton(0)) {
                cameraController.CenterPoint(hit.collider.gameObject.transform.position);
            }
        }
        else {
            ToggleMouseOver(false);
        }

        if (purchasedNode) {
            purchasedNode.transform.position = mousePos;
            if (Input.GetMouseButton(0) && placementValid)
            {
                purchasedNode.placing = false;
                PlaceNode();
            }
            else if (Input.GetMouseButton(1))
            {
                Debug.Log("cancel");
                purchasedNode.placing = false;
                
                RefundPurchase();
            }
        }
        
    }


    void ToggleMouseOver(bool state)
    {
        if (state) { 
            if (!mousedNode.adjRadActive) mousedNode.ToggleRadiusDisplay();
        }
        else {
            if (mousedNode) { 
                if (!nodeManager.displayActive) {
                    mousedNode.ToggleRadiusDisplay();
                    mousedNode = null;
                
                }
            }
        }
    }

    public void PlaceNode() {

        nodeManager.fungusNodes.Add(purchasedNode);
        purchasedNode.ConfirmPlace();
        purchasedNode = null;
    }

    public void BuyMycelium() { 
        if (carbonStores > 25) {
            carbonStores -= 25;
            purchasedNode = Instantiate(myceliumPrefab, mousePos, Quaternion.identity, nodeManager.transform).GetComponent<PA_Placeable>();
            purchasedNode.Place();
        }
    }

    public void RefundPurchase() {
        Destroy(purchasedNode.gameObject);
        carbonStores += 25;
    }
}
