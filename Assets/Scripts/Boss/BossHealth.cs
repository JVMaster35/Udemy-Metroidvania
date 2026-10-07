using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Health + UI")]
    public int currentHealth;
    public int maxHealth = 100;
    [SerializeField] private GameObject bossHPUI;
    [SerializeField] private Slider bossHPSlider;
    [SerializeField] private Sprite bossImage;
    [SerializeField] private Sprite enragedImage;
    [SerializeField] private Image bossIconUI;

    [Header("Other")]
    public int expToGive;
    [SerializeField] private Animator bossAnim;
    public bool isInvulnerable = false;
    public GameObject exitlLevel;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealth();
        bossAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        bossHPSlider.maxValue = maxHealth;
        bossHPSlider.value = currentHealth;

        if (currentHealth < maxHealth / 2)
        {
            bossIconUI.sprite = enragedImage;
            bossAnim.SetBool("Enraged", true);
        }
        else
        {
            bossIconUI.sprite = bossImage;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return;
        }

        currentHealth -= damage;

        bossHPUI.SetActive(true);

        if (currentHealth < maxHealth / 2)
        {
            bossAnim.SetBool("Enraged", true);
        }

        //Play Hurt Animation
        bossAnim.SetTrigger("Hurt");

        if (currentHealth < 0)
        {
            Death();
            exitlLevel.SetActive(true);
        }
    }

    public void Death()
    {
        //Play Boss Death Animation
        bossAnim.SetBool("IsDead", true);

        StartCoroutine(BossDeathBehaviour());

        //Destory & Disable Boss
        Destroy(gameObject, 5f);

        bossHPUI.SetActive(false);
    }


    [Header("Boss Death Effect")]
    public GameObject deathExplosionFX;
    public Vector2 deathExplosionRadius = new Vector2(2, 3);

    IEnumerator BossDeathBehaviour()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            Instantiate(deathExplosionFX, transform.position + new Vector3(Random.Range(-deathExplosionRadius.x, deathExplosionRadius.x), 
                Random.Range(0, deathExplosionRadius.y), 0), Quaternion.identity);

            AudioManager.instance.PlayPlayerSFX(0);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 5; i++)
        {
            Instantiate(deathExplosionFX, transform.position + new Vector3(Random.Range(-deathExplosionRadius.x, deathExplosionRadius.x),
                Random.Range(0, deathExplosionRadius.y), 0), Quaternion.identity);

            AudioManager.instance.PlayPlayerSFX(0);
            yield return new WaitForSeconds(0.5f);
        }

        gameObject.SetActive(false);
    }
}