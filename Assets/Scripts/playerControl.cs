using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class playerControl : MonoBehaviour
{

    bool mouseMode = false;
    Rigidbody2D playerRigidbody;

    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    [SerializeField] GameObject _bullet1Prefab;
    [SerializeField] int bullFrames;
    [SerializeField] ParticleSystem thrust;

    int lastFrames = 0;
    Bounds bounds;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;

        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse1))
        {
            var tempFrames = Time.frameCount;

            if (lastFrames + bullFrames < tempFrames)
            {
                lastFrames = tempFrames;
                shoot();
            }
        }

        // Check if user has warped
        boundsWarp();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Mouse0))
        {
            var thrustTemp = thrust.emission;
            thrustTemp.enabled = true;

            Vector2 forceDirection = transform.rotation * Vector3.up;

            playerRigidbody.AddForce(forceDirection * speed);

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
            playerRigidbody.MoveRotation(playerRigidbody.rotation - rotSpeed);

            mouseMode = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRigidbody.MoveRotation(playerRigidbody.rotation + rotSpeed);

            mouseMode = false;
        }


        if (mouseMode) followMouse();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "astroid")
        {
            // TODO Explosion here
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
