using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using BabyBindings;

public class BabyClient : MonoBehaviour {

	public GameObjects_Info player_info;
    public GameObject player;
    private IPAddress serverIP;
    private Socket clientSocket;
    private byte[] buffer;

	private List<GameObjects_Info> others_info;
    private Dictionary<short, GameObject> activePlayers;

    private void Start () {
        player_info = new GameObjects_Info ();
        others_info = new List<GameObjects_Info> ();
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
        HandleOthersInfo ();
    }

    private void ReceiveInfo () {
        int datalen = clientSocket.Receive (buffer);

        if (datalen == 2) {
            short myNewID = BitConverter.ToInt16 (buffer, 0);
            player.GetComponent<NetID> ().ID = myNewID;
            player_info.UpdateID (myNewID);
            return;
        }

        if(datalen == 8){
            player.transform.position = new Vector2(BitConverter.ToSingle(buffer, 0), BitConverter.ToSingle(buffer, 4));
            return;
        }

        for (int i = 0; i < datalen; i += 14) {
            GameObjects_Info temp = new GameObjects_Info ();
            temp.FromBuffer (buffer, i);
            short tempID = BitConverter.ToInt16 (temp.PlayerID, 0);

            if(tempID == player.GetComponent<NetID> ().ID)
                continue; 

            if (activePlayers.ContainsKey (tempID) == false) {
                others_info.Add (temp);
                CreateNewPlayer (tempID);
            } else {
                Debug.Log(BitConverter.ToInt16 (others_info[0].PlayerID, 0));
                UpdateInfo(temp);
            }
        }
    }

    private void UpdateInfo (GameObjects_Info pInfo) {
        foreach (var info in others_info) {
            if ((BitConverter.ToInt16 (info.PlayerID, 0) == BitConverter.ToInt16 (pInfo.PlayerID, 0)))
                info.PositionX = pInfo.PositionX;
                info.PositionY = pInfo.PositionY;
        }
    }


    private void CreateNewPlayer (short id) {
		
        GameObject newInstance = Instantiate (player, Vector3.right, Quaternion.identity);
        newInstance.AddComponent (typeof (NetID));
        newInstance.GetComponent<NetID> ().ID = id;
		
        activePlayers.Add (id, player);
    }

    private void HandleOthersInfo () {
        foreach (var p in activePlayers) {
            foreach (var info in others_info) {
                if (p.Key == BitConverter.ToInt16 (info.PlayerID, 0)) {
                    Vector2 newPosition = Vector2.zero;
                    float newZRot = 0;
                    info.ConvertToUsableValues (ref newPosition.x, ref newPosition.y, ref newZRot);
                    p.Value.transform.position = new Vector3 (newPosition.x, newPosition.y, p.Value.transform.position.z);
                    p.Value.transform.eulerAngles = new Vector3(p.Value.transform.eulerAngles.x, p.Value.transform.eulerAngles.y, newZRot);
                }
            }
        }
    }

}
