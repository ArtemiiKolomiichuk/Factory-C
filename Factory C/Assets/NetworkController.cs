using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    public static NetworkController Instance { get; private set; }

    void Awake()
    {
        /*
        if (!NetworkManager.Singleton.IsHost)
        {
            Destroy(gameObject);
        }
        */
    }

    [Rpc(SendTo.Server)]
    public void TryPickingUpRpc(NetworkObject item, NetworkObject player)
    {
        throw new NotImplementedException();
    }

    [Rpc(SendTo.Server)]
    public void TryPuttingItemRpc(NetworkObject item, NetworkObject player)
    {
        throw new NotImplementedException();
    }

    [Rpc(SendTo.Server)]
    public void TryThrowingItemRpc(NetworkObject item, NetworkObject player)
    {
        throw new NotImplementedException();
    }

    [Rpc(SendTo.Server)]
    public void TryPlayingOnWorkstationRpc(NetworkObject workstation, NetworkObject player)
    {
        throw new NotImplementedException();
    }

    [Rpc(SendTo.Server)]
    public void AddOrderRpc(string info)
    {
        throw new NotImplementedException();
    }

    [Rpc(SendTo.Server)]
    public void AddAdventurerRpc(string info)
    {
        throw new NotImplementedException();
    }

    [Rpc(SendTo.Server)]
    public void AddMonsterRpc(string info)
    {
        throw new NotImplementedException();
    }
}
