using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory : MonoBehaviour
{
    [SerializeField]
    private float m_TileSizeX;
    [SerializeField]
    private float m_TileSizeY;
    [SerializeField]
    private float m_TileSizeZ;
    [SerializeField]
    private float m_wallHeight;

    [SerializeField]
    private GameObject[] m_TileOBJ;
    [SerializeField]
    private GameObject[] m_CeilingOBJ;
    [SerializeField]
    private GameObject m_WallOBJ;

    private const int m_MaxLine = 100;
    private int m_TileCount;

    private bool[,] TileisEmpty = new bool[m_MaxLine, m_MaxLine];

    private Stack<Tile> m_tileStack = new Stack<Tile>();
    private Queue<Tile> m_tileQueue = new Queue<Tile>();

    class Tile
    {
        public int x;
        public int z;
        public Tile(int _x, int _z)
        {
            x = _x;
            z = _z;
        }
    }

    void CreateTile(int x, int z)
    {
        int type = Random.Range(0, 4);
        Tile newTile = new Tile(x, z);

        Instantiate(m_TileOBJ[type], new Vector3(x * m_TileSizeX, 0, z * m_TileSizeZ), Quaternion.Euler(0, 0, 0));
        Instantiate(m_TileOBJ[type], new Vector3(x * m_TileSizeX, m_TileSizeY * 2, z * m_TileSizeZ), Quaternion.Euler(0, 0, 0));

        m_tileStack.Push(newTile);
        m_tileQueue.Enqueue(newTile);

        TileisEmpty[x, z] = false;
        m_TileCount++;
    }

    void CreateWall(int _x, int _z, int dir)
    {
        int type = Random.Range(0, 4);
        Tile newTile = new Tile(_x, _z);

        float x = _x * m_TileSizeX;
        float z = _z * m_TileSizeZ;

        float angle = 0;

        switch (dir)
        {
            case 0:
                z += m_TileSizeZ;
                angle = 90;
                break;
            case 1:
                z -= m_TileSizeZ;
                angle = 90;
                break;
            case 2:
                x -= m_TileSizeX;
                break;
            case 3:
                x += m_TileSizeX;
                break;
        }

        if (dir - 2 < 0)
            angle = 90;

        Instantiate(m_WallOBJ, new Vector3(x, m_TileSizeY, z), Quaternion.Euler(0, angle, 0));
    }

    int FindEmpty(int _x, int _z, int createType)
    {
        int dir = 0;

        int[] usedDir = { -1, -1, -1, -1};

        int count = 0;

        int x;
        int z;

        while (count < 4)
        {
            x = _x;
            z = _z;

            if(createType == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    dir = Random.Range(0, 4);
                    if (dir == usedDir[i]) continue;
                    usedDir[count] = dir;
                    count++;
                    break;
                }
            }
            else
            {
                dir = count++;                
            }

            switch (dir)
            {
                case 0:
                    z += 1;
                    break;
                case 1:
                    z -= 1;
                    break;
                case 2:
                    x -= 1;
                    break;
                case 3:
                    x += 1;
                    break;
            }

            if (createType == 0)
            {
                if (x < 0) x = 0;
                if (x >= m_MaxLine) x = m_MaxLine - 1;
                if (z < 0) z = 0;
                if (z >= m_MaxLine) z = m_MaxLine - 1;

                if (TileisEmpty[x, z])
                {
                    CreateTile(x, z);
                    return count;
                }
            }
            else
            {
                if(x > -1 && x < m_MaxLine && z > -1 && z < m_MaxLine)
                {
                    if(TileisEmpty[x, z])
                    CreateWall(_x, _z, dir);
                }
                else
                {
                    CreateWall(_x, _z, dir);
                }
            }
        }

        return count;
    }

    void TileCreateLoop()
    {
        int x = 0;
        int z = 0;

        int random = 0;

        while (m_TileCount < 100)
        {
            random = Random.Range(0, 4);

            if(FindEmpty(x, z, 0) < 4)
            {
                x = m_tileStack.Peek().x;
                z = m_tileStack.Peek().z;
                continue;
            }
            else
            {
                while(true)
                {
                    Tile lastTile = m_tileStack.Pop();

                    x = lastTile.x;
                    z = lastTile.z;

                    if (FindEmpty(x, z, 0) < 4)
                    {
                        break;
                    }
                }
            }
        }
    }

    void WallCreateLoop()
    {
        Tile curTile;

        int x;
        int z;

        while(m_tileQueue.Count > 0)
        {
            curTile = m_tileQueue.Dequeue();
            x = curTile.x;
            z = curTile.z;
            FindEmpty(x, z, 1);
        }
    }


    void Start()
    {
        m_TileCount = 0;

        for(int i = 0; i < m_MaxLine; i++)
        {
            for (int j = 0; j < m_MaxLine; j++)
            {
                TileisEmpty[i, j] = true;
            }
        }
                
        CreateTile(0, 0);
        TileCreateLoop();
        WallCreateLoop();
    }
}