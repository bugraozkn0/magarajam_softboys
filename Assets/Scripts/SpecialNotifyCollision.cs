using UnityEngine;

public class SpecialNotifyCollision : MonoBehaviour
{
    private PlayerMovement pm;
    private void Awake()
    {
        pm = transform.root.GetComponent<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<IInteractable>().OnInteractStarted();
        }
        if(other.CompareTag("Launchpad"))
        {
            int f = other.GetComponent<CustomLaunchPad>().GetForceAmount();
            pm.AddJumpForce(f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<IInteractable>().OnInteractEnded();
        }
    }
}