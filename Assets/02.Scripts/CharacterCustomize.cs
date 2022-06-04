using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomize : MonoBehaviour
{
    public bool isPlayer;
    public enum CustomType
    {
        BackHair,
        Hair,
        Head,
        Face,
        Hat,
        Accessory,
        Robe,
        UpperBody,
        HandStuff,
        LowerBody,
        BackStuff
    }

    [SerializeField] bool randomCustom;

    [SerializeField] Sprite[] hairSprites;
    [SerializeField] Sprite[] backHairSprites;
    [SerializeField] Sprite[] emotionSprites;
    [SerializeField] Sprite[] accessoriesSprites;

    [SerializeField] SpriteRenderer BackHairSR;
    [SerializeField] SpriteRenderer HairSR;
    [SerializeField] SpriteRenderer HeadSR;
    [SerializeField] SpriteRenderer FaceSR;

    [SerializeField] SpriteRenderer HatSR;
    [SerializeField] SpriteRenderer BackHatSR;
    [SerializeField] SpriteRenderer AccessorySR;

    [SerializeField] SpriteRenderer RobeSR;
    [SerializeField] SpriteRenderer RobeLeftArmSR;
    [SerializeField] SpriteRenderer RobeRightArmSR;

    [SerializeField] SpriteRenderer UpperBodySR;
    [SerializeField] SpriteRenderer LeftArmSR;
    [SerializeField] SpriteRenderer LeftHandSR;
    [SerializeField] SpriteRenderer LeftHandStuffSR;

    [SerializeField] SpriteRenderer RightArmSR;
    [SerializeField] SpriteRenderer RightHandSR;
    [SerializeField] SpriteRenderer RightHandStuffSR;

    [SerializeField] SpriteRenderer LowerBodySR;
    [SerializeField] SpriteRenderer LeftLegSR;
    [SerializeField] SpriteRenderer RightLegSR;

    [SerializeField] SpriteRenderer BackStuffSR;

    [SerializeField] int myFaceSprite;
    Coroutine myEmotionCoroutine;

    [SerializeField] AudioSource fooStepAudioSource;
    [SerializeField] AudioClip[] footStepAudioClips;

    private Color[] originColors = new Color[11];

    private void Start()
    {
        if (randomCustom)
        {
            RandomizeCharacterCustom();
        }
        else if(isPlayer)
        {
            Color hairColor = GameData.hairColor;
            SetSprite(CustomType.Hair, GameData.customIndex[(int)CustomType.Hair], hairColor);
            hairColor -= Color.white * 0.1f;
            hairColor.a = 1;
            SetSprite(CustomType.BackHair, GameData.customIndex[(int)CustomType.BackHair], hairColor);

            SetSprite(CustomType.Accessory, GameData.customIndex[(int)CustomType.Accessory], Color.white);
        }
        ResetOriginColor();
    }

    public void RandomizeCharacterCustom()
    {
        Color hairColor = new Color(Random.value, Random.value, Random.value);
        int hairIndex = Random.Range(0, hairSprites.Length);
        SetSprite(CustomType.Hair, hairIndex, hairColor);
        hairColor -= Color.white * 0.1f;
        hairColor.a = 1;
        int backHairIndex = Random.Range(0, backHairSprites.Length);
        SetSprite(CustomType.BackHair, backHairIndex, hairColor);

        int accessoryIndex = Random.Range(0, accessoriesSprites.Length);
        SetSprite(CustomType.Accessory, accessoryIndex, Color.white);

        if (isPlayer)
        {
            GameData.hairColor = hairColor;
            GameData.customIndex[(int)CustomType.Hair] = hairIndex;
            GameData.customIndex[(int)CustomType.BackHair] = backHairIndex;
            GameData.customIndex[(int)CustomType.Accessory] = accessoryIndex;
        }
    }


    public void SetSprite(CustomType customType,int _index, Color _color)
    {
        switch (customType)
        {
            case CustomType.Hair:
                if (_index >= 0)
                    HairSR.sprite = hairSprites[_index];
                else
                    HairSR.sprite = hairSprites[0];
                HairSR.color = _color;
                break;
            case CustomType.BackHair:
                if (_index >= 0)
                    BackHairSR.sprite = backHairSprites[_index];
                else
                    BackHairSR.sprite = backHairSprites[0];
                BackHairSR.color = _color;
                break;
            case CustomType.Face:
                if (_index >= 0)
                    FaceSR.sprite = emotionSprites[_index];
                else
                    FaceSR.sprite = emotionSprites[0];
                FaceSR.color = Color.white;
                break;
            case CustomType.Accessory:
                if (_index >= 0)
                    AccessorySR.sprite = accessoriesSprites[_index];
                else
                    AccessorySR.sprite = accessoriesSprites[0];
                AccessorySR.color = Color.white;
                break;
        }
    }

    public void SetEmotionFace(int emoIndex)
    {
        print("SetEmotionf");
        if (myEmotionCoroutine != null)
            StopCoroutine(myEmotionCoroutine);
        SetSprite(CustomType.Face,emoIndex, Color.white);
        myEmotionCoroutine = StartCoroutine(EmotionCoroutine());
    }
    public void ForceEmotionFace(int emoIndex)
    {
        if (myEmotionCoroutine != null)
            StopCoroutine(myEmotionCoroutine);
        SetSprite(CustomType.Face,emoIndex, Color.white);
    }

    IEnumerator EmotionCoroutine()
    {
        //Sprite _curFace = myFaceSprite;
        yield return new WaitForSeconds(3f);
        SetSprite(CustomType.Face,myFaceSprite, Color.white);
    }

    public void PlayFootStep()
    {
        fooStepAudioSource.clip = footStepAudioClips[Random.Range(0, footStepAudioClips.Length)];
        fooStepAudioSource.Play();
    }


    public void ResetColor()
    {
        BackHairSR.color = originColors[(int)CustomType.BackHair];
        HairSR.color = originColors[(int)CustomType.Hair];
        HeadSR.color = originColors[(int)CustomType.Head];
        FaceSR.sprite = emotionSprites[myFaceSprite];
        FaceSR.color = originColors[(int)CustomType.Face];

        HatSR.color = originColors[(int)CustomType.Hat];
        BackHatSR.color = originColors[(int)CustomType.Hat];
        AccessorySR.color = originColors[(int)CustomType.Accessory];

        RobeSR.color = originColors[(int)CustomType.Robe];
        RobeLeftArmSR.color = originColors[(int)CustomType.Robe];
        RobeRightArmSR.color = originColors[(int)CustomType.Robe];

        UpperBodySR.color = originColors[(int)CustomType.UpperBody];
        LeftArmSR.color = originColors[(int)CustomType.UpperBody];
        LeftHandSR.color = originColors[(int)CustomType.UpperBody];
        LeftHandStuffSR.color = originColors[(int)CustomType.HandStuff];

        RightArmSR.color = originColors[(int)CustomType.UpperBody];
        RightHandSR.color = originColors[(int)CustomType.UpperBody];
        RightHandStuffSR.color = originColors[(int)CustomType.HandStuff];

        LowerBodySR.color = originColors[(int)CustomType.LowerBody];
        LeftLegSR.color = originColors[(int)CustomType.LowerBody];
        RightLegSR.color = originColors[(int)CustomType.LowerBody];

        BackStuffSR.color = originColors[(int)CustomType.BackStuff];
    }
    public void ResetOriginColor()
    {
        originColors[(int)CustomType.BackHair]= BackHairSR.color;
        originColors[(int)CustomType.Hair]= HairSR.color;
        originColors[(int)CustomType.Head]= HeadSR.color;
        originColors[(int)CustomType.Face] = FaceSR.color;
        originColors[(int)CustomType.Hat]= HatSR.color;
        originColors[(int)CustomType.Accessory] = AccessorySR.color;
        originColors[(int)CustomType.Robe]= RobeSR.color;
        originColors[(int)CustomType.UpperBody]= UpperBodySR.color;
        originColors[(int)CustomType.HandStuff]= LeftHandStuffSR.color;
        originColors[(int)CustomType.LowerBody]= LowerBodySR.color;
        originColors[(int)CustomType.BackStuff]= BackStuffSR.color;
    }

    public void SetTemporalSpriteColor(Color targetColor, float speed)
    {
        BackHairSR.color = Color.Lerp(BackHairSR.color, targetColor, speed * Time.deltaTime);
        HairSR.color = Color.Lerp(HairSR.color, targetColor, speed * Time.deltaTime);
        HeadSR.color = Color.Lerp(HeadSR.color, targetColor, speed * Time.deltaTime);
        FaceSR.color = Color.Lerp(FaceSR.color, targetColor, speed * Time.deltaTime);

        HatSR.color = Color.Lerp(HatSR.color, targetColor, speed * Time.deltaTime);
        BackHatSR.color = Color.Lerp(BackHatSR.color, targetColor, speed * Time.deltaTime);
        AccessorySR.color = Color.Lerp(AccessorySR.color, targetColor, speed * Time.deltaTime);

        RobeSR.color = Color.Lerp(RobeSR.color, targetColor, speed * Time.deltaTime);
        RobeLeftArmSR.color = Color.Lerp(RobeLeftArmSR.color, targetColor, speed * Time.deltaTime);
        RobeRightArmSR.color = Color.Lerp(RobeRightArmSR.color, targetColor, speed * Time.deltaTime);

        UpperBodySR.color = Color.Lerp(UpperBodySR.color, targetColor, speed * Time.deltaTime);
        LeftArmSR.color = Color.Lerp(LeftArmSR.color, targetColor, speed * Time.deltaTime);
        LeftHandSR.color = Color.Lerp(LeftHandSR.color, targetColor, speed * Time.deltaTime);
        LeftHandStuffSR.color = Color.Lerp(LeftHandStuffSR.color, targetColor, speed * Time.deltaTime);
        RightArmSR.color = Color.Lerp(RightArmSR.color, targetColor, speed * Time.deltaTime);
        RightHandSR.color = Color.Lerp(RightHandSR.color, targetColor, speed * Time.deltaTime);
        RightHandStuffSR.color = Color.Lerp(RightHandStuffSR.color, targetColor, speed * Time.deltaTime);

        LowerBodySR.color = Color.Lerp(LowerBodySR.color, targetColor, speed * Time.deltaTime);
        LeftLegSR.color = Color.Lerp(LeftLegSR.color, targetColor, speed * Time.deltaTime);
        RightLegSR.color = Color.Lerp(RightLegSR.color, targetColor, speed * Time.deltaTime);

        BackStuffSR.color = Color.Lerp(BackStuffSR.color, targetColor, speed * Time.deltaTime);
    }
}
