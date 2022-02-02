
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using BettingApp.Manager;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    private const int MainSceneIndex = 1;
    private const int MaxPlayers = 2;

    public static MultiplayerManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UIManager.Instance.ShowInfoText("Disconnected");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        UIManager.Instance.ShowInfoText("Connected");
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(CheckForPlayersConnected());
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateCustomRoom();
    }

    /// <summary>
    /// Create a room with the custom settings provided
    /// </summary>
    private void CreateCustomRoom()
    {
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)MaxPlayers,
        };

        PhotonNetwork.CreateRoom(null, roomOps);

        UIManager.Instance.ShowInfoText("RoomCreated");
    }

    /// <summary>
    /// Wait until both peers are connected in the room,
    /// When both peers are ready then the scene is loaded.
    /// </summary>
    IEnumerator CheckForPlayersConnected()
    {
        UIManager.Instance.ShowInfoText("Looking For Opponent...");

        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayers);

        UIManager.Instance.ShowInfoText(string.Empty);

        LoadSceneGame();
    }

    /// <summary>
    /// Ready to start the game, load the scene
    /// </summary>
    private void LoadSceneGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(MainSceneIndex);
        }
    }

    /// <summary>
    /// Called by the Play Button
    /// </summary>
    public void StartMatch()
    {
        PhotonNetwork.NickName = UIManager.Instance.nickName;

        PhotonNetwork.JoinRandomRoom();
    }
}

