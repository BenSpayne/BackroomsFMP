using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour
{
    public Animator screenFade;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReturnToMainMenu();
        }

        void ReturnToMainMenu()
        {
            StartCoroutine(FadeAndLoadMainMenu());
        }

        IEnumerator FadeAndLoadMainMenu()
        {
            Time.timeScale = 1f;
                Debug.Log("Playing animation...");
                screenFade.SetTrigger("WakeUp");

                yield return null;
                yield return new WaitForSeconds(1f);
            
            SceneManager.LoadScene("BackroomsComplete");
        }
    }
}
