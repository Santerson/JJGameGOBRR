using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] Texture2D mouseTexture;
    [SerializeField] Vector2 Offset;
    static MouseCursor instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Cursor.SetCursor(mouseTexture, Offset, CursorMode.ForceSoftware);
    }
}
