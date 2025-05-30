using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromEndScenetoStartScene : MonoBehaviour
{
    public Animation fadeScreen;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Removed the automatic call to FadeAndLoadMainMenu
    }

    // Call this from a UI button or event
    public void ReturnToMainMenu()
    {
        StartCoroutine(FadeAndLoadMainMenu());
    }

    private IEnumerator FadeAndLoadMainMenu()
    {
        Time.timeScale = 1f;

        if (fadeScreen != null)
        {
            fadeScreen.Play();
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene("Backrooms");
    }
}

