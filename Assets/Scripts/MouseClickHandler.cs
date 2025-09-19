using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    [SerializeField] ConnectionPoint myPoint;

    private Collider2D boxCollider;

    private void Awake() => boxCollider = GetComponent<Collider2D>();
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null)
            {
                if (hit == boxCollider)
                    myPoint.Disconnect();
            }
        }
    }
}