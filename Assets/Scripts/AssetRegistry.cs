using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetRegistry : MonoBehaviour
{
    public enum AssetType
    {
        GUN
    }
    public bool RegisterItem(AssetType type, object Item)
    {
        return true; // return true if register was successful
    }
}
