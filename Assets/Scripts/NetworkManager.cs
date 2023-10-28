using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{

	private const string ROOM = "room";
    private const string PLAYER = "player";
    private const int MAX_PLAYERS = 2;

	[SerializeField] private UiManager uiManager;

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
		Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined a room with level: {(QnoRoom)PhotonNetwork.CurrentRoom.CustomProperties[ROOM]}");
		//gameInitializer.CreateMultiplayerBoard();
		PreparePlayerSelectionOptions();
		uiManager.ShowPlayerSelectionScreen();

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
		}
	}

	#endregion

	public void SetPlayerRoom(QnoRoom room)
	{
		playerRoom = room;
		PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { ROOM, room } });
	}
    public void SelectPlayer(int player)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PLAYER, player } });
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