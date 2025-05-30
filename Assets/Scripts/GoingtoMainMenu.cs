using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoingtoMainMenu : MonoBehaviour
{
    public Animator fadeScreen;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(FadeAndLoadMainMenu());
    }

    private IEnumerator FadeAndLoadMainMenu()
    {
        Time.timeScale = 1f;

        if (fadeScreen != null)
        {
            Debug.Log("Playing animation...");
            fadeScreen.SetTrigger("WakeUp");

            // Wait one frame so the animation actually begins visually
            yield return null;

            // Now wait for the full animation duration
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene("Backrooms");
    }
}
