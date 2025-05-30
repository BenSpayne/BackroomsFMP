using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource), typeof(SphereCollider))]
public class TriggerAudioFade : MonoBehaviour
{
    public float fadeDuration = 1.0f; // Duration to fade in/out
    public float targetVolume = 1.0f; // Volume to reach when fading in

    private AudioSource audioSource;
    private Coroutine currentFade;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.loop = true; // optional: make sure it loops
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            if (currentFade != null)
                StopCoroutine(currentFade);

            currentFade = StartCoroutine(FadeAudio(targetVolume));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentFade != null)
                StopCoroutine(currentFade);

            currentFade = StartCoroutine(FadeAudio(0f, stopAfterFade: true));
        }
    }

    private IEnumerator FadeAudio(float target, bool stopAfterFade = false)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, target, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = target;

        if (stopAfterFade && target == 0f)
            audioSource.Stop();

        currentFade = null;
    }
}