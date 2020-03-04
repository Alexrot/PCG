
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
    int maxCanvas;

    int nPoly;

    public Transform poligon;

    Triangulator tr;
    public int seed;
    float scale;
    int octaveNumber;
    float persistance;
    float lacunarity;
    public float[,] noise;

    List<Zone> mappa;
    List<Zone> humStart;



    public bool useSeed;


    public GodsEye god;
    Voronoi voronoi;
    public Transform poligono;

    MapData data;



    public void Generate(GodsEye a, Transform p, MapData data)
    {
        maxCanvas = 2000;//base per poi non dover modificare anche la temperatura
        scale = 400;
        octaveNumber = 4;
        persistance = 0.15f;
        lacunarity = 1f;
        this.data = data;
        poligon = p;
        nPoly = 0;
        CreateNew();
        ////////ogni stagione
        ///
        god = a;
        god.SetZone(mappa, humStart);
        god.UpdateData();
    }



    public void CreateNew()
    {
        mappa = new List<Zone>();
        humStart = new List<Zone>();
        if (!data.useSeed)
        {
            seed = Random.Range(0, maxCanvas);
            data.seed = seed;
        }
        else
        {
            seed = data.seed;
        }

        Rect bounds = new Rect(0, 0, maxCanvas, maxCanvas);
        //punti randomici NON QUELLI DA UTILIZZARE
        List<Vector2> points = CreateRandomPoint(data.polygonNumber);
        voronoi = new Voronoi(points, bounds, 3);
        List<Node> poligonoUno = new List<Node>();
        noise = Noise.GenerateNoiseMap(maxCanvas, maxCanvas, seed, scale, octaveNumber, persistance, lacunarity, new Vector2(maxCanvas / 2, maxCanvas / 2));
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
    }


    private float[,] ApplyMask(float[,] mask)
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
        ///penisola non ha ragione di esistere nella configuarione attuale 



        if (data.isola == 3)
        {
            int size = 300;
            List<Vector2> isole = new List<Vector2>();
            for (int i = 0; i < 4; i++)
            {
                isole.Add(new Vector2(Random.Range(300, 1700), Random.Range(300, 1700)));
            }

            foreach (Vector2 a in isole)
            {

                for (int x = 0; x < maxCanvas; x++)
                {
                    for (int y = 0; y < maxCanvas; y++)
                    {
                        if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 300, 2))
                        {
                            if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 250, 2))
                            {
                                if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 200, 2))
                                {
                                    if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 150, 2))
                                    {
                                        if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 100, 2))
                                        {

                                            if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size, 2))
                                            {
                                                mask[x, y] = mask[x, y] + 0.1f;
                                            }
                                            else
                                            {
                                                mask[x, y] = mask[x, y] - (Random.Range(0, 0.02f));
                                            }
                                        }
                                        else
                                        {
                                            mask[x, y] = mask[x, y] - (Random.Range(0.02f, 0.04f));
                                        }
                                    }
                                    else
                                    {
                                        mask[x, y] = mask[x, y] - (Random.Range(0.04f, 0.6f));
                                    }
                                }
                                else
                                {
                                    mask[x, y] = mask[x, y] - (Random.Range(0.6f, 0.8f));
                                }
                            }
                            else
                            {
                                mask[x, y] = mask[x, y] - (Random.Range(0.8f, 0.1f));
                            }
                        }
                        else
                        {
                            mask[x, y] = mask[x, y] - (Random.Range(0.1f, 0.12f));
                        }
                    }


                }

            }




            /*
            foreach (Vector2 a in isole)
            { 
                int size = 400;
                for (int x = 0; x < maxCanvas; x++)
                {
                    for (int y = 0; y < maxCanvas; y++)
                    {
                        if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 400, 2))
                        {
                            if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 300, 2))
                            {
                                if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 250, 2))
                                {
                                    if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 200, 2))
                                    {
                                        if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size + 150, 2))
                                        {
                                                if (Math.Pow((x - a.x), 2) + Math.Pow((y - a.y), 2) < Math.Pow(size, 2))
                                                {
                                                    mask[x, y] = mask[x, y] + 0.1f;
                                                }
                                                else
                                                {
                                                    mask[x, y] = mask[x, y] - (Random.Range(0, 0.02f));
                                                }
                                            }
                                            else
                                            {
                                                mask[x, y] = mask[x, y] - (Random.Range(0.02f, 0.04f));
                                            }
                                        }
                                        else
                                        {
                                            mask[x, y] = mask[x, y] - (Random.Range(0.04f, 0.6f));
                                        }
                                    }
                                    else
                                    {
                                        mask[x, y] = mask[x, y] - (Random.Range(0.6f, 0.8f));
                                    }
                                }
                                else
                                {
                                    mask[x, y] = mask[x, y] - (Random.Range(0.8f, 0.1f));
                                }
                            }
                            else
                            {
                                mask[x, y] = mask[x, y] - (Random.Range(0.1f, 0.12f));
                            }
                        }
                        else
                        {
                            mask[x, y] = mask[x, y] - 0.14f;
                        }
                    }
                }
            }*/

        }



        if (data.isola < 3)
        {

            int iterazioni = 0;
            if (data.isola == 0)
            {
                iterazioni = 800;

            }
            else if (data.isola == 1)
            {
                iterazioni = 600;

            }
            else if (data.isola == 2)
            {
                iterazioni = 400;

            }
            for (int x = 0; x < maxCanvas; x++)
            {
                for (int y = 0; y < maxCanvas; y++)
                {
                    if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni + 300, 2))
                    {
                        if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni + 250, 2))
                        {
                            if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni + 200, 2))
                            {
                                if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni + 150, 2))
                                {
                                    if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni + 100, 2))
                                    {
                                        if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni + 50, 2))
                                        {
                                            if (Math.Pow((x - 1000), 2) + Math.Pow((y - 1000), 2) < Math.Pow(iterazioni, 2))
                                            {
                                                mask[x, y] = mask[x, y] + 0.08f;
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
                                mask[x, y] = mask[x, y] - (Random.Range(0.2f, 0.25f));
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
            if (a.typeBiome == 1 || a.typeBiome == 0 || a.typeBiome == 2 || a.typeBiome == 3)
            {
                a.SetUmidità(1);
                humStart.Add(a);
            }
            else if (a.typeBiome == 12 || a.typeBiome == 11)
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