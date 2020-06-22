using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager m_instance = null;
    public int m_playerFoodPoints = 100;
    [HideInInspector] public bool m_isPlayerTurn = true;

    public float m_turnDelay = 0.1f; //Delay between each Player turn.
    private MapBuilder m_mapBuilder;
    private int m_level = 5;
    private List<Enemy> m_enemies = new List<Enemy>();
    private bool m_enemiesIsMoving;

    public void GameOver()
    {
        enabled = false;
    }

    public void AddEnemyToList(Enemy enemy)
    {
        m_enemies.Add(enemy);
    }

    //Use this for initialization
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        m_mapBuilder = GetComponent<MapBuilder>();
        InitGame();
    }
    private void InitGame()
    {
        m_enemies.Clear();
        m_mapBuilder.SetupScene(m_level);
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_isPlayerTurn || m_enemiesIsMoving) return;

        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        m_enemiesIsMoving = true;
        yield return new WaitForSeconds(m_turnDelay);
        if (m_enemies.Count == 0)
        {
            yield return new WaitForSeconds(m_turnDelay);
        }

        foreach (Enemy enemy in m_enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.m_moveTime);
        }

        m_isPlayerTurn = true;
        m_enemiesIsMoving = false;
    }
}
