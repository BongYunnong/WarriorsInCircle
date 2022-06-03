using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    private Vector3 CalculateMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 spawnPos = Vector3.zero;

        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(mousePos);
        spawnPos = new Vector3(screenPoint.x, screenPoint.y, 0f);
        return spawnPos;
    }
    protected override void Update()
    {
        if (GameManager.GetInstance().GameEnded)
        {
            return;
        }
        base.Update();

        Vector3 dir = CalculateMousePos();
        dir.z = 0f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Owner!=null && Owner.stamina>= attackCost && currOverhittingTime <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[0].position, Quaternion.identity);
                Owner.stamina -= attackCost;
                currBullet.GetComponent<Bullet>().InitializeBullet((dir - this.transform.position).normalized, true, 0, bulletColors[0], bulletColors[1]);
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameObject currBullet = Instantiate(BulletPrefab, bulletSpawnTRs[1].position, Quaternion.identity);
                Owner.stamina -= attackCost;
                currBullet.GetComponent<Bullet>().InitializeBullet((dir - this.transform.position).normalized, false, 0, bulletColors[0], bulletColors[1]);
            }
            if (Owner.stamina < attackCost)
            {
                currOverhittingTime = overHittingTime;
            }
        }
    }
}
