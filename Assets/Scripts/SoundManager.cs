using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager m_instance = null;

    [SerializeField] private AudioSource m_efxSource = null;
    [SerializeField] private AudioSource m_musicSource = null;
    private float m_lowPitchRange = 0.5f;
    private float m_highPitchRange = 1.05f;

    void Awake()
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
    }

    public void PlaySingle(AudioClip clip_)
    {
        m_efxSource.clip = clip_;
        m_efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips_)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips_.Length);
        float randomPitch = UnityEngine.Random.Range(m_lowPitchRange, m_highPitchRange);

        m_efxSource.pitch = randomPitch;
        m_efxSource.clip = clips_[randomIndex];
        m_efxSource.Play();
    }

    public void StopMusic()
    {
        m_musicSource.Stop();
    }
}
