using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameManager : MonoBehaviour
{
    private MapBuilder m_mapBuilder;    
    public static GameManager instance = null;
    public int m_playerFoodPoints = 100;
    [HideInInspector] public bool m_playersTurn = true;

    private int m_level = 5;

    //Use this for initialization
    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        m_mapBuilder = GetComponent<MapBuilder>();
        InitGame();
    }
    private void InitGame()
    {
        m_mapBuilder.SetupScene(m_level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
