using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private float dodgeProbability;
    protected override void Start()
    {
        base.Start();
        StartCoroutine("ChangeMoveInputCoroutine");
        StartCoroutine("ChangeDodgeInputCoroutine");
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
                rb2D.velocity = moveInput * (speed) + Impact;
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
        moveInput = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        yield return new WaitForSeconds(1f);
        StartCoroutine("ChangeMoveInputCoroutine");
    }
    IEnumerator ChangeDodgeInputCoroutine()
    {
        dodgeProbability = Random.value;
        yield return new WaitForSeconds(1f);
        StartCoroutine("ChangeDodgeInputCoroutine");
    }
}
