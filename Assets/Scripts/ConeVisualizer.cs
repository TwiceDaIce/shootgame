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
        coneMesh.name = "ViewCone";
        GetComponent<MeshFilter>().mesh = coneMesh;
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];
        Vector3[] normals = new Vector3[segments + 1]; // New array to store normals

        // Center vertex
        vertices[0] = Vector3.zero;
        normals[0] = Vector3.zero; // Center normal is (0, 0, 0)

        // Generate vertices and normals
        for (int i = 1; i <= segments; i++)
        {
            float angle = (float)i / segments * fieldOfViewAngle * Mathf.Deg2Rad;
            Vector3 vertexPosition = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * Vector3.up * viewDistance;
            vertices[i] = vertexPosition;
            normals[i] = vertexPosition.normalized; // Normalized vertex position
        }

        // Generate triangles
        for (int i = 0; i < segments - 1; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.normals = normals; // Set the normals
        coneMesh.RecalculateBounds();
    }

    private void Update()
    {
        // Always recalculate cone mesh vertices
        //CreateConeMesh();

        // Check for collisions with objects tagged as 'Visblock'
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, viewDistance);

        if (true)
        {
            // Find the closest hit point
            if (hits.Length > 0)
            {
                RaycastHit closestHit = hits[0];
                foreach (RaycastHit hit in hits)
                {
                    if (!hit.collider.CompareTag("Transparent") && hit.distance < closestHit.distance)
                    {
                        closestHit = hit;
                    }
                }
            }

            // Calculate intersection points
            Vector3[] vertices = coneMesh.vertices;
            for (int i = 1; i <= segments; i++)
            {
                float angle = (float)i / segments * fieldOfViewAngle * Mathf.Deg2Rad;
                Vector3 direction = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0) * transform.forward;
                Ray ray = new Ray(transform.position, direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, viewDistance))
                {
                    vertices[i] = transform.InverseTransformPoint(hit.point);
                }
                else
                {
                    vertices[i] = transform.InverseTransformPoint(transform.position + direction * viewDistance);
                }
            }

            // Update cone mesh vertices
            coneMesh.vertices = vertices;
            coneMesh.RecalculateNormals();
            coneMesh.RecalculateBounds();
        }
    }
}