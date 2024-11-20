using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UserInputController : MonoBehaviour
{
    public GameObject playerInventoryObject = null;
    public UniverslaResourceHolderInterface playerInventory = null;
    public UserInteraction interactor = null;

    public KeyCode useKey = KeyCode.Keypad1;
    public KeyCode getItemKey = KeyCode.Keypad2;
    public KeyCode putItemKey = KeyCode.Keypad3;


    private void Start()
    {
        playerInventory = playerInventoryObject.GetComponent<UniverslaResourceHolderInterface>();
    }

    void Update()
    {
        HandleInput();
    }

    
    private void HandleInput()
    {
        if (Input.GetKeyDown(useKey))
        {
            Debug.Log("TryingToUseWorkstation");
            interactor.InteractWithHead(useKey, true);
        }
        else if (Input.GetKeyDown(getItemKey))
        {
            Debug.Log("TRYING TO GET ITEM 1 | "+ playerInventory.HaveFreeSpace());
            if (playerInventory.HaveFreeSpace())
            {
                InteractionResult get = interactor.InteractWithHead(getItemKey, true);
                Debug.Log("TRYING TO GET ITEM 2 | " + get.Success);
                if (get.Success)
                {
                    Debug.Log("TRYING TO GET ITEM 3 | " + (ResourceType)get.Data);
                    playerInventory.PutResourceType((ResourceType)get.Data);
                }
            }
        }
        else if (Input.GetKeyDown(putItemKey))
        {
            if (playerInventory.HaveResource())
            {
                var getType = playerInventory.PullResourceType();
                InteractionResult get = interactor.InteractWithHead(putItemKey, getType);
                if (!get.Success || !((bool)get.Data))
                {
                    playerInventory.PutResourceType(getType);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            interactor.InteractWithHead(KeyCode.P, playerInventory);
        }

        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("USER_INTERACTION | " + interactor.targets.Count);
        }
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    if (tmpPlayerInventory.HaveResource()) 
        //    {
        //        var getType = tmpPlayerInventory.PullResourceType();
        //        bool get = (bool) InteractWithHead(KeyCode.UpArrow, getType);
        //        if (!get) 
        //        { 
        //            tmpPlayerInventory.PutResourceType(getType);
        //        }
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    if (tmpPlayerInventory.HaveFreeSpace())
        //    {
        //        ResourceType get = (ResourceType) InteractWithHead(KeyCode.DownArrow, true);
        //        tmpPlayerInventory.PutResourceType(get);
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    if (tmpPlayerInventory.HaveFreeSpace())
        //    {
        //        InteractWithHead(KeyCode.RightArrow, true);
        //    }
        //}
    }
    
}
