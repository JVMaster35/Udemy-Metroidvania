using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField]
    private int healthUpgradePrice, healthRegenUpgradePrice;
    [SerializeField]
    private float healthAmount, healthRegenAmount;
    [SerializeField]
    private int magicUpgradePrice, magicRegenUpgradePrice;
    [SerializeField]
    private int magicAmount, magicRegenAmount;
    [SerializeField]
    private int attackDamagePrice;
    [SerializeField]
    private int attackDamageAmount;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        SaveManager.instance.activeSave.currentCoin = GameManager.instance.currentCoins;
    }

    public void HealthUpgrade()
    {
        if (GameManager.instance.currentCoins >= healthUpgradePrice)
        {
            Player.instance.maxHealth += healthAmount;
            Player.instance.currentHealth += healthAmount;
            Player.instance.UpdateHealth();

            SaveManager.instance.activeSave.maxHealth = Player.instance.maxHealth;

            GameManager.instance.currentCoins -= healthUpgradePrice;
            GameManager.instance.UpdateCoins();

            AudioManager.instance.PlayUiSFX(6);
        }
    }

    public void MagicUpgrade()
    {
        if (GameManager.instance.currentCoins >= magicUpgradePrice)
        {
            Player.instance.maxMagic += magicAmount;
            Player.instance.currentMagic += magicAmount;
            Player.instance.UpdateMagic();

            SaveManager.instance.activeSave.maxMagic = Player.instance.maxMagic;


            GameManager.instance.currentCoins -= magicUpgradePrice;
            GameManager.instance.UpdateCoins();

            AudioManager.instance.PlayUiSFX(6);
        }
    }

    public void HealthRegenUpgrade()
    {
        if (GameManager.instance.currentCoins >= healthRegenUpgradePrice)
        {
            Player.instance.healthRegenSpeed += healthRegenAmount;
            Player.instance.HealthRegeneration();

            SaveManager.instance.activeSave.healthRegen = Player.instance.healthRegenSpeed;

            GameManager.instance.currentCoins -= healthRegenUpgradePrice;
            GameManager.instance.UpdateCoins();

            AudioManager.instance.PlayUiSFX(6);
        }
    }

    public void MagicRegenUpgrade()
    {
        if (GameManager.instance.currentCoins >= magicRegenUpgradePrice)
        {
            Player.instance.magicRegenSpeed += magicRegenAmount;
            Player.instance.RegenMagic();

            SaveManager.instance.activeSave.magicRegen = Player.instance.magicRegenSpeed;

            GameManager.instance.currentCoins -= magicRegenUpgradePrice;
            GameManager.instance.UpdateCoins();

            AudioManager.instance.PlayUiSFX(6);
        }
    }

    public void AttackDamageUpgrade()
    {
        if (GameManager.instance.currentCoins >= attackDamagePrice)
        {
            Player.instance.attackDamage += attackDamageAmount;

            SaveManager.instance.activeSave.attackDamage = Player.instance.attackDamage;

            GameManager.instance.currentCoins -= attackDamagePrice;
            GameManager.instance.UpdateCoins();

            AudioManager.instance.PlayUiSFX(6);
        }
    }
}
