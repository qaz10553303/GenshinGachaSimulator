using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    private static Gacha instance;
    public static Gacha Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
    }

    public int selectedPoolIndex;

    public Dictionary<int, Model.Item> itemsDict;
    public Dictionary<int, Model.Pool> poolsDict;



    public int totalWishesCounter;//wishes amount in total
    public int eventPoolCounter;
    public int weaponPoolCounter;
    public int standardPoolCounter;
    public int total5StarsCounter;
    public int total4StarsCounter;
    public int total3StarsCounter;

    public int event5StarsCounter;//total pulls since last 5 stars in events
    public int event4StarsCounter;
    public int weapon5StarsCounter;
    public int weapon4StarsCounter;
    public int standard5StarsCounter;
    public int standard4StarsCounter;


    public int event4StarsNoCharacterCounter;
    public int event4StarsNoWeaponCounter;
    public int weapon4StarsNoCharacterCounter;
    public int weapon4StarsNoWeaponCounter;
    public int standard5StarsNoCharacterCounter;//number of pulls does not contain any characters
    public int standard5StarsNoWeaponCounter;
    public int standard4StarsNoCharacterCounter;
    public int standard4StarsNoWeaponCounter;


    public bool isNextGuaranteed5Stars;//If the 5 stars item obtained is not the promotional character, the next 5 stars item is guaranteed to be the promotional character.
    public bool isNextGuaranteed4Stars;
    public bool isNextGuaranteedWeapon;
    public int selectedWeapon;//0=not in use, 1=weapon 1,2=weapon2
    public int selectedWeaponCounter;//will 100% get the selected weapon when reaches 2

    public const int weightMax = 10000;//the total weight of all items cannot exceed this value

    public int w_5Stars;//final weight of 5 stars, will change upon guarantee rules
    public int w_4Stars;
    public int w_3Stars;

    public List<Model.Item> pullResultList;

    void Start()
    {
        selectedPoolIndex = 3000;
        _Init();
    }


    void Update()
    {

    }

    public void Pull(int pullAmount)//pull * 1
    {
        pullResultList = new List<Model.Item>();
        int highestRarity = 3;
        for (int i = 0; i < pullAmount; i++)
        {
            totalWishesCounter += 1;
            Model.Pool pool = poolsDict[selectedPoolIndex];
            int rarity = RarityCheck(pool);//#1 rarity check
            bool isRateUp = RateUpCheck(pool, rarity);//#2 guarantee check
            bool isCharacter = ItemTypeCheck(pool, rarity);//#3 balance check
            int pullResultIndex = GetRandomItem(pool, rarity, isRateUp, isCharacter);//#4 get pull result index
            Model.Item item = GetItemById(pullResultIndex);//#5 get item based on its index
            //Debug.Log("You got: " + item.name + ", rarity: " + item.rarity+", id: "+item.id);
            pullResultList.Add(item);
            if (rarity > highestRarity)
            {
                highestRarity = rarity;
            }
        }
        UIManager.Instance.ShowResult(pullResultList,highestRarity);
    }



    public Model.Item GetItemById(int index)
    {
        if (itemsDict.ContainsKey(index))
        {
            return itemsDict[index];
        }
        else
        {
            Debug.LogError("Items dictionary does not contain index "+index);
            return null;
        }
    }

    public int GetRandomItem(Model.Pool pool, int rarity, bool isRateUp, bool isCharacter)
    {
        int pullRes,pullResultIndex=0;
        Model.PoolType poolType = (Model.PoolType)System.Enum.Parse(typeof(Model.PoolType), pool.poolType);
        switch (poolType)
        {
            case Model.PoolType.EVENT:
                if(rarity==5)
                {
                    event5StarsCounter = 0;
                    if (isRateUp)
                    {
                        pullRes = Random.Range(0, pool.character_boost5Stars.Count);
                        return pullResultIndex = pool.character_boost5Stars[pullRes];
                    }
                    else
                    {
                        pullRes = Random.Range(0, pool.character_5Stars.Count);
                        return pullResultIndex = pool.character_5Stars[pullRes];
                    }
                }
                else if (rarity == 4)
                {
                    event4StarsCounter = 0;
                    if(isRateUp)
                    {
                        if (isCharacter)
                        {
                            pullRes = Random.Range(0, pool.character_boost4Stars.Count);
                            return pullResultIndex = pool.character_boost4Stars[pullRes];
                        }
                        else
                        {
                            pullRes = Random.Range(0, pool.weapon_4Stars.Count);
                            return pullResultIndex = pool.weapon_4Stars[pullRes];
                        }
                    }
                    else
                    {
                        pullRes = Random.Range(0, pool.character_4Stars.Count);
                        return pullResultIndex = pool.character_4Stars[pullRes];
                    }
                }
                else
                {
                    pullRes = Random.Range(0, pool.weapon_3Stars.Count);
                    return pullResultIndex = pool.weapon_3Stars[pullRes];
                }
            case Model.PoolType.WEAPON:
                if (rarity == 5)
                {
                    weapon5StarsCounter = 0;
                    if (isRateUp)
                    {
                        pullRes = Random.Range(0, pool.weapon_boost5Stars.Count);
                        return pullResultIndex = pool.weapon_boost5Stars[pullRes];
                    }
                    else
                    {
                        pullRes = Random.Range(0, pool.weapon_5Stars.Count);
                        return pullResultIndex = pool.weapon_5Stars[pullRes];
                    }
                }
                else if (rarity == 4)
                {
                    weapon4StarsCounter = 0;
                    if (isRateUp)
                    {
                        pullRes = Random.Range(0, pool.weapon_boost4Stars.Count);
                        return pullResultIndex = pool.weapon_boost4Stars[pullRes];
                    }
                    else
                    {
                        if (isCharacter)
                        {
                            pullRes = Random.Range(0, pool.character_4Stars.Count);
                            return pullResultIndex = pool.character_4Stars[pullRes];
                        }
                        else
                        {
                            pullRes = Random.Range(0, pool.weapon_4Stars.Count);
                            return pullResultIndex = pool.weapon_4Stars[pullRes];
                        }
                    }
                }
                else
                {
                    pullRes = Random.Range(0, pool.weapon_3Stars.Count);
                    return pullResultIndex = pool.weapon_3Stars[pullRes];
                }
            case Model.PoolType.STANDARD:
                if (rarity == 5)
                {
                    standard5StarsCounter = 0;
                    if (isCharacter)
                    {
                        pullRes = Random.Range(0, pool.character_5Stars.Count);
                        return pullResultIndex = pool.character_5Stars[pullRes];
                    }
                    else
                    {
                        pullRes = Random.Range(0, pool.weapon_5Stars.Count);
                        return pullResultIndex = pool.weapon_5Stars[pullRes];
                    }
                }
                else if (rarity == 4)
                {
                    standard4StarsCounter = 0;
                    if (isCharacter)
                    {
                        pullRes = Random.Range(0, pool.character_4Stars.Count);
                        return pullResultIndex = pool.character_4Stars[pullRes];
                    }
                    else
                    {
                        pullRes = Random.Range(0, pool.weapon_4Stars.Count);
                        return pullResultIndex = pool.weapon_4Stars[pullRes];
                    }
                }
                else
                {
                    pullRes = Random.Range(0, pool.weapon_3Stars.Count);
                    return pullResultIndex = pool.weapon_3Stars[pullRes];
                }
        }
        Debug.LogError("Error occurs in GetRandomItem function!");
        return pullResultIndex;//should never trigger this line
    }



    public bool ItemTypeCheck(Model.Pool pool, int rarity)//true=character,false=weapon
    {
        int w_character, w_weapon;
        Model.PoolType poolType = (Model.PoolType)System.Enum.Parse(typeof(Model.PoolType), pool.poolType);
        switch (poolType)
        {
            case Model.PoolType.EVENT:
                event4StarsNoCharacterCounter += 1;//plus 1 before culculate
                event4StarsNoWeaponCounter += 1;
                if (rarity == 5)//5бя
                    return true;
                else if (rarity == 4)//4бя
                {
                    w_character = event4StarsNoCharacterCounter >= 18 ? 255 + (event4StarsNoCharacterCounter - 17) * 2550 : 255;
                    w_weapon = event4StarsNoWeaponCounter >= 18 ? 255 + (event4StarsNoWeaponCounter - 17) * 2550 : 255;
                    if (Random.Range(1, w_character + w_weapon + 1) <= w_character)
                    {
                        event4StarsNoCharacterCounter = 0;
                        return true;
                    }
                    else 
                    {
                        event4StarsNoWeaponCounter = 0;
                        return false;
                    }
                }
                else return false;//3бя
            case Model.PoolType.WEAPON:
                weapon4StarsNoCharacterCounter += 1;
                weapon4StarsNoWeaponCounter += 1;
                if (rarity == 5)//5бя
                    return false;
                else if (rarity == 4)//4бя
                {
                    w_character = weapon4StarsNoCharacterCounter >= 16 ? 300 + (weapon4StarsNoCharacterCounter - 16) * 3000 : 300;
                    w_weapon = weapon4StarsNoWeaponCounter >= 16 ? 300 + (weapon4StarsNoWeaponCounter - 16) * 3000 : 300;
                    if (Random.Range(1, w_character + w_weapon + 1) <= w_character)
                    {
                        weapon4StarsNoCharacterCounter = 0;
                        return true;
                    }
                    else 
                    {
                        weapon4StarsNoWeaponCounter = 0;
                        return false;
                    }
                }
                else return false;//3бя
            case Model.PoolType.STANDARD:
                standard5StarsNoCharacterCounter += 1;
                standard5StarsNoWeaponCounter += 1;
                standard4StarsNoCharacterCounter += 1;
                standard4StarsNoWeaponCounter += 1;
                if (rarity == 5)//5бя
                {
                    w_character = standard5StarsNoCharacterCounter >= 148 ? 30 + (standard5StarsNoCharacterCounter - 148) * 300 : 30;
                    w_weapon = standard5StarsNoWeaponCounter >= 148 ? 30 + (standard5StarsNoWeaponCounter - 148) * 300 : 30;
                    if (Random.Range(1, w_character + w_weapon + 1) <= w_character)
                    {
                        standard5StarsNoCharacterCounter = 0;
                        return true;
                    }
                    else 
                    {
                        standard5StarsNoWeaponCounter = 0;
                        return false;
                    }
                }
                else if (rarity == 4)//4бя
                {
                    w_character = standard4StarsNoCharacterCounter >= 16 ? 300 + (standard4StarsNoCharacterCounter - 16) * 3000 : 300;
                    w_weapon = standard4StarsNoWeaponCounter >= 16 ? 300 + (standard4StarsNoWeaponCounter - 16) * 3000 : 300;
                    if (Random.Range(1, w_character + w_weapon + 1) <= w_character)
                    {
                        standard4StarsNoCharacterCounter = 0;
                        return true;
                    }
                    else 
                    {
                        standard4StarsNoWeaponCounter = 0;
                        return false;
                    }
                }
                else return false;//3бя
        }
        Debug.LogError("Error occurs in ItemTypeCheck function!");
        return false;//should never trigger this line
    }

    public bool RateUpCheck(Model.Pool pool, int rarity)
    {
        Model.PoolType poolType = (Model.PoolType)System.Enum.Parse(typeof(Model.PoolType), pool.poolType);
        switch (poolType)
        {
            case Model.PoolType.EVENT:
                if (rarity == 5)//5бя
                {
                    if (isNextGuaranteed5Stars)//guarantee check
                    {
                        isNextGuaranteed5Stars = false;
                        return true;
                    }
                    else//when not triggering guarantee rules
                    {
                        if (Random.Range(0, 2) == 0)//0=not rate up,1=rate up
                        {
                            isNextGuaranteed5Stars = true;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else if(rarity == 4)//4бя
                {
                    if (isNextGuaranteed4Stars)
                    {
                        isNextGuaranteed4Stars = false;
                        return true;
                    }
                    else
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            isNextGuaranteed4Stars = true;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else if(rarity==3)//3бя
                {
                    return false;
                }
                break;
            case Model.PoolType.WEAPON:
                if (rarity == 5)
                {
                    if (isNextGuaranteedWeapon)
                    {
                        isNextGuaranteedWeapon = false;
                        return true;
                    }
                    else
                    {
                        if (Random.Range(0, 4) == 0)//0=not rate up,1=rate up 0=25%,1=75%
                        {
                            isNextGuaranteedWeapon = true;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                break;
            case Model.PoolType.STANDARD:
                return false;//standard does not contain rate up items
        }
        Debug.LogError("Error occurs in RateUpCheck function!");
        return false;//should never trigger this line
    }

    public int RarityCheck(Model.Pool pool)
    {
        Model.PoolType poolType = (Model.PoolType)System.Enum.Parse(typeof(Model.PoolType), pool.poolType);
        switch (poolType)
        {
            case Model.PoolType.EVENT:
                eventPoolCounter += 1;
                event5StarsCounter += 1;
                event4StarsCounter += 1;
                if (event5StarsCounter <= 73) w_5Stars = 60;
                else if(event5StarsCounter>=74) w_5Stars = 60 + (event5StarsCounter - 73) * 600;
                if (event4StarsCounter <= 8) w_4Stars = 510;
                else if (event4StarsCounter >= 9) w_4Stars = 510 + (event4StarsCounter - 8) * 5100;
                w_3Stars = 9430;
                break;
            case Model.PoolType.WEAPON:
                weaponPoolCounter += 1;
                weapon5StarsCounter += 1;
                weapon4StarsCounter += 1;
                if (weapon5StarsCounter <= 62) w_5Stars = 70;
                else if (weapon5StarsCounter >= 63 && weapon5StarsCounter <= 73) w_5Stars = 70 + (weapon5StarsCounter - 62) * 700;
                else if (weapon5StarsCounter >= 74) w_5Stars = 7770 + (weapon5StarsCounter - 73) * 350;
                if (weapon4StarsCounter <= 7) w_4Stars = 600;
                else if (weapon4StarsCounter == 8) w_4Stars = 6600;
                else if (weapon4StarsCounter >= 9) w_4Stars = 6600 + (weapon4StarsCounter - 8) * 3000;
                w_3Stars = 9330;
                break;
            case Model.PoolType.STANDARD:
                standardPoolCounter += 1;
                standard5StarsCounter += 1;
                standard4StarsCounter += 1;
                if (standard5StarsCounter <= 73) w_5Stars = 60;
                else if (standard5StarsCounter >= 74) w_5Stars = 60 + (standard5StarsCounter - 73) * 600;
                if (standard4StarsCounter <= 8) w_4Stars = 510;
                else if (standard4StarsCounter >= 9) w_4Stars = 510 + (standard4StarsCounter - 8) * 5100;
                w_3Stars = 9430;
                break;
        }
        int pullResult = Random.Range(1, Mathf.Min(w_5Stars + w_4Stars + w_3Stars + 1, weightMax + 1));//should plus 1 because this value is exclusive
        if (pullResult <= w_5Stars)
        {
            total5StarsCounter += 1;
            return 5;//5бя
        }
        else if (pullResult <= w_5Stars + w_4Stars)
        {
            total4StarsCounter += 1;
            return 4;//4бя
        }
        else
        {
            total3StarsCounter += 1;
            return 3;//3бя
        } 
    }

    public void _Init()
    {
        totalWishesCounter = 0;
        standardPoolCounter = 0;
        eventPoolCounter = 0;
        weaponPoolCounter = 0;
        total5StarsCounter = 0;
        total4StarsCounter = 0;
        total3StarsCounter = 0;
        event5StarsCounter = 0;
        event4StarsCounter = 0;
        weapon5StarsCounter = 0;
        weapon4StarsCounter = 0;
        standard5StarsCounter = 0;
        standard4StarsCounter = 0;
        standard5StarsNoCharacterCounter=0;
        standard5StarsNoWeaponCounter=0;
        standard4StarsNoCharacterCounter=0;
        standard4StarsNoWeaponCounter=0;
        event4StarsNoCharacterCounter=0;
        event4StarsNoWeaponCounter=0;
        weapon4StarsNoCharacterCounter=0;
        weapon4StarsNoWeaponCounter=0;
        isNextGuaranteed5Stars = false;
        isNextGuaranteed4Stars=false;
        isNextGuaranteedWeapon = false;
        selectedWeapon = 0;
        selectedWeaponCounter = 0;
    }
}
