using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Vector2 openOffset = new Vector2(2f, 0f); // Kapalı pozisyona göre açık offset
    [SerializeField] float openSpeed = 2f;
    [SerializeField] float closeSpeed = 4f;

    private Vector2 closedPosition;
    private Vector2 openPosition;
    private Coroutine moveCoroutine;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    public void OpenDoor()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveDoor(openPosition, openSpeed));
    }

    public void CloseDoor()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveDoor(closedPosition, closeSpeed));
    }

    private IEnumerator MoveDoor(Vector2 targetPosition, float doorSpeed)
    {
        Vector2 startPosition = transform.position;
        float distance = Vector2.Distance(startPosition, targetPosition);
        float duration = distance / doorSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        moveCoroutine = null;
    }
}