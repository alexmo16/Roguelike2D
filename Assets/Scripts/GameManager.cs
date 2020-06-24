using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager m_instance = null;
    public int m_playerFoodPoints = 100;
    [HideInInspector] public bool m_isPlayerTurn = true;

    public float m_turnDelay = 0.1f; //Delay between each Player turn.
    private bool m_iniatilizationPhase = false;
    private MapBuilder m_mapBuilder;

    private int m_level = 1;
    private float m_levelStartDelay = 2f;
    private Text m_levelText;
    private GameObject m_levelImage;

    private List<Enemy> m_enemies = new List<Enemy>();
    private bool m_enemiesIsMoving;

    public void GameOver()
    {
        m_levelText.text = "After " + m_level + " days, you starved.";
        m_levelImage.SetActive(true);
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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        m_instance.m_level++;
        m_instance.InitGame();
    }

    private void InitGame()
    {
        m_iniatilizationPhase = true;
        m_levelImage = GameObject.Find("LevelImage");
        m_levelText = GameObject.Find("LevelText").GetComponent<Text>();
        m_levelText.text = "Day " + m_level;
        m_levelImage.SetActive(true);
        Invoke("HideLevelImage", m_levelStartDelay);

        m_enemies.Clear();
        m_mapBuilder.SetupScene(m_level);
    }

    private void HideLevelImage()
    {
        m_levelImage.SetActive(false);
        m_iniatilizationPhase = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_isPlayerTurn || m_enemiesIsMoving || m_iniatilizationPhase) return;

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
