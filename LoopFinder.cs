﻿using System;
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
            ///passo 1*
            /// trova il duplicato
            ///passo 2*
            ///controlla tra i suoi nodi adiacenti quali sono presenti nei livelli superiori nella matice
            ///passo 3*
            ///ripeti il passo 2 fino a che non sei al layer 0 o non hai trovato un'altro duplicato(2 volte lo stesso nodo)
            ///questa volta e piu facile dato che troveremo al massimo un nodo per direzione e abbiamo 2 direzioni
            ///passo 4 *
            ///controlla che sia un ciclo
            ///passo 5 *
            ///diminuisci tutti gli archi del ciclo di 1
            ///passo 6*
            ///passa il ciclo alla funzione che genera il poligono
            ///passo 7*
            ///elimina archi e nodi morti (senza value, nodi o archi)/*/*/*/*/*/*/*/*/solo nodi/*/*/*/*/*/*/*/*
            ///passo 8*
            ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
    /// </summary>




    List<Arc> percorsoUsato= new List<Arc>();
    List<Node> livello;
    Boolean loop = false;
    //Boolean complete = false;
    List<List<Node>> matrice;
    public MeshGenerator poliGen;
    Transform poligono;


    List<Node> loopNode;
    List<Arc> loopArc;
    
    



    int matrixLayer = 0;

    public void FindLoop(Node node, Transform p)
    {
        poliGen = new MeshGenerator();
        loopArc = new List<Arc>();
        loopNode = new List<Node>();
        livello = new List<Node>();
        matrice = new List<List<Node>>();
        
        poligono = p;
        FindLoops(node);
    }

    public void FindLoops(Node start)
    {
        List<Node> layerNode = new List<Node>();

        /// passo 1
        /// seleziona un punto di partenza
        livello.Add(start);
        matrice.Add(livello.ToList());
     
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
                matrice.Add(livello.ToList());
                IsLoop(matrice);
            }

            /// passo 4(TROVATO)
            /// se c'è hai trovato un ciclo, se non c'è ripeti il passo 2
            //TO DO
            Debug.Log("loop e="+loop);
            
            LoopFound();
            CheckGraph();
        

    }

    private void CheckGraph()
    {
        //controll che il grafo non sia vuoto e che ci siano ancora nodi da scegliere
    }

    /// passo 5
    /// hai il ciclo ora fai "retromarcia" fino a trovare 2 volte lo stesso nodo
    private void LoopFound()
    {
        ///passo 1*
        ///trova il duplicato
        ///passo 2*
        ///controlla tra i suoi nodi adiacenti quali sono presenti nei livelli superiori nella matice
        ///passo 3*
        ///ripeti il passo 2 fino a che non sei al layer 0 o non hai trovato un'altro duplicato(2 volte lo stesso nodo)
        ///questa volta e piu facile dato che troveremo al massimo un nodo per direzione e abbiamo 2 direzioni
        ///passo 4 *
        ///controlla che sia un ciclo
        ///passo 5 *
        ///diminuisci tutti gli archi del ciclo di 1
        ///passo 6*
        ///passa il ciclo alla funzione che genera il poligono
        ///passo 7*//////////////////////aeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
        ///elimina archi e nodi morti (senza value, nodi o archi)
        ///passo 8*
        ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
        List<Node> duplicato = new List<Node>();
        //Debug.Log(matrice[matrixLayer].Count);
        //Debug.Log(matrice[matrixLayer - 1].Count);
        duplicato.Add(TrovaDuplicato());
        loopNode.Add(duplicato[0]);
        BackTrack(duplicato);
        ///passo 4 *
        ///controlla che sia un ciclo
        ///passo 5 *
        ///diminuisci tutti gli archi del ciclo di 1
        LoopCheck();
        foreach(Arc a in loopArc)
        {
            a.ReduceValue();
        }
        ///passo 6*
        ///passa il ciclo alla funzione che genera il poligono
        poliGen.PolyGen(loopNode,poligono);
        ///passo 8*
        ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
        //LookForNext();


    }




    /// passo 3
    /// /// controlla che non ci sia un dupicato 
    private void IsLoop(List<List<Node>> matrice)
    {
        Debug.Log(" e lastTwo il problema");
        //controllo l'ultimo e il penultimo, poiche non è possibile che si generano cicli tra livelli più distanti
        List<Node> lastTwo=new List<Node>();
        lastTwo.AddRange(matrice[matrixLayer]);
        lastTwo.AddRange(matrice[matrixLayer-1]);
        foreach(Node a in lastTwo)
        Debug.Log("trovaaaaaaaaaaaaaaaaaaaatoooooooooooooooooooooooo:"+a.position);
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
            if (!percorsoUsato.Contains(visitato))
            {
                percorsoUsato.Add(visitato);
            }
            
        }

    }



    /// passo 2 
    /// prendi tutti gli archi collegati all'arco di partenza (con value!=0)
    List<Node> Bfs(Node a)
    {
        //passa tutti i nodi non ancora visitati collegati al nodo passato in input
        List<Arc> c = a.ConnectedArc();
        //ControlloVuoto(c);
        List<Node> newNodes = a.NearNodes(RemoveUsedArc(c));

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

    private List<Arc> RemoveUsedArc(List<Arc> a)
    {
        //darà in output solo archi non gia utilizzati, ovvero quelli non presenti nella lista percorsoUsato
        List<Arc> retArc= new List<Arc>();
        foreach(Arc trovato in a)
        {
            if (!percorsoUsato.Contains(trovato)&&trovato.GetValue()>0)
            {
                retArc.Add(trovato);
            }
        }
        return retArc;
    }

    ///--------------///////////////----------------/////POLIGONO TROVATO/////--------------///////////////----------------///


    ///passo 1*
    ///trova il duplicato
    private Node TrovaDuplicato()
    {
        //controlla le ultime 2 righe per trovare duplicati
        List<Node> ultimeDue = new List<Node>();
        matrixLayer = matrice.Count();
        Debug.Log(matrixLayer);

        ultimeDue.AddRange(matrice[matrixLayer-1]);
        ultimeDue.AddRange(matrice[matrixLayer - 2]);

        Debug.Log("duplicato trovato");

        //Node duplicates = (Node)ultimeDue.GroupBy(s => s).SelectMany(grp => grp.Skip(1));
        Node duplicates =(Node)ultimeDue.GroupBy(s => s).Where(s => s.Count() > 1).First().First();

        return duplicates;
    }


    ///passo 2*
    ///controlla tra i suoi nodi adiacenti quali sono presenti nei livelli superiori nella matice
    private void BackTrack(List<Node> a)
    {
        List<Node> prossimiDaControllare = new List<Node>();
        List<Node> nodiVicini =new List<Node>();
        foreach(Node k in a)
        {
            nodiVicini = k.NearNodes(k.ConnectedArc());
        }

            //SE IN UN LIVELLO SUPERIORE SI RITROVA L'ARCO STESSO (PROBABILE CHE SUCCEDA COL DUPLICATO APPENA SCELTO)
            //CONTROLLA ANCHE QUELLO SUPERIORE DI LIVELLO CON QUELLO STESSO NODO
            if (nodiVicini.Contains(loopNode[0]))
            {
            prossimiDaControllare.Add(loopNode[0]);
            }
            //controlla per errore di indici
            //debug.log
        foreach(Node nodiLivello in matrice[matrixLayer - 2])
        {
            if (nodiVicini.Contains(nodiLivello))
            {
                if (!loopNode.Contains(nodiLivello))
                {

                    loopNode.Add(nodiLivello);
                    prossimiDaControllare.Add(nodiLivello);
                }

            }
        }
            
        //ATTENZIONE POTREBBE PORTARE PROBLEMI
        matrixLayer--;
        ///passo 3*
        ///ripeti il passo 2 fino a che non sei al layer 0 o non hai trovato un'altro duplicato(2 volte lo stesso nodo)
        ///questa volta e piu facile dato che troveremo al massimo un nodo per direzione e abbiamo 2 direzioni
        if (prossimiDaControllare.Count != 1)
        {
            //e uno solo se siamo arrivati al nodo sorgente da cui siamo partiti
            BackTrack(prossimiDaControllare);
        }
        

        //dalla penultima riga della matrice controlla i nodi e prendi quelli ai livelli più alti
        //foreach ()

    }

    ///passo 4 *
    ///controlla che sia un ciclo
    ///passo 5 *
    ///diminuisci tutti gli archi del ciclo di 1
    private void LoopCheck()
    {
        List<Node> vicini = new List<Node>();
        
        foreach(Node a in loopNode)
        {
            vicini.AddRange(a.NearNodes(a.ConnectedArc()).ToList());
            vicini.AddRange(loopNode);
            Node[] u = vicini
                    .GroupBy(i => i)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key).ToArray();
            foreach(Node b in u)
            {
                if(!loopArc.Contains(a.FindArc(a, b)))
                {
                    loopArc.Add(a.FindArc(a, b));
                }                
            }

        }
    }


    ///passo 8*
    ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
    private void LookForNext()
    {
        bool trovato = false;
        //controlla il secondo livello della matrice e vedi se ci sono nodi con archi non vuoti che quindi possiamo usare per il prossimo poligono
        foreach(Node a in matrice[1])
        {
            foreach(Arc c in a.ConnectedArc())
            {
                if (c.value == 1)
                {
                    trovato = true;
                }
                if (trovato)
                {
                    FindLoops(a);
                }
            }
        }
    }


}
