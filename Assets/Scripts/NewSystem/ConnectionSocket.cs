using UnityEngine;
using System.Collections.Generic;
public class ConnectionSocket : MonoBehaviour
{
    public int socketIndex;
    [SerializeField] float rayLength = 1f;

    [Header("Ignore Raycast Collider")]
    [SerializeField] List<Collider2D> myColliders = new();
    [SerializeField] float cooldown = 1f;

    private ConnectionManager manager;
    private ConnectionManager connectedTarget;
    private ConnectionSocket connectedTargetSocket;
    private bool canAutoConnect = true;

    private float cooldownTimer = 0f; // her socket için ayrı timer

    public bool IsConnected => connectedTargetSocket != null;
    public ConnectionSocket ConnectedTargetSocket => connectedTargetSocket;
    public FixedJoint2D Joint { get; private set; }

    public void Initialize(ConnectionManager cm)
    {
        manager = cm;
        Joint = GetComponent<FixedJoint2D>();
        if (Joint != null)
        {
            Joint.connectedBody = null;
            Joint.enabled = false;
        }
    }

    public void Connect(ConnectionSocket targetSocket)
    {
        if (targetSocket == null || targetSocket.IsConnected) return;

        connectedTargetSocket = targetSocket;
        connectedTarget = targetSocket.manager;

        manager.AddConnection(connectedTarget);
        connectedTarget.AddConnection(manager);

        if (Joint != null)
        {
            Joint.connectedBody = targetSocket.GetComponent<Rigidbody2D>();
            Joint.enabled = true;
        }

        if (targetSocket.Joint != null)
        {
            targetSocket.Joint.connectedBody = GetComponent<Rigidbody2D>();
            targetSocket.Joint.enabled = true;
        }

        targetSocket.connectedTargetSocket = this;
        targetSocket.connectedTarget = manager;

        Debug.Log($"[Connect] {manager.name}:{socketIndex} <-> {targetSocket.manager.name}:{targetSocket.socketIndex}");

        // Bağlandıktan sonra cooldown başlat
        cooldownTimer = cooldown;
    }

    public void Disconnect()
    {
        if (connectedTargetSocket == null) return;

        if (Joint != null)
        {
            Joint.connectedBody = null;
            Joint.enabled = false;
        }

        if (connectedTargetSocket.Joint != null)
        {
            connectedTargetSocket.Joint.connectedBody = null;
            connectedTargetSocket.Joint.enabled = false;
        }

        manager.RemoveConnection(connectedTarget);
        connectedTarget.RemoveConnection(manager);

        connectedTargetSocket.connectedTarget = null;
        connectedTargetSocket.connectedTargetSocket = null;

        connectedTarget = null;
        connectedTargetSocket = null;

        Debug.Log($"[Disconnect] {manager.name}:{socketIndex}");

        // Disconnect sonrası cooldown başlat
        cooldownTimer = cooldown;
    }

    public void HandleMouseDown()
    {
        if (IsConnected)
        {
            Debug.Log($"[HandleMouseDown] {name} Disconnect ediliyor.");
            Disconnect();
        }
    }

    void Update()
    {
        if (!canAutoConnect) return;
        if (manager == null) return;
        if (IsConnected) return;
        if (!manager.isMainPlayer && !manager.IsConnectedToMainPlayer()) return;

        // Cooldown kontrolü
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        Vector2 rayStart = transform.position;
        Vector2 rayDir = -transform.right;

        Debug.DrawRay(rayStart, rayDir * rayLength, Color.cyan);

        RaycastHit2D[] hits = Physics2D.RaycastAll(rayStart, rayDir, rayLength, manager.connectionLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null || myColliders.Contains(hit.collider)) continue;

            ConnectionSocket targetSocket = hit.collider.GetComponentInParent<ConnectionSocket>();
            if (targetSocket == null || targetSocket.IsConnected) continue;

            if (targetSocket.manager == manager)
            {
                Debug.Log($"{transform.root.name}, {transform.name} ile {targetSocket.name} Kendi kendine bağlanmaya çalışıyor, atlanıyor.");
                continue;
            }
            Connect(targetSocket);
            break;
        }
    }
}