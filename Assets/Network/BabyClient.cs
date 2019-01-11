using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using BabyBindings;

public class BabyClient : MonoBehaviour {

	public GameObjects_Info player_info, player2_info;
    public GameObject player, player2;
    private Socket clientSocket;
    private byte[] buffer;

    private Dictionary<short, GameObject> activePlayers;


    private void Start () {
        player_info = new GameObjects_Info ();
        player2_info = new GameObjects_Info();
        activePlayers = new Dictionary<short, GameObject> ();

        clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        clientSocket.Connect (IPAddress.Loopback, Constants.Port);

        buffer = new byte[GameObjects_Info.MaxWireSize];
    }

    private void Update () {
        player_info.Update (player.transform.position.x, player.transform.position.y); 
		player_info.ToBuffer (ref buffer);
        clientSocket.Send (buffer);
        ReceiveInfo ();
        ControlInfo ();
    }

    private void ReceiveInfo () {
        int data = clientSocket.Receive (buffer);

        if (data == 2) {
            short myNewID = BitConverter.ToInt16 (buffer, 0);
            player.GetComponent<NetID> ().ID = myNewID;
            player_info.UpdateID (myNewID);
            return;
        }

        if(data == 8){
            player.transform.position = new Vector2(BitConverter.ToSingle(buffer, 2), BitConverter.ToSingle(buffer, 6));
            return;
        }

        for (int i = 0; i < data; i += 10) {
            GameObjects_Info temp = new GameObjects_Info ();
            temp.FromBuffer (buffer, i);
            short tempID = BitConverter.ToInt16 (temp.PlayerID, 0);

            if(tempID == player.GetComponent<NetID> ().ID)
                continue; 

            if (activePlayers.ContainsKey (tempID) == false) {
                player2_info = temp;
                CreateNewPlayer (tempID);
            }

            else {
                Debug.Log(BitConverter.ToInt16 (player2_info.PlayerID, 0));
                UpdateInfo(temp);
            }
        }
    }

    private void UpdateInfo (GameObjects_Info pInfo) {
        
        if((BitConverter.ToInt16 (player2_info.PlayerID, 0) == BitConverter.ToInt16 (pInfo.PlayerID, 0))){
            player2_info.PositionX = pInfo.PositionX;
            player2_info.PositionY = pInfo.PositionY;
        }
        
    }

    private void CreateNewPlayer (short id) {
        GameObject newInstance = Instantiate (player, Vector3.right, Quaternion.identity);
        newInstance.AddComponent (typeof (NetID));
        newInstance.GetComponent<NetID> ().ID = id;
        activePlayers.Add (id, player);
    }


    private void ControlInfo () {
        
        foreach (var p in activePlayers) {
             Vector2 newPosition = Vector2.zero;
                
                if (p.Key == BitConverter.ToInt16 (player2_info.PlayerID, 0)) {
                 
                    player2_info.Convert(ref newPosition.x, ref newPosition.y);
                    p.Value.transform.position = new Vector3 (newPosition.x, newPosition.y);
            }
        }

    }
}