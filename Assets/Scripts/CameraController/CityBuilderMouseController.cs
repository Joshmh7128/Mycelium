using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuilderMouseController : MonoBehaviour
{
    /// <summary>
    /// This script manages the mouse movement and placement of buildings
    /// </summary>
    /// 

    // our mouse world position
    Vector3 castTargetPosition, worldPosition;

    private void Update()
    {
        // move this to the position of our mouse in world space
        castTargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1000f));
        // do a raycast from the camera to that position and see if there is anything intersecting
        RaycastHit hit;
        if (Physics.Linecast(Camera.main.transform.position, castTargetPosition, out hit))
        {
            worldPosition = hit.point;
        }

        transform.position = worldPosition;
    }

    private void FixedUpdate()
    {
        // lerp our position to the mouse position
        // transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime);  
    }
}
