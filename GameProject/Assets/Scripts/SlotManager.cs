using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum MovingTurn
{
    LocalPlayerTurn,
    OtherPlayerTurn
}
public class SlotManager : NetworkBehaviour
{
    public static SlotManager instance;
    public int indexAt;
    public MovingTurn currentTurn;
    public List<Button> slots;
    public GameObject[] turnDisplay;

    public List<int> TempSlots;

    private NetworkVariable<int> slotsUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    public ulong localPlayerId;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        GameEvents.instance.OnChangeTurn += ChangeTurnServerRpc;
        TempSlots.Clear();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        GameEvents.instance.OnChangeTurn -= ChangeTurnServerRpc;
    }

    private void NetworkManager_OnDisconnect()
    {
        Debug.Log("Disconnected");
        //GameEvents.instance.ResetGame();
    }

    public void SetActivePlayerAtStart()
    {
        ulong chosenClient = NetworkManager.Singleton.ConnectedClientsIds[Random.Range(0, NetworkManager.Singleton.ConnectedClientsIds.Count)];
        Debug.Log(chosenClient);
        SetActivePlayerAtStartClientRpc(chosenClient);
    }
    
    [ClientRpc]
    private void SetActivePlayerAtStartClientRpc(ulong chosenId)
    {
        GameEvents.instance.StartGame();
        if (localPlayerId == chosenId)
        {
            currentTurn = MovingTurn.LocalPlayerTurn;
            EnableButtons();
        }
        else
        {
            DisableButtons();
            currentTurn = MovingTurn.OtherPlayerTurn;
        }
        EnableTurnText();
    }

    private void EnableTurnText()
    {
        if (currentTurn == MovingTurn.LocalPlayerTurn)
        {
            turnDisplay[0].SetActive(true);
            turnDisplay[1].SetActive(false);
        }
        else if(currentTurn == MovingTurn.OtherPlayerTurn)
        {
            turnDisplay[1].SetActive(true);
            turnDisplay[0].SetActive(false);
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void ChangeTurnServerRpc()
    {
        ChangeTurnClientRpc();
    }
    [ClientRpc]
    private void ChangeTurnClientRpc()
    {
        switch (currentTurn)
        {
            case MovingTurn.LocalPlayerTurn :
                currentTurn = MovingTurn.OtherPlayerTurn;
                DisableButtons();
                break;
            case MovingTurn.OtherPlayerTurn :
                currentTurn = MovingTurn.LocalPlayerTurn;
                EnableButtons();
                break;
        }
        EnableTurnText();
    }

    public void CheckBoard(int slotNumber)
    {
        if ((slotsUsed.Value + 1) == 9)
        {
            //GameDraw
            Debug.Log("Game Drew");
            GameEndedResultServerRpc(0, new ServerRpcParams());
            return;
        }
        else
        {
            switch (slotNumber)
            {
                case 0:
                    if (CheckFirstRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }

                    else if (CheckFirstColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if(CheckLeftDiagonal())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 1:
                    if (CheckFirstRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }

                    else if (CheckSecondColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 2:
                    if (CheckFirstRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }

                    else if (CheckThirdColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if(CheckRightDiagonal())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 3:
                    if (CheckSecondRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }

                    else if (CheckFirstColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 4:
                    if (CheckSecondRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }

                    else if (CheckSecondColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckLeftDiagonal())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckRightDiagonal())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 5:
                    if (CheckSecondRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckThirdColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 6:
                    if (CheckThirdRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckFirstColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckRightDiagonal())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 7:
                    if (CheckThirdRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckSecondColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
                case 8:
                    if (CheckThirdRow())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckThirdColumn())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    else if (CheckLeftDiagonal())
                    {
                        GameEndedResultServerRpc(1,new ServerRpcParams());
                        return;
                    }
                    break;
            }
            IncreaseSlotCountServerRpc();
        }
    }

    private bool CheckFirstRow()
    {
        if(slots[0].GetComponent<ClickerSlot>().slotText.text ==
           slots[1].GetComponent<ClickerSlot>().slotText.text
           && slots[0].GetComponent<ClickerSlot>().slotText.text ==
           slots[2].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }
    private bool CheckSecondRow()
    {
        if(slots[3].GetComponent<ClickerSlot>().slotText.text ==
           slots[4].GetComponent<ClickerSlot>().slotText.text
           && slots[3].GetComponent<ClickerSlot>().slotText.text ==
           slots[5].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }
    private bool CheckThirdRow()
    {
        if(slots[6].GetComponent<ClickerSlot>().slotText.text ==
           slots[7].GetComponent<ClickerSlot>().slotText.text
           && slots[6].GetComponent<ClickerSlot>().slotText.text ==
           slots[8].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }
    private bool CheckFirstColumn()
    {
        if(slots[0].GetComponent<ClickerSlot>().slotText.text ==
           slots[3].GetComponent<ClickerSlot>().slotText.text
           && slots[0].GetComponent<ClickerSlot>().slotText.text ==
           slots[6].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }
    private bool CheckSecondColumn()
    {
        if(slots[1].GetComponent<ClickerSlot>().slotText.text ==
           slots[4].GetComponent<ClickerSlot>().slotText.text
           && slots[1].GetComponent<ClickerSlot>().slotText.text ==
           slots[7].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }
    private bool CheckThirdColumn()
    {
        if(slots[2].GetComponent<ClickerSlot>().slotText.text ==
           slots[5].GetComponent<ClickerSlot>().slotText.text
           && slots[2].GetComponent<ClickerSlot>().slotText.text ==
           slots[8].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }
    private bool CheckLeftDiagonal()
    {
        if(slots[0].GetComponent<ClickerSlot>().slotText.text ==
           slots[4].GetComponent<ClickerSlot>().slotText.text
           && slots[0].GetComponent<ClickerSlot>().slotText.text ==
           slots[8].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }
    private bool CheckRightDiagonal()
    {
        if(slots[2].GetComponent<ClickerSlot>().slotText.text ==
           slots[4].GetComponent<ClickerSlot>().slotText.text
           && slots[2].GetComponent<ClickerSlot>().slotText.text ==
           slots[6].GetComponent<ClickerSlot>().slotText.text)
        {
            return true;
        }
        return false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void IncreaseSlotCountServerRpc()
    {
        slotsUsed.Value += 1;
    }
    [ServerRpc(RequireOwnership = false)]
    private void GameEndedResultServerRpc(int resultIndex,ServerRpcParams serverRpcParams)
    {
        GameEndedResultClientRpc(resultIndex,serverRpcParams.Receive.SenderClientId);
    }
    
    [ClientRpc]
    private void GameEndedResultClientRpc(int resultIndex,ulong winnerId)
    {
        if (resultIndex == 0)
        {
            GameEvents.instance.GameEndedDraw();
            return;
        }
        if (GetLocalPlayerID() == winnerId)
        {
            GameEvents.instance.GameWon(true);
        }
        else
        {
            GameEvents.instance.GameWon(false);
        }
    }

    public ulong GetLocalPlayerID()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                return player.GetComponent<NetworkObject>().OwnerClientId;
            }
        }
        return 0;
    }
    [ServerRpc(RequireOwnership = false)]
    public void AddToDisableButtonsServerRpc(int slotNumber)
    {
        AddToDisableButtonsClientRpc(slotNumber);
    }

    [ClientRpc]
    public void AddToDisableButtonsClientRpc(int slotNumber)
    {
        TempSlots.Add(slotNumber);
    }

    public void SetClientId(ulong id)
    {
        localPlayerId = id;
    }
    private void DisableButtons()
    {
        foreach (Button button in slots)
        {
            button.interactable = false;
        }
    }

    private void EnableButtons()
    {
        foreach (Button button in slots)
        {
            button.interactable = true;
        }

        foreach (int index in TempSlots)
        {
            slots[index].interactable = false;
        }
    }
}
