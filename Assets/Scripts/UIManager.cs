using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance 
    {
        get { return instance; }
    }

    public GameObject onePullResultPanel;
    public GameObject tenPullsResultPanel;
    public Card onePullResult;
    public List<Card> tenPullsResult;
    public Text spentText;//Acquaint Fates spent amount 
    public Texture2D cursorSprite;
    public Dropdown poolSelect;
    public Image bannerImg;

    public VideoPlayer vp;
    public VideoPlayer ap;
    public GameObject animCanvas;
    public List<VideoClip> animations;//as the same order of animation in model.cs

    public bool isTenWishes;
    public List<Text> stats;

    private void Awake()
    {
        instance = this;
    }

    
    void Start()
    {
        _Init();

    }

    // Update is called once per frame
    void Update()
    {
        spentText.text = "Event:"+Gacha.Instance.eventPoolCounter+/*" Weapon:"+ Gacha.Instance.weaponPoolCounter+*/" Standard:"+Gacha.Instance.standardPoolCounter;
        stats[0].text = "TotalWishes: " + Gacha.Instance.totalWishesCounter;
        stats[1].text = "EventWishes: " + Gacha.Instance.eventPoolCounter;
        stats[2].text = "WeaponWishes: " + Gacha.Instance.weaponPoolCounter;
        stats[3].text = "StandardWishes: " + Gacha.Instance.standardPoolCounter;
        stats[4].text = "5°ÔAmount: " + Gacha.Instance.total5StarsCounter;
        stats[5].text = "4°ÔAmount: " + Gacha.Instance.total4StarsCounter;
        stats[6].text = "3°ÔAmount: " + Gacha.Instance.total3StarsCounter;
        stats[7].text = "Event5°ÔGuaranteeCounter: " + Gacha.Instance.event5StarsCounter;
        stats[8].text = "Event4°ÔGuaranteeCounter: " + Gacha.Instance.event4StarsCounter;
        stats[9].text = "Weapon5°ÔGuaranteeCounter: " + Gacha.Instance.weapon5StarsCounter;
        stats[10].text = "Weapon4°ÔGuaranteeCounter: " + Gacha.Instance.weapon4StarsCounter;
        stats[11].text = "Standard5°ÔGuaranteeCounter: " + Gacha.Instance.standard5StarsCounter;
        stats[12].text = "Standard4°ÔGuaranteeCounter: " + Gacha.Instance.standard4StarsCounter;
        stats[13].text = "Event4°ÔNoCharacterCounter: " + Gacha.Instance.event4StarsNoCharacterCounter;
        stats[14].text = "Event4°ÔNoWeaponCounter: " + Gacha.Instance.event4StarsNoWeaponCounter;
        stats[15].text = "Weapon4°ÔNoCharacterCounter: " + Gacha.Instance.weapon4StarsNoCharacterCounter;
        stats[16].text = "Weapon4°ÔNoWeaponCounter: " + Gacha.Instance.weapon4StarsNoWeaponCounter;
        stats[17].text = "Standard5°ÔNoCharacterCounter: " + Gacha.Instance.standard5StarsNoCharacterCounter;
        stats[18].text = "Standard5°ÔNoWeaponCounter: " + Gacha.Instance.standard5StarsNoWeaponCounter;
        stats[19].text = "Standard4°ÔNoCharacterCounter: " + Gacha.Instance.standard4StarsNoCharacterCounter;
        stats[20].text = "Standard4°ÔNoWeaponCounter: " + Gacha.Instance.standard4StarsNoWeaponCounter;
        stats[21].text = "Current5°ÔWeight: " + Gacha.Instance.w_5Stars;
        stats[22].text = "Current4°ÔWeight: " + Gacha.Instance.w_4Stars;
        stats[23].text = "Current3°ÔWeight: " + Gacha.Instance.w_3Stars;
    }

    public void ShowResult(List<Model.Item> itemList, int highestRarity)
    {
        if (itemList.Count == 1)//1 wish
        {

            if (itemList[0].isCharacter)
            {
                onePullResult.nameText.text = itemList[0].name;
                onePullResult.element.sprite = Resources.Load<Sprite>("Icons/" +"Element_"+ FirstLetterToUpper(itemList[0].element));
                onePullResult.element.enabled = true;
                onePullResult.icon.sprite = Resources.Load<Sprite>("Icons/Characters/" + "Character_" + itemList[0].name.Replace(" ", "_") + "_Thumb");
                onePullResult.rarityBG.sprite = Resources.Load<Sprite>("Icons/" + "Rarity_" + itemList[0].rarity + "_background");
            }
            else//weapon
            {
                onePullResult.nameText.text = itemList[0].name;
                onePullResult.element.enabled = false;
                onePullResult.icon.sprite = Resources.Load<Sprite>("Icons/Weapons/" + "Weapon_" + itemList[0].name.Replace(" ", "_"));
                onePullResult.rarityBG.sprite = Resources.Load<Sprite>("Icons/" + "Rarity_" + itemList[0].rarity + "_background");
            }
            //onePullResultPanel.SetActive(true);
            switch (highestRarity)
            {
                case 3:
                    vp.clip = animations[0];
                    break;
                case 4:
                    vp.clip = animations[1];
                    break;
                case 5:
                    vp.clip = animations[3];
                    break;
            }
            isTenWishes = false;
            PlayWishAnimation();
        }
        else//10 wishes
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].isCharacter)
                {
                    tenPullsResult[i].nameText.text = itemList[i].name;
                    tenPullsResult[i].element.sprite = Resources.Load<Sprite>("Icons/" + "Element_"+ FirstLetterToUpper(itemList[i].element));
                    tenPullsResult[i].element.enabled = true;
                    tenPullsResult[i].icon.sprite = Resources.Load<Sprite>("Icons/Characters/" + "Character_" + itemList[i].name.Replace(" ", "_") + "_Thumb");
                    tenPullsResult[i].rarityBG.sprite = Resources.Load<Sprite>("Icons/" + "Rarity_" + itemList[i].rarity + "_background");
                }
                else
                {
                    tenPullsResult[i].nameText.text = itemList[i].name;
                    tenPullsResult[i].element.enabled = false;
                    tenPullsResult[i].icon.sprite = Resources.Load<Sprite>("Icons/Weapons/" + "Weapon_" + itemList[i].name.Replace(" ", "_"));
                    tenPullsResult[i].rarityBG.sprite = Resources.Load<Sprite>("Icons/" + "Rarity_" + itemList[i].rarity + "_background");
                }
            }
            if (highestRarity == 5)
                vp.clip = animations[4];
            else
                vp.clip = animations[2];
            isTenWishes = true;
            PlayWishAnimation();
            //tenPullsResultPanel.SetActive(true);
        }
    }

    void PlayWishAnimation()
    {
        animCanvas.SetActive(true);
        //if (isTenPulls)
        //{
        //    vp.loopPointReached += StopAnimation;
        //}
        //else
        //{
        //    vp.loopPointReached += StopAnimation;
        //}
        vp.loopPointReached += StopAnimation;
        vp.Play();
        ap.Play();
    }

    public void StopAnimation(VideoPlayer source)
    {
        vp.Stop();
        ap.Stop();
        animCanvas.SetActive(false);
        if (isTenWishes)
        {
            tenPullsResultPanel.gameObject.SetActive(true);
        }
        else
        {
            onePullResultPanel.gameObject.SetActive(true);
        }
    }

    private void Vp_loopPointReached_Single(VideoPlayer source)
    {
        animCanvas.SetActive(false);
        onePullResultPanel.gameObject.SetActive(true);
    }

    private void Vp_loopPointReached_Ten(VideoPlayer source)
    {
        animCanvas.SetActive(false);
        tenPullsResultPanel.gameObject.SetActive(true);
    }

    public static string FirstLetterToUpper(string str)
    {
        str.ToLower();
        char[] charArr = str.ToCharArray();
        charArr[0].ToString().ToUpper();
        return new string(charArr);
    }

    public void SelectBanner()
    {
        foreach (Model.Pool pool in Gacha.Instance.poolsDict.Values)
        {
            Debug.Log(poolSelect.options[poolSelect.value].text.ToString());
            Debug.Log(pool.poolName);
            if (poolSelect.options[poolSelect.value].text.ToString() == pool.poolName)
            {
                Gacha.Instance.selectedPoolIndex = pool.id;
                break;
            }
        }
        string poolName = Gacha.Instance.poolsDict[Gacha.Instance.selectedPoolIndex].poolName;
        bannerImg.sprite = Resources.Load<Sprite>("Icons/Banners/" + "Wish_" + poolName.Replace(" ", "_"));
        bannerImg.SetNativeSize();
    }

    void _Init()
    {
        Cursor.SetCursor(cursorSprite, new Vector2(0, 0), CursorMode.ForceSoftware);

        //poolSelect.AddOptions(new List<Dropdown.OptionData>());
        //Dropdown.OptionData option= new Dropdown.OptionData();
        //option.text = "1";
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        foreach (Model.Pool pool in Gacha.Instance.poolsDict.Values)
        {
            Dropdown.OptionData newOption = new Dropdown.OptionData();
            newOption.text = pool.poolName;
            optionList.Add(newOption);
        }
        poolSelect.AddOptions(optionList);

    }
}
