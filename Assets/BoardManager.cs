using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public enum TileType
	{
		Wall, Floor,
	}

	public int columns;
	public int rows;
	public int numRooms;
	public int roomHeight;
	public int roomWidth;
	public int corridorLeght;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	//public GameObject[] otherTiles;

	private TileType[][] tiles;
	private Room[] rooms;
	//private Corridor[] corridors;
	private GameObject boardHolder;




}
