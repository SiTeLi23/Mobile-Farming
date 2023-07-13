using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] MobileJoystick joystick;
    private CharacterController characterController;
    private PlayerAnimator playerAnimator;

    [Header(" Settings ")]
    [SerializeField] float moveSpeed = 1f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageMovement();
    }

    private void ManageMovement() 
    {
        Vector3 moveVector = joystick.GetMoveVector()* moveSpeed * Time.deltaTime / Screen.width;

        //translate movement from joystic's vector 2 to world place vector 3
        moveVector.z = moveVector.y;
        moveVector.y = 0;

        characterController.Move(moveVector);
        playerAnimator.ManageAnimation(moveVector);
    }
}
