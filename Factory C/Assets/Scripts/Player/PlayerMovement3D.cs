using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputPlayer))]
public class PlayerMovement3D : MonoBehaviour
{
    private InputPlayer _input;
    private Animator _animator;
    //private CharacterController characterController;

    [SerializeField]
    private bool RotateTowardMouse;

    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private Camera Camera;

    private void Awake()
    {
        _input = GetComponent<InputPlayer>();
        _animator = GetComponent<Animator>();
       // characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        //characterController.Move(targetVector);
        // print(_input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);
        bool isWalking = (_input.InputVector.x != 0 || _input.InputVector.y != 0);
        _animator.SetBool("walk", isWalking);

        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        if (RotateTowardMouse)
        {
            RotateFromMouseVector();
        }

    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = MovementSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        print(targetPosition);
        //characterController.velocity = speed;
        //characterController.Move(targetPosition);
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
}