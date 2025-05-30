using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    public Transform virtualCamera;     // The camera GameObject to move
    public Transform targetTransform;   // The position the camera should move to
    public Transform lookAtTarget;      // The object the camera should look at (optional)

    public float moveDuration = 0.5f;   // Duration for the camera to move
    public float returnDelay = 1f;      // Delay before returning to original position

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public void MoveCameraToTarget()
    {
        StopAllCoroutines(); // In case you're already moving

        // Save original position and rotation before moving
        originalPosition = virtualCamera.position;
        originalRotation = virtualCamera.rotation;

        StartCoroutine(MoveCameraRoutine());
    }

    private IEnumerator MoveCameraRoutine()
    {
        Debug.Log("Moving cinematic camera");
        Vector3 startPosition = virtualCamera.position;
        Quaternion startRotation = virtualCamera.rotation;

        Vector3 endPosition = targetTransform.position;
        Quaternion endRotation = targetTransform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            t = Mathf.SmoothStep(0, 1, t); // Smooth easing

            virtualCamera.position = Vector3.Lerp(startPosition, endPosition, t);
            virtualCamera.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.position = endPosition;
        virtualCamera.rotation = endRotation;

        // Wait before returning
        yield return new WaitForSeconds(returnDelay);

        // Return smoothly to original position
        elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            t = Mathf.SmoothStep(0, 1, t);

            virtualCamera.position = Vector3.Lerp(endPosition, originalPosition, t);
            virtualCamera.rotation = Quaternion.Slerp(endRotation, originalRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.position = originalPosition;
        virtualCamera.rotation = originalRotation;
    }
}