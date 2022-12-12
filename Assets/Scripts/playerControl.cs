using Assets.Scripts.Weapons;
using Assets.Scripts.Powerups;
using System;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    private bool mouseMode = false;
    private bool dead = false;
    private Rigidbody2D playerRigidbody;
    private float lastTime = 0;
    private float bulletTime = 0.3f;
    private float scale = 0.8f;
    private Bounds bounds;

    internal static Weapon Weapon { get => weapon; set => weapon = value; }
    private static Weapon weapon;

    private Powerup powerup;

    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] ParticleSystem thrust;
    [SerializeField] AudioSource thrustAudio;

    [SerializeField] float speed = 70;
    [SerializeField] float rotSpeed = 3;

    [SerializeField] float rateOfFire = 1;
    [SerializeField] float rateOfRotation = 1;
    [SerializeField] float rateOfSpeed = 1;
    [SerializeField] float rateOfScale = 1;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;
        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        setScale();
        setBulletSpeed();

        // Set default weapon / powerup
        weapon = new Pew();
        powerup = new NoPowerup();
    }

    private void Update()
    {
        if (dead) return;

        // Check if user has warped
        boundsWarp();
    }

    void FixedUpdate()
    {
        if (dead) return;

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
        if (collision.tag == "asteroid")
        {
            this.dead = true;
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
        Weapon.shoot(transform);
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

    public bool isDead()
    {
        return this.dead;
    }

    public void setScale()
    {
        float tempScale = scale * rateOfScale;
        transform.localScale = new Vector3(tempScale, tempScale, tempScale);
    }

    public void setBulletSpeed()
    {
        bulletTime = bulletTime * rateOfFire;
    }
}
