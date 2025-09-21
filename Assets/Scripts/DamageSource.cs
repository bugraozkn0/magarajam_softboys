using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 1f; // Bu objenin verebileceği damage miktarı

    [Header("Visual Effects (Opsiyonel)")]
    public ParticleSystem damageEffect;
    public AudioClip damageSound;

    void Start()
    {
        // Bu objenin Danger tag'ine sahip olduğundan emin ol
        if (!gameObject.CompareTag("Danger"))
        {
            Debug.LogWarning("DamageSource objesi 'Danger' tag'ine sahip değil! " + gameObject.name);
        }

        // Trigger olduğundan emin ol
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("DamageSource collider'ı trigger değil! " + gameObject.name);
        }
    }

    // Bu method'u başka scriptlerden çağırarak damage'i dinamik olarak değiştirebilirsin
    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }

    // Visual/Audio effect'leri tetiklemek için (opsiyonel)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<NotifyCollision>() != null)
        {
            // Particle effect çal
            if (damageEffect != null)
            {
                damageEffect.Play();
            }

            // Ses efekti çal
            if (damageSound != null)
            {
                AudioSource.PlayClipAtPoint(damageSound, transform.position);
            }
        }
    }
}