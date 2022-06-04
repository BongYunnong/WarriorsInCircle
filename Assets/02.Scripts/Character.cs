using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb2D;
    protected CharacterCustomize characterCustomize;
    [SerializeField] protected Transform body;
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float staminaRecoveryRate = 5f;
    [SerializeField] protected float ImpactMultiplier = 3f;
    [SerializeField] private float dodgeForce = 20f;
    [SerializeField] private float dodgeStaminaCost = 10f;

    public int teamIndex;
    public float health { get; protected set; }
    public float maxHealth = 100f;
    public float stamina;
    public float maxStamina = 100f;

    public float currOverhittingTime;
    public float overHittingTime;

    protected Vector2 moveInput = new Vector2(-1, 0);

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

            if(moveInput.magnitude>0.1 || dodged)
            {
                characterCustomize.ResetColor();
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

    public void Attacked(float damage, Vector2 knockBackImpact)
    {
        if (dodged == false && health>0)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            FindObjectOfType<GameManager>().UpdateCharacterHealth();

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
        //GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(stunTime);
        //GetComponentInChildren<SpriteRenderer>().color = Color.white;
        attacked = false;
        characterCustomize.ResetColor();
    }

    public void Dodge(Vector3 _dir)
    {
        if (dodged==false)
        {
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
                    FindObjectOfType<GameManager>().TriggerGotStaminaAnim(teamIndex, false);
                    FindObjectOfType<GameManager>().TriggerOverHeatedAnim(teamIndex);
                    currOverhittingTime = overHittingTime;
                }
            }
            else
            {
                FindObjectOfType<GameManager>().TriggerGotStaminaAnim(teamIndex, false);
                FindObjectOfType<GameManager>().TriggerOverHeatedAnim(teamIndex);
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
