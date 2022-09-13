using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuilderCameraController : MonoBehaviour
{
    // This script is going to move the player's camera around
    private float rotationX = 0.0f;
    private float rotationZ = 0.0f;
    [SerializeField] private float normalMoveSpeed, fastMoveFactor, slowMoveFactor, cameraSensitivity, posLerpDelta, rotLerpDelta;
    [SerializeField] private Transform lerperObject;
    [SerializeField] private Transform cameraObject;
    [SerializeField] private Transform zRotContainer;
    [SerializeField] private float minimumZRot, maximumZRot, cameraClose, cameraFar; // look up and down

    private void FixedUpdate()
    {
        ProcessFixedMovements();
        ProcessCameraLerp();
    }

    void ProcessFixedMovements()
    {
        // get our movement
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationX -= (cameraSensitivity / 2) * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rotationX += (cameraSensitivity / 2) * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        }

        // camera rotation
        if (Input.GetMouseButton(2) || (Input.GetMouseButton(1)))
        {
            // mouse movement
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;

            rotationZ += Input.GetAxis("Mouse Y") * -cameraSensitivity * Time.deltaTime;
            rotationZ = Mathf.Clamp(rotationZ, minimumZRot, maximumZRot);
            // apply it
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            zRotContainer.localRotation = Quaternion.AngleAxis(rotationZ, Vector3.right);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void ProcessCameraLerp()
    {
        // lerp our position and rotation
        cameraObject.position = Vector3.Lerp(cameraObject.position, lerperObject.position, Time.deltaTime * posLerpDelta);
        Quaternion targetLerpRot = Quaternion.Euler(lerperObject.eulerAngles.x, lerperObject.eulerAngles.y, 0);
        cameraObject.rotation = Quaternion.Slerp(cameraObject.rotation, targetLerpRot, Time.deltaTime * rotLerpDelta);
        cameraObject.rotation = Quaternion.Euler(cameraObject.rotation.eulerAngles.x, cameraObject.rotation.eulerAngles.y, 0);

    }

    // Update is called once per frame
    void Update()
    {
        ProcessUpdateMovements();
    }

    void ProcessUpdateMovements()
    {
        // camera zooming in / out
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // forward
        {
            if (cameraObject.position.y < cameraFar)
            {
                lerperObject.localPosition -= new Vector3(0, -0.2f, 1);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f) // backwards
        {
            if (cameraObject.position.y > cameraClose)
            {
                lerperObject.localPosition += new Vector3(0, -0.2f, 1);
            }
        }
    }
}