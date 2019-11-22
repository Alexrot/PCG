using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoopFinder : MonoBehaviour
{
    /// <summary>
    /// passo 1
    /// seleziona un punto di partenza
    /// passo 2 
    /// prendi tutti gli archi collegati all'arco di partenza (con value!=0)
    /// passo 3
    /// controlla che non ci sia un dupicato 
    /// passo 4
    /// se c'è hai trovato un ciclo, se non c'è ripeti il passo 2
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
    ///         ovvero
            ///passo 1
            /// trova il duplicato
            ///passo 2
            ///controlla tra i suoi nodi adiacenti quali sono presenti nei livelli superiori nella matice
            ///passo 3
            ///ripeti il passo 2 fino a che non sei al layer 0 o non hai trovato un'altro duplicato(2 volte lo stesso nodo)
            ///questa volta e piu facile dato che troveremo al massimo un nodo per direzione e abbiamo 2 direzioni
            ///passo 4 
            ///controlla che sia un ciclo
            ///passo 5 
            ///diminuisci tutti gli archi del ciclo di 1
            ///passo 6
            ///passa il ciclo alla funzione che genera il poligono
            ///passo 7
            ///elimina archi e nodi morti (senza value, nodi o archi)
            ///passo 8
            ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
    /// </summary>




    List<Arc> percorsoUsato= new List<Arc>();
    List<Node> livello=null;
    Boolean loop = false;
    Boolean complete = false;
    List<List<Node>> matrice=null;


    List<Node> loopNode;
    List<Arc> arcToLoop;



    int matrixLayer = 0;
    
    void FindLoop(Node start)
    {
        Node[] layerNode = { };

        /// passo 1
        /// seleziona un punto di partenza
        livello.Add(start);
        matrice.Add(livello);
        do
        {
            while (!loop)
            {
                /// passo 4(NON TROVATO)
                /// se c'è hai trovato un ciclo, se non c'è ripeti il passo 2
                matrixLayer++;
                livello.Clear();

                foreach (Node a in matrice[matrixLayer - 1])
                {
                    /// passo 2 
                    /// prendi tutti gli archi collegati all'arco di partenza (con value!=0)
                    layerNode = Bfs(a);
                    livello.AddRange(layerNode);
                    Usato(a);
                }
                matrice.Add(layerNode.ToList<Node>());
                IsLoop(matrice);
            }

            /// passo 4(TROVATO)
            /// se c'è hai trovato un ciclo, se non c'è ripeti il passo 2
            //TO DO
            LoopFound();
            CheckGraph();
        } while (!complete);

    }

    private void CheckGraph()
    {
        //controll che il grafo non sia vuoto e che ci siano ancora nodi da scegliere
    }

    /// passo 5
    /// hai il ciclo ora fai "retromarcia" fino a trovare 2 volte lo stesso nodo
    private void LoopFound()
    {
        ///passo 1
        ///trova il duplicato
        ///passo 2
        ///controlla tra i suoi nodi adiacenti quali sono presenti nei livelli superiori nella matice
        ///passo 3
        ///ripeti il passo 2 fino a che non sei al layer 0 o non hai trovato un'altro duplicato(2 volte lo stesso nodo)
        ///questa volta e piu facile dato che troveremo al massimo un nodo per direzione e abbiamo 2 direzioni
        ///passo 4 
        ///controlla che sia un ciclo
        ///passo 5 
        ///diminuisci tutti gli archi del ciclo di 1
        ///passo 6
        ///passa il ciclo alla funzione che genera il poligono
        ///passo 7
        ///elimina archi e nodi morti (senza value, nodi o archi)
        ///passo 8
        ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
        TrovaDuplicato();
    }


    ///passo 1
    ///trova il duplicato
    private Node TrovaDuplicato()
    {
        //controlla le ultime 2 righe per trovare duplicati
        List<Node> ultimeDue = null;
        matrixLayer = matrice.Count();
        ultimeDue.AddRange(matrice[matrixLayer]);
        ultimeDue.AddRange(matrice[matrixLayer-1]);
        Node duplicates = (Node)ultimeDue.GroupBy(s => s).SelectMany(grp => grp.Skip(1));
        Console.WriteLine(duplicates);
        return duplicates;
    }


    /// passo 3
    /// /// controlla che non ci sia un dupicato 
    private void IsLoop(List<List<Node>> matrice)
    {
        //controllo l'ultimo e il penultimo, poiche non è possibile che si generano cicli tra livelli più distanti
        List<Node> lastTwo=null;
        lastTwo.AddRange(matrice[matrice.Count()]);
        lastTwo.AddRange(matrice[matrice.Count()-1]);
        if (lastTwo.GroupBy(n => n).Any(c => c.Count() > 1))
        {
            loop = true;
        }
    }



    private void Usato(Node Node)
    {
        /*aggiunge gli archi appena visitati a quelli che non devono essere piu presi in futuro*/
        foreach(Arc visitato in Node.ConnectedArc())
        {
            percorsoUsato.Add(visitato);
        }

    }



    /// passo 2 
    /// prendi tutti gli archi collegati all'arco di partenza (con value!=0)
    Node[] Bfs(Node a)
    {
        //passa tutti i nodi non ancora visitati collegati al nodo passato in input
        Arc[] c = a.ConnectedArc();
        //ControlloVuoto(c);
        Arc[] nextArc = RemoveUsedArc(c);
        Node[] newNodes = a.NearNodes(nextArc);

        //CONTROLLO ESTERNO
        //Node[] newNodes = a.NearNodes(c);
        return newNodes;

    }


    /*
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
*/

    private Arc[] RemoveUsedArc(Arc[] a)
    {
        //darà in output solo archi non gia utilizzati, ovvero quelli non presenti nella lista percorsoUsato
        Arc[] retArc= { };
        int i = 0;
        foreach(Arc trovato in a)
        {
            if (!percorsoUsato.Contains(trovato))
            {
                retArc[i] = trovato;
                i++;
            }
        }
        return retArc;
    }










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
