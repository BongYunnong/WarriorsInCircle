using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public enum AIMode
    {
        Opensive,
        Defensive,
        Idle,
    }
    public AIMode aiMode;

    private float dodgeProbability;
    public bool skillInput;
    protected override void Start()
    {
        GameManager _gm = GameManager.GetInstance();
        maxHealth = Random.Range(_gm.enemyHealthMixMax.x, _gm.enemyHealthMixMax.y);
        maxStamina = Random.Range(_gm.enemyStaminaMinMax.x, _gm.enemyStaminaMinMax.y);
        speed = Random.Range(_gm.enemySpeedMinMax.x, _gm.enemySpeedMinMax.y);

        EnemyGun enemeyGun= GetComponentInChildren<EnemyGun>();
        enemeyGun.SetGunDamage(Random.Range(_gm.enemyDamageMinMax.x, _gm.enemyDamageMinMax.y));
        enemeyGun.SetAttackRapid(Random.Range(_gm.enemyAttackRateMinMax.x, _gm.enemyAttackRateMinMax.y));

        base.Start();

        StartCoroutine("ChangeMoveInputCoroutine");
        StartCoroutine("ChangeDodgeInputCoroutine");
        StartCoroutine("ChangeSkillInputCoroutine");
        StartCoroutine("ChangeAIModeCoroutine");
    }
    
    protected override void Update()
    {
        base.Update();

        if (health > 0)
        {
            if (attacked)
            {
                if (dodgeProbability <= 0.3f)
                {
                    Dodge(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
                }
            }
            else if (dodgeProbability < 0.05f)
            {
                Dodge(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
            }

            anim.SetFloat("Vel", moveInput.magnitude * speed);
            if (dodged)
            {
                rb2D.velocity = Impact;
            }
            else
            {
                rb2D.velocity = moveInput * (speed* speedMultiplier) + Impact;
            }

            RaycastHit2D hit= Physics2D.Raycast(this.transform.position, new Vector2(moveInput.x, moveInput.y), 1, 1 << LayerMask.NameToLayer("Wall"));
            if (hit)
            {
                Debug.DrawRay(transform.position, new Vector3(moveInput.x, moveInput.y).normalized * 1f, Color.green);
                if (Random.Range(0f, 1f) < 0.5f)
                    moveInput = new Vector3(moveInput.x, -moveInput.y);
                else
                    moveInput = new Vector3(-moveInput.x, moveInput.y);
            }

            if (rb2D.velocity.x != 0f)
            {
                if (rb2D.velocity.x > 0)
                    body.transform.localScale = new Vector3(1, 1, 1);
                else
                    body.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    IEnumerator ChangeMoveInputCoroutine()
    {
        Vector3 targetPos = FindObjectOfType<Player>().transform.position;
        switch (aiMode)
        {
            case AIMode.Opensive:
                if (Vector3.Distance(targetPos, this.transform.position) > 2f)
                    moveInput = (targetPos - this.transform.position).normalized;
                else
                    aiMode = (AIMode)Random.Range(0, 3);
                break;
            case AIMode.Defensive:
                if (Vector3.Distance(targetPos, this.transform.position) < 3f)
                    moveInput = (-targetPos + this.transform.position).normalized;
                else
                    aiMode = (AIMode)Random.Range(0, 3);
                break;
            case AIMode.Idle:
                moveInput = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                break;
        }
        yield return new WaitForSeconds(Random.Range(1f,2f));
        StartCoroutine("ChangeMoveInputCoroutine");
    }
    IEnumerator ChangeAIModeCoroutine()
    {
        Player player = FindObjectOfType<Player>();
        if (skill.skillType == Skill.SkillType.Invisible
            || skill.skillType == Skill.SkillType.Haste
            || skill.skillType == Skill.SkillType.Guard)
        {
            if(health>= player.health && stamina>= player.stamina)
            {
                aiMode = AIMode.Opensive; 
            }else if (health < player.health && stamina < player.stamina)
            {
                aiMode = AIMode.Defensive;
            }
        }
        else if (skill.skillType == Skill.SkillType.Berserker)
        {
            aiMode = AIMode.Opensive;
        }
        else if(currOverhittingTime>0)
        {
            aiMode = AIMode.Defensive;
        }
        else
        {
            aiMode = (AIMode)Random.Range(0, 3);
        }

        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine("ChangeAIModeCoroutine");
    }
    IEnumerator ChangeDodgeInputCoroutine()
    {
        dodgeProbability = Random.value;
        yield return new WaitForSeconds(1f);
        StartCoroutine("ChangeDodgeInputCoroutine");
    }
    IEnumerator ChangeSkillInputCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(1f,10f));
        skillInput = true;
        StartCoroutine("ChangeSkillInputCoroutine");
    }
}
