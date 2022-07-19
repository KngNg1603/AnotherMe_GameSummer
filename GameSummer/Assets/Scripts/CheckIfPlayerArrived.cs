using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPlayerArrived : MonoBehaviour
{
    [HideInInspector] public bool arrived = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            arrived = true;
        }
    }
}
