using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 30f;

    public float damage = 1;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * speed);
    }
}
