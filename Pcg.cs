using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using csDelaunay;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Pcg : MonoBehaviour
{
    
    public Text tBase;
    int maxCanvas= 500; //base per poi non dover modificare anche la temperatura

    public int polygonNumber = 1000;



    int nPoly = 0;

    public Transform poligon;
    public Material mPlain;
    public Material mSea;
    public Material mMountain;
    Triangulator tr;
    public int seed;
    public float scale = 0.3f;
    public int octaveNumber = 8;
    public float persistance = 2f;
    public float lacunarity = 0.4f;

    List<Zone> mappa;

    public Transform poligono;

    private void Start()
    {
        mappa = new List<Zone>();
        seed = Random.Range(0, maxCanvas);
        Rect bounds = new Rect(0, 0, maxCanvas, maxCanvas);
        //punti randomici NON QUELLI DA UTILIZZARE
        List<Vector2> points = CreateRandomPoint(polygonNumber);
        Voronoi voronoi = new Voronoi(points, bounds, 2);
        List<Node> poligonoUno = new List<Node>();
        float[,] noise = Noise.GenerateNoiseMap(maxCanvas, maxCanvas,seed,scale,octaveNumber,persistance, lacunarity, new Vector2(maxCanvas/2,maxCanvas/2));

        
        foreach (Vector2 vor in voronoi.SiteCoords())
        {
            mappa.Add(new Zone(voronoi.Region(vor), vor, PolyGen(voronoi.Region(vor), poligon), noise[(int)vor.x,(int)vor.y]));
            Debug.Log(noise[(int)vor.x, (int)vor.y]);  
        }
       

    }

    private List<Vector2> CreateRandomPoint(int polygonNumber)
    {

        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < polygonNumber; i++)
        {
            points.Add(new Vector2(Random.Range(0, maxCanvas), Random.Range(0, maxCanvas)));

        }

        return points;
    }


    public void RecalcolateBiome(Renderer biome, int type)
    {

    }

    public Transform PolyGen(List<Vector2> a, Transform p)
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
        
        /*
        if (IsOdd(nPoly))
        {
            poligon.GetComponent<Renderer>().material = mSea;
        }
        else
        {
            
            if (IsOdd((int) a[0].x))
            {
                poligon.GetComponent<Renderer>().material = mPlain;
            }
            else
            {
                poligon.GetComponent<Renderer>().material = mMountain;
            }
        }*/


        poligon = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);
        poligon.GetComponent<MeshFilter>().mesh = msh;

        //m_Material.color = new Color32(255, 165, 0,0);
        poligon.name = nPoly.ToString();
        nPoly++;

        return poligon;


    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

}