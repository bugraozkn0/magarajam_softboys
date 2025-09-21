using UnityEngine;

public class Trap : MonoBehaviour
{
    public float knockbackForce = 10f; // Geri tepme kuvveti

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu dikenlere �arpt�! (Trigger)");

            // Oyuncuya geri tepme uygula
            ApplyKnockback(other.transform);
        }
    }

    private void ApplyKnockback(Transform player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Oyuncuyu tuza��n tersi y�nde it
            Vector2 knockbackDirection = (player.position - transform.position).normalized;
            playerRb.linearVelocity = Vector2.zero; // Mevcut h�z� s�f�rla
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}