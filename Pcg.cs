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

    public int lloyd = 3;//3 e quella piu carina

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
    List<Zone> humStart;


    public bool isolaGrande;
    public bool isolaMedia;
    public bool isolaPiccola;
    public bool penisola;
    public bool side;


    Voronoi voronoi;
    public Transform poligono;

    private void Start()
    {
        
        mappa = new List<Zone>();
        humStart = new List<Zone>();
        seed = Random.Range(0, maxCanvas);
        Rect bounds = new Rect(0, 0, maxCanvas, maxCanvas);
        //punti randomici NON QUELLI DA UTILIZZARE
        List<Vector2> points = CreateRandomPoint(polygonNumber);
         voronoi = new Voronoi(points, bounds, lloyd);
        List<Node> poligonoUno = new List<Node>();
        float[,] noise = Noise.GenerateNoiseMap(maxCanvas, maxCanvas,seed,scale,octaveNumber,persistance, lacunarity, new Vector2(maxCanvas/2,maxCanvas/2));
        noise = ApplyMask(noise); 
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
        DetectHum();





        ////////ogni stagione
        ///
        GodsEye god = new GodsEye();
        god.SetZone(mappa,humStart);
        UpdateData();
    }


    private float[,] ApplyMask(float[,]  mask)
    {

        ///per la creazione di NoiseMask dobbiamo
        ///creare degli algoritmi che andranno a sottrarre dalla noise che poi generiamo
        ///partiamo dal preset isola
        ///ISOLA
        ///qui possiamo dare una grandezza per l'isola
        ///in quanto ci basterà settare a -1 tutte le posizioni piu vicine ai bordi e poi diminuire a -0.8 -0.5 -0.3 
        ///e poi +0.3 nella zona centrale
        ///in questo modo possiamo decidere quanto sarà grande l'isola
        ///PENISOLA
        ///stesso concetto di isola ma un lato random non verra toccato
        ///LATO DEL CONTINENTE
        ///come penisola ma questa volta saranno 3 i lati da non toccare
        ///ARCIPELAGO/////piu in la
        ///fare un'altro voronoi 
        ///prendi il 1/10 dei centri e i poligoni vicini
        ///tutti gli altri poligoni saranno acqua
        ///
        
        if (penisola||side)
        {
            
            int lati = 1;
            if (penisola)
            {
                lati = 3;
            }
            int j = Random.Range(1, 4);

            
            if (penisola)
            {

                /*
             *+++ 
             *+++ 
             *--- 
             */
                if(j!=1)
                    for (int i = 0; i < maxCanvas; i++)
                    {
                        for (int k = 0; k < iterazioni; k++)
                        {
                        mask[i, k] = mask[i, k] - 0.44f;
                        }
                    }
                /*
                   *--- 
                   *+++ 
                   *+++ 
                   */
                if (j != 1)
                    for (int i = 0; i < maxCanvas; i++)
                    {
                        for (int k = y; k < maxCanvas; k++)
                        {
                        mask[i, k] = mask[i, k] - 0.44f;

                        }
                    }
                /*
                    *-++ 
                    *-++ 
                    *-++ 
                    */
                if (j != 1)
                    for (int i = 0; i < iterazioni; i++)
                    {
                        for (int k = iterazioni; k < y; k++)
                        {
                        mask[i, k] = mask[i, k] - 0.44f;
                        }
                    }
                /*
                   *++- 
                   *++- 
                   *++- 
                   */
                if (j != 1)
                    for (int i = y; i < maxCanvas; i++)
                    {
                        for (int k = iterazioni; k < y; k++)
                        {
                        mask[i, k] = mask[i, k] - 0.44f;
                        }
                    }
            }
            
        }

   }
  



       



        if (isolaGrande || isolaMedia || isolaPiccola)
        {

            int iterazioni = 0;
            if (isolaGrande)
            {
                iterazioni = 800;

            }
            else if (isolaMedia)
            {
                iterazioni = 600;

            }
            else if (isolaPiccola)
            {
                iterazioni = 400;

            }
            for (int x=0;x<maxCanvas;x++)
            {
                for (int y=0;y<maxCanvas ;y++)
                {
                    if(Math.Pow((x - 1000), 2)+ Math.Pow((y - 1000), 2)< Math.Pow(iterazioni+300, 2))
                    {
                        if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni + 250, 2))
                        {
                            if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni+200, 2))
                            {
                                if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni+150, 2))
                                {
                                    if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni+100, 2))
                                    {
                                        if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni+50, 2))
                                        {
                                            if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni, 2))
                                            {
                                                mask[x, y] = mask[x, y] + 0.04f;
                                            }
                                            else
                                            {
                                                mask[x, y] = mask[x, y] - (Random.Range(0, 0.05f));
                                            }
                                        }
                                        else
                                        {
                                            mask[x, y] = mask[x, y] - (Random.Range(0.05f, 0.1f));
                                        }
                                    }
                                    else
                                    {
                                        mask[x, y] = mask[x, y] - (Random.Range(0.1f, 0.15f));
                                    }
                                }
                                else
                                {
                                    mask[x, y] = mask[x, y] - (Random.Range(0.15f, 0.2f));
                                }
                            }
                            else
                            {
                                mask[x, y] = mask[x, y] - (Random.Range(0.2f,0.25f));
                            }
                        }
                        else
                        {
                            mask[x, y] = mask[x, y] - (Random.Range(0.25f, 0.30f));
                        }
                    }
                    else
                    {
                        mask[x, y] = mask[x, y] - 0.40f;
                    }
                }
            }

            

            

        }
        return mask;
    }

    private void UpdateData()
    {
        foreach (Zone a in mappa)
        {
            a.DefineZone();
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

    public void DetectHum()
    {
        
        List<Zone> humNext = new List<Zone>();
        foreach (Zone a in mappa)
        {
            if (a.typeBiome == 1 || a.typeBiome == 0)
            {
                a.SetUmidità(1);
                humStart.Add(a);
            } else if(a.typeBiome == 7)
            {
                a.SetUmidità(0.7f);
                humStart.Add(a);
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