using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SwitchWorld : MonoBehaviour
    {
        private Vector3 velocity = Vector3.zero;
        [SerializeField] Transform map1;
        [SerializeField] Transform map2;
        public GameObject player;
        public GameObject playerParallel;

        [SerializeField] private CameraController cam;

        private void Start()
        {
            playerParallel.GetComponent<Controller>().enabled = false;
        }

        private void Update()
        {
            Switch();
        }

        public void Switch(bool isInSwitchScene = false)
        {
            if (isInSwitchScene == true || (Input.GetKeyDown(KeyCode.Q) && player != null && playerParallel != null))
            {
                if (this.transform.position.y >= map2.position.y + 1 || this.transform.position.y <= map2.position.y - 1)
                {
                    player.GetComponent<Controller>().enabled = false;                    
                    cam.MoveToAnotherWorld(map2);
                    playerParallel.GetComponent<Controller>().enabled = true;                    
                }
                else
                {                  
                    playerParallel.GetComponent<Controller>().enabled = false;
                    cam.MoveToAnotherWorld(map1);
                    player.GetComponent<Controller>().enabled = true;                
                }
            }
        }
    }
}

