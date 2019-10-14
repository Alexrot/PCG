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



    void FindLoop(List<Node> grafo, Node start)
    {
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




    }











    /// <summary>
    /// passo 1
    /// seleziona un punto di partenza
    /// passo 2 
    /// prendi tutti gli archi collegati all'arco di partenza (con value!=0)
    /// passo 3
    /// controlla che non sia un ciclo
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
