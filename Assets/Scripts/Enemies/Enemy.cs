using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    int currentHealth;
    public int maxHealth = 100;

    public int expToGive;

    [SerializeField] private Slider healthSlider;

    private EnemyController parentEnemy;

    public Animator enemyAnim;

    [Header("Loot Table")]
    public GameObject[] itemDrops;
    public float itemDropChance;
    public float itemDrop2Chance;
    public float itemDrop3Chance;
    public float itemDropRate;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealth();

        parentEnemy = GetComponentInParent<EnemyController>();

        itemDropRate = Random.Range(0f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //Play Hurt Animation
        enemyAnim.SetTrigger("Hurt");

        AudioManager.instance.PlayEnemyDeathSFX(3);

        if (currentHealth < 0)
        {
            parentEnemy.isDead = true;
            Death();
        }

        healthSlider.gameObject.SetActive(true);
    }

    public void UpdateHealth()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void Death()
    {
        //Play Enemy Death Animation
        enemyAnim.SetBool("IsDead", true);

        //Destory & Disable Enemy, Give Player XP, & Drop Item/Loot
        Destroy(gameObject, 1);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        Player.instance.PlayerLevelUp(expToGive);

        if (itemDropRate <= itemDropChance)
        {
            Instantiate(itemDrops[0], transform.position, transform.rotation);
        } 
        else if (itemDropRate <= itemDrop2Chance)
        {
            Instantiate(itemDrops[1], transform.position, transform.rotation);
        }
        else if (itemDropRate <= itemDrop3Chance)
        {
            Instantiate(itemDrops[2], transform.position, transform.rotation);
        }
    }
}
