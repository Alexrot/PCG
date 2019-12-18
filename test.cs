using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    
      //public Transform poligon;
    


    // Start is called before the first frame update
    void Start()
    {          
        /*
        Mesh mesh = new Mesh();
        Transform newPoly = Instantiate(poligon, new Vector3(0, 0, 0), Quaternion.identity);

      
        Vector3[] vertices = new Vector3[5];

        vertices[0] = new Vector3(.5f, .5f);
        vertices[1] = new Vector3(-1, .8f);
        vertices[2] = new Vector3(-.2f, 1.2f);
        vertices[3] = new Vector3(.8f, 1.4f);
        vertices[4] = new Vector3(1, .8f);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        //GetComponent<MeshFilter>().mesh = mesh;
        newPoly.GetComponent<MeshFilter>().mesh = mesh;

        */














        /*


        Transform newPoly = Instantiate(poligon, new Vector3(0, 0, 0), Quaternion.identity);

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[7];
        List<Node> nodes = new List<Node>();

        vertices[0] = new Vector3(1f, .5f);
        vertices[1] = new Vector3(1.5f, .5f);
        vertices[2] = new Vector3(.5f, 1.5f);
        /*
        vertices[3] = new Vector3(.25f, .5f);
        vertices[4] = new Vector3(.5f, .5f);
        vertices[5] = new Vector3(-1, .8f);
        vertices[6] = new Vector3(-.2f, 1.2f);
        //vertices[3] = new Vector3(.8f, 1.4f);
        //vertices[4] = new Vector3(1, .8f);
        nodes.Add(new Node(vertices[0]));
        nodes.Add(new Node(vertices[1]));
        nodes.Add(new Node(vertices[2]));
        nodes.Add(new Node(vertices[3]));
        nodes.Add(new Node(vertices[4]));
        nodes.Add(new Node(vertices[5]));
        nodes.Add(new Node(vertices[6]));
        */
        /*
        mesh.vertices = vertices;
        Node a =nodes.Find(x => x.position.Equals(new Vector3(.5f, .5f)));
        bool b = nodes.Exists(x => x.position.Equals(new Vector3(1f, .5f)));
        
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        //GetComponent<MeshFilter>().mesh = mesh;
        newPoly.GetComponent<MeshFilter>().mesh = mesh;

        
        //GUI.DrawTexture(new Rect(10, 10, 60, 60), aTexture, ScaleMode.ScaleToFit, true, 10.0F);
        */
        
        int[] lastTwo = { 1,0,1,2,3,6,5,3,7,8,9};
        int[] b = lastTwo.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToArray();
        foreach (int a in b)
        {
            Debug.Log("funge "+a);
        }

    }
}


/*



///passo 2*
///controlla tra i suoi nodi adiacenti quali sono presenti nei livelli superiori nella matice
private void BackTrack(List<Node> a)
{
    /*
    List<Node> prossimiDaControllare = new List<Node>();
    List<Node> nodiVicini = new List<Node>();
    foreach (Node k in a)
    {
        nodiVicini = k.NearNodes(k.ConnectedArc());
    }

    //SE IN UN LIVELLO SUPERIORE SI RITROVA L'ARCO STESSO (PROBABILE CHE SUCCEDA COL DUPLICATO APPENA SCELTO)
    //CONTROLLA ANCHE QUELLO SUPERIORE DI LIVELLO CON QUELLO STESSO NODO
    if (nodiVicini.Contains(loopNode[0]))
    {
        prossimiDaControllare.Add(loopNode[0]);
    }
    */
    //controlla per errore di indici
    //debug.log
    /*
     * 
     * NON FUNZIONA
     * 
     * 
     * 
     */
     /*

    Debug.Log(matrixLayerDev);
    if (matrixLayer == matrixLayerDev)
        foreach (Node nodiLivello in matrice[matrixLayerDev - 1])
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
    foreach (Node nodiLivello in matrice[matrixLayerDev - 2])
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


    matrixLayerDev--;
    ///passo 3*
    ///ripeti il passo 2 fino a che non sei al layer 0 o non hai trovato un'altro duplicato(2 volte lo stesso nodo)
    ///questa volta e piu facile dato che troveremo al massimo un nodo per direzione e abbiamo 2 direzioni
    ////errore!!!!!!!!!!CONTINUO SOLO SE NON HO ANCORA TROVATO UN DUPLICATO //
    ////e ancora possibilie trovare errori ....forse
    ///
    Debug.Log(prossimiDaControllare.Count + " sono i prossimi");

    if (prossimiDaControllare.Count > 1)
    {
        //e uno solo se siamo in cima alla matrice
        BackTrack(prossimiDaControllare);
    }
    else if (prossimiDaControllare.Count == 0 && prossimiDaControllare[0] == loopNode[0])
    {
        BackTrack(prossimiDaControllare);
    }

    Debug.Log("NUMERO nodi ciclo :" + loopNode.Count);
    foreach (Node ga in loopNode)
    {
        Debug.Log("nodi ciclo :" + ga.position);
    }
    //dalla penultima riga della matrice controlla i nodi e prendi quelli ai livelli più alti
    //foreach ()

}
*/