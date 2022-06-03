using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine("ChangeMoveInputCoroutine");
    }
    protected override void Update()
    {
        base.Update();
        if (moveInput.x != 0f)
        {
            if (moveInput.x > 0)
                body.transform.localScale = new Vector3(1, 1, 1);
            else
                body.transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetFloat("Vel", moveInput.magnitude * speed);
        rb2D.velocity = moveInput * (speed) + Impact;
    }

    IEnumerator ChangeMoveInputCoroutine()
    {
        moveInput = new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f)).normalized;
        yield return new WaitForSeconds(1f);
        StartCoroutine("ChangeMoveInputCoroutine");
    }
}
