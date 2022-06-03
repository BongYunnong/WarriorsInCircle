using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Vector2 lastInput = new Vector2(-1, 0);

    protected override void Update()
    {
        base.Update();
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        moveInput = new Vector2(hor, ver).normalized;
        if (hor != 0f || ver != 0f)
        {
            lastInput = moveInput;
            if (hor > 0)
                body.transform.localScale = new Vector3(1,1,1);
            else
                body.transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetFloat("Vel", moveInput.magnitude * speed);
        rb2D.velocity = moveInput * (speed) + Impact;
    }
}
