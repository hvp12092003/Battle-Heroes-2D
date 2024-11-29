using System.IO;
using UnityEngine;
using HVPUnityBase.Base.DesignPattern;
using System.Collections;
using UnityEngine.Networking;
using GameNamespace;
using System;
using Photon.Pun;
using Unity.VisualScripting;
using System.Reflection;
using System.Net.NetworkInformation;
using DG.Tweening.Plugins.Core.PathCore;
using SimpleJSON;
public class UserData : MonoBehaviour
{
    public Transform bodyHero;
    public DataLevelBarrack dataLevelBarrack;
    public BarrackAttribute barrackAttribute;
    public DataLevelHeroes[] dataLevelHeroes;
    public HeroAttribute[] heroAttributes;
    public string[] jsonFiles;
    public int count = 0;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        string folderPath = Application.streamingAssetsPath + "/DataUser/LevelHeroes";
        jsonFiles = Directory.GetFiles(folderPath, "*.json");
        int jsonFileCount = jsonFiles.Length;

        dataLevelHeroes = new DataLevelHeroes[jsonFileCount];
        heroAttributes = new HeroAttribute[jsonFileCount];
        dataLevelBarrack = new DataLevelBarrack();
        //read json
        for (int i = 0; i < jsonFileCount; i++)
        {
            StartCoroutine(LoadLevelHeroJsonFromStreamingAssets(i));
        }
        StartCoroutine(LoadLevelBarrackJsonFromStreamingAssets());
    }
    // load data heroes user 
    private IEnumerator LoadLevelHeroJsonFromStreamingAssets(int index)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath + "/DataUser/LevelHeroes", "Hero" + (index + 1).ToString() + ".json");
        UnityWebRequest request = UnityWebRequest.Get(filePath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            dataLevelHeroes[index] = JsonUtility.FromJson<DataLevelHeroes>(jsonData);            
            //    Debug.Log($"Level: {dataLevelHeroes[index].level},PercentEXP: {dataLevelHeroes[index].percentEXP}");
        }
        else
        {
            Debug.LogError("Can't load file JSON: " + request.error);
        }
        LoadJsonFromResources(index);
    }
    private IEnumerator LoadLevelBarrackJsonFromStreamingAssets()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath + "/DataUser/LevelBarrack", "Barrack.json");
        UnityWebRequest request = UnityWebRequest.Get(filePath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            dataLevelBarrack = JsonUtility.FromJson<DataLevelBarrack>(jsonData);
            Debug.Log($"Level: {dataLevelBarrack.level},amountEXP: {dataLevelBarrack.amountEXP}");
        }
        else
        {
            Debug.LogError("Can't load file JSON: " + request.error);
        }
        LoadDataBarrackAttributeJsonFromResources(dataLevelBarrack.level);
    }

    public void LoadDataBarrackAttributeJsonFromResources(float index)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonData/Barrack/BarrackLv" + index.ToString());
        Debug.Log(jsonFile.text);
        barrackAttribute = JsonUtility.FromJson<BarrackAttribute>(jsonFile.text);
    }
    public void HandlerLevelBarrackData(float numEXP)
    {
        dataLevelBarrack.amountEXP += numEXP;
        if (dataLevelBarrack.amountEXP >= dataLevelBarrack.level * 1000)
        {
            dataLevelBarrack.amountEXP -= dataLevelBarrack.level * 1000;
            dataLevelBarrack.level++;
        }
        WriteBarrackLevelDataToJsonStreamingAssets();
    }
    private void WriteBarrackLevelDataToJsonStreamingAssets()
    {
        string jsonData = JsonUtility.ToJson(dataLevelBarrack, true);
        File.WriteAllText(Application.streamingAssetsPath + "/DataUser/LevelBarrack/Barrack.json", jsonData);

        //Debug.Log("ghi tại: " + Application.streamingAssetsPath + "/DataUser/LevelHeroes/Hero" + (index + 1).ToString() + ".json");
    }
    public void HandlerLevelHeroData(int index, float numEXP)
    {
        this.dataLevelHeroes[index].amountEXP += numEXP;
        if (this.dataLevelHeroes[index].amountEXP >= (100 + (100 * (this.dataLevelHeroes[index].level * 5f) / 100)))
        {
            this.dataLevelHeroes[index].amountEXP -= (100 + (100 * (this.dataLevelHeroes[index].level * 5f) / 100));
            this.dataLevelHeroes[index].level++;
        }
        WriteHeroLevelDataToJsonStreamingAssets(index);
    }
    private void WriteHeroLevelDataToJsonStreamingAssets(int index)
    {
        string jsonData = JsonUtility.ToJson(dataLevelHeroes[index], true);
        File.WriteAllText(Application.streamingAssetsPath + "/DataUser/LevelHeroes/Hero" + (index + 1).ToString() + ".json", jsonData);

        //Debug.Log("ghi tại: " + Application.streamingAssetsPath + "/DataUser/LevelHeroes/Hero" + (index + 1).ToString() + ".json");
    }
    // Load the JSON file from Resources
    void LoadJsonFromResources(int index)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonData/Heroes/Hero" + (index + 1).ToString());
        heroAttributes[index] = JsonUtility.FromJson<HeroAttribute>(jsonFile.text);
        /*  Debug.Log($"HP: {heroAttributes[index].baseHP + 1}, speed: {heroAttributes[index].speed}, baseDamage: {heroAttributes[index].baseDamage}," +
        $"baseTimeEffect: {heroAttributes[index].baseTimeEffect}, baseCoolDownAttack: {heroAttributes[index].baseCoolDownAttack}");*/
        heroAttributes[index] = HandlerAttributeHeroData(heroAttributes[index], index);
    }
    public HeroAttribute HandlerAttributeHeroData(HeroAttribute heroAttribute, int index)
    {
        heroAttribute.baseHP += (heroAttribute.baseHP * (dataLevelHeroes[index].level * 5f)) / 100;
        heroAttributes[index].baseDamage += (heroAttribute.baseDamage * (dataLevelHeroes[index].level * 5f)) / 100;
        heroAttributes[index].baseTimeEffect += (heroAttribute.baseTimeEffect * (dataLevelHeroes[index].level * 5f)) / 100;
        if (heroAttributes[index].baseCoolDownAttack > 0.5f) heroAttributes[index].baseCoolDownAttack -= dataLevelHeroes[index].level * 0.05f;
        Debug.Log($"Level: {dataLevelHeroes[index].level}, HP: {heroAttributes[index].baseHP}, speed: {heroAttributes[index].speed}, baseDamage: {heroAttributes[index].baseDamage}," +
     $"baseTimeEffect: {heroAttributes[index].baseTimeEffect}, baseCoolDownAttack: {heroAttributes[index].baseCoolDownAttack}");
        return heroAttribute;
    }

    //Test
    /* public void Update()
     {
         if (Input.GetKeyUp(KeyCode.Space))
         {
             //write json
             for (int i = 0; i < 20; i++)
             {
                 DataLevelHeroes dataHeroes = new DataLevelHeroes
                 {
                     level = 0,
                     amountEXP = 0
                 };
                 WriteToJsonStreamingAssetsTest(i, dataHeroes);
             }
         }
     }
     public void WriteToJsonStreamingAssetsTest(int index, DataLevelHeroes dataLevelHeroes)
     {
         string jsonData = JsonUtility.ToJson(dataLevelHeroes, true);
         File.WriteAllText(Application.streamingAssetsPath + "/DataUser/LevelHeroes/Hero" + (index + 1).ToString() + ".json", jsonData);
     }*/
}
public class UData : SingletonMonoBehaviour<UserData> { }