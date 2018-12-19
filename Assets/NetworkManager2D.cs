using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager2D : NetworkManager {

	public short count;
	public Vector2 playerPosition;

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId) {
		Debug.Log ("Bem-vindo jogador " + playerControllerId);

		//Vector2 spawnPosition = GetRandomSpawnPosition ();
		GameObject player = (GameObject) Instantiate (playerPrefab, playerPosition, Quaternion.identity);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}

	public override void OnClientConnect (NetworkConnection conn) {

		//ClientScene.Ready (conn);
		ClientScene.AddPlayer (conn, count++);
	}



}
