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

    private void Start()
    {
        HideMouse();
    }

    private void Update()
    {
        SetMousePos();
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

    void HideMouse()
    {
        Cursor.visible = false;
    }

}
