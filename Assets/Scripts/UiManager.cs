using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Scene Dependencies")]
    [SerializeField] private NetworkManager networkManager;

    [Header("Buttons")]
    [SerializeField] private Button Player1Button;
    [SerializeField] private Button Player2Button;

    [Header("Texts")]
    [SerializeField] private Text connectionStatusText;

    [Header("Screen GameObjects")]
    [SerializeField] private GameObject connectScreen;
    [SerializeField] private GameObject playerSelectionScreen;

    [Header("Other UI")]
    [SerializeField] private Dropdown gameRoomSelection;
    

    private void Awake()
    {
        gameRoomSelection.AddOptions(Enum.GetNames(typeof(QnoRoom)).ToList());
        OnGameLaunched();
    }

    private void OnGameLaunched()
    {
        DisableAllScreens();
        connectScreen.SetActive(true);
    }
    public void OnConnect()
    {
        connectionStatusText.gameObject.SetActive(true);
        networkManager.SetPlayerRoom((QnoRoom)gameRoomSelection.value);
        networkManager.Connect();
    }
    public void ShowPlayerSelectionScreen()
    {
        DisableAllScreens();
        playerSelectionScreen.SetActive(true);
    }
    public void DisableAllScreens()
    {
        connectScreen.SetActive(false);
        connectionStatusText.gameObject.SetActive(false);
        playerSelectionScreen.SetActive(false);

    }
    public void SetConnectionStatusText(string status)
    {
        connectionStatusText.text= status;
    }
    public void SelectPlayer(int player)
    {
        networkManager.SelectPlayer(player);
        DisableAllScreens();
    }
    public void RestrictPlayerChoice(PlayerNo occupiedPlayer)
    {
        var buttonToDeactivate = occupiedPlayer == PlayerNo.Player1 ? Player1Button : Player2Button;
        buttonToDeactivate.interactable = false;
    }
}
