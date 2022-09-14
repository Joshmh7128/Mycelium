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
    [SerializeField] float lerpSpeed; // how fast we move from layer to layer
    bool is2D;
    [SerializeField] Vector3 targetRot, targetPos;
    [SerializeField] float yRot, yRotIncrement, zoom, zoomSpeed, moveSpeed; // our target y rotation

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
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            if (layers[targetLayerInt + 1]) { targetLayerInt++; }
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            if (layers[targetLayerInt - 1]) { targetLayerInt--; }
        }

        // Y rot
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (yRot + yRotIncrement < 360) { yRot += yRotIncrement; } else { yRot = 0; }
            targetRot = new Vector3(targetRot.x, yRot, targetRot.z);
        }        
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (yRot - yRotIncrement > 0) { yRot -= yRotIncrement; } else { yRot = 360; }
            targetRot = new Vector3(targetRot.x, yRot, targetRot.z);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            is2D = !is2D;
            Swap2D(is2D);
        }

        // scroll input
        if (Input.mouseScrollDelta.y < 0) { zoom += zoomSpeed; }
        if (Input.mouseScrollDelta.y > 0) { zoom -= zoomSpeed; }
        zoom = Mathf.Clamp(zoom, 7, 100);

        // movement input
        if (Mathf.Abs(Input.GetAxis("Vertical")) != 0 || Mathf.Abs(Input.GetAxis("Horizontal")) != 0)
        {
            // move around
            targetPos += new Vector3(Input.GetAxis("Horizontal") * moveSpeed, targetPos.y, Input.GetAxis("Vertical") * moveSpeed);
        }

        // update our layers
        UpdateLayers();
    }

    // move to the position of the target layer
    void ProcessMovement()
    {
        // adjust target position on the Y axis
        targetPos = new Vector3(targetPos.x, layers[targetLayerInt].transform.position.y, targetPos.z);
        // lerp to our movement input on all axes
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        // process rotation with a quaternion lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot.x, yRot, targetRot.z), lerpSpeed * Time.deltaTime);
        // our zooming
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom, lerpSpeed * Time.deltaTime);
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
        if (state) { targetRot = new Vector3(2, yRot, 0); }
    }
}
