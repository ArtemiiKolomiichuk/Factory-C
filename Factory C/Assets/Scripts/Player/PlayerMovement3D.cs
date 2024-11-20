using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement3D : NetworkBehaviour
{
    private InputPlayer _input;
    private Animator _animator;
    private Rigidbody _rb;
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

    private Vector3 targetVector;

    private void Awake()
    {
        _input = GetComponent<InputPlayer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        if (!NetworkCompanion.networkEnabled) return;
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner && !IsHost)
        {
            SceneManager_sceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    private void Start()
    {
        SceneManager_sceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode _)
    {
        if(scene.name != LobbyController.TargetScene) return;
        StartCoroutine(MoveCorrectlyCoroutine());
        
    }

    private IEnumerator MoveCorrectlyCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (NetworkCompanion.networkEnabled && !IsOwner) yield break;
        transform.localPosition = new Vector3(-33.3999977f, 3.47999978f, -8.19999981f);
        Camera = Camera.main;
        if (Camera.GetComponent<SmoothCameraFollow>() is SmoothCameraFollow smoothCameraFollow)
        {
            smoothCameraFollow.target = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkCompanion.networkEnabled && !IsOwner || Camera == null) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("qqqqq");
            HandleDisguiseUpdate();
        }

        targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        //characterController.Move(targetVector);
        // print(_input.InputVector.y);
        //var movementVector = 
        //MoveTowardTarget(targetVector);
        //bool isWalking = (_input.InputVector.x != 0 || _input.InputVector.y != 0);
        //_animator.SetBool("walk", isWalking);

        //if (!RotateTowardMouse)
        //{
        //    //RotateTowardMovementVector(movementVector);
        //}
        //if (RotateTowardMouse)
        //{
        //    RotateFromMouseVector();
        //}

    }
    private void FixedUpdate()
    {
        if (NetworkCompanion.networkEnabled && !IsOwner || Camera == null) return;
        MoveTowardTarget(targetVector);

        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(targetVector);
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

    private void MoveTowardTarget(Vector3 inputVector)
    {
        
        Vector3 movementDirection = Quaternion.Euler(0, Camera.transform.eulerAngles.y, 0) * inputVector.normalized;
       //print(movementDirection);
        //Vector3 movement = movementDirection * MovementSpeed * Time.fixedDeltaTime;

        Vector3 desiredVelocity = movementDirection * MovementSpeed;
        //print(desiredVelocity);

        _rb.velocity = desiredVelocity;

        //_rb.MovePosition(_rb.position + movement);
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
}