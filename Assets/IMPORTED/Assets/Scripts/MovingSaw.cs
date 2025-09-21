using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    public float speed = 2f;          // Hareket hýzý
    public float moveDistance = 3f;   // Ne kadar yukarý aþaðý hareket edeceði

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
        // Yukarý aþaðý hareket
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
            Debug.Log("Oyuncu testereye çarptý!");
        }
    }
}
