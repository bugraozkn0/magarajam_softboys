using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float launchForce = 15f; // Yukar� f�rlatma g�c�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // �nce dikey h�z�n� s�f�rla
                rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
                Debug.Log("Oyuncu yukar� f�rlat�ld�!");
            }
        }
    }
}
