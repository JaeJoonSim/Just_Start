using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory : MonoBehaviour
{
    [SerializeField]
    private float m_TileSize;

    [SerializeField]
    private GameObject[] m_MapOBJ;

    private const int m_MaxLine = 100;
    private int m_TileCount;

    bool[,] TileisEmpty = new bool[m_MaxLine, m_MaxLine];

    Stack<Tile> m_tileStack = new Stack<Tile>();

    class Tile
    {
        public int x;
        public int z;
        public GameObject m_TileOBJ;
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

        newTile.m_TileOBJ
            = Instantiate(m_MapOBJ[0], new Vector3(x * m_TileSize, 0, z * m_TileSize), Quaternion.Euler(0, 0, 0));

        m_tileStack.Push(newTile);
        TileisEmpty[x, z] = false;
        m_TileCount++;
    }

    int FindEmpty(int x, int z)
    {
        int dir = 0;

        int[] usedDir = { -1, -1, -1, -1};

        int count = 0;

        int createX;
        int createZ;

        while (count < 4)
        {
            createX = x;
            createZ = z;

            for (int i = 0; i < 4; i++)
            {
                dir = Random.Range(0, 4);
                if (dir == usedDir[i]) continue;
                usedDir[count] = dir;
                count++;
                break;
            }

            switch (dir)
            {
                case 0:
                    createZ += 1;
                    break;
                case 1:
                    createZ -= 1;
                    break;
                case 2:
                    createX -= 1;
                    break;
                case 3:
                    createX += 1;
                    break;
            }

            if (createX < 0) createX = 0;
            if (createX >= m_MaxLine) createX = m_MaxLine - 1;
            if (createZ < 0) createZ = 0;
            if (createZ >= m_MaxLine) createZ = m_MaxLine - 1;

            if (TileisEmpty[createX, createZ])
            {
                CreateTile(createX, createZ);
                return count;
            }
        }       
        return count;
    }

    void CreateLoop()
    {

        int x = 0;
        int z = 0;

        while (m_TileCount < 120)
        {
            if(FindEmpty(x, z) < 4)
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

                    if (FindEmpty(x, z) < 4)
                    {
                        break;
                    }
                }
            }
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
        CreateLoop();
    }

    private void FixedUpdate()
    {
    }
}