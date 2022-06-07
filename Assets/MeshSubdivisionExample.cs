using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;

public class MeshSubdivisionExample : MonoBehaviour
{
    private Mesh mesh;
    private HDMesh HDmesh;
    void Start()
    {
        InitMesh();

        HDmesh = new HDMesh();
        Color color = Color.white;
        Color colorFacade = Color.white;

        HDMeshFactory.AddBox(HDmesh, 3f, 2f, 1f, -3f, -2f, -1f, colorFacade);

    }
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            Debug.Log("start");
            StartCoroutine(subdivideWaitForSeconds(HDmesh, 1f));
            displayHDMesh(HDmesh);
        }
        //subdivide(HDmesh);
    }

    private void InitMesh()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;
    }

    IEnumerator subdivideWaitForSeconds(HDMesh HDmesh, float sec)
    {
        subdivide(HDmesh);
        yield return new WaitForSeconds(sec);

    }
    private void subdivide(HDMesh HDmesh)
    {
        HDMesh newMesh = new HDMesh();
        for (int i = 0; i < HDmesh.faces.Count; i++)
        {
            Vector3[] face_vertices = HDUtilsVertex.face_vertices(HDmesh, HDmesh.faces[i]);
            List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude(face_vertices, 1f);

            foreach (var new_face_vertices in new_faces_vertices)
            {
                newMesh.AddQuad(new_face_vertices, Color.white);
            }
        }
        this.HDmesh = newMesh;
    }

    private void displayHDMesh(HDMesh hdmesh)
    {
        HDMesh displayMesh = hdmesh.copy();

        displayMesh.TriangulateQuads();
        displayMesh.SeparateVertices();
        mesh.Clear();
        mesh.vertices = displayMesh.VertexArray();
        mesh.triangles = displayMesh.FlattenedTriangles();
        mesh.SetColors(displayMesh.Colors);
        mesh.RecalculateNormals();
    }
}
