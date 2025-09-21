using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent OnInteractStart;
    [Space(20)]
    [SerializeField] UnityEvent OnInteractEnd;

    public void OnInteractStarted()
    {
        OnInteractStart?.Invoke();
    }
    public void OnInteractEnded()
    {
        OnInteractEnd?.Invoke();
    }
}
public interface IInteractable
{
    void OnInteractStarted();
    void OnInteractEnded();
}