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
        Mesh mesh = new Mesh();
        Transform newPoly = Instantiate(poligon, new Vector3(0, 0, 0), Quaternion.identity);

      
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
















        /*


        Transform newPoly = Instantiate(poligon, new Vector3(0, 0, 0), Quaternion.identity);

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[7];
        List<Node> nodes = new List<Node>();

        vertices[0] = new Vector3(1f, .5f);
        vertices[1] = new Vector3(1.5f, .5f);
        vertices[2] = new Vector3(.5f, 1.5f);
        /*
        vertices[3] = new Vector3(.25f, .5f);
        vertices[4] = new Vector3(.5f, .5f);
        vertices[5] = new Vector3(-1, .8f);
        vertices[6] = new Vector3(-.2f, 1.2f);
        //vertices[3] = new Vector3(.8f, 1.4f);
        //vertices[4] = new Vector3(1, .8f);
        nodes.Add(new Node(vertices[0]));
        nodes.Add(new Node(vertices[1]));
        nodes.Add(new Node(vertices[2]));
        nodes.Add(new Node(vertices[3]));
        nodes.Add(new Node(vertices[4]));
        nodes.Add(new Node(vertices[5]));
        nodes.Add(new Node(vertices[6]));
        */
        /*
        mesh.vertices = vertices;
        Node a =nodes.Find(x => x.position.Equals(new Vector3(.5f, .5f)));
        bool b = nodes.Exists(x => x.position.Equals(new Vector3(1f, .5f)));
        
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        //GetComponent<MeshFilter>().mesh = mesh;
        newPoly.GetComponent<MeshFilter>().mesh = mesh;

        
        //GUI.DrawTexture(new Rect(10, 10, 60, 60), aTexture, ScaleMode.ScaleToFit, true, 10.0F);

        /*
        int[] lastTwo = { 1,0,1,2,3,6,5,4,7,8,9};
        if (lastTwo.GroupBy(n => n).Any(c => c.Count() > 1))
        {
            Debug.Log("funge");
        }
        */
    }
}





