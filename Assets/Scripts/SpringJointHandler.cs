using UnityEngine;

public class SpringJointHandler : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private Transform t;
    [SerializeField] private int jointCount = 8;

    [Space(20)]

    [Header("Settings")]
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float dampingRatio = 0.5f;
    [SerializeField] private bool autoConfigureDistance = true;
    [SerializeField] private bool autoConfigureConnectedAnchor = true;

    [ContextMenu("Create Joints")]
    private void CreateJoints()
    {
        for (int i = 0; i < jointCount; i++)
        {
            var joint = t.gameObject.AddComponent<SpringJoint2D>();
            joint.connectedBody = t.parent.GetChild(i).GetComponent<Rigidbody2D>();
        }
        Debug.Log($"<color=green>Joints Added to {t.name} </color>");
    }
    [ContextMenu("Setup Settings")]
    private void SetupSettings()
    {
        for(int i = 0; i < jointCount; i++)
        {
            var j = t.GetComponents<SpringJoint2D>();
            foreach(var joint in j)
            {
                joint.frequency = frequency;
                joint.dampingRatio = dampingRatio;
                joint.autoConfigureDistance = autoConfigureDistance;
                joint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            }
        }
        Debug.Log($"<color=yellow>Joints Modified from {t.name} </color>");
    }
    [ContextMenu("Remove Joints")]
    private void RemoveJoints()
    {
        for (int i = 0; i < jointCount; i++)
        {
            var joint = t.GetComponent<SpringJoint2D>();
            DestroyImmediate(joint);
        }
        Debug.Log($"<color=red>Joints Removed from {t.name} </color>");
    }
}