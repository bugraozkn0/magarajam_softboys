using UnityEngine;

public class ConnectionSocketInput : MonoBehaviour
{
    private ConnectionSocket target;
    [SerializeField] private LayerMask layerMask;

    private Collider2D circleCollider;
    private void Awake()
    {
        circleCollider = GetComponent<Collider2D>();
        target = GetComponentInParent<ConnectionSocket>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

            if (hit.collider != null && hit.collider == circleCollider)
                target.HandleMouseDown();
        }
    }
}