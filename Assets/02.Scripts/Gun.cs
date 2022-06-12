using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected Transform[] bulletSpawnTRs;

    [SerializeField] protected Color[] bulletColors;

    protected Character Owner;
    [SerializeField] protected float attackCost;

    public float gunDamage;

    protected virtual void Start()
    {
        Owner = GetComponentInParent<Character>(); 
    }

    public void SetGunDamage(float _damage)
    {
        gunDamage = _damage;
    }

    protected virtual void Update()
    {
        if (GameManager.GetInstance().GameEnded || Owner.health < 0)
        {
            return;
        }
        Owner.currOverhittingTime -= Time.deltaTime;
        if (Owner.currOverhittingTime <= 0)
        {
            GameManager.GetInstance().TriggerGotStaminaAnim(Owner.teamIndex,true);
        }
    }
}
