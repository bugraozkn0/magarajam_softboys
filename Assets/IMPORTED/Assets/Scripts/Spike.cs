using UnityEngine;

public class Spike : MonoBehaviour
{
    public Transform openPosition; // Şelalenin gideceği üst pozisyon
    public float speed = 2f; // Şelalenin hareket hızı
    public float waitTime = 3f; // En üstte veya en altta bekleyeceği süre

    private Vector3 closedPosition;
    private bool isMovingUp = true;
    private float waitTimer;

    public float knockbackForce = 10f;

    private void Start()
    {
        closedPosition = transform.position; // Başlangıç pozisyonunu kaydet
        waitTimer = waitTime;
    }

    private void Update()
    {
        if (isMovingUp)
        {
            // Şelale yukarı doğru hareket ediyor
            transform.position = Vector3.MoveTowards(transform.position, openPosition.position, speed * Time.deltaTime);

            // Eğer hedefe ulaştıysa yönü değiştir
            if (Vector3.Distance(transform.position, openPosition.position) < 0.01f)
            {
                isMovingUp = false;
                waitTimer = waitTime; // Bekleme süresini başlat
            }
        }
        else
        {
            // Şelale aşağı doğru hareket ediyor
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime);

            // Eğer hedefe ulaştıysa yönü değiştir
            if (Vector3.Distance(transform.position, closedPosition) < 0.01f)
            {
                isMovingUp = true;
                waitTimer = waitTime; // Bekleme süresini başlat
            }
        }

        // Bekleme süresini yönet
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu şelaleye çarptı ve hasar aldı!");
            ApplyKnockback(other.transform);
        }
    }

    private void ApplyKnockback(Transform player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 knockbackDirection = (player.position - transform.position).normalized;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}