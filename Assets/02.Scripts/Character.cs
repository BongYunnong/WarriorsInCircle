using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb2D;
    public Skill skill { get; protected set; }
    public CharacterCustomize characterCustomize { get; protected set; }
    [SerializeField] protected Transform body;
    [SerializeField] protected float speed = 3f;
    protected float speedMultiplier=1f;
    [SerializeField] private float staminaRecoveryRate = 5f;
    [SerializeField] protected float ImpactMultiplier = 3f;
    [SerializeField] private float dodgeForce = 20f;
    [SerializeField] private float dodgeStaminaCost = 3f;

    public int teamIndex;
    public float health { get; protected set; }
    public float maxHealth = 100f;
    public float stamina;
    public float maxStamina = 100f;

    public float currOverhittingTime;
    public float overHittingTime;

    protected Vector2 moveInput = new Vector2(0, 0);

    protected Vector2 Impact;
    protected bool attacked;
    protected bool dodged;
    class AttackedInfo
    {
        public AttackedInfo(float _coolTime = 1f)
        {
            coolTime = _coolTime;
        }
        public float coolTime;
    }
    private List<AttackedInfo> attackedList = new List<AttackedInfo>();

    protected virtual void Start()
    {
        anim = body.GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        skill = GetComponentInChildren<Skill>();
        characterCustomize = GetComponentInChildren<CharacterCustomize>();

        attacked = false;

        health = maxHealth;
        stamina = maxStamina;
    }

    protected virtual void Update()
    {
        if (health > 0)
        {
            health = Mathf.Clamp(health, 0, maxHealth);

            stamina += staminaRecoveryRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);

            Impact = Vector3.Lerp(Impact, Vector3.zero, Time.deltaTime*5f);

            for (int i = 0; i < attackedList.Count; i++)
            {
                attackedList[i].coolTime -= Time.deltaTime;
                if (attackedList[i].coolTime <= 0)
                {
                    attackedList.RemoveAt(i);
                    break;
                }
            }

            if (attacked)
            {
                characterCustomize.SetTemporalSpriteColor(Color.red, 5f);
            }

            // moveInput.magnitude>0.1 || 이거 왜 넣었지..?
            if (dodged)
            {
                characterCustomize.ResetColor();
            }

            speedMultiplier = 1f;
            if (skill && skill.isActivated)
            {
                if (skill.skillType == Skill.SkillType.Guard)
                    speedMultiplier += -0.5f;
                else if (skill.skillType == Skill.SkillType.Invisible)
                    speedMultiplier += -0.1f;
                else if (skill.skillType == Skill.SkillType.Haste)
                    speedMultiplier += 0.5f;
            }

        }
        else
        {
            characterCustomize.SetTemporalSpriteColor(Color.black, 2f);
            Impact = Vector2.zero;
            moveInput = Vector2.zero;
            rb2D.velocity = Vector2.zero;
        }
    }
    public void AddPlayerHealth(float _add)
    {
        health += _add;
        health = Mathf.Clamp(health, 0, maxHealth);
        GameManager.GetInstance().UpdateCharacterHealth();
    }
    public void Attacked(float damage, Vector2 knockBackImpact)
    {
        if (dodged == false && health>0)
        {
            if (skill)
                if (skill.skillType == Skill.SkillType.Guard 
                    && skill.isActivated)
                    damage *= 0.25f;

            AddPlayerHealth(-damage);

            if(skill)
                skill.CharacterHitted();

            Impact += knockBackImpact * ImpactMultiplier;
            if (health <= 0)
            {
                anim.SetTrigger("Die");
            }
            else
            {
                if (damage > 0)
                {
                    StopCoroutine("AttackedCoroutine");
                    StartCoroutine("AttackedCoroutine", 0.1f);
                }
            }
        }
    }
    IEnumerator AttackedCoroutine(float stunTime)
    {
        attacked = true;
        yield return new WaitForSeconds(stunTime);
        attacked = false;
        characterCustomize.ResetColor();
    }

    public void Dodge(Vector3 _dir)
    {
        if (dodged==false)
        {
            GameManager _gm = GameManager.GetInstance();
            if (currOverhittingTime <= 0)
            {
                if (stamina >= dodgeStaminaCost)
                {
                    stamina -= dodgeStaminaCost;

                    Impact = _dir * dodgeForce;
                    StopCoroutine("DodgeCoroutine");
                    StartCoroutine("DodgeCoroutine", 0.5f);
                }
                else
                {
                    _gm.TriggerGotStaminaAnim(teamIndex, false);
                    _gm.TriggerOverHeatedAnim(teamIndex);
                    currOverhittingTime = overHittingTime;
                }
            }
            else
            {
                _gm.TriggerGotStaminaAnim(teamIndex, false);
                _gm.TriggerOverHeatedAnim(teamIndex);
            }
        }
    }

    IEnumerator DodgeCoroutine(float dodgeTime)
    {
        dodged = true;
        attacked = false;
        anim.SetTrigger("Dodge");
        characterCustomize.GetComponent<GhostTrail>().StartMotionTrail();
        characterCustomize.ResetColor();
        yield return new WaitForSeconds(dodgeTime);
        characterCustomize.GetComponent<GhostTrail>().EndMotionTrail();
        dodged = false;
    }
}
