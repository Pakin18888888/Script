using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareSanityLink : MonoBehaviour
{
    public float extraSanityDamage = 25f;

    public void TriggerSanityLoss()
    {
        if (sanitySystem.Instance != null)
        {
            sanitySystem.Instance.TakeSanityDamage(extraSanityDamage);
        }
    }
}
