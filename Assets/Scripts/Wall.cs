using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite m_dmgSprite;
    
    private int m_hp = 4;
    private SpriteRenderer m_spriteRenderer;

    //Use this for initialization
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int lossHp_)
    {
        m_spriteRenderer.sprite = m_dmgSprite;
        m_hp -= lossHp_;
        if (m_hp <= 0)
        {
            //gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
