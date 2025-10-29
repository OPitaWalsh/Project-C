using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public string roomCode = "Map1";
    public GameObject player;
    public Transform spawnPoint;
    [Space]
    public GameObject roomCamera;

    private string currentName;



    void Start()
    {
    }


    public void ChangeName(string _name)
    {
        currentName = _name;
    }


    // connect to master server
    public void ConnectToServer()
    {
        Debug.Log("Connecting...");
        
        PhotonNetwork.ConnectUsingSettings();
    }


    // join lobby (view all active rooms)
     public override void OnConnectedToMaster()
    {
        Debug.Log("Joining lobby...");

        PhotonNetwork.JoinLobby();
    }


    // join room
    public override void OnJoinedLobby()
    {
        Debug.Log("Joining room...");

        PhotonNetwork.JoinOrCreateRoom(roomCode, null, null);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Spawning player...");

        PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        roomCamera.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = currentName;
    }
}
