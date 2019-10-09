using csDelaunay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiToGraph : MonoBehaviour
{

    Graph poligoni;
    float maxCanvas;



    /// <summary>
    /// CLASSE CHE TRASFORMA VORONOI(LLOYD) IN UN GRAFO 
    ///QUESTA CLASSE MANDERà ALLA CLASSE CHE GENERERà I VARI POLIGONI I DATI E I PUNTI RELATIVI
    ///
    /// GeneraGrafo aggiunge tutti i nodi del grafo e i suoi archi
    /// </summary>
    /// <param name="archiDelGrafo"></param>
    /// 





    public void  GeneraGrafo(List<Edge> archiDelGrafo, float maxCanvas)
    {
        this.maxCanvas = maxCanvas;
        foreach (Edge edge in archiDelGrafo)
        {
            // if the edge doesn't have clippedEnds, if was not within the bounds, dont draw it
            if (edge.ClippedEnds == null) continue;

             poligoni = new Graph(99);
            poligoni.AddEdge(poligoni.AddNode(edge.ClippedEnds[LR.LEFT]), poligoni.AddNode(edge.ClippedEnds[LR.RIGHT]));
            SearchGraph();
        }
    }

    public Graph Get() { return poligoni; }




    public void SearchGraph()
    {
        Node altoSx = poligoni.AddNode(new Vector2f(0f, 0f));
        Node altoDx = poligoni.AddNode(new Vector2f(0f, maxCanvas));
        Node bassoSx = poligoni.AddNode(new Vector2f(maxCanvas, 0f));
        Node bassoDx = poligoni.AddNode(new Vector2f(maxCanvas, maxCanvas));
        
        //parte 2 genera i punti etremi del contenitore (0,0)(x,0)(0,x)(x,x)
        bool notOver1 =true;
        bool notOver2 = true;
        Node temp;
        Node temp1=altoSx;
        Node temp2=bassoSx;



        
        //parte 3 collega i nodi piu vicini sui bordi del contenitore
        //true cerca in orizzontale
        
            while (notOver1&& notOver2)
            {
                if (temp1.position != altoDx.position)
                {//da in alto a sx fino a in alto a dx

                    poligoni.AddEdge(temp1, temp = poligoni.FindClose(temp1, true));
                    temp1 = temp;
                }
                else
                {
                    notOver1 = false;
                }
                if (temp2.position != bassoDx.position)
                {//da in basso a sx fino a in basso a dx

                    poligoni.AddEdge(temp2, temp = poligoni.FindClose(temp2, true));
                    temp2 = temp;
                }
                else
                {
                    notOver2 = false;
                }
            }
        
        //else cerca in verticale 
        
             notOver1 = true;
             notOver2 = true;

             temp1 = altoSx;
             temp2 = altoDx;

            while (notOver1 && notOver2)
            {
                if (temp1.position == bassoSx.position)
                {//da in alto a sx fino a in basso a sx 
                    poligoni.AddEdge(temp1, temp = poligoni.FindClose(temp1, false));
                    temp1 = temp;
                }
                else
                {
                    notOver1 = false;
                }
                if (temp2.position == bassoDx.position)
                {//da in alto a dx fino a in basso a dx
                    poligoni.AddEdge(temp2, temp = poligoni.FindClose(temp1, false));
                    temp2 = temp;
                }
                else
                {
                    notOver2 = false;
                }
            }
    }
}
