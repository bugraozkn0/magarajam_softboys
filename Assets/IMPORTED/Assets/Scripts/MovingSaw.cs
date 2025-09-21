using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    public float speed = 2f;          // Hareket h�z�
    public float moveDistance = 3f;   // Ne kadar yukar� a�a�� hareket edece�i

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingUp = true;

    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + Vector3.up * moveDistance;
    }

    private void Update()
    {
        // Yukar� a�a�� hareket
        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, endPos) < 0.01f)
                movingUp = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
                movingUp = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu testereye �arpt�!");
        }
    }
}
