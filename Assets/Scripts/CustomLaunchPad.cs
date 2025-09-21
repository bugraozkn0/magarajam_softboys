using System.Collections;
using UnityEngine;

public class CustomLaunchPad : MonoBehaviour
{
    [SerializeField] int forceAmount = 200;

    private AnimationPlayerHandler aph;
    private void Awake()
    {
        aph = transform.GetChild(0).GetComponent<AnimationPlayerHandler>();
    }
    public int GetForceAmount()
    {
        aph.PlayOpenAnimation();
        StartCoroutine(DelayB());

        return forceAmount;
    }
    IEnumerator DelayB()
    {
        yield return new WaitForSeconds(0.5f);
        aph.PlayCloseAnimation();
    }
}