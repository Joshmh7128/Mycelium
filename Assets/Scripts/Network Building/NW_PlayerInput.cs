using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NW_PlayerInput : MonoBehaviour
{
    /// <summary>
    /// This script manages the mouse movement and placement of buildings
    /// </summary>
    /// 

    // our mouse world position
    Vector3 castTargetPosition, worldPosition;
    [SerializeField] Transform mouseTransform;

    NW_NodeClass activeNode;

    private void Start()
    {
        HideMouse();
    }

    private void Update()
    {
        SetMousePos();
        if (activeNode != null) SelectedNodeDisplay();
    }

    void SetMousePos()
    {
        // move this to the position of our mouse in world space
        castTargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10000f));
        // do a raycast from the camera to that position and see if there is anything intersecting
        RaycastHit hit;
        if (Physics.Linecast(Camera.main.transform.position, castTargetPosition, out hit, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            worldPosition = hit.point;
        }

        mouseTransform.position = worldPosition;
    }

    void GetMouseDown() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            // Selecting a new node
            if (activeNode != null)
            {
                // Get all colliders within the cursor bounds
                Collider[] colliders = Physics.OverlapSphere(mouseTransform.position, 2f);
                foreach (Collider p in colliders)
                {
                    if (p.GetComponent<NW_NodeClass>())
                    {
                        // Set default node to first
                        if (activeNode == null) activeNode = p.GetComponent<NW_NodeClass>();
                        // Get closest node to cursor
                        if (Vector3.Distance(p.transform.position, mouseTransform.position) < Vector3.Distance(activeNode.transform.position, mouseTransform.position))
                            activeNode = p.GetComponent<NW_NodeClass>();
                    }
                }
            }
            else
            {
                // Placing an emitter from selected node


            }
        }
    }

    void SelectedNodeDisplay() {


    }

    void HideMouse()
    {
        Cursor.visible = false;
    }

}
