using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using csDelaunay;
using System;
using Random = UnityEngine.Random;

public class Pcg : MonoBehaviour
{

    
    VoronoiToGraph vtg=null;

    private Dictionary<Vector2f, Site> puntiLloyd;
    private List<Edge> archiDelGrafo;

    private void Start()
    {
        //genera un immagine su cui lloyd e voronoi lavoreranno
        Rectf bounds = new Rectf(0, 0, 512, 512);
        //punti randomici NON QUELLI DA UTILIZZARE
        List<Vector2f> points = CreateRandomPoint(100);
        //genero voronoi e modifico tramite lloyd
        Voronoi voronoi = new Voronoi(points, bounds, 4);
        puntiLloyd = voronoi.SitesIndexedByLocation;
        archiDelGrafo = voronoi.Edges;
        vtg.GeneraGrafo(archiDelGrafo, 512f);
    }











    //PUNTI PRE-LLOYD
    private List<Vector2f> CreateRandomPoint(int polygonNumber)
    {
      
        List<Vector2f> points = new List<Vector2f>();
        for (int i = 0; i < polygonNumber; i++)
        {
            points.Add(new Vector2f(Random.Range(0, 512), Random.Range(0, 512)));
            
        }

        return points;
    }


    ///passo 1 genera voronoi
    ///passo 2 trsforma con lloyd
    ///passo 3 prendi la linee dal risultato
    ///passo 4 prendi i punti 
    ///passo 5 controlla a quali punti appartengono le linee (le linee hanno 2 "genitori")(se un punto ha linee che contengono uno zero allora e agli estremi della mappa)
    ///passo 6 passa i punti e le linee a meshGenerator(numeroDiLinee vector3[], posizione Vector3)
    ///passo 7 genera poligoni
    ///

    /**
     * PASSO 1  
     * genera voronoi con l'ausilio delle librerie
     * 
     */
    /**
     * PASSO 2
     * manda voronoi a lloyd e genere dai poligoni bilanciati
     * 
     */
    /**
     * PASSO 3
     * ottieni le linee ottenute da lloyd tramite il codice
     * foreach (Edge edge in edges)
            {
                // if the edge doesn't have clippedEnds, if was not within the bounds, dont draw it
                if (edge.ClippedEnds == null) continue;

                DrawLine(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], tx, Color.black);
            }
     * 
     */
    /**
     * PASSO 4
     * prendi i punti generati da lloyd tramite il codice
     * foreach (KeyValuePair<Vector2f, Site> kv in sites)
           {
               tx.SetPixel((int)kv.Key.x, (int)kv.Key.y, Color.red);
           }
        *
        */
    /**
     * PASSO 5
     * funzione che passa in rassegna tutte le linee per capire quale appartiene a quale punto
     * NB: ogni linea ha 2 genitori per ridurre il numero di ricerche potrebbe essere una buona idea settare un contatore che controlla se una linea ha ancora dei genitori,
     * in caso contrario possiamo eliminarla dalla lista delle linee senza problema
     *  presumendo di salvare le linee in ordine crescente di posizione nel grafo
     *      es: x(1,1) y(1,2) precede nell'array x(2,2) y(2,3)
     *  e presumendo che questo valga anche per i punti 
     *  possiamo iniziare a generare i poligoni partendo dall'angolo superiore sinistro a scendere
     *      quindi for(i<x){for(k<y)} naturalmente controllando solamente i punti generati e non tutti i punti del grafo
     *      quindi controllando prima le colonne partendo da sinistra andando verso destra
     *  inseriamo il risultato in un oggetto che poi verra passato al generatore di poligoni
     *  
     */
    /**
     * PASSO 6
     * l'oggetto passera i dati ottenuti dalla funzione precedente a MeshGenerator che avrà piu cotruttori in base
     * al numero di lati che ha il poligono
     * NB meshGenerator genera poligoni tramite triangoli quindi dovranno essere passati anche in ordine corretto
     * ogni poligono avrà un "ancora" ovvero un punto presente in tutti i triangoli che lo compongono 
     * ad ogni iterazione di agiunta di triangoli al poligono la funzione otterrà :
     *     l'ancora
     *     l'ultimo nodo del triangolo precedente
     *     un nuovo nodo
     *
     *detto questo l'ultima cosa che manca è la posizione del poligono sullo schermo ottenible tramite il punto base
     * 
     */
    /**
     * PASSO 7
     * gerera il poligono tramite MeshGenerator
     * 
     */

}