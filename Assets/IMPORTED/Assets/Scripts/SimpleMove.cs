// File: SimpleMove.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleMove : MonoBehaviour
{
    public float speed = 5f; // Hız (Inspector'dan ayarla)
    public float jumpForce = 10f; // Zıplama kuvveti (Inspector'dan ayarla)

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Eğer zıplama olacaksa gravityScale'ı 0 yapmayın, varsayılan değerinde kalsın.
        // rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        // Yatay hareket
        float h = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        Vector2 vel = new Vector2(h * speed, rb.linearVelocity.y);
        rb.linearVelocity = vel;

        // Zıplama
        if (Input.GetButtonDown("Jump"))
        {
            // Zıplama tuşuna basıldığında (varsayılan Space tuşu) ve oyuncu zemindeyken
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    
}