using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class BabyClient : MonoBehaviour {

    public GameObjectProperties player_properties, player2_properties;
    public GameObject player, player2;
    private Socket clientSocket;
    private byte[] buffer;

    private void Start () {
        player_properties = new GameObjectProperties ();
        player2_properties = new GameObjectProperties ();
        clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        clientSocket.Connect (IPAddress.Loopback, Constants.Port);

        buffer = new byte[GameObjectProperties.MaxSize * 2];
    }

    private void Update () {
        player_properties.Update (player.transform.position.x, player.transform.position.y,
            player.transform.GetComponent<Animator>().GetFloat("Horizontal"), 
            player.transform.GetComponent<Animator>().GetFloat("Vertical"),
            player.transform.GetComponent<Animator>().GetBool("isMoving"),
            player.transform.GetComponent<Animator>().GetBool("isAttacking"));
        player_properties.ToBuffer (ref buffer);
        clientSocket.Send (buffer);
        ReceiveProperties ();
        ControlProperties ();
    }

    private void ReceiveProperties () {
        int data = clientSocket.Receive (buffer);

        if (data == 2) {
            short myNewID = BitConverter.ToInt16 (buffer, 0);
            short otherPlayerID = 0;

            switch (myNewID) {
                case 1:
                    otherPlayerID = 2;
                    break;
                case 2:
                    otherPlayerID = 1;
                    break;
                default:
                    break;
            }

            player.GetComponent<NetID> ().ID = myNewID;
            player2.GetComponent<NetID> ().ID = otherPlayerID;
            player_properties.UpdateID (myNewID);
            player2_properties.UpdateID (otherPlayerID);
            return;
        }

        if (data == 8) {
            player.transform.position = new Vector2 (BitConverter.ToSingle (buffer, 2), BitConverter.ToSingle (buffer, 6));
            return;
        }

        if(data == 20){
            player.transform.GetComponent<Animator>().SetFloat("Horizontal", BitConverter.ToSingle (buffer, 10));
            player.transform.GetComponent<Animator>().SetFloat("Vertical", BitConverter.ToSingle (buffer, 14));
            player.transform.GetComponent<Animator>().SetBool("isMoving", (bool)BitConverter.ToBoolean (buffer, 18));
            player.transform.GetComponent<Animator>().SetBool("isAttacking", (bool)BitConverter.ToBoolean (buffer, 19));
        }

        for (int i = 0; i < data; i += 20) {
            GameObjectProperties temp = new GameObjectProperties ();
            temp.FromBuffer (buffer, i);
            short tempID = BitConverter.ToInt16 (temp.PlayerID, 0);

            if (tempID == player.GetComponent<NetID> ().ID)
                continue;

            Debug.Log (BitConverter.ToInt16 (player2_properties.PlayerID, 0));
            UpdateProperties (temp);
        }
    }

    private void UpdateProperties (GameObjectProperties pInfo) {

        if ((BitConverter.ToInt16 (player2_properties.PlayerID, 0) == BitConverter.ToInt16 (pInfo.PlayerID, 0))) {
            player2_properties.PositionX = pInfo.PositionX;
            player2_properties.PositionY = pInfo.PositionY;
            player2_properties.Horizontal = pInfo.Horizontal;
            player2_properties.Vertical = pInfo.Vertical;
            player2_properties.IsMoving = pInfo.IsMoving;
            player2_properties.IsAttacking = pInfo.IsAttacking;
        }

    }

    private void ControlProperties () {

        Vector2 newPosition = Vector2.zero;
        float horizontal = 0;
        float vertical = 0;
        bool is_moving = false;
        bool is_attacking = false;

        player2_properties.Convert (ref newPosition.x, ref newPosition.y, ref horizontal, ref vertical, ref is_moving, ref is_attacking);
        player2.transform.position = new Vector3 (newPosition.x, newPosition.y);
        player2.transform.GetComponent<Animator>().SetFloat("Horizontal", horizontal);
        player2.transform.GetComponent<Animator>().SetFloat("Vertical", vertical);
        player2.transform.GetComponent<Animator>().SetBool("isMoving", is_moving);
        player2.transform.GetComponent<Animator>().SetBool("isAttacking", is_attacking);
    }
}