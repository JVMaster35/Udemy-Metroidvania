using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    [Header("Victory")]
    public float winDelay, timeToExit;
    public GameObject gameWinScreen;
    public int nextSceneLoad;
    public Animator _anim;

    private void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        _anim = GetComponent<Animator>();
    }

    public void GameWin()
    {
        StartCoroutine(GameWinCounter());
    }

    public IEnumerator GameWinCounter()
    {
        yield return new WaitForSeconds(winDelay);
        gameWinScreen.SetActive(true);
        AudioManager.instance.PlayPlayerSFX(13);
        yield return new WaitForSeconds(timeToExit);

        SceneManager.LoadScene(nextSceneLoad);

        if(nextSceneLoad > PlayerPrefs.GetInt("lvlAt"))
        {
            PlayerPrefs.SetInt("lvlAt", nextSceneLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _anim.SetBool("Open", true);
            GameWin();
        }
    }
}
