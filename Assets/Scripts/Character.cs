using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb2D;
    [SerializeField] protected Transform body;
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float staminaRecoveryRate = 5f;
    [SerializeField] protected float ImpactMultiplier = 3f;

    public float health { get; protected set; }
    public float maxHealth = 100f;
    public float stamina;
    public float maxStamina = 100f;

    protected Vector2 moveInput = new Vector2(-1, 0);

    protected Vector2 Impact;
    protected bool attacked;
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

        }
    }

    public void Attacked(float damage, Vector2 knockBackImpact)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        FindObjectOfType<GameManager>().UpdateCharacterHealth();

        Impact += knockBackImpact* ImpactMultiplier;
        if (health <= 0)
        {
            Destroy(this.gameObject);
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
    IEnumerator AttackedCoroutine(float stunTime)
    {
        attacked = true;
        //GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(stunTime);
        //GetComponentInChildren<SpriteRenderer>().color = Color.white;
        attacked = false;
    }

}
