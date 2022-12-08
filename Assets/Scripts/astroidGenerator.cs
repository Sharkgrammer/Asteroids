using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class astroidGenerator : MonoBehaviour
{
    public int maxAstroids;
    public GameObject[] _astroidPrefabs;
    public TextMeshProUGUI killCounter;

    public void Update()
    {
        GameObject[] astroids = GameObject.FindGameObjectsWithTag("astroid");

        if (astroids.Length < maxAstroids)
        {
            GameObject ob = Instantiate(_astroidPrefabs[Random.Range(0, _astroidPrefabs.Length)], transform.position, Quaternion.identity);
            ob.GetComponent<astroidHandler>().killCounter = killCounter;
        }
    }

    public void Start()
    {
        InvokeRepeating("updateMaxAstroids", 20.0f, 10.0f);
    }

    public void updateMaxAstroids()
    {
        maxAstroids += 5;
    }

}
