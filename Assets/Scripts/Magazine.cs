using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public int roundsRemaining;

    public bool IsEmpty()
    {
        if (roundsRemaining <= 0) return true; else { return false; }
    }
}
