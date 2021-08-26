using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1xxx-character id
//2xxx-weapon id
//3xxx-pool id
//
//

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public class ItemList
    {
        public List<Model.Item> Characters;
        public List<Model.Item> Weapons;
    }

    public class PoolList
    {
        public List<Model.Pool> Pools;
    }

    private void Awake()
    {
        instance = this;
    }




    void Start()
    {
        _Init();
        LoadItemListFromJson();
        LoadPoolListFromJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPoolListFromJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("Data/PoolList");//read file
        string jsonData = itemText.text;//convert to json string
        Debug.Log("Data/PoolList: " + jsonData);
        PoolList data = JsonUtility.FromJson<PoolList>(jsonData);//convert to json array
        AddPoolsToDict(data);
    }

    public void LoadItemListFromJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("Data/ItemList");//read file
        string jsonData = itemText.text;//convert to json string
        Debug.Log("Data/ItemList: " + jsonData);
        ItemList data = JsonUtility.FromJson<ItemList>(jsonData);//convert to json array
        AddItemsToDict(data);
    }

    void AddPoolsToDict(PoolList data)
    {
        foreach (Model.Pool pool in data.Pools)
        {
            if (!Gacha.Instance.poolsDict.ContainsKey(pool.id))
            {
                Gacha.Instance.poolsDict.Add(pool.id, pool);
            }
        }
        int importedNumOfPools = Gacha.Instance.poolsDict.Count;
        Debug.Log("Imported Pools Number: " + importedNumOfPools);
    }

    void AddItemsToDict(ItemList data)
    {
        foreach (Model.Item item in data.Characters)
        {
            if (!Gacha.Instance.itemsDict.ContainsKey(item.id))
            {
                Gacha.Instance.itemsDict.Add(item.id, item);
            }
        }
        int importedNumOfCharacters = Gacha.Instance.itemsDict.Count;
        Debug.Log("Imported Characters Number: " + importedNumOfCharacters);
        foreach (Model.Item item in data.Weapons)
        {
            if (!Gacha.Instance.itemsDict.ContainsKey(item.id))
            {
                Gacha.Instance.itemsDict.Add(item.id, item);
            }
        }
        int importedNumOfWeapons = Gacha.Instance.itemsDict.Count- importedNumOfCharacters;
        Debug.Log("Imported Weapons Number: " + importedNumOfWeapons);
    }

    void _Init()
    {
        Gacha.Instance.itemsDict = new Dictionary<int, Model.Item>();
        Gacha.Instance.poolsDict = new Dictionary<int, Model.Pool>();
    }
}
