using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameEvents : NetworkBehaviour
{
    public static GameEvents instance;

    [Header("References")] 
    [SerializeField] private TextMeshProUGUI winText;
    [FormerlySerializedAs("GameEndedPanel")] [SerializeField] private GameObject gameEndedPanel;

        private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public event Action OnGameStarted;
    public event Action OnGameEndedOnDraw;
    public event Action<bool> OnGameWin;
    public event Action<int> OnSlotTouched;
    public event Action OnChangeTurn;
    public event Action OnGameReset;

    public GameObject startPanel;
    [Header("References")] public Button startGameButton;
    [SerializeField] private NetworkManagerHud networkManagerHUD;

    private void Start()
    {
        OnGameStarted += RemoveStartPanel;
        OnGameWin += GameCondition;
        OnGameEndedOnDraw += GameDrawCondition;
        startGameButton.onClick.AddListener(() =>
        {
            //OnGameStarted?.Invoke();
            GameStartServerRpc();
        });
    }

    private void GameCondition(bool winResult)
    {
        gameEndedPanel.SetActive(true);
        switch (winResult)
        {
            case true:
                winText.text = "You won the game.";
                break;
            case false:
                winText.text = "Harisakis ta, khelna chodde , tero kei hune wala chaina";
                break;
        }
    }

    private void GameDrawCondition()
    {
        gameEndedPanel.SetActive(true);
        winText.text = "Game Ended in a draw, Reset the game to play again";
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        OnGameStarted -= RemoveStartPanel;
        OnGameWin += GameCondition;
    }

    [ServerRpc(RequireOwnership = false)]
    private void GameStartServerRpc()
    {
        SlotManager.instance.SetActivePlayerAtStart();
    }

    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    public void GameWon(bool winResult)
    {
        OnGameWin?.Invoke(winResult);
    }

    public void GameEndedDraw()
    {
        OnGameEndedOnDraw?.Invoke();
    }
    public void ChangePlayerTurn()
    {
        SlotManager.instance.ChangeTurnServerRpc();
    }

    public void SlotTouched(int slotNumber)
    {
        OnSlotTouched?.Invoke(slotNumber);
    }

    public void GameReset()
    {
        networkManagerHUD.m_NetworkManager.Shutdown();
        SceneManager.LoadScene(0);
    }
    private void RemoveStartPanel()
    {
        startPanel.SetActive(false);
    }
}
