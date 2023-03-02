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
    Vector2 force, manualTopForce;
    Bounds bounds;
    Image healhbar;
    float boundsGap = 6, health, maxHealth, rotationSpeed;

    private bool pause = false;
    private bool gameover = false;
    private bool typeManuallySet = false;

    void Start()
    {
        healhbar = healhbarCanvas.GetComponentInChildren<Image>();
        healhbar.enabled = false;

        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * 2;

        bounds = new Bounds(Camera.main.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        rbody = GetComponent<Rigidbody2D>();
        rbody.rotation = Random.Range(0.0f, 360.0f);


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
                tempForce = 0.6f;
                tempScale = 1.5f;
                tempHealth = 100;
                break;

            case AsteroidTypes.Small:
                tempForce = 1;
                tempScale = 1;
                tempHealth = 50;
                break;
        }


        // This asteroid was created by 
        if (!typeManuallySet)
        {
            force = new Vector2(Random.Range(-tempForce, tempForce), Random.Range(-tempForce, tempForce));
        }

        rotationSpeed = Random.Range(-tempForce, tempForce);
        
        float scale = Random.Range(tempScale - 0.2f, tempScale + 0.2f);
        // We don't bother to get the actual width of the asteroid so scale is used to effect it
        boundsGap *= scale;

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

        if (typeManuallySet)
        {
            calcManualForceChange();
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

    private void calcManualForceChange()
    {
        if (force == manualTopForce) return;

        // Slowly accerlate up
        float accerlation = Random.Range(0.001f, 0.01f);

        if (Math.Abs(force.x) < Math.Abs(manualTopForce.x))
        {
            if (manualTopForce.x > 0)
            {
                force.x += accerlation;
            }
            else
            {
                force.x -= accerlation;
            }
        }
        else
        {
            force.x = manualTopForce.x;
        }

        if (Math.Abs(force.y) < Math.Abs(manualTopForce.y))
        {
            if (manualTopForce.y > 0)
            {
                force.y += accerlation;
            }
            else
            {
                force.y -= accerlation;
            }
        }
        else
        {
            force.y = manualTopForce.y;
        }


        Debug.Log(force);

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

                // Code to spawn new asteroids from its corpse
                
                if (type != AsteroidTypes.Small)
                {

                    GameObject player = GameObject.FindGameObjectWithTag("player");

                    Vector3 playerPos = player.transform.position;
                    AsteroidTypes newType = type == AsteroidTypes.Big ? AsteroidTypes.Medium : AsteroidTypes.Small;

                    Vector3 worldPlayerPoint = playerPos;
                    Vector3 worldPoint = transform.position;

                    Debug.Log(worldPlayerPoint + "     " + worldPoint);

                    Vector2 newDirection = new Vector2(1, 1);

                    if (worldPlayerPoint.x >= worldPoint.x)
                    {
                        newDirection.x *= -1;
                    }

                    if (worldPlayerPoint.y >= worldPoint.y)
                    {
                        newDirection.y *= -1;
                    }

                    int amt = Random.Range(1, 4);

                    amt = 1;

                    while (amt-- > 0)
                    {
                        GameObject test = Instantiate(_asteroidPrefab, rbody.position / scale, Quaternion.identity);

                        test.GetComponentInChildren<asteroidHandler>().generateChild(newType, newDirection);
                    }
                }


                Destroy(transform.parent.gameObject);
            }
        }
    }

    void generatePoint()
    {
        float x = bounds.extents.x / 2 + boundsGap;
        float y = bounds.extents.y / 2 + boundsGap;

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

        // transform.position = new Vector2(x, y);


        transform.position = new Vector2(10, 10);

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

    public void generateChild(AsteroidTypes type, Vector2 childDirection)
    {
        this.type = type;

        float forceX = Random.Range(1, 1), forceY = Random.Range(1, 1);

        manualTopForce = new Vector2(forceX, forceY);
        manualTopForce = manualTopForce * childDirection;
        

        force = new Vector2(manualTopForce.x / 10, manualTopForce.y / 10);


        Debug.Log(force + " " + manualTopForce);

    }
}
