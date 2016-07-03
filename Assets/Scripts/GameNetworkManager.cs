using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameNetworkManager : NetworkManager
{
    // Might leave networking stuff just for the NetworkManager and make new monobehaviour inherit script for match preperation to set below values.
    [Tooltip("Minimum players to start the match, Default: 1")]
    public int MinPlayers = 1;

    private RegisterPrefabs _registerPrefabs;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)                // Spawns the player.
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, 
            new Vector3(0, 5, 0), Quaternion.identity);

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnStartClient(NetworkClient client)                                                // Registers prefabs for a client. (Every joined player).
    {     
        _registerPrefabs = GetComponent<RegisterPrefabs>();
        _registerPrefabs.RegisterPrefabsForNetwork();
    }

    public override void OnClientDisconnect(NetworkConnection conn)                                         // When player has disconnected the server.
    {
        StopClient();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    public void HostGame()      // When host button is clicked.
    {
        StartHost();        
    }


    public void JoinGame()
    {
        StartClient();
    }
}
