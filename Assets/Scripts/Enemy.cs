﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy : MovingObject
{
    protected virtual int m_damage => 10;
    private Animator m_animator;
    private Transform m_target;
    private bool m_isSkippingMove;
    [SerializeField] private AudioClip m_Attack1 = null;
    [SerializeField] private AudioClip m_Attack2 = null;

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        //If the player and the enemy is in the same column.
        if (Mathf.Abs(m_target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = m_target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = m_target.position.x > transform.position.x ? 1 : -1;
        }

        //T is the component we want to verify if there is a collision with. In this case, toward the player.
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void Start()
    {
        GameManager.m_instance.AddEnemyToList(this);
        m_animator = GetComponent<Animator>();
        m_target = GameObject.Find("Player").transform; // wash
        base.Start();
    }

    protected override void OnCantMove<T>(T component_)
    {
        Player hitPlayer = component_ as Player;
        m_animator.SetTrigger("enemyAttack");
        SoundManager.m_instance.RandomizeSfx(m_Attack1, m_Attack2);
        hitPlayer.LoseFood(m_damage);
    }

    protected override void AttemptMove<T>(int xDir_, int yDir_)
    {
        if (m_isSkippingMove)
        {
            m_isSkippingMove = false;
            return;
        }

        base.AttemptMove<T>(xDir_, yDir_);

        m_isSkippingMove = true;
    }
}
