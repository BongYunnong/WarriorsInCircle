using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Vector2 lastInput = new Vector2(-1, 0);

    protected override void Update()
    {
        base.Update();
        if (health > 0)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            moveInput = new Vector2(hor, ver).normalized;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dodge((GameManager.CalculateMousePos() - this.transform.position).normalized);
            }

            anim.SetFloat("Vel", moveInput.magnitude * speed);
            if (dodged)
            {
                rb2D.velocity = Impact;
            }
            else
            {
                rb2D.velocity = moveInput * (speed* speedMultiplier) + Impact;
            }


            if (Mathf.Abs(rb2D.velocity.x) >0.1f)
            {
                lastInput = moveInput;
                if (rb2D.velocity.x > 0)
                    body.transform.localScale = new Vector3(1, 1, 1);
                else
                    body.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                if (lastInput.x > 0)
                    body.transform.localScale = new Vector3(1, 1, 1);
                else
                    body.transform.localScale = new Vector3(-1, 1, 1);
            }
            EmotionFunction();
        }
    }

    private void EmotionFunction()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                SetEmotion(i);
            }
        }
    }
    public void SetEmotion(int _index)
    {
        anim.SetInteger("EmotionIndex", _index);
        anim.SetTrigger("EmotionTrigger");
    }
}
