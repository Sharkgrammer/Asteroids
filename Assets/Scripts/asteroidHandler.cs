using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public partial class asteroidHandler : MonoBehaviour
{
    public TextMeshProUGUI killCounter;
    public AsteroidTypes type = AsteroidTypes.Random;
    public Vector2 playerPos;
    public enum AsteroidTypes : int
    {
        Random = 0,
        Big = 1,
        Medium = 2,
        Small = 3
    }

    [SerializeField] Canvas healhbarCanvas;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] GameObject _asteroidPrefab;

    Rigidbody2D rbody;
    Vector2 force;
    Bounds bounds;
    Image healhbar;
    float boundsGap = 8, health, maxHealth, rotationSpeed;

    private bool pause = false;
    private bool gameover = false;

    

    void Start()
    {
        healhbar = healhbarCanvas.GetComponentInChildren<Image>();
        healhbar.enabled = false;

        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;

        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        rbody = GetComponent<Rigidbody2D>();
        rbody.rotation = Random.Range(0.0f, 360.0f);

        Boolean typeManuallySet = false;

        if (type == AsteroidTypes.Random)
        {
            // type is only ever set to random by default. If its set manually, the code setting it should work out posistion. 
            generatePoint();
            type = (AsteroidTypes) Random.Range(1, (int)Enum.GetValues(typeof(AsteroidTypes)).Cast<AsteroidTypes>().Max());
            type = AsteroidTypes.Big;
        }
        else
        {
            typeManuallySet = true;
        }

        float tempForce = 0, tempScale = 0, tempHealth = 0;
        switch (type)
        {
            case AsteroidTypes.Big:
                tempForce = 0.4f;
                tempScale = 2f;
                tempHealth = 200;
                break;

            case AsteroidTypes.Medium:
                tempForce = 0.8f;
                tempScale = 1.5f;
                tempHealth = 100;
                break;

            case AsteroidTypes.Small:
                tempForce = 1.4f;
                tempScale = 1;
                tempHealth = 50;
                break;
        }

        if (typeManuallySet)
        {
            float forceX = Random.Range(0.2f, tempForce), forceY = Random.Range(0.2f, tempForce);

            Debug.Log(playerPos.x + " " + transform.position.x);
            Debug.Log(playerPos.y + " " + transform.position.y);


            if (transform.position.x > 0)
            {
                if (playerPos.x < transform.position.x)
                {
                    forceX *= -1;
                }
            }
            else
            {
                if (playerPos.x > transform.position.x)
                {
                    forceX *= -1;
                }
            }

            if (transform.position.y > 0)
            {
                if (playerPos.y < transform.position.y)
                {
                    forceY *= -1;
                }
            }
            else
            {
                if (playerPos.y > transform.position.y)
                {
                    forceX *= -1;
                }
            }


            force = new Vector2(forceX, forceY);
        }
        else
        {
            force = new Vector2(Random.Range(-tempForce, tempForce), Random.Range(-tempForce, tempForce));
        }

        rotationSpeed = Random.Range(-tempForce, tempForce);
        
        float scale = Random.Range(tempScale, tempScale + 0.2f);

        transform.parent.localScale = new Vector3(scale, scale, scale);

        maxHealth = health = Random.Range(tempHealth, tempHealth + 50);
    }

    private void Update()
    {
        if (this.pause || this.gameover) return;

        boundsWarp();
    }

    void FixedUpdate()
    {
        if (this.pause) return;

        if (this.gameover)
        {
            // If the asteroids just stop it looks jarring
            // Force them to decelerate a small bit instead
            float x = force.x;
            float y = force.y;
            float dec = 0.01f;

            if (x == 0 && y == 0 && rotationSpeed == 0)
            {
                return;
            }

            if (x != 0)
            {
                force.x = calcForceSlowdownChange(x, dec);
            }

            if (y != 0)
            {
                force.y = calcForceSlowdownChange(y, dec);
            }

            if (rotationSpeed != 0)
            {
                rotationSpeed = calcForceSlowdownChange(rotationSpeed, dec);
            }
        }

        rbody.MovePosition(rbody.position + force);
        rbody.MoveRotation(rbody.rotation + rotationSpeed);

        // Force the healthbar to follow the asteroid
        healhbarCanvas.transform.position = transform.position;
    }

    private float calcForceSlowdownChange(float force, float dec)
    {
        if (Math.Abs(force) < dec)
        {
            force = 0;
        }
        else if (force > 0)
        {
            force -= dec;
        }
        else if (force < 0)
        {
            force += dec;
        }

        return force;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {

            health -= playerControl.Weapon.getDamage();
            healhbar.enabled = true;

            healhbar.fillAmount = health / maxHealth;

            if (health <= 0)
            {
                string[] str = killCounter.text.Split(" ");
                int counter = int.Parse(str[1]);

                killCounter.text = str[0] + " " + ++counter;

                GameObject exp = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(exp, 2);

                float scale = exp.transform.localScale.x;
                switch (type)
                {
                    case AsteroidTypes.Big:
                        scale += 1.75f;
                        break;

                    case AsteroidTypes.Medium:
                        scale += 1.25f;
                        break;

                    case AsteroidTypes.Small:
                        scale += 0.5f;
                        break;
                }
                exp.transform.localScale = new Vector3(scale, scale, scale);

                Vector3 v3 = transform.parent.position / transform.parent.localScale.x;
                Vector3 playerPos = collision.gameObject.transform.position;

                if (type == AsteroidTypes.Big)
                {
                    int amt = Random.Range(2, 4);

                    while (amt-- > 0)
                    {
                        GameObject test = Instantiate(_asteroidPrefab, v3, Quaternion.identity);
                        test.GetComponentInChildren<asteroidHandler>().generateChild(AsteroidTypes.Medium, playerPos);
                    }
                }

                if (type == AsteroidTypes.Medium)
                {
                    int amt = Random.Range(2, 4);

                    while (amt-- > 0)
                    {
                        GameObject test = Instantiate(_asteroidPrefab, v3, Quaternion.identity);
                        test.GetComponentInChildren<asteroidHandler>().generateChild(AsteroidTypes.Small, playerPos);
                    }
                }

                Destroy(transform.parent.gameObject);
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
            
            if (temp.x > 0)
            {
                temp.x -= 0.2f;
            }
            else
            {
                temp.x += 0.2f;
            }

            temp.x *= -1;
            //force.x *= -1;



            rbody.position = temp;
        }

        if (Math.Abs(transform.position.y) - boundsGap > bounds.extents.y)
        {
            Vector2 temp = rbody.position;

            if (temp.y > 0)
            {
                temp.y -= 0.2f;
            }
            else
            {
                temp.y += 0.2f;
            }

            temp.y *= -1;
            //force.y *= -1;



            rbody.position = temp;
        }
    }

    public void pauseObj()
    {
        this.pause = true;
    }

    public void playerDead()
    {
        this.gameover = true;
    }

    public void generateChild(AsteroidTypes type, Vector3 player)
    {
        this.type = type;
        this.playerPos = player;
    }
}
