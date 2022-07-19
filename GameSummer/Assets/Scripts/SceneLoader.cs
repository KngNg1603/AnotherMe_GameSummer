using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string NextSceneName;
    [SerializeField] string CurrentSceneName;
    [SerializeField] CheckIfPlayerArrived portal1;
    [SerializeField] CheckIfPlayerArrived portal2;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    private void Update()
    {
        if(portal1.arrived == true && portal2.arrived == true)
        {
            StartCoroutine(LoadNextLevel());
        }
        if(player1 == null || player2 == null)
        {
            SceneManager.LoadScene(CurrentSceneName);
        }
    }

    IEnumerator LoadNextLevel()
    {      
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(NextSceneName);
    }
}
