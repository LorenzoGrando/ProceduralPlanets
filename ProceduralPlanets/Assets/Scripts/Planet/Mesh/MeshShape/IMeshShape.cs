using System.Collections.Generic;
using System;
using UnityEngine;

public interface IMeshShape
{
    public Mesh ConstructMesh(int resolution);
}
