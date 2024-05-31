using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    public Transform targetObject; // Use Transform instead of GameObject for the starting point

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Assuming "Fire1" is the button for shooting
        {
            ShootRaycast();
        }
    }

    public void ShootRaycast()
    {
        RaycastHit hit;

        // Shoot a raycast from targetObject position forward
        if (Physics.Raycast(targetObject.position, targetObject.forward, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                // If the raycast hits an object with the "Enemy" tag, delete it
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
