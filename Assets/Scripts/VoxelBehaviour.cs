using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;

public class VoxelBehaviour : MonoBehaviour
{
    public Material material;

    int nX = 16;
    int nY = 7;
    int nZ = 16;
    private HDGrid<bool> grid;
    private Mesh mesh;

    void Start()
    {
        InitMesh();

        grid = InitGrid(nX, nY, nZ);

        HDMesh myMesh = new HDMesh();
        for (int x = 0; x < nX; x++)
        {
            for (int y = 0; y < nY; y++)
            {
                for (int z = 0; z < nZ; z++)
                {
                    if (grid[x, y, z])
                    {

                        int index = grid.GetIndex(x, y, z);

                        Color colorFacade = Color.white;
                        HDMeshFactory.AddBox(myMesh, x, y, z, x + 1, y + 1f, z + 1, colorFacade);

                    }
                }
            }
        }

        myMesh.SeparateVertices();
        myMesh.FillUnityMesh(mesh);
    }

    private HDGrid<bool> InitGrid(int nX, int nY, int nZ)
    {
        HDGrid<bool> grid = new HDGrid<bool>(nX, nY, nZ);
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i] = Random.value > 0.5;
        }
        return grid;
    }

    private void InitMesh()
    {
        // init mesh filter
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (null == meshFilter)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
        }
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;

        //// init mesh renderer
        //MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        //if (renderer == null)
        //{
        //    renderer = gameObject.AddComponent<MeshRenderer>();
        //}
        //renderer.material = material;
    }
}
