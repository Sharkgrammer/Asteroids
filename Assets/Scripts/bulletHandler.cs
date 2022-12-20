using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletHandler : MonoBehaviour
{

    private Weapon weapon;

    public void setBulletData(Weapon weapon)
    {
        this.weapon = weapon;
    }

    void scaleBullet()
    {
        if (weapon.getScaleAdj() != 0)
        {
            float scale = transform.localScale.x + weapon.getScaleAdj();

            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void Start()
    {
        scaleBullet();

        Destroy(gameObject, weapon.getLifetime());
    }

    void Update()
    {
        Vector3 direction = Vector3.up;
        int modifier = 0;

        if (weapon.getName() == "Sword")
        {
            // Code for Sword. The sword should retract itself if the player isn't shooting
            if (!(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse1)))
            {
                direction = Vector3.down;

                if (transform.localPosition.y <= 0)
                {
                    direction = new Vector3(0, 0, 0);
                }
                else
                {
                    modifier = 25;
                }
            }
        }

        transform.Translate(direction * (weapon.getSpeed() + modifier) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (weapon.getName() == "Pew")
        {
            float scale = transform.localScale.x;

            scale += 0.2f;

            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "asteroid")
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
