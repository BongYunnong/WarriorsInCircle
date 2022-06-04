using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    [SerializeField] Transform target;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackRapid = 0.1f;
    [SerializeField] float currAttackCoolTime = 0.1f;

    [SerializeField] int attackCount = 0;
    protected override void Start()
    {
        base.Start();
        currAttackCoolTime = attackRapid;
    }

    protected override void Update()
    {
        if (GameManager.GetInstance().GameEnded || Owner.health < 0)
        {
            return;
        }
        base.Update();
        currAttackCoolTime -= Time.deltaTime;
        if (Owner != null && Owner.currOverhittingTime <= 0)
        {
            if (Owner.stamina >= attackCost)
            {
                if (currAttackCoolTime <= 0 && Vector3.Distance(target.position, this.transform.position) <= attackRange)
                {
                    Vector3 dir = target.position - this.transform.position;
                    dir.z = 0f;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[attackCount % 2].position, Quaternion.identity);
                    currBullet.GetComponent<Bullet>().InitializeBullet((dir).normalized, attackCount % 2 == 0, Owner.teamIndex, bulletColors[0], bulletColors[1]);
                    Owner.stamina -= attackCost;
                    if (Owner.stamina < attackCost)
                    {
                        FindObjectOfType<GameManager>().TriggerGotStaminaAnim(Owner.teamIndex, false);
                        FindObjectOfType<GameManager>().TriggerOverHeatedAnim(Owner.teamIndex);
                        Owner.currOverhittingTime = Owner.overHittingTime;
                    }
                    attackCount++;
                    currAttackCoolTime = attackRapid;
                }
            }
            else
            {
                FindObjectOfType<GameManager>().TriggerGotStaminaAnim(Owner.teamIndex, false);
                FindObjectOfType<GameManager>().TriggerOverHeatedAnim(Owner.teamIndex);
            }
        }
    }
}
