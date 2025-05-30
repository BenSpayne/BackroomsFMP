using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private Coroutine audioFadeCoroutine;
    public float audioFadeDuration = 0.3f;

    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject gameUI;
    public Animator screenFader; // <-- Reference to your fade UI animator

    [Header("Player References")]
    public GameObject player;
    public GameObject playerCamera;
    private PlayerLook playerLook;

    private bool isPaused = false;

    void Start()
    {
        if (playerCamera != null)
            playerLook = playerCamera.GetComponent<PlayerLook>();

        ResumeGame(); // Ensure game starts unpaused
    }

    void Update()
    {
        bool isPlaying = player.activeSelf && gameUI.activeSelf;

        if (isPlaying || isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerLook != null)
            playerLook.enabled = false;

        // Start fade out
        if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
        audioFadeCoroutine = StartCoroutine(FadeAudio(0f, audioFadeDuration));
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);

        if (player.activeSelf && gameUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ResetCursorToCenter();

            if (playerLook != null)
                playerLook.enabled = true;
        }

        // Start fade in
        if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
        audioFadeCoroutine = StartCoroutine(FadeAudio(1f, audioFadeDuration));
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(FadeAndLoadMainMenu());
    }

    private IEnumerator FadeAndLoadMainMenu()
    {
        Time.timeScale = 1f;

        if (screenFader != null)
        {

            screenFader.SetTrigger("WakeUp");
            yield return new WaitForSeconds(1f); // Adjust if your animation is longer/shorter
        }

        SceneManager.LoadScene("Backrooms");
    }

    private void ResetCursorToCenter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private IEnumerator FadeAudio(float targetVolume, float duration)
    {
        float startVolume = AudioListener.volume;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime; 
            AudioListener.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            yield return null;
        }

        AudioListener.volume = targetVolume;
    }
}
