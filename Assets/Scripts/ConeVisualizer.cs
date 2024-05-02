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
        meshRenderer.enabled = true; // Set to true to visualize the cone
        GetComponent<MeshCollider>().sharedMesh = coneMesh;
    }

    public void CreateConeMesh()
    {
        coneMesh = new Mesh();
        coneMesh.name = "ViewCone";
        GetComponent<MeshFilter>().mesh = coneMesh;

        int numVertices = segments * 3 + 1; // Vertices for top, bottom, and edge points
        int numTriangles = segments * 3; // Triangles for side faces

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles * 3]; // Each triangle has 3 vertices
        Vector3[] normals = new Vector3[numVertices];

        // Generate vertices for the top circle
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);
            vertices[i] = new Vector3(x, 1, z); // Top circle
        }

        // Generate vertices for the bottom circle and apex
        vertices[segments] = Vector3.zero; // Apex (center)
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);
            vertices[i + segments + 1] = new Vector3(x, 0, z); // Bottom circle
        }

        // Set normals for top circle vertices (pointing upwards)
        for (int i = 0; i < segments; i++)
        {
            normals[i] = Vector3.up;
        }

        // Set normals for bottom circle and apex vertices (pointing downwards)
        for (int i = segments; i < numVertices; i++)
        {
            normals[i] = Vector3.down;
        }

        // Generate triangles for side faces
        for (int i = 0; i < segments; i++)
        {
            int baseIndex = i * 3;
            int nextIndex = (i + 1) % segments;

            // Side face triangles
            triangles[baseIndex] = i;
            triangles[baseIndex + 1] = (i + 1) % segments;
            triangles[baseIndex + 2] = segments; // Apex

            triangles[baseIndex + segments * 3] = i + segments + 1;
            triangles[baseIndex + segments * 3 + 1] = nextIndex + segments + 1;
            triangles[baseIndex + segments * 3 + 2] = segments; // Apex
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.normals = normals;
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