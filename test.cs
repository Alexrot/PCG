using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


using csDelaunay;

using Random = UnityEngine.Random;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    int nPoly = 0;
    
    public Transform poligon;
    public Material mPlain;
    public Material mSea;
    public Material mMountain;

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

        if (IsOdd(nPoly))
        {
            poligon.GetComponent<Renderer>().material = mSea;
        }
        else
        {
            poligon.GetComponent<Renderer>().material = mPlain;
        }


        poligon = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);
        poligon.GetComponent<MeshFilter>().mesh = msh;
        
        //m_Material.color = new Color32(255, 165, 0,255);
        poligon.name = nPoly.ToString();
        nPoly++;


        

    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        Mesh mesh = new Mesh();
        Transform newPoly = Instantiate(poligon, new Vector3(0, 0, 0), Quaternion.identity);

      
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(500, 0);
        vertices[2] = new Vector3(0, 500);
        vertices[3] = new Vector3(500, 500);
       //vertices[4] = new Vector3(1, .8f);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3,};
        //GetComponent<MeshFilter>().mesh = mesh;
        newPoly.GetComponent<MeshFilter>().mesh = mesh;

        */


        

        

        Rect bounds = new Rect(0, 0, 512, 512);
        //punti randomici NON QUELLI DA UTILIZZARE
        List<Vector2> points = CreateRandomPoint(1000);
        Voronoi voronoi = new Voronoi(points, bounds, 2);
        List<Node> poligonoUno = new List<Node>();
        






        List<List<Vector2>> b = voronoi.Regions();
        foreach(List<Vector2> a in b)
        {
            PolyGen(a, poligon);
        }
        
        foreach (Vector2 a in voronoi.Region(points[0]))
        {
            poligonoUno.Add(new Node(a));
        }

        foreach(Node a in poligonoUno)
        {
            Debug.Log("il primo poligono ha il nodo " + a);
        }
        
        //poliGen.PolyGen(a, poligon);

        











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
        
        
        mesh.vertices = vertices;
        Node a =nodes.Find(x => x.position.Equals(new Vector3(.5f, .5f)));
        bool b = nodes.Exists(x => x.position.Equals(new Vector3(1f, .5f)));
        
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        //GetComponent<MeshFilter>().mesh = mesh;
        newPoly.GetComponent<MeshFilter>().mesh = mesh;

        
        //GUI.DrawTexture(new Rect(10, 10, 60, 60), aTexture, ScaleMode.ScaleToFit, true, 10.0F);
        
        /*
        int[] lastTwo = { 1,0,1,2,3,6,5,3,7,8,9};
        int[] b = lastTwo.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToArray();
        foreach (int a in b)
        {
            Debug.Log("funge "+a);
        }
        */
    }


    private List<Vector2> CreateRandomPoint(int polygonNumber)
    {

        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < polygonNumber; i++)
        {
            points.Add(new Vector2(Random.Range(0, 512), Random.Range(0, 512)));

        }

        return points;
    }
}


