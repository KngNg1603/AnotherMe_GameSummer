using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosY;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, new Vector3(this.transform.position.x, currentPosY, this.transform.position.z),
           ref velocity, speed * Time.deltaTime);
    }

    public void MoveToAnotherWorld(Transform world)
    {
        currentPosY = world.position.y;
    }
}
