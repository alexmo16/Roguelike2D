using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLoader : MonoBehaviour
{
    public GameManager gameManager;
    private void Awake()
    {
        if (GameManager.m_instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
