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
    int maxCanvas= 2000; //base per poi non dover modificare anche la temperatura

    public int polygonNumber = 1000;



    int nPoly = 0;

    public Transform poligon;
    /*
    public Material mPlain;
    public Material mSea;
    public Material mMountain;
    */
    Triangulator tr;
    public int seed;
    public float scale = 0.3f;
    public int octaveNumber = 8;
    public float persistance = 2f;
    public float lacunarity = 1.4f;

    List<Zone> mappa;
    

    Voronoi voronoi;
    public Transform poligono;

    private void Start()
    {
        mappa = new List<Zone>();
        //seed = Random.Range(0, maxCanvas);
        Rect bounds = new Rect(0, 0, maxCanvas, maxCanvas);
        //punti randomici NON QUELLI DA UTILIZZARE
        List<Vector2> points = CreateRandomPoint(polygonNumber);
         voronoi = new Voronoi(points, bounds, 3);
        List<Node> poligonoUno = new List<Node>();
        float[,] noise = Noise.GenerateNoiseMap(maxCanvas, maxCanvas,seed,scale,octaveNumber,persistance, lacunarity, new Vector2(maxCanvas/2,maxCanvas/2));

        Transform dev;
        Zone devz;
        foreach (Vector2 vor in voronoi.SiteCoords())
        {
            dev = PolyGen(voronoi.Region(vor), poligon, vor);
            devz = new Zone(voronoi.Region(vor), vor, dev, noise[(int)vor.x, (int)vor.y]);
            mappa.Add(devz);
            dev.GetComponent<PolygonInteraction>().SetData(devz);

        }
        SetNeighbor();
        UpdateMapUmidità();





        ////////ogni stagione
        ///
        UpdateData();
    }

    private void UpdateData()
    {
        foreach (Zone a in mappa)
        {
            a.polyGO.GetComponent<PolygonInteraction>().UpdateData();
        }
    }

    public void SetNeighbor()
    {
        foreach (Zone a in mappa)
        {
            foreach (Vector2 site in voronoi.NeighborSitesForSite(a.centro))
            {
                foreach (Zone b in mappa)
                {
                    if (b.centro == site)
                    {
                        a.AddNeighbor(b);
                    }
                }
            }
        }
    }

    public void UpdateMapUmidità()
    {

        foreach (Zone a in mappa)
        {
            if (a.typeBiome == 1 || a.typeBiome == 0)
            {
                a.SetUmidità(1);
            }
        }
        foreach(Zone a in mappa)
        {
            if (a.umidità != 1)
            {
                Zone closer=a;
                int newHum = 0;
                foreach(Zone b in a.vicini)
                {
                    if (newHum == 0)
                    {
                        closer = b;

                        newHum = (int)Math.Pow(closer.umidità - 0.00001, Math.Abs((a.centro.x - b.centro.x) + (a.centro.y - b.centro.y)))*100;
                        if (newHum < 0)
                        {
                            newHum = 0;
                        }
                    }
                    else if ((float)Math.Pow(closer.umidità - 0.00001, Math.Abs((a.centro.x - b.centro.x) + (a.centro.y - b.centro.y))) > newHum)
                    {
                        closer = b;
                        newHum = (int)Math.Pow(closer.umidità - 0.00001, Math.Abs((a.centro.x - b.centro.x) + (a.centro.y - b.centro.y))) * 100;
                        if (newHum < 0)
                        {
                            newHum = 0;
                        }
                    }
                }
                a.umidità = newHum;
            }
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
    public Transform PolyGen(List<Vector2> a, Transform p, Vector2 centro)
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
        //poligon = Instantiate(p, new Vector3(centro.x, centro.y, 0), Quaternion.identity);
        poligon.GetComponent<MeshFilter>().mesh = msh;
        poligon.gameObject.GetComponent<MeshCollider>().sharedMesh = msh;
        //m_Material.color = new Color32(255, 165, 0,0);
        poligon.name = nPoly.ToString();
        nPoly++;

        return poligon;


    }



}