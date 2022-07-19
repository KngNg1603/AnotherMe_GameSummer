using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class Spawning : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] GameObject player;

        private void Awake()
        {
            player.SetActive(false);
            StartCoroutine(FirstSpawn());
        }

        IEnumerator FirstSpawn()
        {
            anim.SetBool("isSpawning", true);
            yield return new WaitForSeconds(0.37f);
            anim.SetBool("isSpawning", false);
            player.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}

