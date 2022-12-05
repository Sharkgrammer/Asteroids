using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletHandler : MonoBehaviour
{

    float speed;
    public float damage;

    void Start()
    {
        speed = 100.0f;
        damage = 50.0f;
        Destroy(gameObject, 2);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "astroid")
        {
            // TODO Explosion here
            Destroy(gameObject);
        }
    }
}
