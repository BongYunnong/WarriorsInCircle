using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] float lifeTime=1f;
    void Start()
    {
        Invoke("DestroyWhenOverTime", lifeTime);
    }

    private void DestroyWhenOverTime()
    {
        Destroy(this.gameObject);
    }
}
