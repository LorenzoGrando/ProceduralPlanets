using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    #region Dependencies
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    #endregion
    
    #region Parameters

    [Header("Mesh Shape")] 
    public MeshShape.MeshShapeType shapeType;
    [Range(0, 6)] 
    public int resolution = 3;
    
    #endregion
    
    private bool initialized = false;

    private IMeshShape meshShape;
    private Mesh currentMesh;
    
    #region Unity Methods
    private void Start()
    {
        initialized = false;
        GetDependencies();
    }

    #endregion

    private void GetDependencies()
    {
        meshFilter ??= GetComponent<MeshFilter>();
        meshRenderer ??= GetComponent<MeshRenderer>();
        
        initialized = true;
    }

    public void UpdateMeshShape()
    {
        if (meshShape != null)
        {
            meshShape = null;
        }
        meshShape = MeshShape.GetMeshShapeFromType(shapeType);
    }

    public void GenerateBaseMesh()
    {
        if(!initialized)
            GetDependencies();
        
        if (currentMesh != null)
        {
#if UNITY_EDITOR
            if(!Application.isPlaying)
                DestroyImmediate(currentMesh);        
            else
                Destroy(currentMesh);
#else
            Destroy(currentMesh);
#endif
        }
        
        UpdateMeshShape();
        currentMesh = meshShape.ConstructMesh(resolution);
        meshFilter.mesh = currentMesh;
    }
    
    #region Debug
#if UNITY_EDITOR
    [Header("Debug")]
    public bool drawVertices;

    private void OnDrawGizmos()
    {
        if (meshFilter == null) return;
        Mesh mesh = Application.isPlaying ? meshFilter.mesh : meshFilter.sharedMesh;
        
        if (drawVertices && mesh != null)
        {
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.position + mesh.vertices[i], 0.025f);
            }
        }
    }
#endif
    #endregion
}
