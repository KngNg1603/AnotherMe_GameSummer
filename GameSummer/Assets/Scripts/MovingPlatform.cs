using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] pos;
    public Transform startPos;
    private int count = 0;

    public float speed = 5f;
    private Vector3 nextPos;

    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float startWaitTime = 1f;

    private void Start()
    {
        nextPos = startPos.position;
    }

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        if (transform.position == pos[count].position && count != pos.Length - 1)
        {
            if(waitTime <= 0)
            {
                waitTime = startWaitTime;
                count += 1;
                nextPos = pos[count].position;
            }
            else
            {
                waitTime -= Time.deltaTime;
                nextPos = pos[count].position;
            }
            
        }
        else if (transform.position == pos[count].position && count == pos.Length - 1)
        {
            if(waitTime <= 0)
            {
                waitTime = startWaitTime;
                count = 0;
                nextPos = pos[count].position;
            }
            else
            {
                waitTime -= Time.deltaTime;
                nextPos = pos[count].position;
            }
            
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
}
