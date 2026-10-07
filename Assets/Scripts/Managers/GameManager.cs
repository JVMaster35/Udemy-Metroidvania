using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Currency")]
    public int currentCoins;
    public TMP_Text coinTxt;

    [Header("GameOver")]
    public float gameOverDelay, timeToRespawn;
    public GameObject gameOverScreen;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCoins = SaveManager.instance.activeSave.currentCoin;
        UpdateCoins();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCoins();
    }

    public void GetCoins(int coinsToGive)
    {
        currentCoins += coinsToGive;
    }

    public void UpdateCoins()
    {
        coinTxt.text = currentCoins.ToString();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCounter());
    }

    public IEnumerator GameOverCounter()
    {
        yield return new WaitForSeconds(gameOverDelay);
        gameOverScreen.SetActive(true);
        AudioManager.instance.PlayPlayerSFX(2);
        yield return new WaitForSeconds(timeToRespawn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
