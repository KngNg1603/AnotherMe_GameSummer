using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBlock : MonoBehaviour
{
    [SerializeField] GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        block.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            block.SetActive(true);
        }
    }
}

