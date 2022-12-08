using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class astroidHandler : MonoBehaviour
{
    public TextMeshProUGUI killCounter;

    Rigidbody2D rbody;
    Vector2 force;
    Bounds bounds;
    int boundsGap = 8;
    float health;

    void Start()
    {
        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;

        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        generatePoint();

        rbody = GetComponent<Rigidbody2D>();

        force = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        rbody.rotation = Random.Range(0.0f, 360.0f);

        health = Random.Range(80, 150);

        int scale = Random.Range(15, 25);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void Update()
    {
        boundsWarp();
    }

    void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + force);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {

            health -= collision.gameObject.GetComponent<bulletHandler>().damage;

            // TODO Explosion here
            if (health <= 0)
            {
                string[] str = killCounter.text.Split(" ");
                int counter = int.Parse(str[1]);

                killCounter.text = str[0] + " " + ++counter;

                Destroy(gameObject);
            }
        }
    }

    void generatePoint()
    {
        float x = bounds.extents.x + boundsGap;
        float y = bounds.extents.y + boundsGap;

        Boolean topBounds = Random.Range(0, 2) == 0;

        if (topBounds)
        {
            y *= Random.Range(0, 2) == 0 ? -1 : 1;
            x = Random.Range(-x, x);
        }
        else
        {
            x *= Random.Range(0, 2) == 0 ? -1 : 1;
            y = Random.Range(-y, y);
        }

        transform.position = new Vector2(x, y);
    }

    void boundsWarp()
    {
        if (Math.Abs(transform.position.x) - boundsGap > bounds.extents.x)
        {
            Vector2 temp = rbody.position;
            temp.x *= -1;
            //force.x *= -1;

            rbody.position = temp;
        }

        if (Math.Abs(transform.position.y) - boundsGap > bounds.extents.y)
        {
            Vector2 temp = rbody.position;
            temp.y *= -1;
            //force.y *= -1;

            rbody.position = temp;
        }
    }

    Boolean withinRange()
    {
        return false;
    }
}
