using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ConicalMeshGenerator : MonoBehaviour
{
    public int numSegments = 50; // Number of segments around the cone
    public float heightMultiplier = 1f; // Multiplier for the height
    public float radiusMultiplier = 1f; // Multiplier for the radius
    public float projectionDistance = 1f; // Distance to project out the cone
    public bool performCollisionDetection = true; // Flag to control whether to perform collision detection

    private MeshFilter meshFilter;
    private Vector3 apexVertex;
    private Vector3 baseVertex;
    private Vector3[] topVertices;
    private Vector3[] bottomVertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        GenerateMesh();
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        // Calculate vertices
        CalculateVertices();

        // Combine vertices
        CombineVertices();

        // Generate triangles
        int numTriangles = numSegments * 2 * 3; // Each segment has 2 triangles, each triangle has 3 vertices
        int[] triangles = new int[numTriangles];
        int t = 0;
        for (int i = 1; i <= numSegments; i++)
        {
            int nextIndex = (i % numSegments) + 1;

            // Top triangle
            triangles[t++] = i;
            triangles[t++] = nextIndex;
            triangles[t++] = 0;

            // Bottom triangle
            triangles[t++] = i + numSegments;
            triangles[t++] = topVertices.Length - 1; // Base vertex
            triangles[t++] = nextIndex + numSegments;
        }
        mesh.triangles = triangles;

        // Calculate normals
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    void CalculateVertices()
    {
        // Vertices
        int numVertices = numSegments * 2 + 2; // Perimeter vertices for top and bottom, plus apex and base vertex

        // Apex vertex
        apexVertex = Vector3.zero;

        // Base vertex
        baseVertex = new Vector3(0f, 0f, projectionDistance);

        // Top vertices
        topVertices = new Vector3[numSegments];
        float angleIncrement = Mathf.PI * 2f / numSegments;
        for (int i = 0; i < numSegments; i++)
        {
            float angle = i * angleIncrement;
            float x = Mathf.Sin(angle) * radiusMultiplier;
            float y = Mathf.Cos(angle) * heightMultiplier;
            topVertices[i] = new Vector3(x, y, projectionDistance);

            // Perform collision detection if enabled
            if (performCollisionDetection)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, topVertices[i], out hit))
                {
                    topVertices[i] = hit.point; // Update vertex to intersection point
                }
            }
        }

        // Bottom vertices
        bottomVertices = new Vector3[numSegments];
        for (int i = 0; i < numSegments; i++)
        {
            bottomVertices[i] = topVertices[i]; // Bottom vertices are the same as top vertices, as they share the same position
            bottomVertices[i].z = 0f; // Set z-coordinate to 0 for the base
        }
    }

    void CombineVertices()
    {
        // Combine all vertices into a single array
        Vector3[] vertices = new Vector3[topVertices.Length + bottomVertices.Length + 2]; // +2 for apex and base vertex
        vertices[0] = apexVertex; // Apex vertex
        vertices[1] = baseVertex; // Base vertex

        // Set the rest of the vertices
        for (int i = 0; i < topVertices.Length; i++)
        {
            vertices[i + 2] = topVertices[i]; // Set top vertices after apex and base
        }

        for (int i = 0; i < bottomVertices.Length; i++)
        {
            vertices[i + topVertices.Length + 2] = bottomVertices[i]; // Set bottom vertices after top vertices
        }

        // Assign the combined vertices back to the mesh
        meshFilter.mesh.vertices = vertices;
    }


    private void Update()
    {
        GenerateMesh();
    }
}
