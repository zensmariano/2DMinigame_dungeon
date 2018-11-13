using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour {

    
    [HideInInspector]
    public GameObject tile;
    
    public int rows, columns;
    public int minSize, maxSize;
    private GameObject[,] boardPositions;
   

    private void InitMap()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
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
                    CreateBSP(subDungeon.left);
                    CreateBSP(subDungeon.right);
                }
            }
        }
    }

    private void DrawMap(SubDungeon subDungeon)
    {
        if (subDungeon.IsLeaf())
        {
            for (int i = (int)subDungeon.room.x; i < (int)subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < (int)subDungeon.room.yMax; j++)
                {
                    GameObject instance = Instantiate(tile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    boardPositions[i, j] = instance;
                }
            }
        }
        else
        {
            if (subDungeon.left != null) DrawMap(subDungeon.left);
            if (subDungeon.right != null) DrawMap(subDungeon.right);
        }

        foreach (Rect hallway in subDungeon.hallways)
        {
            for (int i = (int)hallway.x; i < hallway.xMax; i++)
            {
                for (int j = (int)hallway.y; j < hallway.yMax; j++)
                {
                    
                        GameObject instance = Instantiate(tile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(transform);
                        boardPositions[i, j] = instance;
                   
                }
            }
        }
    }
    
    public class SubDungeon
    {
        public SubDungeon left, right;
        public Rect rect;
        public Rect room = new Rect(-1, -1, 0, 0); 
        public List<Rect> hallways = new List<Rect>();

        public SubDungeon(Rect mrect)
        {
            rect = mrect;
        }

        public bool IsLeaf()
        {
            return left == null && right == null;
        }
        
        public void GenerateMap()
        {
            if (left != null)
            {
                left.GenerateMap();
            }
            if (right != null)
            {
                right.GenerateMap();
            }

            if (left != null && right != null)
            {
                GenerateHallway(left, right);
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
            if (left != null)
            {
                Rect lroom = left.GetRoom();
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }
            if (right != null)
            {
                Rect rroom = right.GetRoom();
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
                    hallways.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 1));

                    if (h < 0)
                    {
                        hallways.Add(new Rect(rpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        hallways.Add(new Rect(rpoint.x, lpoint.y, 1, -Mathf.Abs(h)));
                    }
                }
                else
                {
                    if (h < 0)
                    {
                        hallways.Add(new Rect(lpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        hallways.Add(new Rect(lpoint.x, rpoint.y, 1, Mathf.Abs(h)));
                    }

                    hallways.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 1));
                }
            }
            else
            {
                if (h < 0)
                {
                    hallways.Add(new Rect((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    hallways.Add(new Rect((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs(h)));
                }
            }

        }

        public bool Split(int minSize, int maxSize)
        {
            if (!IsLeaf())
            {
                return false;
            }

            bool splitH;
            if (rect.width / rect.height >= 1.25)
            {
                splitH = false;
            }
            else if (rect.height / rect.width >= 1.25)
            {
                splitH = true;
            }
            else
            {
                splitH = Random.Range(0.0f, 1.0f) > 0.5;
            }

            if (Mathf.Min(rect.height, rect.width) / 2 < minSize)
            {
                return false;
            }

            if (splitH)
            {
                int split = Random.Range(minSize, (int)(rect.width - minSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, rect.width, split));
                right = new SubDungeon(
                  new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
            }
            else
            {
                int split = Random.Range(minSize, (int)(rect.height - minSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, split, rect.height));
                right = new SubDungeon(
                  new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
            }

            return true;
        }
    }

    public void GenerateDungeon()
    {
        InitMap();

        SubDungeon rootDungeon = new SubDungeon(new Rect(0, 0, rows, columns));
        CreateBSP(rootDungeon);

        rootDungeon.GenerateMap();

        boardPositions = new GameObject[rows, columns];
        
        DrawMap(rootDungeon);
    }

    public void Start()
    {
        GenerateDungeon();
    }
    
}
