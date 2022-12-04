using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet1 : MonoBehaviour
{

    float speed;

    void Start()
    {
        speed = 100.0f;

        Destroy(gameObject, 2);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y >= 300)
        {
            if (this.gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter()
    {
        //Instantiate(explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
