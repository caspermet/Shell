using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemController))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject cameraHolder;

	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

	[SerializeField] Item[] items;

	int itemIndex;
	int previousItemIndex = -1;

	float verticalLookRotation;
	bool grounded;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

	Rigidbody rb;
	PhotonView PV;
	private bool isGrounded;

	ItemController itemController;

	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();
		itemController = GetComponent<ItemController>();
	}

	void Start()
	{
		if (PV.IsMine)
		{
			EquipItem(0);
		}
		else
		{
			//Destroy(GetComponentInChildren<Camera>().gameObject);
			//Destroy(rb);
				
		}
	}

	void Update()
	{
		//if (!PV.IsMine)
			//return;

		LookSystem();
		MoveSystemm();
		JumpSystem();
		WeaponSystem();


		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
		{
			if (itemIndex >= items.Length - 1)
			{
				EquipItem(0);
			}
			else
			{
				EquipItem(itemIndex + 1);
			}
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
		{
			if (itemIndex <= 0)
			{
				EquipItem(items.Length - 1);
			}
			else
			{
				EquipItem(itemIndex - 1);
			}
		}
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

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
	}

	void JumpSystem()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			rb.AddForce(transform.up * jumpForce);
		}
	}

	void WeaponSystem()
	{
		if (Input.GetMouseButton(0))
		{
			print(cameraHolder.transform.position);
			itemController.OnTriggerHold(cameraHolder.transform);
		}

		if (Input.GetMouseButtonUp(0))
		{
			itemController.OnTriggerRelease();
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

		if(previousItemIndex != -1)
		{
			//items[previousItemIndex].itemGameObject.SetActive(false);
		}

		previousItemIndex = itemIndex;
	}

	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}

	void FixedUpdate()
	{
		//if (!PV.IsMine)
			//return;

		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}
}