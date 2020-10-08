using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    PhotonView PV;
    bool isGrounded;
    bool isMouseButton2 = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public ItemController itemController;
    public Animator animator;
    public AudioListener audioListener;

    public bool isDebug = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine || isDebug)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (!PV.IsMine && isDebug == false)
        {
            return;
        }

        LookSystem();
        MoveSystemm();
        JumpSystem();
        WeaponSystem();
    }

    void LookSystem()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void MoveSystemm()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float moveSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && (itemController.GetItemType() != ItemType.Gun || !isMouseButton2))
        {
            moveSpeed = sprintSpeed;
        }

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * moveSpeed, ref smoothMoveVelocity, smoothTime);
    }

    void JumpSystem()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //rb.AddForce(transform.up * jumpForce);
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }

    void WeaponSystem()
    {
        if (Input.GetMouseButton(0))
        {
            itemController.OnTriggerHold(cameraHolder.transform);
        }

        if (Input.GetMouseButtonUp(0))
        {
            itemController.OnTriggerRelease();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            isMouseButton2 = true;
            itemController.OnTriggerHoldFire2();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            isMouseButton2 = false;
            itemController.OnTriggerReleaseFire2();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            itemController.Reload();
        }
    }

    void EquipItem(int _index)
    {
        itemIndex = _index;

        //items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            //items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}