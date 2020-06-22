using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int m_wallDamage = 1;
    public int m_pointsPerFood = 10;
    public int m_pointsPerSoda = 20;
    public float m_restartLevelDelay = 1f;

    private Animator m_animator;
    private int m_foodPoints = 0;


    public void LoseFood(int loss_)
    {
        m_animator.SetTrigger("playerHit");
        m_foodPoints -= loss_;
        CheckIfGameOver();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        m_animator = GetComponent<Animator>();
        if (GameManager.m_instance == null)
        {
            m_foodPoints = GameManager.m_instance.m_playerFoodPoints;
        }

        base.Start();
    }

    protected override void AttemptMove<T>(int xDir_, int yDir_)
    {
        m_foodPoints--;
        base.AttemptMove<T>(xDir_, yDir_);

        //RaycastHit2D hit;
        CheckIfGameOver();

        GameManager.m_instance.m_isPlayerTurn = false;
    }

    protected override void OnCantMove<T>(T component_)
    {
        Wall wall = component_ as Wall;
        wall.DamageWall(m_wallDamage);
        m_animator.SetTrigger("playerChop");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.m_instance.m_isPlayerTurn) return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }
    private void OnDisable()
    {
        GameManager.m_instance.m_playerFoodPoints = m_foodPoints;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CheckIfGameOver()
    {
        if (m_foodPoints <= 0)
        {
            GameManager.m_instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision_)
    {
        if (collision_.tag == "Exit")
        {
            Invoke("Restart", m_restartLevelDelay);
            enabled = false;
        }
        else if (collision_.tag == "Food")
        {
            m_foodPoints += m_pointsPerFood;
            collision_.gameObject.SetActive(false);
        }
        else if (collision_.tag == "Soda")
        {
            m_foodPoints += m_pointsPerSoda;
            collision_.gameObject.SetActive(false);
        }
    }
}
