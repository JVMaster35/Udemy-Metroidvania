using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource mainMenuMusic;
    public AudioSource levelMusic;
    public AudioSource bossMusic;

    public AudioSource[] playerSFX;
    public AudioSource[] enemyDeathSFX;
    public AudioSource[] uiSFX;
    public AudioSource[] effectsSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMenuMusic()
    {
        levelMusic.Stop();
        bossMusic.Stop();

        mainMenuMusic.Play();
    }

    public void PlayLevelMusic()
    {
        bossMusic.Stop();
        mainMenuMusic.Stop();

        levelMusic.Play();
    }

    public void PlayBossMusic()
    {
        levelMusic.Stop();
        mainMenuMusic.Stop();

        bossMusic.Play();
    }

    public void PlayPlayerSFX (int playerSfx)
    {
        playerSFX[playerSfx].Stop();
        playerSFX[playerSfx].Play();
    }

    public void PlayEnemyDeathSFX(int enemyDeathSfx)
    {
        enemyDeathSFX[enemyDeathSfx].Stop();
        enemyDeathSFX[enemyDeathSfx].Play();
    }

    public void PlayUiSFX(int uiSfx)
    {
        uiSFX[uiSfx].Stop();
        uiSFX[uiSfx].Play();
    }

    public void PlayEffectsSFX(int effectsSfx)
    {
        effectsSFX[effectsSfx].Stop();
        effectsSFX[effectsSfx].Play();
    }
}
