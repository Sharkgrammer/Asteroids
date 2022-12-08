using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionPlayerHandler : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject, 5);
    }
}
