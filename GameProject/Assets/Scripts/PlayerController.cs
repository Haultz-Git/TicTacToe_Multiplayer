using Unity.Netcode;
using UnityEngine;

public enum ChosenAction
{
    Circle,
    Cross
}
public class PlayerController : NetworkBehaviour
{
    public static PlayerController Instance;
    public ChosenAction currentShape;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        AssignShapeServerRpc(new ServerRpcParams());
        SlotManager.instance.SetClientId(OwnerClientId);
    }
    [ServerRpc(RequireOwnership = false)]
    private void AssignShapeServerRpc(ServerRpcParams serverRpcParams)
    {
        AssignShapeClientRpc(NetworkManager.Singleton.ConnectedClientsIds[0]);
        // if (NetworkManager.Singleton.ConnectedClientsIds[0] == serverRpcParams.Receive.SenderClientId);
        // {
        //     AssignShapeClientRpc(NetworkManager.Singleton.ConnectedClientsIds[0]);
        // }
    }

    [ClientRpc]
    private void AssignShapeClientRpc(ulong id)
    {
        if (id == OwnerClientId)
        {
            currentShape = ChosenAction.Circle;
        }
        else
        {
            currentShape = ChosenAction.Cross;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
