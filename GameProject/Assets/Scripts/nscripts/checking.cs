using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class checking : NetworkBehaviour
{
    public int x = 5;
    public string message = "Thik cha?";
    
    public struct PlayerData 
    {
        public int hp;
        public string name;
    }
    public override void OnNetworkSpawn()
    {
        Debug.Log("Spawned player "+ OwnerClientId);
    }

    private void Update()
    {
        if(!IsOwner)
            return;
        if (Input.GetKeyDown(KeyCode.H))
        {
            DebugSomethingServerRpc(message,new ServerRpcParams());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DebugSomethingServerRpc(string message,ServerRpcParams serverRpcParams)
    {
        DebugSomething1ClientRpc(message,
            new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new List<ulong>{0,1}
                }
            }
        );
    }

    [ClientRpc]
    private void DebugSomething1ClientRpc(string message ,ClientRpcParams clientRpcParams)
    {
        Debug.Log(message);
    }
    
    [ContextMenu("Show value")]
    public void ShowData()
    {
        
    }
}
