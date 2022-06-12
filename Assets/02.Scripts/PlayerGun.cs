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
                    GameManager _gm = GameManager.GetInstance();
                    if (Owner.currOverhittingTime <= 0)
                    {
                        _gm.CameraConcentrate(0.1f);

                        GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[i].position, Quaternion.identity);
                        Owner.stamina -= attackCost;
                        currBullet.GetComponent<Bullet>().InitializeBullet(gunDamage,(dir - this.transform.position).normalized, i==0, Owner.teamIndex, bulletColors[0], bulletColors[1]);
                        if (Owner.stamina < attackCost)
                        {
                            _gm.TriggerGotStaminaAnim(Owner.teamIndex, false);
                            _gm.TriggerOverHeatedAnim(Owner.teamIndex);
                            Owner.currOverhittingTime = Owner.overHittingTime;
                        }

                        if (Owner.skill)
                            Owner.skill.CharacterAttacked();
                    }
                    else
                    {
                        _gm.TriggerGotStaminaAnim(Owner.teamIndex, false);
                        _gm.TriggerOverHeatedAnim(Owner.teamIndex);
                        break;
                    }
                }
            }
        }
    }
}
