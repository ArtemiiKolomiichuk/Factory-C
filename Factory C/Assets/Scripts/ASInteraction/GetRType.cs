using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRType : MonoBehaviour
{
    public ResourceType resourceType = ResourceType.None;
    public ResourceType GetResourceType() {
        return resourceType;   
    }
}
