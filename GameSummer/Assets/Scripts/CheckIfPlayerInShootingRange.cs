using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPlayerInShootingRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            this.gameObject.GetComponentInChildren<MovingPlatform>().enabled = true;
        }
    }
}
