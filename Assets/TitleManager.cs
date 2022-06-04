using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] CharacterCustomize characterCustomize;
    [SerializeField] SpriteRenderer[] handSRs;
    private void Start()
    {
        RandomizeCharacterCustom();
        RandomizeHandColor(0);
        RandomizeHandColor(1);
    }
    public void RandomizeCharacterCustom()
    {
        characterCustomize.RandomizeCharacterCustom();
    }
    public void RandomizeHandColor(int _index)
    {
        Color randColor = new Color(Random.value, Random.value, Random.value);
        handSRs[_index].color = randColor;
        GameData.bulletColors[_index] = randColor;
    }
}
