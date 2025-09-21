using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Sprite Settings")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] healthSprites; // 10 sprite: 100-90, 90-80, 80-70... 10-0 arası

    // Danger'da olan collider'ları ve damage'lerini takip et
    private Dictionary<NotifyCollision, float> dangerousColliders = new Dictionary<NotifyCollision, float>();
    private Coroutine damageCoroutine;
    private ConnectionManager cm;
    void Start()
    {
        cm = GetComponent<ConnectionManager>();
        currentHealth = maxHealth;
        UpdateSprite();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void AddDangerousCollider(NotifyCollision collider, float damage)
    {
        if (!dangerousColliders.ContainsKey(collider))
        {
            dangerousColliders.Add(collider, damage);

            // İlk tehlikeli collider eklendiyinde damage coroutine'ini başlat
            if (dangerousColliders.Count == 1)
            {
                damageCoroutine = StartCoroutine(ApplyDamageOverTime());
            }
        }
    }

    public void RemoveDangerousCollider(NotifyCollision collider)
    {
        if (dangerousColliders.ContainsKey(collider))
        {
            dangerousColliders.Remove(collider);

            // Hiç tehlikeli collider kalmadıysa damage coroutine'ini durdur
            if (dangerousColliders.Count == 0 && damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (dangerousColliders.Count > 0 && currentHealth > 0)
        {
            // Her tehlikeli collider için damage'i hesapla
            float totalDamage = 0f;

            foreach (var pair in dangerousColliders)
            {
                totalDamage += pair.Value; // Her collider'ın damage'ini topla
            }

            // Damage'i uygula
            TakeDamage(totalDamage);

            // 1 saniye bekle
            yield return new WaitForSeconds(1f);
        }

        damageCoroutine = null;
    }

    private void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateSprite();

        Debug.Log($"Health: {currentHealth}/{maxHealth} - Damage taken: {damage}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateSprite()
    {
        if (healthSprites == null || healthSprites.Length == 0 || spriteRenderer == null)
            return;

        // Health yüzdesini hesapla (0-1 arası)
        float healthPercent = currentHealth / maxHealth;

        // Sprite index'ini hesapla (10 sprite var, her 10% için 1 sprite)
        int spriteIndex = Mathf.FloorToInt(healthPercent * healthSprites.Length);

        // Index'i sınırla (0 ile sprite sayısı-1 arasında)
        spriteIndex = Mathf.Clamp(spriteIndex, 0, healthSprites.Length - 1);

        // Sprite'ı değiştir
        spriteRenderer.sprite = healthSprites[spriteIndex];
    }

    private void Die()
    {
        Debug.Log("Öldün! Marshmallow yanıp kül oldu!");

        // Ölüm işlemleri burada yapılabilir
        // Örneğin: game over ekranı, respawn, vs.
        if(cm.isMainPlayer)
        {
            // HARBI OLDUK AMK
        }
        else
        {
            // bizi kopar
            cm.DisconnectFromEverything();
        }


        // Damage coroutine'ini durdur
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        StartCoroutine(DelayedDestroy(4f)); // 2 saniye sonra yok et
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateSprite();
    }
    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}