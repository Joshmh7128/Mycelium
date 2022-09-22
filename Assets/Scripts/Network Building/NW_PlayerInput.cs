using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NW_PlayerInput : MonoBehaviour
{
    /// prototype mouse control handler
    /// 

    public GameObject worldSpaceMouse; // the mouse we are moving around and controlling
    public Transform camTrans;

    public void Update()
    {
        SetMousePos();
    }

    private void Start() => camTrans = Camera.main.gameObject.transform;

    // move the mouse to the camera world space point 
    void SetMousePos()
    {
        Transform camTrans = Camera.main.gameObject.transform;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rayDir = worldPosition - camTrans.position;

        // raycast in that direction
        RaycastHit hit; Physics.Raycast(camTrans.position, rayDir, out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        Vector3 hitPos = hit.point; // set the hit point

        worldSpaceMouse.transform.position = hitPos;
    }

    private void OnDrawGizmos()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rayDir = worldPosition - camTrans.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(camTrans.position, worldPosition);
    }

}
