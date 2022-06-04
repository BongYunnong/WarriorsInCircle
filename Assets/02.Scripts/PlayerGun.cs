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

        if (Owner!=null && Owner.stamina>= attackCost && currOverhittingTime <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[0].position, Quaternion.identity);
                Owner.stamina -= attackCost;
                currBullet.GetComponent<Bullet>().InitializeBullet((dir - this.transform.position).normalized, true, Owner.teamIndex, bulletColors[0], bulletColors[1]);
                if(Owner.stamina < attackCost)
                {
                    FindObjectOfType<GameManager>().TriggerNotEnoughStaminaAnim(Owner.teamIndex);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[1].position, Quaternion.identity);
                Owner.stamina -= attackCost;
                currBullet.GetComponent<Bullet>().InitializeBullet((dir - this.transform.position).normalized, false, Owner.teamIndex, bulletColors[0], bulletColors[1]);
                if (Owner.stamina < attackCost)
                {
                    FindObjectOfType<GameManager>().TriggerNotEnoughStaminaAnim(Owner.teamIndex);
                }
            }
            if (Owner.stamina < attackCost)
            {
                FindObjectOfType<GameManager>().TriggerGotStaminaAnim(Owner.teamIndex, false);
                FindObjectOfType<GameManager>().TriggerOverHeatedAnim(Owner.teamIndex);
                currOverhittingTime = overHittingTime;
            }
        }
    }
}
