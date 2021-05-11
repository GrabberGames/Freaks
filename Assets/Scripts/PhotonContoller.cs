using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonContoller : MonoBehaviourPunCallbacks
{
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayerPerRoom = 6;

    #region Private Serializable Fields
    #endregion

    #region Private Fields
    string gameVersion = "0";
    #endregion

    #region MonoBehaviour CallBacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ServerManager/OnConnectedToMaster() was called by PUN");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("ServerManager/OnJoinRandomFailed() was called by PUN. No random room available, so we carete one.\nCalling CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ServerManager/OnJoinedRoom() called by PUN. Now this clinet is in a room.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("ServerManager/OnDisconnected() was called by PUN with reason {0}", cause);
    }
    #endregion

    #region Public Methods
    public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion
}
