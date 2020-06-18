using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class Count
{
    public int minimum;
    public int maximum;

    public Count(int min, int max)
    {
        minimum = min;
        maximum = max;
    }
}


public class MapBuilder : MonoBehaviour
{
    public int m_columns = 8;
    public int m_rows = 8;
    public Count m_wallCount = new Count(5, 9);
    public Count m_foodCount = new Count(1, 5);
    public GameObject m_exit;
    public GameObject[] m_floorTiles;
    public GameObject[] m_wallTiles;
    public GameObject[] m_foodTiles;
    public GameObject[] m_enemyTiles;
    public GameObject[] m_outerWallTiles;

    private Transform m_boardHolder;
    private List<Vector3> m_gridPositions = new List<Vector3>();

    public void SetupScene(int level_)
    {
        BoardSetup();
        InitaliseList();
        LayoutObjectAtRandom(m_wallTiles, m_wallCount.minimum, m_wallCount.maximum);
        LayoutObjectAtRandom(m_foodTiles, m_foodCount.minimum, m_foodCount.maximum);
        // Log to always a bigger number of enemies with higher levels.
        int enemiesNumberToSpawn = (int)Mathf.Log(level_, 2f);
        LayoutObjectAtRandom(m_enemyTiles, enemiesNumberToSpawn, enemiesNumberToSpawn);
        Instantiate(m_exit, new Vector3(m_columns - 1, m_rows - 1, 0f), Quaternion.identity);
    }

    private void InitaliseList()
    {
        m_gridPositions.Clear();

        for (int x = 1; x < m_columns - 1; ++x)
        {
            for (int y = 1; y < m_rows - 1; ++y)
            {
                m_gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        m_boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < m_columns + 1; ++x)
        {
            for (int y = -1; y < m_rows + 1; ++y)
            {
                GameObject toInstantiate = m_floorTiles[Random.Range(0, m_floorTiles.Length)];

                if (x == -1 || x == m_columns || y == -1 || y == m_rows)
                {
                    toInstantiate = m_outerWallTiles[Random.Range(0, m_outerWallTiles.Length)];
                }                
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(m_boardHolder);
            }
        }
    }

    private Vector3 GetRandomGridPositionAndRemove()
    {
        int randomIndex = Random.Range(0, m_gridPositions.Count);
        Vector3 randomPosition = m_gridPositions[randomIndex];
        //make sure there is no duplicate.
        m_gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

private void LayoutObjectAtRandom(GameObject[] tileArray_, int minimum_, int maximum_)
    {
        int objectCount = Random.Range(minimum_, maximum_ + 1);
        for (int i = 0; i < objectCount; ++i)
        {
            Vector3 randomPosition = GetRandomGridPositionAndRemove();
            GameObject tileChoice = tileArray_[Random.Range(0, tileArray_.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
}
