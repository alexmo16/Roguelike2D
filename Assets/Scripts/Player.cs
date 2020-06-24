using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int m_wallDamage = 1;
    public int m_pointsPerFood = 10;
    public int m_pointsPerSoda = 20;
    public float m_restartLevelDelay = 1f;
    public Text m_foodText;

    private Animator m_animator;
    private int m_foodPoints = 0;

    [SerializeField] private AudioClip m_moveSound1 = null;
    [SerializeField] private AudioClip m_moveSound2 = null;
    [SerializeField] private AudioClip m_eatSound1 = null;
    [SerializeField] private AudioClip m_eatSound2 = null;
    [SerializeField] private AudioClip m_drinkSound1 = null;
    [SerializeField] private AudioClip m_drinkSound2 = null;
    [SerializeField] private AudioClip m_gameOverSound = null;

    public void LoseFood(int loss_)
    {
        m_animator.SetTrigger("playerHit");
        m_foodPoints -= loss_;
        m_foodText.text = "-" + loss_ + " Food:" + m_foodPoints;
        CheckIfGameOver();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        m_animator = GetComponent<Animator>();
        m_foodPoints = GameManager.m_instance.m_playerFoodPoints;
        m_foodText.text = "Food: " + m_foodPoints;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir_, int yDir_)
    {
        m_foodPoints--;
        m_foodText.text = "Food: " + m_foodPoints;

        base.AttemptMove<T>(xDir_, yDir_);

        RaycastHit2D hit;
        if (Move(xDir_, yDir_, out hit))
        {
            SoundManager.m_instance.RandomizeSfx(m_moveSound1, m_moveSound2);
        }

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
            SoundManager.m_instance.StopMusic();
            SoundManager.m_instance.PlaySingle(m_gameOverSound);
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
            m_foodText.text = "+" + m_pointsPerFood + " Food: " + m_foodPoints;
            SoundManager.m_instance.RandomizeSfx(m_eatSound1, m_eatSound2);
            collision_.gameObject.SetActive(false);
        }
        else if (collision_.tag == "Soda")
        {
            m_foodPoints += m_pointsPerSoda;
            m_foodText.text = "+" + m_pointsPerSoda + " Food: " + m_foodPoints;
            SoundManager.m_instance.RandomizeSfx(m_drinkSound1, m_drinkSound2);
            collision_.gameObject.SetActive(false);
        }
    }
}
