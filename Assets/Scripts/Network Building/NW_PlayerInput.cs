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
    [SerializeField] LineRenderer emitterDisplayLine;

    NW_NodeClass activeNode;

    private void Start()
    {
        HideMouse();
    }

    private void Update()
    {
        SetMousePos();
        if (activeNode != null) SelectedNodeDisplay();

        // on mouse down
        OnGetMouseDown();
    }

    // moves the mouse around the world
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

    void OnGetMouseDown() 
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
            if (activeNode)
            {
                // start the line from the node
                emitterDisplayLine.SetPosition(0, activeNode.transform.position);
            }
        }
    }

    void OnGetMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (activeNode != null)
            {
                // spawn emitter

                // set active node to null
                activeNode = null;
                // reset display line
                emitterDisplayLine.enabled = false;
            }
        }
    }

    // run whenever want to have the first indexed position of the line displayed in game
    void ShowLine() 
    {
        emitterDisplayLine.SetPosition(1, mouseTransform.position);
    }

    void HideMouse()
    {
        Cursor.visible = false;
    }

}
