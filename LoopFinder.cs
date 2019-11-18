using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoopFinder : MonoBehaviour
{
    
    List<Node> visitato;
    int arcCounter;
    LinkedList<Node> ciclo;
    LinkedList<Arc> arcToLoop;


    Node root;
    int matrixLayer = 0;



    void FindLoop(List<Node> grafo, Node start)
    {
        root = start;
        /*
        //passo 1
        visitato.Add(start);

        foreach (Arc a in start.GetArcs())
        {
            if (visitato.Contains(a.GetA()) && a.GetA() != start)
            {

            }
            if (visitato.Contains(a.GetB()) && a.GetB() != start)
            {

            }
        }
        */
        Node[,] matLoop = new Node[30, 30];
        int i = 0, k = 0;
        matLoop[i, k] = start;
        i++;
        // prendi tutti gli archi collegati all'arco di partenza (con value!=0)
        Arc[] c=start.ConnectedArc();
        ControlloVuoto(c);
        Node[] newNodes = start.NearNodes();
        //rimuovi nodi gia presisenza eliminare possibili cicli
        //ovvero elimina dalla lista risultante nodi presenti in matloop ad i-1
        RemoveOld(matLoop,i-1, newNodes);
        if (newNodes.Length == 0)
        {
            //trova un'altro nodo
        }
        for (k = 0; k < c.Length; k++)
        {
            matLoop[i, k] = newNodes[k];
            if (visitato.Contains(newNodes[k]))
            {
                //trovato il ciclo 
                break;
            }
            else
            {
                visitato.Add(newNodes[k]);
            }
        }
        //controlla che non ci siano duplicati nella matrice dalla seconda righa in poi
        
        


    }

    //rimuove archi a nodi gia presi 
    private Node[] RemoveOld(Node[,] matLoop, int v, Node[] newNodes)
    {
        List<Node> nodiB = newNodes.ToList<Node>();
        for(int i=v;i!=0; i--)
        {
            for (int k = 0; ; k++)
            {
                //
                if (nodiB.Contains(matLoop[i, k]))
                {
                    nodiB.Remove(matLoop[i, k]);
                }
            }
        }
        return nodiB.ToArray();
    }




    //controllo se il nodo scelto ha archi con valore minore di 1 e quindi vuoti e lo elimino dalle possibili direzioni scelte
    private void ControlloVuoto(Arc[] c)
    {
        for(int i=0;i<c.Length; i++)
        {
            if (c[i].GetValue() < 1)
            {
                root.RemoveArc(c[i]);
            }
        }
        //aggiorno la lista di archi
        c = root.ConnectedArc();
    }

    void Bfs(Node a, Node[,] matLoop, int layer)
    {
        ///passo un nodo
        ///prendo tutti i nodi cicini e li metto all interno di una matrice al secondo livello
        ///prendo tutti i nodi vicini a quelli del secondo livello e li metto nel liv 3
        ///controllo che non ci siano nodi duplicati 
        ///     se ci sono abbiamo trovato un nodo
        ///     
        
        Arc[] vicini = a.ConnectedArc();
        for (int i=0;i<vicini.Length ;i++)
        {
            if (vicini[i].a==a)
            {
                matLoop[i, layer]=vicini[i].b;
            }
            if (vicini[i].b == a)
            {
                matLoop[i, layer] = vicini[i].a;
            }
        }
    }








    /// <summary>
    /// passo 1
    /// seleziona un punto di partenza
    /// passo 2 
    /// prendi tutti gli archi collegati all'arco di partenza (con value!=0)
    /// passo 3
    /// controlla che non ci sia un dupicato 
    /// passo 4
    /// se ce hai trovato un ciclo, se non c'è ripeti il passo 2
    /// passo 5
    /// hai il ciclo ora fai "retromarcia" fino a trovare 2 volte lo stesso nodo
    ///     ovvero  
    ///         prendi i 2 nodi al livello superiore del duplicato
    ///         poi ripeti fino a trovare lo stesso nodo, quello dovrebbe essere il nodo di partenza
    ///         riduci il peso degli archi e controlla se uno dei nodi appena presi ha solo archi a 0
    ///         in questo caso elimina il nodo
    ///         se uno dei nodi del poligono appena trovato non e stato eliminato usa quello come punto di partenza(usando sempre prima i nodi dei livelli piu vicini alla radice)
    ///         ripeti dal passo 1
    ///         se non ci sono piu nodi finisci
    /// </summary>
    /// <returns></returns>

    /*
        bool Loop()
        {
            for(int i=0; i<arcToLoop.Count; i++)
            while (true)
            {
                if (a.GetArcs&&)
                break;
            }
            return true;
        }



        */





















    class Poligon
    {
        Node[] punti;
        public void GeneratePoligon(Node[] punti)
        {
            this.punti = punti;
        }
    }

}
