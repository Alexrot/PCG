using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    
     public Transform poligon;


    // Start is called before the first frame update
    void Start()
    {
        //poligon= (Transform)Resources.Load("Assets/Poligono", typeof(Transform));
        //poligon= (Transform)GameObject.FindGameObjectsWithTag("Item");

        Transform newPoly = Instantiate(poligon, new Vector3(0, 0, 0), Quaternion.identity);

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[5];

        vertices[0] = new Vector3(.5f, .5f);
        vertices[1] = new Vector3(-1, .8f);
        vertices[2] = new Vector3(-.2f, 1.2f);
        vertices[3] = new Vector3(.8f, 1.4f);
        vertices[4] = new Vector3(1, .8f);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        //GetComponent<MeshFilter>().mesh = mesh;
        newPoly.GetComponent<MeshFilter>().mesh = mesh;


    }
}





