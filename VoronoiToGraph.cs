using csDelaunay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiToGraph : MonoBehaviour
{

    public Graph poligoni;
    float maxCanvas=0;
    Node startingPoint;


    /// <summary>
    /// CLASSE CHE TRASFORMA VORONOI(LLOYD) IN UN GRAFO 
    ///QUESTA CLASSE MANDERà ALLA CLASSE CHE GENERERà I VARI POLIGONI I DATI E I PUNTI RELATIVI
    ///
    /// GeneraGrafo aggiunge tutti i nodi del grafo e i suoi archi
    /// </summary>
    /// <param name="archiDelGrafo"></param>
    /// 
    public Node GetStartingPoint()
    {
        return startingPoint;
    }


    public VoronoiToGraph()
    {
        poligoni = new Graph();
    }

    public Graph GetGraph()
    {
        return poligoni;
    }


    //genera il grafo dei nodi interni
    public void  GeneraGrafo(List<Edge> archiDelGrafo, float maxCanvas)
    {
        Debug.Log("test11111111111111");
        this.maxCanvas = maxCanvas;
        foreach (Edge edge in archiDelGrafo)
        {
            // if the edge doesn't have clippedEnds, if was not within the bounds, dont draw it
            if (edge.ClippedEnds == null) continue;

             
            poligoni.AddEdge(poligoni.AddNode(edge.ClippedEnds[LR.LEFT]), poligoni.AddNode(edge.ClippedEnds[LR.RIGHT]));
            Debug.Log("22222222222222222222222");
        }
        SearchGraph();
    }

    public Graph Get() { return poligoni; }



    /*//////////////////////////////////////////////////////////////////////////////////////////////////*/
    public void SearchGraph()
    {
        Node altoSx = poligoni.AddNode(new Vector2(0f, 0f));
        startingPoint = altoSx;
        Node altoDx = poligoni.AddNode(new Vector2(maxCanvas, 0f));
        Node bassoSx = poligoni.AddNode(new Vector2(0f, maxCanvas));
        Node bassoDx = poligoni.AddNode(new Vector2(maxCanvas, maxCanvas));
        Debug.Log("punti generati");
        //parte 2 genera i punti etremi del contenitore (0,0)(x,0)(0,x)(x,x)
        
        Node temp1=altoSx;
        Node temp2=bassoSx;

        List<Node> x0 = new List<Node>();
        List<Node> xMax = new List<Node>();
        List<Node> y0 = new List<Node>();
        List<Node> yMax = new List<Node>();


        //parte 3 collega i nodi piu vicini sui bordi del contenitore
        //true cerca in orizzontale

        //dividi i nodi in 4 liste che sono i 4 bordi del nostro grafo salvando in ogni lista
        //i nodi che hanno 0 o max in una delle 2 coordinate                                                                                        
        foreach (Node a in poligoni.nodes)
        {
            if (a.position.x == 0)
            {
                x0.Add(a);
            }
            if (a.position.y == 0)
            {
                y0.Add(a);
            }
            if (a.position.x == maxCanvas)
            {
                xMax.Add(a);
            }
            if (a.position.y == maxCanvas)
            {
                yMax.Add(a);
            }
            Debug.Log("vtg LISTE BORDI");
        }

        Debug.Log("vtg START CONNECTION");
        SortBordi(x0, altoSx, "ovest");
        SortBordi(xMax, altoDx, "est");
        SortBordi(y0, altoSx,"nord");
        SortBordi(yMax, bassoSx, "sud");
        Debug.Log("vtg END CONNECTION");

    }

    private void SortBordi(List<Node> a, Node startNode, String bordo)
    {
        //controlla su quale borso siamo
        Node startingPoint = startNode;
        switch(bordo)
        {
            case "nord":
                Debug.Log("vtg BORDO NORD");
                a.Remove(startNode);
                while (a.Count != 0)
                {
                    Node next = a[0];
                    for (int i = 1; i < a.Count - 1; i++)
                    {
                        if (a[i].position.x < next.position.x)
                        {
                            next = a[i];
                        }
                    }

                    poligoni.AddEdgeLimit(startingPoint, next);
                    startingPoint = next;
                    a.Remove(next);
                    if (a.Count == 1)
                    {
                        poligoni.AddEdgeLimit(startingPoint, a[0]);
                        a.Remove(a[0]);
                    }
                }
                break;

            case "sud":
                Debug.Log("vtg BORDO SUD");
                a.Remove(startNode);
                while (a.Count != 0)
                {
                    Node next = a[0];
                    for (int i = 1; i < a.Count - 1; i++)
                    {
                        if (a[i].position.x < next.position.x)
                        {
                            next = a[i];
                        }
                    }

                    poligoni.AddEdgeLimit(startingPoint, next);
                    startingPoint = next;
                    a.Remove(next);
                    if (a.Count == 1)
                    {
                        poligoni.AddEdgeLimit(startingPoint, a[0]);
                        a.Remove(a[0]);
                    }
                }
                break;

            case "ovest":
                Debug.Log("vtg BORDO OVEST");
                a.Remove(startNode);
                while (a.Count != 0)
                {
                    Debug.Log("vtg BORDO OVEST" + a.Count);
                    Node next = a[0];
                    for (int i = 1; i < a.Count - 1; i++)
                    {
                        if (a[i].position.y < next.position.y)
                        {
                            next = a[i];
                        }
                    }
                    Debug.Log("vtg BORDO OVEST" + a.Count);
                    poligoni.AddEdgeLimit(startingPoint, next);
                    startingPoint = next;
                    a.Remove(next);
                    if (a.Count == 1)
                    {
                        Debug.Log("vtg BORDO OVEST if entri?" + a.Count);
                        poligoni.AddEdgeLimit(startingPoint, a[0]);
                        a.Remove(a[0]);
                    }
                }
                Debug.Log("vtg BORDO OVEST sei uscito?" + a.Count);
                break;

            case "est":
                Debug.Log("vtg BORDO EST");
                a.Remove(startNode);
                while (a.Count != 0)
                {
                    Node next = a[0];
                    for (int i = 1; i < a.Count - 1; i++)
                    {
                        if (a[i].position.y < next.position.y)
                        {
                            next = a[i];
                        }
                    }

                    poligoni.AddEdgeLimit(startingPoint, next);
                    startingPoint = next;
                    a.Remove(next);
                    if (a.Count == 1)
                    {
                        poligoni.AddEdgeLimit(startingPoint, a[0]);
                        a.Remove(a[0]);
                    }
                }
                break;
        }

        
    }
    /*//////////////////////////////////////////////////////////////////////////////////////////////////*/
}
