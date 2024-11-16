using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputPlayer))]
public class PlayerMovement3D : NetworkBehaviour
{
    private InputPlayer _input;
    private Animator _animator;
    public bool isDisguised;
    private GameObject currentShowItem;
    public Vector3 offset;
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
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"OnNetworkSpawn {SceneManager.GetActiveScene().name}");
        if (IsOwner && !IsHost)
        {
            SceneManager_sceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (!IsOwner) 
        {
            Destroy(_input);
        }
        if(scene.name != LobbyController.TargetScene) return;
        transform.position = new Vector3(-3.16695094f, 4.55999994f, -35.5f);
        Camera = Camera.main;
        if (Camera.GetComponent<SmoothCameraFollow>() is SmoothCameraFollow smoothCameraFollow)
        {
            smoothCameraFollow.target = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("qqqqq");
            HandleDisguiseUpdate();
        }

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

    private void HandleDisguiseUpdate()
    {
        if (!isDisguised)
        {
            isDisguised = true;
            GameObject item = PrefabSystem.GetMask();
            Quaternion rotation = Quaternion.Euler(-90f, 180f, 0f);
            currentShowItem = Instantiate(item, transform.position + offset, transform.rotation*rotation);
            currentShowItem.transform.SetParent(transform);

        }
        else if (currentShowItem != null)
        {
            isDisguised = false;
            Destroy(currentShowItem);
            currentShowItem = null;
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
        //print(targetPosition);
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