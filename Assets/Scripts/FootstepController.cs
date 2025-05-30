using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public AudioClip walkClip;
    public AudioClip sprintClip;
    public float walkStepRate = 0.6f;
    public float sprintStepRate = 0.35f;
    private float stepTimer;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stepTimer = 0f;
    }

    void Update()
    {
        if (playerMovement == null || walkClip == null || sprintClip == null)
            return;

        bool isMovingInput = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isActuallyMoving = playerMovement.controller.velocity.magnitude > 0.1f;
        bool isGrounded = playerMovement.controller.isGrounded;

        if (!isMovingInput || !isActuallyMoving || !isGrounded)
        {
            stepTimer = 0f;
            return;
        }

        float currentRate = walkStepRate;
        AudioClip currentClip = walkClip;

        if (playerMovement.isSprinting)
        {
            currentRate = sprintStepRate;
            currentClip = sprintClip;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f && !audioSource.isPlaying)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f); 
            audioSource.PlayOneShot(currentClip);
            stepTimer = currentRate;
        }
    }
}