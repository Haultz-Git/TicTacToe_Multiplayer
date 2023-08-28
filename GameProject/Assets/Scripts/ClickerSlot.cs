using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ClickerSlot : NetworkBehaviour
{
    public int slotNumber;
    public Button clickerButton;
    public TextMeshProUGUI slotText;
    public NetworkVariable<ulong> slotOwnerId;
    private NetworkVariable<ChosenAction> choosenShape = new NetworkVariable<ChosenAction>(ChosenAction.Circle,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        clickerButton.onClick.AddListener(PutSymbol);
        //PutSymbol();
    }

    private void PutSymbol()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                ClickSlot(GetShapeFromAction(player.GetComponent<PlayerController>().currentShape));
                ClickSlotServerRpc(GetShapeFromAction(player.GetComponent<PlayerController>().currentShape),new ServerRpcParams());
            }
        }
        
        //Change the player turn
        SlotManager.instance.AddToDisableButtonsServerRpc(slotNumber);
        GameEvents.instance.ChangePlayerTurn();
        FillSlotOwnerIdServerRpc(new ServerRpcParams());
        SlotManager.instance.CheckBoard(slotNumber);
    }

    

    [ServerRpc(RequireOwnership = false)]
    private void FillSlotOwnerIdServerRpc(ServerRpcParams serverRpcParams)
    {
        this.slotOwnerId.Value = serverRpcParams.Receive.SenderClientId;
    }

    private void GetLocalPlayerAction()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                choosenShape.Value = player.GetComponent<PlayerController>().currentShape;
            }
        }
    }

    private string GetShapeFromAction(ChosenAction action)
    {
        switch (action)
        {
            case ChosenAction.Circle:
                return "O";
            case ChosenAction.Cross:
                return "X";
        }
        return "O";
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClickSlotServerRpc(string shape,ServerRpcParams serverRpcParams)
    {
        ClickSlotClientRpc(shape,serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void ClickSlotClientRpc(string shape,ulong senderId)
    {
        slotText.text = shape;
    }

    private void ClickSlot(string shape)
    {
        slotText.text = shape;
    }
}
