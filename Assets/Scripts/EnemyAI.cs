using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float chaseRange = 10f;
    public float deathRange = 0.75f;

    public float patrolRadius = 10f;
    public float patrolSpeed = 2f;
    public float patrolWaitTime = 3f;

    public Animator animator;
    public Animator FadeScreenOut;

    private NavMeshAgent agent;
    private float distanceToPlayer;
    private bool isPatrolling = false;
    private bool isWaiting = false;
    private AudioSource audioSource;
    private bool isChasing = false;

    public float fadeDuration = 1.5f;
    public float targetVolume = 1.0f;
    private Coroutine currentFadeCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 0f; 
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.updatePosition = true;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            StartCoroutine(PatrolRoutine());
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < chaseRange)
        {
            if (!isChasing)
            {
                isChasing = true;
                if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
                currentFadeCoroutine = StartCoroutine(FadeIn());
            }

            StopCoroutine(PatrolRoutine());
            isPatrolling = false;
            isWaiting = false;

            agent.SetDestination(player.transform.position);

            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isIdle", false);

            agent.speed = 21f;

            if (distanceToPlayer < deathRange)
            {
                Time.timeScale = 1f;
                FadeScreenOut.SetTrigger("WakeUp");
                StartCoroutine(LoadSceneAfterDelay(1f));

                IEnumerator LoadSceneAfterDelay(float delay)
                {
                    yield return new WaitForSeconds(delay);
                    SceneManager.LoadScene("BackroomsDead");
                }
            }
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
                currentFadeCoroutine = StartCoroutine(FadeOut());
            }

            if (!isPatrolling && !isWaiting)
            {
                StartCoroutine(PatrolRoutine());
            }
        }
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            isPatrolling = true;

            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, NavMesh.AllAreas))
            {
                agent.speed = patrolSpeed;
                agent.SetDestination(navHit.position);

                animator.SetBool("isWalking", true);
                animator.SetBool("isChasing", false);
                animator.SetBool("isIdle", false);

                while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }

                Debug.Log("Patrolling to: " + navHit.position);
            }

            isWaiting = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", false);
            animator.SetBool("isIdle", true);

            yield return new WaitForSeconds(patrolWaitTime);
            isWaiting = false;
        }
    }

    IEnumerator FadeIn()
    {
        if (audioSource == null) yield break;
        if (!audioSource.isPlaying) audioSource.Play();

        float startVolume = audioSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    IEnumerator FadeOut()
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}