using Assets.Scripts.Weapons;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class pickupHandler : MonoBehaviour
{
    Rigidbody2D rbody;
    Vector2 force;
    Bounds bounds;
    public Boolean isWeapon = true;
    public String pickupName;

    public int weaponidxx = 0;

    [SerializeField] GameObject[] weaponObjects;
    [SerializeField] GameObject[] powerups;

    void Start()
    {
        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;

        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        rbody = GetComponent<Rigidbody2D>();
        Animator anim = GetComponent<Animator>();

        int weaponID = Random.Range(0, weaponObjects.Length);
        String currentWeapon = playerControl.Weapon.getName();
        
        while (currentWeapon == weaponObjects[weaponID].name)
        {
            weaponID = Random.Range(0, weaponObjects.Length);
        }

        weaponID = weaponidxx;
        anim.runtimeAnimatorController = weaponObjects[weaponidxx].GetComponent<Animator>().runtimeAnimatorController;
        anim.enabled = true;

        pickupName = weaponObjects[weaponID].name;

        generatePoint();
    }

    void generatePoint()
    {
        float x = bounds.extents.x + 10;
        float y = bounds.extents.y + 10;
        float forceX = Random.Range(0.2f, 0.5f);
        float forceY = Random.Range(0.2f, 0.5f);

        Boolean topBounds = Random.Range(0, 2) == 0;
        int dir = Random.Range(0, 2) == 0 ? -1 : 1;

        if (topBounds)
        {
            y *= dir;
            x = Random.Range(-x, x);

            forceY *= (dir * -1);
            
            if (x >= 0)
            {
                forceX *= -1;
            }
        }
        else
        {
            x *= dir;
            y = Random.Range(-y, y);

            forceX *= (dir * -1);

            if (y >= 0)
            {
                forceY *= -1;
            }
        }

        transform.position = new Vector2(x, y);
        force = new Vector2(forceX, forceY);
    }

    private void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + force);
    
        if (Math.Abs(transform.position.x) > bounds.extents.x + 15)
        {
            Destroy(gameObject);
        }

        if (Math.Abs(transform.position.y) > bounds.extents.x + 15)
        {
            Destroy(gameObject);
        }

    }

}
