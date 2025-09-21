using System.Collections;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{
    [SerializeField] float offset = 2f; // Yukarı aşağı hareket mesafesi
    [SerializeField] float upSpeed = 1f;   // Yukarı çıkış hızı (birim/saniye)
    [SerializeField] float downSpeed = 2f; // Aşağı iniş hızı (birim/saniye)

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Coroutine moveCoroutine;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector2.up * offset;

        StartMovement();
    }

    private void StartMovement()
    {
        moveCoroutine = StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            // Yukarı hareket
            yield return StartCoroutine(MoveDoor(targetPosition, upSpeed));

            // Aşağı hareket  
            yield return StartCoroutine(MoveDoor(startPosition, downSpeed));
        }
    }

    private IEnumerator MoveDoor(Vector2 targetPos, float doorSpeed)
    {
        Vector2 startPos = transform.position;
        float distance = Vector2.Distance(startPos, targetPos);
        float duration = distance / doorSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }
}