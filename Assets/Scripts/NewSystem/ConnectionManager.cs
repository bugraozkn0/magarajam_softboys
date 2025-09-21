using System.Collections.Generic;
using UnityEngine;
using NoRyte.TickSystem;

public class ConnectionManager : MonoBehaviour, ITickable
{
    [Header("Main Settings")]
    public bool isMainPlayer = false;
    public LayerMask connectionLayer;

    [Header("Force Break Settings")]
    public bool enableForceBreak = true;
    public float breakForce = 1000f;

    [Header("Components")]
    public ConnectionSocket[] sockets;
    public List<CircleCollider2D> boneColliders = new();

    public PlayerMovement playerMovement;
    public FaceHandler faceHandler;

    public HashSet<ConnectionManager> connectedBodies = new();

    private void Awake()
    {
        IgnoreSelfCollision();
        foreach (var s in sockets)
            s.Initialize(this);
    }

    private void OnEnable() => TimedTickManager.Instance.RegisterTimedTickable(this, 1);
    private void OnDisable() => TimedTickManager.Instance.UnregisterTimedTickable(this);

    public void OnTick()
    {
        if (enableForceBreak)
            CheckJointForces();
    }
    public void DisconnectFromEverything()
    {
        foreach (var socket in sockets)
        {
            if (socket.IsConnected)
            {
                socket.Disconnect();
            }
            socket.isDead = true;
            socket.enabled = false;
        }
        foreach(var c in boneColliders)
            c.enabled = false;

        playerMovement.enabled = false; // OPSIYONEL
        this.enabled = false; // OPSIYONEL
    }

    private void CheckJointForces()
    {
        foreach (var socket in sockets)
        {
            if (socket.IsConnected && socket.Joint != null && socket.Joint.enabled)
            {
                if (socket.Joint.reactionForce.magnitude > breakForce)
                {
                    socket.Disconnect();
                }
            }
        }
    }

    private void IgnoreSelfCollision()
    {
        for (int i = 0; i < boneColliders.Count; i++)
            for (int j = 0; j < boneColliders.Count; j++)
                if (i != j)
                    Physics2D.IgnoreCollision(boneColliders[i], boneColliders[j]);
    }

    public void AddConnection(ConnectionManager target)
    {
        if (!connectedBodies.Contains(target))
            connectedBodies.Add(target);
        UpdateChainState();
    }

    public void RemoveConnection(ConnectionManager target)
    {
        if (connectedBodies.Contains(target))
            connectedBodies.Remove(target);
        UpdateChainState();
    }

    private void UpdateChainState()
    {
        var visited = new HashSet<ConnectionManager>();
        var stack = new Stack<ConnectionManager>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Contains(current)) continue;
            visited.Add(current);

            current.UpdateConnectionState();

            foreach (var body in current.connectedBodies)
                if (!visited.Contains(body))
                    stack.Push(body);
        }
    }

    private void UpdateConnectionState()
    {
        bool hasConnection = connectedBodies.Count > 0;
        bool connectedToMain = IsConnectedToMainPlayer();

        if (isMainPlayer)
        {
            playerMovement.enabled = true;
            faceHandler.ChangeFace(connectedToMain ? FaceType.Happy : FaceType.Sad);
        }
        else if (!hasConnection)
        {
            playerMovement.enabled = false;
            faceHandler.ChangeFace(FaceType.Dead);
        }
        else if (!connectedToMain)
        {
            playerMovement.enabled = false;
            faceHandler.ChangeFace(FaceType.Scared);
        }
        else
        {
            playerMovement.enabled = true;
            faceHandler.ChangeFace(FaceType.Happy);
        }   
    }

    public bool IsConnectedToMainPlayer()
    {
        var visited = new HashSet<ConnectionManager>();
        return SearchMainPlayer(this, visited);
    }

    private bool SearchMainPlayer(ConnectionManager current, HashSet<ConnectionManager> visited)
    {
        if (current == null || visited.Contains(current)) return false;
        visited.Add(current);

        if (current.isMainPlayer) return true;

        foreach (var body in current.connectedBodies)
            if (SearchMainPlayer(body, visited)) return true;

        return false;
    }
}