using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowSnake : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float mouseSensitivity = 100.0f;
    public float distanceAddition;
    public float mouseX;
    public float mouseY;

    void Start()
    {
        mouseX = 198.262f;
        mouseY = 37.16789f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // gets the mouse movement inputs
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float horizontal = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float vertical = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            mouseX += horizontal;
            mouseY -= vertical;
        }


        // restrict the camera so it doesnt go into the floor
        mouseY = Mathf.Clamp(mouseY, 0f, 140f);

        // rotates the camera
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.rotation = rotation;


        // gets the scrollwheel inputs
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            distance += distanceAddition * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            distance -= distanceAddition * Time.deltaTime;
        }

        if (target != null)
            transform.position = target.position - (rotation * Vector3.forward * distance + Vector3.up * height);



       // distance = Mathf.Clamp(distance, 6, 45);
    }
}
