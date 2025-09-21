using UnityEngine;

public class AnimationPlayerHandler : MonoBehaviour
{
    [SerializeField] private AnimationClip openClip, closeClip;

    private Animation animationC;
    private void Awake() => animationC = GetComponent<Animation>();

    public void PlayOpenAnimation()
    {
        animationC.clip = openClip;
        animationC.Play();
    }
    public void PlayCloseAnimation()
    {
        animationC.clip = closeClip;
        animationC.Play();
    }
}