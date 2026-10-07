using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button[] levelBanners;

    // Start is called before the first frame update
    void Start()
    {
        int lvlAt = PlayerPrefs.GetInt("lvlAt", 2);

        for(int i = 0; i < levelBanners.Length; i++)
        {
            if(i + 2 > lvlAt)
            {
                levelBanners[i].interactable = false;
            }
        }
    }
}
