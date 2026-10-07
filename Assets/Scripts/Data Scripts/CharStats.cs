using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharStats : MonoBehaviour
{
    [SerializeField]
    private TMP_Text hp, mp, ad, hpr, mpr, speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        hp.text = Player.instance.maxHealth + "";
        mp.text = Player.instance.maxMagic + "";
        ad.text = Player.instance.attackDamage + "";
        hpr.text = Player.instance.healthRegenSpeed + "";
        mpr.text = Player.instance.magicRegenSpeed + "";
        speed.text = Player.instance.moveSpeed + "";
    }
}
