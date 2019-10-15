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


    void Bfs(Node a)
    {
        ///passo un nodo
        ///prendo tutti i nodi cicini e li metto all interno di una matrice al secondo livello
        ///prendo tutti i nodi vicini a quelli del secondo livello e li metto nel liv 3
        ///controllo che non ci siano nodi duplicati 
        ///     se ci sono abbiamo trovato un nodo
        ///     
        Node[,] matLoop=new Node[30,30];
        int i=0, k = 0;
        matLoop[i,k] = a;

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
