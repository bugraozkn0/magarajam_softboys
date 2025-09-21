using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 1f; // Bu objenin verebileceği damage miktarı

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
}