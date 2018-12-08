using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {

    [HideInInspector]
    public GameObject tlTile;
    [HideInInspector]
    public GameObject tmTile;
    [HideInInspector]
    public GameObject trTile;
    [HideInInspector]
    public GameObject mlTile;
    [HideInInspector]
    public GameObject mmTile;
    [HideInInspector]
    public GameObject mrTile;
    [HideInInspector]
    public GameObject blTile;
    [HideInInspector]
    public GameObject bmTile;
    [HideInInspector]
    public GameObject brTile;


    public int rows, columns;
    public int minSize, maxSize;
    private int roomCount;
    private GameObject[,] tilePositions;

    public GameObject player;
    private Vector3 playerPosition;

    public GameObject enemy;
    public GameObject slimeEnemiesContainer;
    public int minEnemiesPerRoom;
    public int maxEnemiesPerRoom;
    private List<Vector3> enemySpawnPoints;


    public class SubDungeon
    {
        public SubDungeon bottomLeft, bottomRight, topLeft, topRight;
        public Rect rect;
        public Rect room = new Rect(-1, -1, 0, 0);
        public List<Rect> hallways = new List<Rect>();

        public SubDungeon(Rect mrect)
        {
            rect = mrect;
        }

        public bool IsLeaf()
        {
            return (bottomLeft == null && bottomRight == null && topLeft == null && topRight == null);
        }

        public void GenerateMap()
        {
            if (topLeft != null)
            {
                topLeft.GenerateMap();
            }
            if (topRight != null)
            {
                topRight.GenerateMap();
            }
            if (bottomLeft != null)
            {
                bottomLeft.GenerateMap();
            }
            if (bottomRight != null)
            {
                bottomRight.GenerateMap();
            }

            if (topLeft != null && topRight != null)
            {
                GenerateHallway(topLeft, topRight);
            }
            
            if (topRight != null && bottomRight != null)
            {
                GenerateHallway(topRight, bottomRight);
            }
            if (bottomLeft != null && bottomRight != null)
            {
                GenerateHallway(bottomLeft, bottomRight);
            }
            if (topLeft != null && bottomLeft != null)
            {
                GenerateHallway(topLeft, bottomLeft);
            }
            

            if (IsLeaf())
            {
                int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
                int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

                room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);

            }
        }

        public Rect GetRoom()
        {
            if (IsLeaf())
            {
                return room;
            }

            if (bottomLeft != null)
            {
                Rect lroom = bottomLeft.GetRoom();
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }
            if (bottomRight != null)
            {
                Rect rroom = bottomRight.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }
            if (topLeft != null)
            {
                Rect rroom = topLeft.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }
            if (topRight != null)
            {
                Rect rroom = topRight.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }

            return new Rect(-1, -1, 0, 0);
        }

        public void GenerateHallway(SubDungeon left, SubDungeon right)
        {
            Rect lroom = left.GetRoom();
            Rect rroom = right.GetRoom();


            Vector2 lpoint = new Vector2((int)Random.Range(lroom.x + 1, lroom.xMax - 1), (int)Random.Range(lroom.y + 1, lroom.yMax - 1));
            Vector2 rpoint = new Vector2((int)Random.Range(rroom.x + 1, rroom.xMax - 1), (int)Random.Range(rroom.y + 1, rroom.yMax - 1));

            if (lpoint.x > rpoint.x)
            {
                Vector2 temp = lpoint;
                lpoint = rpoint;
                rpoint = temp;
            }

            int w = (int)(lpoint.x - rpoint.x);
            int h = (int)(lpoint.y - rpoint.y);


            if (w != 0)
            {
                if (Random.Range(0, 1) > 2)
                {
                    hallways.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 3));

                    if (h < 0)
                    {
                        hallways.Add(new Rect(rpoint.x, lpoint.y, 3, Mathf.Abs(h)));
                    }
                    else
                    {
                        hallways.Add(new Rect(rpoint.x, lpoint.y, 3, -Mathf.Abs(h)));
                    }
                }
                else
                {
                    if (h < 0)
                    {
                        hallways.Add(new Rect(lpoint.x, lpoint.y, 3, Mathf.Abs(h)));
                    }
                    else
                    {
                        hallways.Add(new Rect(lpoint.x, rpoint.y, 3, Mathf.Abs(h)));
                    }

                    hallways.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 3));
                }
            }
            else
            {
                if (h < 0)
                {
                    hallways.Add(new Rect((int)lpoint.x, (int)lpoint.y, 3, Mathf.Abs(h)));
                }
                else
                {
                    hallways.Add(new Rect((int)rpoint.x, (int)rpoint.y, 3, Mathf.Abs(h)));
                }
            }

        }

        public bool Split(int minSize, int maxSize)
        {
            if (!IsLeaf())
            {
                return false;
            }

            if (Mathf.Min(rect.height, rect.width) / 2 < minSize)
            {
                return false;
            }

            int splitH = Random.Range(minSize, (int)(rect.height - minSize));
            int splitW = Random.Range(minSize, (int)(rect.width - minSize));

            bottomLeft = new SubDungeon(new Rect(rect.x, rect.y, splitW, splitH));
            bottomRight = new SubDungeon(new Rect(rect.x + splitW, rect.y, rect.width - splitW, splitH));
            topLeft = new SubDungeon(new Rect(rect.x, rect.y + splitH, splitW, rect.height - splitH));
            topRight = new SubDungeon(new Rect(rect.x + splitW, rect.y + splitH, rect.width - splitW, rect.height - splitH));
            
            return true;
        }
    }


    public void CreateBSP(SubDungeon subDungeon)
    {
		
			if (subDungeon.IsLeaf())
			{
				if (subDungeon.rect.width > maxSize || subDungeon.rect.height > maxSize || Random.Range(0.0f, 1.0f) > 0.25)
				{
					if (subDungeon.Split(minSize, maxSize))
					{
						CreateBSP(subDungeon.bottomLeft);
						CreateBSP(subDungeon.bottomRight);
						CreateBSP(subDungeon.topLeft);
						CreateBSP(subDungeon.topRight);
					}
				}

		}
        
    }

    private void UpdateTilemapUsingTreeNode(SubDungeon subDungeon)
    {

        if (subDungeon == null)
        {
            return;
        }
        if (subDungeon.IsLeaf())
        {
            for (int i = (int)subDungeon.room.x; i < (int)subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < (int)subDungeon.room.yMax; j++)
                {
                    GameObject instance = Instantiate(mmTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    tilePositions[i, j] = instance;
                }
            }
        }
        else
        {
            UpdateTilemapUsingTreeNode(subDungeon.bottomLeft);
            UpdateTilemapUsingTreeNode(subDungeon.bottomRight);
            UpdateTilemapUsingTreeNode(subDungeon.topLeft);
            UpdateTilemapUsingTreeNode(subDungeon.topRight);
        }

        foreach (Rect hallway in subDungeon.hallways)
        {
            for (int i = (int)hallway.x; i < hallway.xMax; i++)
            {
                for (int j = (int)hallway.y; j < hallway.yMax; j++)
                {
                    GameObject instance = Instantiate(mmTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    tilePositions[i, j] = instance;
                }
            }
        }
    }

    private GameObject GetTileByNeihbors(int i, int j)
    {
        //GameObject instance = Instantiate(tilePositions[i, j], new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
        GameObject mmGridTile = tilePositions[i, j];
        if (mmGridTile == null) return null; 

        var blGridTile = tilePositions[i - 1, j - 1];
        var bmGridTile = tilePositions[i, j - 1];
        var brGridTile = tilePositions[i + 1, j - 1];

        var mlGridTile = tilePositions[i - 1, j];
        var mrGridTile = tilePositions[i + 1, j];

        var tlGridTile = tilePositions[i - 1, j + 1];
        var tmGridTile = tilePositions[i, j + 1];
        var trGridTile = tilePositions[i + 1, j + 1];

        
        if (mlGridTile == null && tmGridTile == null) return tlTile;
        if (mlGridTile == null && tmGridTile != null && bmGridTile != null) return mlTile;
        if (mlGridTile == null && bmGridTile == null && tmGridTile != null) return blTile;
        
        if (mlGridTile != null && tmGridTile == null && mrGridTile != null) return tmTile;
        if (mlGridTile != null && bmGridTile == null && mrGridTile != null) return bmTile;
        
        if (mlGridTile != null && tmGridTile == null && mrGridTile == null) return trTile;
        if (tmGridTile != null && bmGridTile != null && mrGridTile == null) return mrTile;
        if (tmGridTile != null && bmGridTile == null && mrGridTile == null) return brTile;

        return mmTile;
    }
  
    private void DrawMap(SubDungeon subDungeon)
    {
        
        if (subDungeon.IsLeaf())
        {
            for (int i = (int)subDungeon.room.x; i < (int)subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < (int)subDungeon.room.yMax; j++)
                {
                    var tile = GetTileByNeihbors(i, j);
                    GameObject instance = Instantiate(tile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    tilePositions[i, j] = instance;

					if (i == (int)(subDungeon.room.x + ((subDungeon.room.xMax - subDungeon.room.x) / 2)) && j == (int)(subDungeon.room.y + ((subDungeon.room.yMax -subDungeon.room.y) / 2)))
					{
                        //Player Spawn Points
                        if (roomCount == 0) playerPosition = new Vector3(i, j, 0f);
                        //Spawn Points for Enemies
                        else if(roomCount > 0)
                        {
                            enemySpawnPoints.Add(new Vector3(i, j, 0f));
                        }
                    }
                }
            }
            roomCount++;
        }
        else
        {
            if (subDungeon.bottomLeft != null)
                DrawMap(subDungeon.bottomLeft);
            if (subDungeon.bottomRight != null)
                DrawMap(subDungeon.bottomRight);
            if (subDungeon.topLeft != null)
                DrawMap(subDungeon.topLeft);
            if (subDungeon.topRight != null)
                DrawMap(subDungeon.topRight);
        }

        foreach (Rect hallway in subDungeon.hallways)
        {
            for (int i = (int)hallway.x; i < hallway.xMax; i++)
            {
                for (int j = (int)hallway.y; j < hallway.yMax; j++)
                {
                    var tile = GetTileByNeihbors(i, j);
                    GameObject instance = Instantiate(tile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    tilePositions[i, j] = instance;
                }
            }
        }
    }

    
    private void SpawnPlayer(SubDungeon subDungeon)
    {
    	player.transform.position = playerPosition;
    }

	private void SpawnEnemies(SubDungeon subDungeon)
    {
        Vector3 enemyPosition;
        int enemiesInRoom;
        
        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            enemyPosition = enemySpawnPoints[i];
            enemiesInRoom = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom);
            for (int j = 0; j < enemiesInRoom; j++)
            {
                GameObject.Instantiate(enemy, enemyPosition, Quaternion.identity, slimeEnemiesContainer.transform);
            }
        }
            
        
	}


    public void CleanDungeon()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        foreach (Transform child in EnemySpawner.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void GenerateDungeon()
    {
        CleanDungeon();
        roomCount = 0;

        SubDungeon rootDungeon = new SubDungeon(new Rect(0, 0, rows, columns));
        CreateBSP(rootDungeon);
        rootDungeon.GenerateMap();

        tilePositions = new GameObject[rows, columns];
        enemySpawnPoints = new List<Vector3>();

        UpdateTilemapUsingTreeNode(rootDungeon);
        DrawMap(rootDungeon);
        SpawnPlayer(rootDungeon);

        SpawnEnemies (rootDungeon);
    }


    public void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        GenerateDungeon();
    }
    
}
