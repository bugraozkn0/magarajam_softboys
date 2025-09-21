using UnityEngine;
using System.Collections.Generic;
public class FaceHandler : MonoBehaviour
{
    [SerializeField] List<Face> faces = new();
    [SerializeField] SpriteRenderer sRenderer;
    [SerializeField] bool isMainPlayer;

    private bool isActive = false;
    private void Start()
    {
        if (!isMainPlayer)
            sRenderer.enabled = false;
    }

    public void ChangeFace(FaceType type)
    {
        if (!isActive)
        {
            sRenderer.enabled = true;
            isActive = true;
            return;
        }
        sRenderer.sprite = faces.Find(x => x.facetype == type).sprite;
    }
    [ContextMenu("Random Face")]
    private void TestRandomFace()
    {
        ChangeFace((FaceType)Random.Range(0, faces.Count));
    }
}
[System.Serializable]
public struct Face
{
    public FaceType facetype;
    public Sprite sprite;
}
public enum FaceType
{
    Dead,
    Happy,
    Sad,
    Scared,
    Excited
}