using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunController))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerLook))]
public class Player : LivingEntity
{
    public float mouseSensitivity;
    public float moveSpeed = 5;
    public Camera viewCamera;
    PlayerMovement playerMovement;
    PlayerLook playerLook;    
    GunController gunController;

    protected override void Start()
    {
        base.Start();

        gunController = GetComponent<GunController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerLook = GetComponent<PlayerLook>();
    }


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        MoveSystem();
        LookSystem();
        WeaponSystem();
    }

    void MoveSystem()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
    }

    void LookSystem()
    {
        Vector2 look;
        look.x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        look.y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerLook.SetPlayerLook(look);
        gunController.Aim(look);
    }

    void WeaponSystem()
    {
        if (Input.GetMouseButton(0))
        {
            gunController.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerRelease();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.Reload();
        }
    }

    public override void Die()
    {
        AudioManager.instance.PlaySound("Player Death", transform.position);
        base.Die();
    }

    internal Camera GetCameraComponent()
    {
        return viewCamera;
    }
}
