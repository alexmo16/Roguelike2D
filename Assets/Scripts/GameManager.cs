using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameManager : MonoBehaviour
{
    private MapBuilder m_mapBuilder;
    private int m_level = 1;

    public static GameManager instance = null;

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

    // Update is called once per frame
    private void Update()
    {
    }
}
