using System;
using System.Collections.Generic;
using UnityEngine;

public class IcosphereMeshShape : IMeshShape
{
    private List<Vector3> vertices;
    private List<int> indices;
    private Dictionary<Int64, int> indicesCache;
    private int index;
    
    public Mesh ConstructMesh(int resolution)
    {
        Mesh mesh = new Mesh();
        vertices = new List<Vector3>();
        indices = new List<int>();
        indicesCache = new Dictionary<Int64, int>();
        index = 0;
        //Side lenghts in the golden ratio
        float t = (1f + Mathf.Sqrt(5f))/2f;
        
        //construct icosahedron vertices
        CreateVertex(-1, t, 0);
        CreateVertex(1, t, 0);
        CreateVertex(-1, -t, 0);
        CreateVertex(1, -t, 0);
        
        CreateVertex(0, -1, t);
        CreateVertex(0, 1, t);
        CreateVertex(0, -1, -t);
        CreateVertex(0, 1, -t);
        
        CreateVertex(t, 0, -1);
        CreateVertex(t, 0, 1);
        CreateVertex(-t, 0, -1);
        CreateVertex(-t, -0, 1);
        
        //icosahedron faces
        CreateTriangle(indices, 0, 11, 5);
        CreateTriangle(indices, 0, 5, 1);
        CreateTriangle(indices, 0, 1, 7);
        CreateTriangle(indices, 0, 7, 10);
        CreateTriangle(indices, 0, 10, 11);
        
        CreateTriangle(indices, 2, 11, 10);
        CreateTriangle(indices, 11, 4, 5);
        CreateTriangle(indices, 5, 9, 1);
        CreateTriangle(indices, 8, 7, 1);
        CreateTriangle(indices, 7, 6, 10);
        
        CreateTriangle(indices, 3, 2, 6);
        CreateTriangle(indices, 3, 4, 2);
        CreateTriangle(indices, 3, 9, 4);
        CreateTriangle(indices, 3, 8, 9);
        CreateTriangle(indices, 3, 6, 8);
        
        CreateTriangle(indices, 6, 2, 10);
        CreateTriangle(indices, 2, 4, 11);
        CreateTriangle(indices, 4, 9, 5);
        CreateTriangle(indices, 9, 8, 1);
        CreateTriangle(indices, 8, 6, 7);
        
        //Subdivide icosahedron
        for (int i = 0; i < resolution; i++)
        {
            List<int> subdividedIndices = new List<int>();
            for (int j = 0; j < indices.Count; j += 3)
            {
                int a = GetMiddlePoint(indices[j], indices[j + 1]);
                int b = GetMiddlePoint(indices[j + 1], indices[j + 2]);
                int c = GetMiddlePoint(indices[j + 2], indices[j]);
                
                CreateTriangle(subdividedIndices, indices[j], a, c);
                CreateTriangle(subdividedIndices, indices[j + 1], b, a);
                CreateTriangle(subdividedIndices, indices[j + 2], c, b);
                CreateTriangle(subdividedIndices, a, b, c);
            }
            
            indices = new List<int>(subdividedIndices);
        }
        
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        return mesh;
    }

    private int CreateVertex(float x, float y, float z)
    {
        float length = Mathf.Sqrt(x * x + y * y + z * z);
        vertices.Add(new Vector3(x/length, y/length, z/length));

        return index++;
    }

    private void CreateTriangle(List<int> targetList, int v1, int v2, int v3) 
    {
        targetList.Add(v1);
        targetList.Add(v2);
        targetList.Add(v3);
    }

    private int GetMiddlePoint(int p1, int p2)
    {
        bool firstIsSmaller = p1 < p2;
        Int64 smaller = firstIsSmaller ? p1 : p2;
        Int64 larger = firstIsSmaller ? p2 : p1;
        Int64 key = (smaller << 32) + larger;

        int res;
        if (indicesCache.TryGetValue(key, out res))
            return res;
        
        Vector3 v1 = vertices[p1];
        Vector3 v2 = vertices[p2];
        Vector3 middle = new Vector3
        (
            (v1.x + v2.x) / 2f,
            (v1.y + v2.y) / 2f,
            (v1.z + v2.z) / 2f
        );
        
        int i = CreateVertex(middle.x, middle.y, middle.z);
        indicesCache.Add(key, i);
        return i;
    }
    
    
}