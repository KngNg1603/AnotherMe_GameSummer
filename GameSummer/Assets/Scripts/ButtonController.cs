using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] GameObject[] Obstacle;
        [SerializeField] Rigidbody2D[] rbObstacle;

        [SerializeField] SwitchWorld sw;
        private int count = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(count == 0)
            {
                if (collision.tag == "Player" && (Obstacle[0].tag == "Trap" || Obstacle[0].tag == "FallingObject"))
                {
                    count++;
                    StartCoroutine(SetKinematic());
                }
                else if (collision.tag == "Player" && Obstacle[0].tag == "Off_Object")
                {
                    count++;
                    StartCoroutine(DisableObject());
                }
                else if (collision.tag == "Player" && Obstacle[0].tag == "On_Object")
                {
                    count++;
                    StartCoroutine(EnableObject());
                }
                else if (collision.tag == "Player" && Obstacle[0].tag == "SpikeBall")
                {
                    count++;
                    StartCoroutine(StopRotatingObject());
                }
            }
            

        }

        IEnumerator SetKinematic()
        {
            sw.Switch(true);
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < rbObstacle.Length; i++)
            {
                rbObstacle[i].isKinematic = false;
            }
            yield return new WaitForSeconds(1f);
            sw.Switch(true);
        }

        IEnumerator DisableObject()
        {
            sw.Switch(true);
            yield return new WaitForSeconds(0.8f);
            for (int i = 0; i < Obstacle.Length; i++)
            {
                Obstacle[i].GetComponentInChildren<MovingPlatform>().speed = 0;
                Obstacle[i].GetComponentInChildren<OffAnimation>().anim.SetBool("IsOff", true);
            }
            yield return new WaitForSeconds(1f);
            sw.Switch(true);
        }

        IEnumerator StopRotatingObject()
        {
            sw.Switch(true);
            yield return new WaitForSeconds(0.8f);
            for (int i = 0; i < Obstacle.Length; i++)
            {
                Obstacle[i].GetComponentInChildren<Rotate>().enabled = false;
            }
            yield return new WaitForSeconds(1f);
            sw.Switch(true);
        }

        IEnumerator EnableObject()
        {
            sw.Switch(true);
            yield return new WaitForSeconds(1.2f);
            for(int i = 0; i < Obstacle.Length; i++)
            {
                Obstacle[i].GetComponentInChildren<MovingPlatform>().enabled = true;
            }
            yield return new WaitForSeconds(1f);
            sw.Switch(true);
        }
    }
}

