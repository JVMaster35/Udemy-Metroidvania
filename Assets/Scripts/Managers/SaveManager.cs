using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData activeSave;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        string dataPath = Application.persistentDataPath;
        if(File.Exists(dataPath + "/Save Game.data"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/Save Game.data", FileMode.Open);

            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Data Loaded!");
        }
    }

    public void SaveGame()
    {
        string dataPath = Application.persistentDataPath;
        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/Save Game.data", FileMode.Create);

        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Data Saved!");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}

[System.Serializable]
public class SaveData
{
    public int currentCoin, attackDamage;
    public float maxHealth, maxMagic, healthRegen, magicRegen, maxRage, rageRegen;

    public int playerLevel = 1;
    public int maxLevel = 50;
    public int currentExp;
    public int[] expToNextLevel;
    public int baseEXP = 500;
}
