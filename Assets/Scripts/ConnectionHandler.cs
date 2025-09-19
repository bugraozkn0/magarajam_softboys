using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ConnectionPointData
{
    public ConnectionPoint cPoint;
    public FixedJoint2D fixedJoint;
}
public class ConnectionHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform coreTransform;
    [SerializeField] private PlayerMovement pMovement;

    [Space(20)]

    [Header("Bone Settings")]
    [SerializeField] List<FixedJoint2D> connectionJoints = new(); // UP DOWN LEFT RIGHT
    [SerializeField] List<CircleCollider2D> boneColliders = new();

    [Space(20)]

    [Header("Ray Settings")]
    [SerializeField] private bool isItPlayer = false;
    [SerializeField] private Vector4 rayLength;
    [SerializeField] private LayerMask rayMask = -1;

    [Space(20)]
    public int connectedBodyAmount = 0;

    public bool[] isConnectedList = new bool[4] { false, false, false, false }; // UP DOWN LEFT RIGHT

    private List<ConnectionPointData> connections = new();

    private void Awake()
    {
        IgnoreSelfCollision();
        foreach (var joint in connectionJoints)
        {
            joint.enabled = false;
        }
    }

    private void IgnoreSelfCollision()
    {
        foreach (var c in boneColliders)
        {
            foreach (var c2 in boneColliders)
            {
                if (c != c2)
                    Physics2D.IgnoreCollision(c, c2);
            }
        }
    }

    private void CalculateRays()
    {
        if (!isConnectedList[0])
        {
            // Upwards Ray
            RaycastHit2D upHit = Physics2D.Raycast(coreTransform.position, coreTransform.right, rayLength.x, rayMask);
            if (upHit)
            {
                Debug.Log($"<color=green>UP Connection available: {upHit.transform.parent.name} </color>");
                ConnectionPoint cPoint;
                if (upHit.transform.TryGetComponent(out cPoint))
                {
                    ConnectWith(cPoint, Direction.Up);
                }
            }
        }
        if (!isConnectedList[1])
        {
            // Downwards Ray
            RaycastHit2D downHit = Physics2D.Raycast(coreTransform.position, -coreTransform.right, rayLength.y, rayMask);
            if (downHit)
            {
                Debug.Log($"<color=green>DOWN Connection available: {downHit.transform.parent.name} </color>");
                ConnectionPoint cPoint;
                if (downHit.transform.TryGetComponent(out cPoint))
                {
                    ConnectWith(cPoint, Direction.Down);
                }
            }
        }
        if (!isConnectedList[2])
        {
            // Left Ray
            RaycastHit2D leftHit = Physics2D.Raycast(coreTransform.position, coreTransform.up, rayLength.w, rayMask);
            if (leftHit)
            {
                Debug.Log($"<color=green>LEFT Connection available: {leftHit.transform.parent.name} </color>");
                ConnectionPoint cPoint;
                if (leftHit.transform.TryGetComponent(out cPoint))
                {
                    ConnectWith(cPoint, Direction.Left);
                }
            }
        }
        if (!isConnectedList[3])
        {
            // Right Ray
            RaycastHit2D rightHit = Physics2D.Raycast(coreTransform.position, -coreTransform.up, rayLength.z, rayMask);
            if (rightHit)
            {
                Debug.Log($"<color=green>RIGHT Connection available: {rightHit.transform.parent.name} </color>");
                ConnectionPoint cPoint;
                if (rightHit.transform.TryGetComponent(out cPoint))
                {
                    ConnectWith(cPoint, Direction.Right);
                }
            }
        }
    }
    private void ConnectWith(ConnectionPoint targetcPoint, Direction hitDirection)
    {
        targetcPoint.Connect(this, hitDirection);

        var rb = targetcPoint.GetComponent<Rigidbody2D>();
        var joint = connectionJoints[(int)hitDirection];

        joint.enabled = true;
        joint.connectedBody = rb;

        isConnectedList[(int)hitDirection] = true;

        connectedBodyAmount++;
        pMovement.UpgradeStats(connectedBodyAmount);
        connections.Add(new ConnectionPointData() { cPoint = targetcPoint, fixedJoint = joint });
    }
    public void DisconnectWith(ConnectionPoint targetcPoint, Direction disconnectDir)
    {
        var c = connections.Find(x => x.cPoint == targetcPoint);
        c.fixedJoint.connectedBody = null;
        c.fixedJoint.enabled = false;
        isConnectedList[(int)disconnectDir] = false;
        connectedBodyAmount--;
        pMovement.UpgradeStats(connectedBodyAmount);

        connections.Remove(c);
    }

    private void FixedUpdate()
    {
        if (!isItPlayer)
            return;

        CalculateRays();
    }
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}