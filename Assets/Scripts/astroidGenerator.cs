using UnityEngine;
using static UnityEditor.PlayerSettings;

public class astroidGenerator : MonoBehaviour
{

    [SerializeField] int maxAstroids = 10;
    [SerializeField] GameObject _astroidPrefab;

    public void Update()
    {
        GameObject[] astroids = GameObject.FindGameObjectsWithTag("astroid");

        if (astroids.Length < maxAstroids)
        {
            Instantiate(_astroidPrefab, transform.position, transform.rotation);
        }
    }

}
