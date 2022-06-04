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


    protected virtual void Start()
    {
        Owner = GetComponentInParent<Character>(); 
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
            FindObjectOfType<GameManager>().TriggerGotStaminaAnim(Owner.teamIndex,true);
        }
    }
}
