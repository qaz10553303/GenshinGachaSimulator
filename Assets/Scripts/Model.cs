using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Model
{
    [System.Serializable]
    public class Pool
    {
        public int id;
        public string poolName;
        public string poolType;
        public List<int> character_5Stars;
        public List<int> character_4Stars;
        public List<int> character_boost5Stars;//Promoted or Featured with a Drop-Rate Boost
        public List<int> character_boost4Stars;
        public List<int> weapon_5Stars;
        public List<int> weapon_4Stars;
        public List<int> weapon_3Stars;
        public List<int> weapon_boost5Stars;
        public List<int> weapon_boost4Stars;
    }


    [System.Serializable]
    public class Item
    {
        public int id;//unique index
        public string name;
        public bool isCharacter;
        public int rarity;
        public string element;
        public string weaponType;
    }


    //------------------------------------------------------//


    public enum PoolType
    {
        EVENT,//character
        WEAPON,
        STANDARD,
    }
    public enum Element
    {
        NONE,
        PYRO,//fire
        HYDRO,//water
        ANEMO,//wind
        ELECTRO,//thunder
        DENDRO,//grass
        CRYO,//ice
        GEO,//stone
    }
    public enum WeaponType
    {
        SWORD,
        CLAYMORE,
        BOW,
        POLEARM,
        CATALYST,
    }


    //------------------------------------------------------//

    public enum Animation
    {
        Single_3,
        Single_4,
        Ten_4,
        Single_5,
        Ten_5
    }

}

