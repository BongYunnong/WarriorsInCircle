using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleManager : MonoBehaviour
{
    [System.Serializable]
    public struct SkillInfo
    {
        public string info;
        public Sprite sprite;
    }
    [System.Serializable]
    public struct WeaponInfo
    {
        public string info;
        public Sprite sprite;
    }
    [SerializeField] CharacterCustomize characterCustomize;
    [SerializeField] Image[] handImgs;
    [SerializeField] Image SkillImg;

    [SerializeField] SkillInfo[] skillInfos;
    [SerializeField] WeaponInfo[] weaponInfos;
    [SerializeField] Text skillInfoTxt;
    [SerializeField] Text weaponInfoTxt;
    
    private void Start()
    {
        RandomizeCharacterCustom();
        RandomizeHandColor(0);
        RandomizeHandColor(1);

        AddWeaponIndex(0);
        AddSkillIndex(0);

    }
    public void RandomizeCharacterCustom()
    {
        characterCustomize.RandomizeCharacterCustom();
    }
    public void RandomizeHandColor(int _index)
    {
        Color randColor = new Color(Random.value, Random.value, Random.value);
        handImgs[_index].color = randColor;
        GameData.bulletColors[_index] = randColor;
    }

    public void AddSkillIndex(int _add)
    {
        GameData.skillIndex += _add;
        GameData.skillIndex = Mathf.Clamp(GameData.skillIndex, 0, skillInfos.Length-1);

        SkillImg.sprite = skillInfos[GameData.skillIndex].sprite;
        skillInfoTxt.text = skillInfos[GameData.skillIndex].info;
    }

    public void AddWeaponIndex(int _add)
    {
        GameData.weaponIndex += _add;
        GameData.weaponIndex = Mathf.Clamp(GameData.weaponIndex, 0, weaponInfos.Length-1);
        handImgs[0].sprite = weaponInfos[GameData.weaponIndex].sprite;
        handImgs[1].sprite = weaponInfos[GameData.weaponIndex].sprite;
        handImgs[2].sprite = weaponInfos[GameData.weaponIndex].sprite;
        weaponInfoTxt.text = weaponInfos[GameData.weaponIndex].info;
    }
}
