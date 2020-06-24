using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite m_dmgSprite;

    private int m_hp = 4;
    private SpriteRenderer m_spriteRenderer;

    [SerializeField] private AudioClip m_chopSound1 = null;
    [SerializeField] private AudioClip m_chopSound2 = null;

    //Use this for initialization
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int lossHp_)
    {
        SoundManager.m_instance.RandomizeSfx(m_chopSound1, m_chopSound2);
        m_spriteRenderer.sprite = m_dmgSprite;
        m_hp -= lossHp_;
        if (m_hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
