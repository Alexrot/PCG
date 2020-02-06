using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoopFinder : MonoBehaviour
{
    /// 
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
    /// 




    List<Arc> percorsoUsato;
    List<Node> livello;
    Boolean loop;
    List<List<Node>> matrice;
    public MeshGenerator poliGen;
    Transform poligono;

    Node exit;
    List<Node> loopNode;
    List<Arc> loopArc;
    int matrixLayer;
    int matrixLayerDev;


    public LoopFinder()
    {
        percorsoUsato = new List<Arc>();
        loopArc = new List<Arc>();
        loopNode = new List<Node>();
        livello = new List<Node>();
        matrice = new List<List<Node>>();
        
        poliGen = new MeshGenerator();
    }

    void PReset()
    {
        percorsoUsato.Clear();
        loopArc.Clear();
        loopNode.Clear();
        livello.Clear();
        matrice.Clear();
        matrixLayer = 0;
        matrixLayerDev = 0;
        loop = false;
    }
    
    public void PolyTransform(Transform p, Node ex)
    {
        poligono = p;
        exit = ex;
    }

    public Node FindLoops(Node start)
    {

        PReset();
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
            if (livello.Count <= 1)
            {
                Debug.Log("errore");
                Debug.Log("nodo iniziale " + matrice[0][0].position);
                return exit;
            }
                matrice.Add(livello.ToList());
                IsLoop(matrice);
            }

            /// passo 4(TROVATO)
            /// se c'è hai trovato un ciclo, se non c'è ripeti il passo 2
     
        LoopFound();
        ///passo 8*
        ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
        /*
        Node next = LookForNext();
        
        if (CheckNext(next))
        {
            return next;
        }
        else
        {
            
            return exit;
        }
        
        */
        return exit;
    }


    private bool CheckNext(Node a)
    {
        //controll che il prossimo nodo non sia vuoto 
        return true;
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
        duplicato.AddRange(TrovaDuplicato());
        int i = 0;
        foreach (Node a in duplicato)
        {
            loopNode.Add(duplicato[i]);
            matrixLayerDev = matrixLayer;
            Debug.Log(matrixLayerDev);
            BackTrack(duplicato, matrixLayer);
            i++;


            ///passo 4 *
            ///controlla che sia un ciclo
            ///passo 5 *
            ///diminuisci tutti gli archi del ciclo di 1
            //OrdinaLoop();
            /*
            foreach(Arc a in loopArc)
            {
                a.ReduceValue();
            }
            */
            ///passo 6*
            ///passa il ciclo alla funzione che genera il poligono
            //poliGen.PolyGen(loopNode, poligono);

        }

    }




    /// passo 3
    /// /// controlla che non ci sia un dupicato 
    private void IsLoop(List<List<Node>> matrice)
    {
        
        //controllo l'ultimo e il penultimo, poiche non è possibile che si generano cicli tra livelli più distanti
        List<Node> lastTwo=new List<Node>();
        lastTwo.AddRange(matrice[matrixLayer]);
        foreach (Node a in lastTwo)

            if (lastTwo.GroupBy(n => n).Any(c => c.Count() > 1))
            {
                loop = true;
                if (loop)
                {
                    break;
                }
            }
        if (!loop)
        {
            lastTwo.AddRange(matrice[matrixLayer - 1]);
            foreach (Node a in lastTwo)

                if (lastTwo.GroupBy(n => n).Any(c => c.Count() > 1))
                {
                    loop = true;
                    if (loop)
                    {
                        break;
                    }
                }
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
    private List<Node> TrovaDuplicato()
    {
        //controlla le ultime 2 righe per trovare duplicati
        List<Node> ultimeDue = new List<Node>();
        matrixLayer = matrice.Count();
        

        ultimeDue.AddRange(matrice[matrixLayer-1]);
        ultimeDue.AddRange(matrice[matrixLayer - 2]);

        Debug.Log("duplicato trovato");

        //Node duplicates = (Node)ultimeDue.GroupBy(s => s).SelectMany(grp => grp.Skip(1));
        List<Node> duplicates = ultimeDue.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

        return duplicates;
    }


    ///passo 2*
    ///controlla tra i suoi nodi adiacenti quali sono presenti nei livelli superiori nella matice
    /// <summary>
    /// torna ai livelli superiori della matrice per trovare i nodi che creano il loop
    /// </summary>
    /// <param name="daControllare">lista di nodi da controllare</param>
    public void BackTrack(List<Node> daControllare, int currentLayer)
    {
        
        List<Node> prossimiDaControllare = new List<Node>();
        List<Node> nodiVicini = new List<Node>();
        
        foreach (Node a in daControllare)
        {
            Debug.Log("BACKTRACK " + a.position);
            //prend tutti i nodi VICINI alla lista di nodi passata
            nodiVicini.AddRange(a.NearNodes(a.ConnectedArc()));
            
        }
                       
        //SE IN UN LIVELLO SUPERIORE SI RITROVA L'ARCO STESSO (PROBABILE CHE SUCCEDA COL DUPLICATO APPENA SCELTO)
        //CONTROLLA ANCHE QUELLO SUPERIORE DI LIVELLO CON QUELLO STESSO NODO
        if (matrice[currentLayer - 2].Contains(loopNode[0]))
        {
            prossimiDaControllare.Add(loopNode[0]);
        }
        foreach (Node nodiViciniA in nodiVicini)
        {
            Debug.Log("BACKTRACK al livello"+ (currentLayer - 1) + "di " + nodiViciniA.position);
            if (matrice[currentLayer - 2].Contains(nodiViciniA))
            {
                if (!loopNode.Contains(nodiViciniA))
                {
                    loopNode.Add(nodiViciniA);
                    prossimiDaControllare.Add(nodiViciniA);
                }
                
            }
        }

        Debug.Log("BACKTRACK numero di nodi da controllare " + prossimiDaControllare.Count+" e sono");
        foreach(Node a in prossimiDaControllare)
        {
            Debug.Log("BACKTRACK prossimi da controllare:" + a.position);
        }

        if (prossimiDaControllare.Count==1)
        {
            //DUPLICATOOOOOOOOOOOOOOOOOOO
            Debug.Log("BACKTRACK esco" );
            
        }
        else
        {
            currentLayer--;
            BackTrack(prossimiDaControllare, currentLayer);
        }
    }

    ///passo 4 *
    ///controlla che sia un ciclo
    ///passo 5 *
    ///diminuisci tutti gli archi del ciclo di 1
    void OrdinaLoop()
    {
        
        List<Node> nodiOrdinati = new List<Node>();
        Node nextToAdd = loopNode[0];
        nodiOrdinati.Add(nextToAdd);
        foreach(Node a in loopNode[0].NearNodes(loopNode[0].ConnectedArc()))
        {
            if (loopNode.Contains(a))
            {
                nextToAdd=a;
                ReduceArc(nodiOrdinati[nodiOrdinati.Count - 1], a);
                break;
            }
        }
        do
        {
            nodiOrdinati.Add(nextToAdd);
            //ReduceArc(nodiOrdinati[nodiOrdinati.Count - 1], nodiOrdinati[nodiOrdinati.Count - 2]);
            nextToAdd = NextToLoop(nodiOrdinati);
        } while (nextToAdd != loopNode[0]);
        ReduceArc(nodiOrdinati[nodiOrdinati.Count - 1], loopNode[0]);

        loopNode.Clear();
        loopNode.AddRange(nodiOrdinati);
    }

    void ReduceArc(Node a, Node b)
    {
        Arc dev = a.FindArc(a, b);
        if (!loopArc.Contains(dev))
        {
            loopArc.Add(dev);
            dev.ReduceValue();
        }
    }

    Node NextToLoop(List<Node> start)
    {
        foreach (Node a in start[start.Count-1].NearNodes(start[start.Count - 1].ConnectedArc()))
        {
            if (loopNode.Contains(a)&&a!= start[start.Count - 2])
            {
                ReduceArc(start[start.Count - 1], a);
                return a;
            }
        }
        return null;
    }



    ///passo 8*
    ///cerca nel secondo livello della matrice un nuovo nodo da usare per ricominciare
    private Node LookForNext()
    {
        bool livUno = false;
        bool trovato = false;
        Node nextN = matrice[0][0];
        //controlliamo se il nodo a liv 0 di matrixLayer non ha piu archi uscenti di valore >0
        foreach (Arc a in nextN.ConnectedArc())
        {
            Debug.Log(a.GetValue()+"valore degli archi");
            if (a.GetValue() > 0)
            {
                livUno = true;
                Debug.Log(nextN.position);
                return nextN;
            }
        }
        if(!livUno)
        {
            
            
            //controlla il secondo livello della matrice e vedi se ci sono nodi con archi non vuoti che quindi possiamo usare per il prossimo poligono
            foreach (Node a in matrice[1])
            {
                foreach (Arc c in a.ConnectedArc())
                {
                    if (c.value == 1)
                    {
                        trovato = true;
                        nextN = a;
                        break;
                    }
                   
                }
            }

        }
        if (trovato)
        {
            Debug.Log(nextN.position);
            return nextN;
        }
        else
        {
            return matrice[0][0];
        }

    }


}
