using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    private bool isConnected = false;

    private ConnectionHandler connectHandler;
    private Direction connectedDirection;
    public void Connect(ConnectionHandler ch, Direction cDir)
    {
        isConnected = true;
        connectHandler = ch;
        connectedDirection = cDir;
    }
    public void Disconnect()
    {
        if(!isConnected) return;

        connectHandler.DisconnectWith(this, connectedDirection);

        isConnected = false;
        connectHandler = null;
    }
}