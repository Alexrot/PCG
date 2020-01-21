
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    Transform poligon;    
    Triangulator tr;

    public void PolyGen(List<Vector2> a, Transform p)
    {
        tr = new Triangulator(a.ToArray());
        int[] indices = tr.Triangulate();
        Vector3[] vertices = new Vector3[a.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(a[i].x, a[i].y, 0);
        }
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        



        
        poligon = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);
        poligon.GetComponent<MeshFilter>().mesh = msh;
      
        

    }
}






