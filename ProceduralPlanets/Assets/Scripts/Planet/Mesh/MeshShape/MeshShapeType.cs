using UnityEngine;
using System;


public static class MeshShape
{
    public enum MeshShapeType
    {
        Icosphere
    }

    public static IMeshShape GetMeshShapeFromType(MeshShapeType meshShapeType)
    {
        switch (meshShapeType)
        {
            case MeshShapeType.Icosphere:
                return new IcosphereMeshShape();
        }

        return null;
    }
}
