﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public static class Extentions
    {
        public static Vector3 ToUnityVector3(this Assimp.Vector3D vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Mesh ToUnityMesh(this Assimp.Mesh mesh)
        {
            Mesh result = new Mesh
            {
                name = mesh.Name,
                vertices = mesh.Vertices.Select(x => x.ToUnityVector3()).ToArray(),
                triangles = mesh.GetIntIndices().Reverse().ToArray(),
                normals = mesh.Normals.Select(x => x.ToUnityVector3()).ToArray(),
                uv = mesh.GetTextureCoords(0).Select(x => new Vector2(x.X, x.Y)).ToArray()
            };
            return result;
        }

        public static Matrix4x4 ToUnityMatrix(this Assimp.Matrix4x4 matrix)
        {
            Matrix4x4 unityMatrix = new Matrix4x4();

            unityMatrix.SetRow(0, new Vector4(matrix.A1, matrix.A2, matrix.A3, matrix.A4));
            unityMatrix.SetRow(1, new Vector4(matrix.B1, matrix.B2, matrix.B3, matrix.B4));
            unityMatrix.SetRow(2, new Vector4(matrix.C1, matrix.C2, matrix.C3, matrix.C4));
            unityMatrix.SetRow(3, new Vector4(matrix.D1, matrix.D2, matrix.D3, matrix.D4));

            return unityMatrix;
        }

        public static Color ToUnityColor(this Assimp.Color4D color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
