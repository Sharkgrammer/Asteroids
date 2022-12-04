using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class astroidHandler : MonoBehaviour
{

    Rigidbody2D rbody;
    Vector2 force;
    Bounds bounds;
    int boundsGap = 8;
    int health = 3;

    void Start()
    {   
        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;

        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        rbody = GetComponent<Rigidbody2D>();

        force = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        rbody.rotation = Random.Range(0.0f, 360.0f);

        health = Random.Range(2, 5);

        generatePoint();
    }

    private void Update()
    {
        boundsWarp();
    }

    void FixedUpdate()
    {
        rbody.AddForce(force);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            // TODO Explosion here
            if (health <= 0)
            {
                Destroy(gameObject);
            }
            health--;
        }
    }

    void generatePoint()
    {
        float x = bounds.extents.x + boundsGap;
        float y = bounds.extents.y + boundsGap;

        Boolean topBounds = Random.Range(0, 1) == 0;

        if (topBounds)
        {
            y *= Random.Range(0, 1) == 0 ? -1 : 1;
            x = Random.Range(-x, x);
        }
        else
        {
            x *= Random.Range(0, 1) == 0 ? -1 : 1;
            y = Random.Range(-y, y);
        }

        transform.position = new Vector2(x, y);
    }

    void boundsWarp()
    {
        if (Math.Abs(transform.position.x) - boundsGap > bounds.extents.x)
        {
            Vector3 temp = transform.position;
            temp.x = temp.x * -1;

            transform.position = temp;
        }

        if (Math.Abs(transform.position.y) - boundsGap > bounds.extents.y)
        {
            Vector3 temp = transform.position;
            temp.y = temp.y * -1;

            transform.position = temp;
        }
    }
}
