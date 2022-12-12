using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using Random = UnityEngine.Random;

public class generator : MonoBehaviour
{
    [SerializeField] int maxAsteroids;
    [SerializeField] TextMeshProUGUI killCounter;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject[] _asteroidPrefabs;
    [SerializeField] GameObject player;

    private bool playerDead = false;
    private bool allowReset = false;

    public void Start()
    {
        deathScreen.SetActive(false);

        InvokeRepeating("updateMaxAsteroid", 20.0f, 10.0f);
    }


    public void Update()
    {
        if (allowReset)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        if (playerDead) return;

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("asteroid");

        if (asteroids.Length < maxAsteroids)
        {
            GameObject ob = Instantiate(_asteroidPrefabs[Random.Range(0, _asteroidPrefabs.Length)], transform.position, Quaternion.identity);
            ob.GetComponent<asteroidHandler>().killCounter = killCounter;
        }

        if (!player)
        {
            CancelInvoke();

            // Setup death screen. 
            int score = int.Parse(killCounter.text.Split(" ")[1]);
            killCounter.enabled = false;

            TextMeshProUGUI[] deathTexts = deathScreen.GetComponentsInChildren<TextMeshProUGUI>();

            deathTexts[1].text += score;

            float time = Time.timeSinceLevelLoad;
            String timeText = "seconds";

            if (time >= 60)
            {
                timeText = "minutes";
                time /= 60;
            }

            deathTexts[2].text += Math.Round(time, 2) + " " + timeText;

            deathScreen.SetActive(true);

            foreach (GameObject asteroid in asteroids)
            {
                asteroid.GetComponent<asteroidHandler>().playerDead();
            }

            playerDead = true;
            Invoke("setAllowReset", 2.0f);
        }
    }


    public void updateMaxAsteroid()
    {
        maxAsteroids += 5;
    }

    public void setAllowReset()
    {
        allowReset = true;
    }

}
