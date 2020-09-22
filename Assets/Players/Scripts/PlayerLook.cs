using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera viewCamera;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetPlayerLook(Vector2 look)
    {
        xRotation -= look.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        viewCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * look.x);
    }
}
