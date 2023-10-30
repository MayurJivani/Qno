using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    
    private List<Card> deck;
    private const string ROOM = "room";
    private const string PLAYER = "player";
    private const int MAX_PLAYERS = 2;
    [SerializeField] private UiManager uiManager;
    
    [Serializable]
    public class DeckContainer
    {
        public List<Card> cards;
    }
    private PhotonView networkManagerPhotonView;

    private QnoRoom playerRoom;
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene=true;
    }
    public void Connect()
	{
		if (PhotonNetwork.IsConnected)
		{
            Debug.LogError($"Connected to server. Looking for random room with level {playerRoom}");
            PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { ROOM, playerRoom } }, MAX_PLAYERS);
			//PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	private void Update()
	{
		uiManager.SetConnectionStatusText(PhotonNetwork.NetworkClientState.ToString());
	}

	#region Photon Callbacks

	public override void OnConnectedToMaster()
	{
		
		Debug.LogError($"Connected to server. Looking for random room with level {playerRoom}");
		PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { ROOM, playerRoom } }, MAX_PLAYERS);
		//PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.LogError($"Joining random room failed becuse of {message}. Creating new one with player room {playerRoom}"); 
        PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			CustomRoomPropertiesForLobby = new string[] { ROOM },
			MaxPlayers = MAX_PLAYERS,
			CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { ROOM, playerRoom } }
		});
        //PhotonNetwork.CreateRoom(null);
	}

	public override void OnJoinedRoom()
	{
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.LogError($"Client {PhotonNetwork.LocalPlayer.ActorNumber} joined a room with level: {(QnoRoom)PhotonNetwork.CurrentRoom.CustomProperties[ROOM]}");
		SelectPlayer(playerNumber);
        //gameInitializer.CreateMultiplayerBoard();
        GameManager.SetupCamerasForPlayer(playerNumber);
        //PreparePlayerSelectionOptions();
		//uiManager.ShowPlayerSelectionScreen();
        uiManager.DisableAllScreens();
		PrepareDeck();
    }

	public void PrepareDeck()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
		{
            deck = CardDeckGenerator.getDeck();
            string deckJson = SerializeDeck(deck);
            photonView.RPC("SendDeckToOthers", RpcTarget.Others, deckJson);
           GameManager.MultiplayerGameStart();
        }
	}
    [PunRPC]
    public void SendDeckToOthers(string deckJson)
    {
        // Handle received deck data from other players
        HandleReceivedDeck(deckJson);
    }

    private void HandleReceivedDeck(string deckJson)
    {
        List<Card> receivedDeck = DeserializeDeck(deckJson);

        if (receivedDeck != null)
        {
            // Now you have the received deck as a list of cards
            Debug.Log("Received deck from the network: " + receivedDeck.Count + " cards.");

            // Call a method to handle the received deck, for example, passing it to your GameManager
            GameManager.ReceiveDeckFromNetwork(receivedDeck);
        }
        else
        {
            Debug.LogError("Failed to handle received deck. Deck is null.");
        }
    }

    public string SerializeDeck(List<Card> deck)
    {
        DeckContainer container = new DeckContainer
        {
            cards = deck
        };

        return JsonUtility.ToJson(container);
    }

    public List<Card> DeserializeDeck(string deckJson)
    {
        DeckContainer container = JsonUtility.FromJson<DeckContainer>(deckJson);
        return container?.cards ?? new List<Card>();
    }
    private void PreparePlayerSelectionOptions()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
		{
			var player = PhotonNetwork.CurrentRoom.GetPlayer(1);
			if (player.CustomProperties.ContainsKey(PLAYER))
			{ 
				var occupiedPlayer = player.CustomProperties[PLAYER];
				uiManager.RestrictPlayerChoice((PlayerNo)occupiedPlayer);
			}
            //PhotonNetwork.SetMasterClient(player);
			//Debug.LogError($"Master is {player}");
            //deckGenerateSync();
        }
	}

	#endregion
	public void deckGenerateSync()
	{
        deck = CardDeckGenerator.getDeck();
        Debug.LogError($"deck is {deck}");
        //gameManager.InitialiseGameObjects();
        // gameManager.StartNewGame();
        /*string deckDataJson = JsonUtility.ToJson(deck);
        networkManagerPhotonView.RPC("SendDeckDataRPC", RpcTarget.Others, deckDataJson, gameManager);*/
    }
	public void SetPlayerRoom(QnoRoom room)
	{
		playerRoom = room;
		PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { ROOM, room } });
	}
    public void SelectPlayer(int player)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PLAYER, player } });
        Debug.LogError($"player selected is {player}");

    }
	/*public void SetPlayerTeam(int teamInt)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
		{
			var player = PhotonNetwork.CurrentRoom.GetPlayer(1);
			if (player.CustomProperties.ContainsKey(TEAM))
			{
				var occupiedTeam = player.CustomProperties[TEAM];
				teamInt = (int)occupiedTeam == 0 ? 1 : 0;
			}
		}
		PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, teamInt } });
		gameInitializer.InitializeMultiplayerController();
		chessGameController.SetupCamera((TeamColor)teamInt);
		chessGameController.SetLocalPlayer((TeamColor)teamInt);
		chessGameController.StartNewGame();
	}*/

	

    internal bool IsRoomFull()
	{
		return PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
	}

}