using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class playerControl : MonoBehaviour
{

    bool mouseMode = false;
    Rigidbody2D playerRigidbody;
    float lastTime = 0;
    float bulletTime = 0.3f;
    Bounds bounds;

    [SerializeField] GameObject _bullet1Prefab;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] ParticleSystem thrust;
    [SerializeField] AudioSource thrustAudio;
    [SerializeField] AudioSource bulletAudio;

    [SerializeField] float speed = 70;
    [SerializeField] float rotSpeed = 3;
    [SerializeField] float rateOfFire = 1;
    [SerializeField] float rateOfRotation = 1;
    [SerializeField] float rateOfSpeed = 1;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;

        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        bulletTime = bulletTime * rateOfFire;
    }

    private void Update()
    {
        //if (dead) return; 

        /*
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse1))
        {
            var tempFrames = Time.frameCount;

            if (lastFrames + bullFrames < tempFrames)
            {
                lastFrames = tempFrames;
                shoot();
            }
        }*/

        // Check if user has warped
        boundsWarp();
    }

    void FixedUpdate()
    {
        //if (dead) return;

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse1))
        {
            var tempFrames = Time.time;

            if (lastTime + bulletTime < tempFrames)
            {
                lastTime = tempFrames;
                shoot();
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Mouse0))
        {
            var thrustTemp = thrust.emission;
            thrustTemp.enabled = true;
            
            if (!thrustAudio.isPlaying)
            {
                thrustAudio.Play();
            }

            Vector2 forceDirection = transform.rotation * Vector3.up;

            playerRigidbody.AddForce(forceDirection * (speed * rateOfSpeed));

            if (Input.GetKey(KeyCode.Mouse0))
            {
                mouseMode = true;
            }
        }
        else
        {
            var thrustTemp = thrust.emission;
            thrustTemp.enabled = false;
        }

        /*
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector2 forceDirection = transform.rotation * Vector3.down;

            playerRigidbody.AddForce(forceDirection * speed);
        }
        */

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRigidbody.MoveRotation(playerRigidbody.rotation - (rotSpeed * rateOfRotation));

            mouseMode = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRigidbody.MoveRotation(playerRigidbody.rotation + (rotSpeed * rateOfRotation));

            mouseMode = false;
        }


        if (mouseMode) followMouse();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerCollision(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        playerCollision(collision.gameObject);
    }

    void playerCollision(GameObject collision)
    {
        if (collision.tag == "astroid")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void followMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerRigidbody.position;
        
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle - 90);

        playerRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotSpeed));
    }

    void shoot()
    {
        GameObject bullet = Instantiate(_bullet1Prefab, transform.position, transform.rotation, null);

        //if (!bulletAudio.isPlaying)
       // {
            bulletAudio.Play();
        //}
        // Sword?
        //bullet.transform.SetParent(transform);
        //bullet.transform.localPosition = new Vector3(0, 5, 0);
    }

    void boundsWarp()
    {
        if (Math.Abs(transform.position.x) > bounds.extents.x)
        {
            Vector3 temp = transform.position;
            temp.x = temp.x * -1;

            transform.position = temp;
        }

        if (Math.Abs(transform.position.y) > bounds.extents.y)
        {
            Vector3 temp = transform.position;
            temp.y = temp.y * -1;

            transform.position = temp;
        }
    }
}
