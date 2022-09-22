using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCameraController : MonoBehaviour
{
    /// this camera controller functions layers
    /// it moves to the layer based off of player inputs
    /// it turns off the layers that we're not using
    /// 

    // target layers
    [SerializeField] List<GameObject> layers; // our layers
    [SerializeField] GameObject targetLayer; // our current layer
    [SerializeField] int targetLayerInt; // the current layer we're working on
    [SerializeField] float moveSpeed; // how fast we move from layer to layer
    bool is2D;
    [SerializeField] Vector3 targetRot, targetPos;
    [SerializeField] float yRot, yRotIncrement, zoom, zoomSpeed; // our target y rotation

    private void Start()
    {
        targetLayer = layers[targetLayerInt];
        Swap2D(is2D);
    }

    // update
    private void Update()
    {
        // process input
        ProcessInput();
        // process lerp
        ProcessMovement();
    }

    // move when we do inputs
    void ProcessInput()
    {
        // up and down movement
        if (Input.GetKeyDown(KeyCode.Z))
        {
            targetPos.y = 0f;
            if (layers[targetLayerInt + 1]) { targetLayerInt++; }

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            targetPos.y = 0f;
            if (layers[targetLayerInt - 1]) { targetLayerInt--; }
        }

        // Y rot
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //if (yRot + yRotIncrement < 360) { yRot += yRotIncrement; } else { yRot = 0;
                yRot += yRotIncrement;
                targetRot = new Vector3(targetRot.x, yRot, targetRot.z);
        }        
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if (yRot - yRotIncrement > 0) { yRot -= yRotIncrement; } else { yRot = 360; }
            yRot -= yRotIncrement;
            targetRot = new Vector3(targetRot.x, yRot, targetRot.z);
        }

        /*
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            is2D = !is2D;
            Swap2D(is2D);
        }*/

        // scroll input
        if (Input.mouseScrollDelta.y > 0) { zoom += zoomSpeed; }
        if (Input.mouseScrollDelta.y < 0) { zoom -= zoomSpeed; }
        zoom = Mathf.Clamp(zoom, -5000, -1000);

        // movement input
        if (Mathf.Abs(Input.GetAxis("Vertical")) != 0 || Mathf.Abs(Input.GetAxis("Horizontal")) != 0)
        {
            // move around
            // get movement relative to the player's forward direction
            if (!is2D)
            {
                targetPos += transform.forward * Input.GetAxis("Vertical");
                targetPos += transform.right * Input.GetAxis("Horizontal");
            } 
            
            if (is2D)
            {
                targetPos += transform.up * Input.GetAxis("Vertical");
                targetPos += transform.right * Input.GetAxis("Horizontal");
            }

        }

        // update our layers
        UpdateLayers();
    }

    // move to the position of the target layer
    void ProcessMovement()
    {
        // adjust target position on the Y axis
        if (!is2D) { targetPos = new Vector3(targetPos.x, layers[targetLayerInt].transform.position.y, targetPos.z); }

        if (is2D) { targetPos = new Vector3(targetPos.x, targetPos.y, targetPos.z); }
        // lerp to our movement input on all axes
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        // process rotation with a quaternion lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot.x, yRot, targetRot.z), moveSpeed * Time.deltaTime);
        // our zooming
        Camera.main.transform.position = transform.position + Camera.main.transform.forward * zoom;
    }


    // disable layers above
    void UpdateLayers()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            if (i < targetLayerInt)
            {
                layers[i].SetActive(false);
            }

            if (i >= targetLayerInt)
            {
                layers[i].SetActive(true);
            }
        }
    }

    // swap 2d
    void Swap2D(bool state)
    {

        if (!state) { targetRot = new Vector3(30, yRot, 0); }
        if (state) { targetRot = new Vector3(0, yRot, 0); targetLayerInt = 0; }
    }
}
