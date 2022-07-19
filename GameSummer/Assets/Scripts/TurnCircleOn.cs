using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCircleOn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject circle;
    void Start()
    {
        circle.SetActive(false);
        StartCoroutine(TurnOn());
    }

   IEnumerator TurnOn()
    {
        yield return new WaitForSeconds(0.35f);
        circle.SetActive(true);
    }
}
