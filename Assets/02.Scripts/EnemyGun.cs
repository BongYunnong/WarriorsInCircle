using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    [SerializeField] int weaponIndex;
    [SerializeField] Transform target;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackRapid = 0.1f;
    [SerializeField] float currAttackCoolTime = 0.1f;

    [SerializeField] int attackCount = 0;
    protected override void Start()
    {
        base.Start();
        currAttackCoolTime = attackRapid;

        // GUN
        if (weaponIndex == 1)
        {
            attackRange += 5f;
        }
    }
    public void SetAttackRapid(float _rapid)
    {
        attackRapid = _rapid;
    }

    protected override void Update()
    {
        if (GameManager.GetInstance().GameEnded || Owner.health <= 0 || Owner.GetComponent<Enemy>().isDummy)
        {
            return;
        }
        base.Update();
        currAttackCoolTime -= Time.deltaTime;
        if (Owner != null && Owner.currOverhittingTime <= 0)
        {
            GameManager _gm = GameManager.GetInstance();
            if (Owner.stamina >= attackCost)
            {
                if (currAttackCoolTime <= 0 
                    && Vector3.Distance(target.position, this.transform.position) <= attackRange)
                {
                    Skill targetSkill = target.GetComponentInChildren<Skill>();
                    if (targetSkill)
                    {
                        if (targetSkill.skillType == Skill.SkillType.Invisible 
                            && targetSkill.isActivated)
                        {
                            return;
                        }
                    }

                    
                    if (Owner.skill)
                    {
                        if (Owner.skill.skillType == Skill.SkillType.Invisible 
                            && Owner.skill.isActivated
                            && Vector3.Distance(target.position, this.transform.position) > attackRange-2.0f)
                        {
                            return;
                        }
                    }

                    Vector3 dir = target.position - this.transform.position;
                    dir.z = 0f;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    RaycastHit2D hit = Physics2D.Raycast(this.transform.position, new Vector2(dir.x, dir.y), attackRange, 1 << LayerMask.NameToLayer("Wall"));
                    if (hit)
                    {
                        return;
                    }

                    GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[attackCount % 2].position, Quaternion.identity);
                    currBullet.GetComponent<Bullet>().InitializeBullet(weaponIndex,gunDamage, (dir).normalized, attackCount % 2 == 0, Owner.teamIndex, bulletColors[0], bulletColors[1]);
                    Owner.stamina -= attackCost;
                    if (Owner.stamina < attackCost)
                    {
                        _gm.TriggerGotStaminaAnim(Owner.teamIndex, false);
                        _gm.TriggerOverHeatedAnim(Owner.teamIndex);
                        Owner.currOverhittingTime = Owner.overHittingTime;
                    }
                    attackCount++;
                    currAttackCoolTime = attackRapid;


                    if (Owner.skill)
                        Owner.skill.CharacterAttacked();
                }
            }
            else
            {
                _gm.TriggerGotStaminaAnim(Owner.teamIndex, false);
                _gm.TriggerOverHeatedAnim(Owner.teamIndex);
            }
        }
    }
}
