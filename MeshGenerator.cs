
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    public Transform poligon;
    
    Mesh mesh;

    // Use this for initialization
    void Start()
    {

        
         
        /*
        Vector3[] vertices = new Vector3[5];

        vertices[0] = new Vector3(.5f, .5f);
        vertices[1] = new Vector3(-1, .8f);
        vertices[2] = new Vector3(-.2f, 1.2f);
        vertices[3] = new Vector3(.8f, 1.4f);
        vertices[4] = new Vector3(1, .8f);
         
        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        */
        //GetComponent<MeshFilter>().mesh = mesh;
        //newPoly.GetComponent<MeshFilter>().mesh = mesh;
        
    }

    public void PolyGen(List<Node> poligono, Transform p)
    {

        mesh = new Mesh();
        poligon = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);

        int pos=0;
        Vector3[] vertice = new Vector3[20];
        foreach(Node a in poligono)
        {
            vertice[pos] = a.GetPosition();
            pos++;
        }
        mesh.vertices = vertice;
        //List<int> point = new List<int>();
        //int i = 0;

        /*
        while (true)
        {
            
            point.Add(0);
            i++;
            point.Add(i);
            i++;
            point.Add(i);
            
            if (i == point.Count-1)
            {
                break;
            }
            //i--;
        }
        */
        //provo a dare array gia prestabiliti
        int[] points = new int[poligono.Count];
        switch (poligono.Count)
            {
            case 3:
                points = new int[] { 0, 1, 2};
                break;
            case 4:
                points = new int[] { 0, 1, 2, 0, 2, 3 };
                break;
            case 5:
                points = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4};
                break;
            case 6:
                points = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5};
                break;
            case 7:
                points = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6};
                break;
            case 8:
                points = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6 ,7};
                break;
            case 9:
                points = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 7, 0, 7, 8 };
                break;
            case 10:
                points = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 7, 0, 7, 8, 0, 8, 9};
                break;
                
        }
        
        
        
        mesh.triangles = points;
        poligon.GetComponent<MeshFilter>().mesh = mesh;
        Debug.Log(points.Length+"numero"+ poligono.Count);
        foreach(Node a in poligono)
        {
            Debug.Log(a.position);
        }


    }


}



    


