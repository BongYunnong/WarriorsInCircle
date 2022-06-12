using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public enum SkillType
    {
        Invisible,
        Guard,
        BuildBlock,
        Berserker,
        Haste,
        Heal
    }
    [SerializeField] bool RandomizeSkill;
    public SkillType skillType;
    public bool isActivated { get; protected set; }
    private float activatedTime = 0;
    protected Character Owner;

    private float skillCoolTime=0;
    private float maxSkillCoolTime = 10f;

    [SerializeField] Transform BuildBlockCursor;
    [SerializeField] GameObject BuildBlockPrefab;
    [SerializeField] GameObject HealEffectObj;

    protected virtual void Start()
    {
        Owner = GetComponentInParent<Character>();

        if (RandomizeSkill)
        {
            skillType = (SkillType)Random.Range(0, 6);
        }else if (Owner.teamIndex == 0)
        {
            skillType = (SkillType)GameData.skillIndex;
        }

        switch (skillType)
        {
            case SkillType.Invisible:
                maxSkillCoolTime = 10f;
                break;
            case SkillType.Berserker:
                maxSkillCoolTime = 10f;
                break;
            case SkillType.Guard:
                maxSkillCoolTime = 10f;
                break;
            case SkillType.BuildBlock:
                maxSkillCoolTime = 10f;
                break;
            case SkillType.Haste:
                maxSkillCoolTime = 10f;
                break;
            case SkillType.Heal:
                maxSkillCoolTime = 10f;
                break;
        }
    }

    protected virtual void Update()
    {
        if (GameManager.GetInstance().GameEnded || Owner.health < 0)
        {
            return;
        }

        if (Owner.teamIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && skillCoolTime <= 0)
            {
                SetActiveSkill(true);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {

            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && isActivated)
            {
                SetActiveSkill(false);
            }
        }
        else
        {
            if(Owner.GetComponent<Enemy>().skillInput && skillCoolTime <= 0)
            {
                SetActiveSkill(true);
            }
        }


        if (isActivated)
        {
            activatedTime+=Time.deltaTime;
            switch (skillType)
            {
                case SkillType.Invisible:
                    Skill_Invisible();
                    break;
                case SkillType.Berserker:
                    Skill_Berserker();
                    break;
                case SkillType.Guard:
                    Skill_Guard();
                    break;
                case SkillType.BuildBlock:
                    Skill_Block();
                    break;
                case SkillType.Haste:
                    Skill_Haste();
                    break;
                case SkillType.Heal:
                    Skill_Heal();
                    break;
            }
        }
        else
        {
            skillCoolTime -= Time.deltaTime;
        }
        skillCoolTime = Mathf.Clamp(skillCoolTime, 0, 1000f);
        if (Owner.teamIndex == 0)
        {
            GameManager.GetInstance().UpdateSkillCoolTime(skillCoolTime/ maxSkillCoolTime);
        }
    }

    public void SetActiveSkill(bool _active)
    {
        isActivated = _active;


        if (isActivated == true)
        {
            skillCoolTime = maxSkillCoolTime;

            switch (skillType)
            {
                case SkillType.BuildBlock:
                    BuildBlockCursor.gameObject.SetActive(true);
                    break;
                case SkillType.Heal:
                    HealEffectObj.SetActive(true);
                    break;
            }
        }
        else
        {
            activatedTime = 0;

            if (Owner.teamIndex == 1)
            {
                Owner.GetComponent<Enemy>().skillInput = false;
            }

            switch (skillType)
            {
                case SkillType.Invisible:
                    Owner.characterCustomize.ResetColor();
                    break;
                case SkillType.Berserker:
                    Owner.characterCustomize.ResetColor();
                    Owner.stamina = 0;

                    GameManager _gm = GameManager.GetInstance();
                    _gm.TriggerGotStaminaAnim(Owner.teamIndex, false);
                    _gm.TriggerOverHeatedAnim(Owner.teamIndex);
                    Owner.currOverhittingTime = Owner.overHittingTime;
                    break;
                case SkillType.Guard:
                    Owner.characterCustomize.ResetColor();
                    break;
                case SkillType.BuildBlock:
                    BuildBlockCursor.gameObject.SetActive(false);
                    Transform guideObj = BuildBlockCursor.GetChild(0);
                    Instantiate(BuildBlockPrefab, guideObj.transform.position, guideObj.transform.rotation);
                    break;
                case SkillType.Haste:
                    Owner.characterCustomize.ResetColor();
                    break;
                case SkillType.Heal:
                    HealEffectObj.SetActive(false);
                    break;
            }
        }
    }

    public void CharacterHitted()
    {
        switch (skillType)
        {
            case SkillType.Invisible:
                SetActiveSkill(false);
                break;
        }
    }

    public void CharacterAttacked()
    {
        switch (skillType)
        {
            case SkillType.Invisible:
                SetActiveSkill(false);
                break;
        }
    }



    protected void Skill_Invisible()
    {
        Owner.characterCustomize.SetTemporalSpriteColor(new Color(0f, 0f, 0f, 0f), 5f);
        Owner.stamina -= Time.deltaTime * 10f;
        if (Owner.stamina < Time.deltaTime * 10f)
        {
            GameManager _gm = GameManager.GetInstance();
            _gm.TriggerGotStaminaAnim(Owner.teamIndex, false);
            _gm.TriggerOverHeatedAnim(Owner.teamIndex);
            SetActiveSkill(false);
        }
    }
    protected void Skill_Berserker()
    {
        Owner.characterCustomize.SetTemporalSpriteColor(Color.red, 5f);
        Owner.stamina += Time.deltaTime * 10f;

        if (activatedTime >= 5f)
        {
            SetActiveSkill(false);
        }
    }
    protected void Skill_Haste()
    {
        Owner.characterCustomize.SetTemporalSpriteColor(Color.cyan, 2f);
        Owner.stamina -= Time.deltaTime * 5f;
        if (Owner.stamina < Time.deltaTime * 5f)
        {
            GameManager _gm = GameManager.GetInstance();
            _gm.TriggerGotStaminaAnim(Owner.teamIndex, false);
            _gm.TriggerOverHeatedAnim(Owner.teamIndex);
            SetActiveSkill(false);
        }
    }
    protected void Skill_Guard()
    {
        Owner.characterCustomize.SetTemporalSpriteColor(new Color(0.2f, 0.2f, 0.2f), 2f);
        if (activatedTime >= 5f)
        {
            SetActiveSkill(false);
        }
    }

    protected void Skill_Block()
    {
        Vector3 dir;
        if (Owner.teamIndex == 0)
        {
            dir = GameManager.CalculateMousePos() - this.transform.position;
        }
        else
        {
            dir = FindObjectOfType<Player>().transform.position - this.transform.position;
        }
        dir.z = 0f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        BuildBlockCursor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Owner.teamIndex != 0)
        {
            SetActiveSkill(false);
        }
    }

    protected void Skill_Heal()
    {
        Owner.AddPlayerHealth(Time.deltaTime * 5f);

        if (activatedTime >= 5f)
        {
            SetActiveSkill(false);
        }
    }
}

