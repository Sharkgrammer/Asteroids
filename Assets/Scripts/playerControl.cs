using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class playerControl : MonoBehaviour
{

    bool mouseMode = false;
    Rigidbody2D playerRigidbody;

    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    [SerializeField] private GameObject _bullet1Prefab;

    [SerializeField] int bullFrames;
    int lastFrames = 0;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
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
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 forceDirection = transform.rotation * Vector3.up;

            playerRigidbody.AddForce(forceDirection * speed);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                mouseMode = true;
            }
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector2 forceDirection = transform.rotation * Vector3.down;

            playerRigidbody.AddForce(forceDirection * speed);
        }

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
        Instantiate(_bullet1Prefab, transform.position, transform.rotation, null);
    }
}
