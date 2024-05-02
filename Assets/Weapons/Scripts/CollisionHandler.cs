using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public delegate void CollisionEvent(GameObject other);
    public static event CollisionEvent OnCollisionOccured;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("Collided");
        if (OnCollisionOccured != null)
        {
            OnCollisionOccured(collision.gameObject);
            Debug.Log("Object Seen!");
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.LogWarning("Player Seen?");
            }
        }
    }

    private void Start()
    {
        Debug.Log("Don't Remove Me");
    } 
    
}
