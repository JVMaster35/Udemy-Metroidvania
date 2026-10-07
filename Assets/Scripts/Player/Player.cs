using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player instance;

    public bool rageMode;

    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth;
    public float healthRegenSpeed;
    public bool isDead;

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthTxt;

    [Header("Magic")]
    public float maxMagic = 100;
    public float currentMagic;
    public float magicRegenSpeed;
    public float magicBulletCost;

    [Header("Magic UI")]
    [SerializeField] private Slider magicSlider;
    [SerializeField] private TMP_Text magicTxt;

    [Header("Movement")]
    public float moveSpeed;
    private Rigidbody2D playerRDBY;

    [Header("Jump")]
    public float jumpForce;
    public Transform groundCheck;
    private bool isGrounded;
    private bool doDoubleJump;
    public LayerMask groundLayer;
    public GameObject jumpEffect;

    [Header("Melee")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public LayerMask bossLayer;
    public int attackDamage = 20;
    public float attackRate = 2f;
    public float nextAttackTime = 0f;

    [Header("Range")]
    public PlayerShot playerShot;
    public Transform shootPoint;

    [Header("Rage")]
    public float maxRage = 30;
    public float currentRage;
    public float rageRegenSpeed;
    public float derageAmount;

    [Header("Rage UI")]
    [SerializeField] private Slider rageSlider;
    [SerializeField] private TMP_Text rageTxt;

    [Header("Level Up System")]
    public int playerLevel = 1;
    public int maxLevel = 50;
    public int currentExp;
    public int[] expToNextLevel;
    public int baseEXP = 500;

    [Header("LevelUp UI")]
    [SerializeField] private Slider currentXpSlider;
    [SerializeField] private TMP_Text levelTxt;

    [Header("Animation")]
    public Animator playerAnim;

    [Header("Sprite")]
    public SpriteRenderer playerRenderer;

    public Interractable Interractable { get; set; }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Player Health Start
        healthRegenSpeed = SaveManager.instance.activeSave.healthRegen;
        maxHealth = SaveManager.instance.activeSave.maxHealth;
        UpdateHealth();
        currentHealth = maxHealth;

        //Player Magic Start
        magicRegenSpeed = SaveManager.instance.activeSave.magicRegen;
        maxMagic = SaveManager.instance.activeSave.maxMagic;
        UpdateMagic();
        currentMagic = maxMagic;

        //Player Rage Start
        rageRegenSpeed = SaveManager.instance.activeSave.rageRegen;
        maxRage = SaveManager.instance.activeSave.maxRage;
        UpdateRage();
        currentRage = maxRage;

        //Player Attack Damage
        attackDamage = SaveManager.instance.activeSave.attackDamage;

        //Player Level, XP, & Level Up System
        playerLevel = SaveManager.instance.activeSave.playerLevel;
        expToNextLevel = SaveManager.instance.activeSave.expToNextLevel;
        maxLevel = SaveManager.instance.activeSave.maxLevel;
        currentExp = SaveManager.instance.activeSave.currentExp;
        baseEXP = SaveManager.instance.activeSave.baseEXP;
        LevelUpSystem();
        UpdateLevel();

        playerRDBY = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(Input.GetMouseButtonDown(1))
        {
            Interractable?.Interact(this);
        }

        //If Player is Alive
        if (!isDead && !DialogueManager.instance.IsOpen)
        {
            //Updates Player Health UI & Regeneration
            UpdateHealth();
            HealthRegeneration();

            //Updates Player Magic UI & Refills
            UpdateMagic();
            RegenMagic();

            //Updates Player Rage UI, Regeneration & Derage if in Rage Mode
            UpdateRage();
            RegenRage();
            if(rageMode)
            {
                Derage();
            }

            //Player Level UI
            UpdateLevel();

            //Player Movement
            PlayerMove();

            //Ground Check and Player Jump
            PlayerJump();

            //Player Melee Attack Functions
            if (Time.time >= nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    PlayerMelee();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }

            //Player Range Attack Function
            if (Time.time >= nextAttackTime)
            {
                if (Input.GetButtonDown("Fire1") && currentMagic >= magicBulletCost && rageMode)
                {
                    PlayerShoot();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }

            //Handles Player Animations
            PlayerAnimation();

            if(Input.GetKeyDown(KeyCode.L))
            {
                PlayerLevelUp(100);
            }
        }
        else
        {
            playerRDBY.linearVelocity = Vector2.zero;
        }
    }

    public void PlayerMove()
    {
        playerRDBY.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRDBY.linearVelocity.y);

        //Flip Player
        if(playerRDBY.linearVelocity.x > 0 )
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (playerRDBY.linearVelocity.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
    }

    public void PlayerJump()
    {
        //Checks for Ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (Input.GetButtonDown("Jump") && (isGrounded || doDoubleJump))
        {
            if(isGrounded)
            {
                doDoubleJump = true;
                Instantiate(jumpEffect, groundCheck.position, groundCheck.rotation);
                AudioManager.instance.PlayPlayerSFX(6);
            }
            else
            {
                doDoubleJump= false;
                Instantiate(jumpEffect, groundCheck.position, groundCheck.rotation);
                playerAnim.SetTrigger("Double Jump");
                AudioManager.instance.PlayPlayerSFX(7);
            }

            playerRDBY.linearVelocity = new Vector2(playerRDBY.linearVelocity.x, jumpForce);
        }
    }

    public void PlayerLevelUp(int XP)
    {
        currentExp += XP;
        SaveManager.instance.activeSave.playerLevel = playerLevel;
        SaveManager.instance.activeSave.expToNextLevel = expToNextLevel;
        SaveManager.instance.activeSave.currentExp = currentExp;
        UpdateLevel();

        if (playerLevel < maxLevel)
        {
            if (currentExp > expToNextLevel[playerLevel])
            {
                currentExp -= expToNextLevel[playerLevel];
                SaveManager.instance.activeSave.playerLevel = playerLevel;
                SaveManager.instance.activeSave.expToNextLevel = expToNextLevel;
                SaveManager.instance.activeSave.currentExp = currentExp;
                playerLevel++;
                UpdateLevel();

                maxHealth += 10;
                SaveManager.instance.activeSave.maxHealth = Player.instance.maxHealth;

                maxMagic += 5;
                SaveManager.instance.activeSave.maxMagic = Player.instance.maxMagic;

                attackDamage += 2;
                SaveManager.instance.activeSave.attackDamage = Player.instance.attackDamage;

                currentHealth = maxHealth;
                currentMagic = maxMagic;
            }
        }
        else
        {
            currentExp = 0;
        }
    }

    public void PlayerMelee()
    {
        playerAnim.SetTrigger("Melee Attack");
        AudioManager.instance.PlayPlayerSFX(8);

        //Enemies Hit
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        //Bosses Hit
        Collider2D[] bossesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, bossLayer);

        foreach (Collider2D boss in bossesHit)
        {
            boss.GetComponent<BossHealth>().TakeDamage(attackDamage);
        }
    }

    public void PlayerShoot()
    {
        playerAnim.SetTrigger("Cast");
        Instantiate(playerShot, shootPoint.position, shootPoint.rotation).moveDirection = new Vector2(transform.localScale.x, 0);
        AudioManager.instance.PlayPlayerSFX(0);
        currentMagic -= magicBulletCost;
    }

    public void PlayerAnimation()
    {
        playerAnim.SetBool("IsGrounded", isGrounded);

        playerAnim.SetFloat("Speed", Mathf.Abs(playerRDBY.linearVelocity.x));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //Play Hurt Animation
        playerAnim.SetTrigger("Hurt");

        AudioManager.instance.PlayPlayerSFX(3);

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthTxt.text = Mathf.RoundToInt(currentHealth) + "/" + maxHealth;
    }

    public void UpdateMagic()
    {
        magicSlider.maxValue = maxMagic;
        magicSlider.value = currentMagic;

        magicTxt.text = Mathf.RoundToInt(currentMagic) + "/" + maxMagic;
    }

    public void UpdateRage()
    {
        rageSlider.maxValue = maxRage;
        rageSlider.value = currentRage;

        rageTxt.text = Mathf.RoundToInt(currentRage) + "/" + maxRage;
    }

    public void Derage()
    {
        currentRage -= derageAmount * Time.deltaTime;

        if (currentRage <= 0)
        {
            rageMode = false;
            playerAnim.SetBool("IsRage", false);
        }

        UpdateRage();
    }

    public void RegenRage()
    {
        if (!rageMode)
        {
            currentRage += rageRegenSpeed * Time.deltaTime;
            if (currentRage > maxRage)
            {
                currentRage = maxRage;
            }
        }
    }

    public void UpdateLevel()
    {
        if (playerLevel < maxLevel)
        {
            currentXpSlider.maxValue = expToNextLevel[playerLevel];
            currentXpSlider.value = currentExp;

            levelTxt.text = "LV: " + playerLevel;
        }

        if (playerLevel == maxLevel)
        {
            currentXpSlider.maxValue = 0;
            levelTxt.text = "MAX LV";
        }
    }

    public void RegenMagic()
    {
        currentMagic += magicRegenSpeed * Time.deltaTime;
        if(currentMagic > maxMagic)
        {
            currentMagic = maxMagic;
        }
    }

    public void HealthRegeneration()
    {
        currentHealth += healthRegenSpeed * Time.deltaTime;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void LevelUpSystem()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.2f);
        }
    }

    void Die()
    {
        //Play Player Death Animation
        playerAnim.SetBool("IsDead", true);

        AudioManager.instance.PlayPlayerSFX(5);

        GetComponent<Collider2D>().enabled = false;

        isDead = true;

        GameManager.instance.GameOver();
    }

    public void RestoreHealth(int givenHealth)
    {
        currentHealth += givenHealth;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealth();
    }

    public void RageMode()
    {
        rageMode = true;
        playerAnim.SetBool("IsRage", true);
    }

    public void UnRageMode()
    {
        rageMode = false;
        playerAnim.SetBool("IsRage", false);
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
