using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeVisualizer : MonoBehaviour
{
    public float fieldOfViewAngle = 45f;
    public float viewDistance = 10f;
    public int segments = 50;

    private Mesh coneMesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        // Create cone mesh
        CreateConeMesh();

        // Get mesh filter and renderer components
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Apply material to the cone mesh
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Toggle visibility
        meshRenderer.enabled = false; // Set to true to visualize the cone
    }

    public void CreateConeMesh()
    {
        coneMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = coneMesh;
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        // Center vertex
        vertices[0] = Vector3.zero;

        // Generate vertices
        for (int i = 1; i <= segments; i++)
        {
            float angle = (float)i / segments * fieldOfViewAngle * Mathf.Deg2Rad;
            vertices[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * viewDistance;
        }

        // Generate triangles
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.RecalculateNormals();
        coneMesh.RecalculateBounds();
    }

    private void Update()
    {
        // Check for collisions with objects tagged as 'Visblock'
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, viewDistance))
        {
            if (hit.collider.CompareTag("Visblock"))
            {
                // Calculate the clipping point
                float distanceToHit = hit.distance;
                float angleToHit = Mathf.Atan2(hit.point.y, hit.point.x) * Mathf.Rad2Deg;
                float clippedAngle = Mathf.Clamp(angleToHit, -fieldOfViewAngle / 2f, fieldOfViewAngle / 2f);
                float clippedDistance = distanceToHit / Mathf.Cos(clippedAngle * Mathf.Deg2Rad);

                // Adjust the vertices of the cone mesh to clip at the point of occlusion
                Vector3[] vertices = coneMesh.vertices;
                vertices[segments] = new Vector3(Mathf.Cos(clippedAngle * Mathf.Deg2Rad) * clippedDistance, Mathf.Sin(clippedAngle * Mathf.Deg2Rad) * clippedDistance, 0f);
                coneMesh.vertices = vertices;
                coneMesh.RecalculateNormals();
                coneMesh.RecalculateBounds();
            }
        }
        else
        {
            // Reset the vertices if not occluded
            CreateConeMesh();
        }
    }
}