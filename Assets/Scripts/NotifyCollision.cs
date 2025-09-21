using UnityEngine;

public class NotifyCollision : MonoBehaviour
{
    private HealthManager healthManager;
    private bool isInDanger = false;

    void Start()
    {
        // Root health manager'ı bul
        healthManager = GetComponentInParent<HealthManager>();

        if (healthManager == null)
        {
            Debug.LogError("HealthManager bulunamadı! " + gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Danger taglı objeyle çarpışma kontrolü
        if (other.CompareTag("Danger") && !isInDanger)
        {
            isInDanger = true;

            // Damage değerini çarptığımız objeden al
            DamageSource damageSource = other.GetComponent<DamageSource>();
            float damage = damageSource != null ? damageSource.damageAmount : 1f; // Default 1

            // HealthManager'a bu collider'ın danger'da olduğunu bildir
            if (healthManager != null)
            {
                healthManager.AddDangerousCollider(this, damage);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Danger taglı objeden çıkış kontrolü
        if (other.CompareTag("Danger") && isInDanger)
        {
            isInDanger = false;

            // HealthManager'a bu collider'ın artık güvende olduğunu bildir
            if (healthManager != null)
            {
                healthManager.RemoveDangerousCollider(this);
            }
        }
    }
}