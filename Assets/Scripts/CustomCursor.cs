using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorImg;
    [SerializeField] Vector2 clickPos = Vector2.zero;
    private void Awake()
    {
        Cursor.SetCursor(cursorImg, clickPos, CursorMode.Auto);
    }
}