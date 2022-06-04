using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    protected override void Start()
    {
        base.Start();

        bulletColors[0] = GameData.bulletColors[0];
        bulletColors[1] = GameData.bulletColors[1];
    }
    protected override void Update()
    {
        if (GameManager.GetInstance().GameEnded || Owner.health < 0)
        {
            return;
        }
        base.Update();

        Vector3 dir = GameManager.CalculateMousePos();
        dir.z = 0f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Owner!=null && Owner.stamina>= attackCost)
        {
            for(int i = 0; i < 2; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    if (Owner.currOverhittingTime <= 0)
                    {
                        GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[i].position, Quaternion.identity);
                        Owner.stamina -= attackCost;
                        currBullet.GetComponent<Bullet>().InitializeBullet((dir - this.transform.position).normalized, i==0, Owner.teamIndex, bulletColors[0], bulletColors[1]);
                        if (Owner.stamina < attackCost)
                        {
                            FindObjectOfType<GameManager>().TriggerGotStaminaAnim(Owner.teamIndex, false);
                            FindObjectOfType<GameManager>().TriggerOverHeatedAnim(Owner.teamIndex);
                            Owner.currOverhittingTime = Owner.overHittingTime;
                        }
                    }
                    else
                    {
                        FindObjectOfType<GameManager>().TriggerGotStaminaAnim(Owner.teamIndex, false);
                        FindObjectOfType<GameManager>().TriggerOverHeatedAnim(Owner.teamIndex);
                        break;
                    }
                }
            }
        }
    }
}
