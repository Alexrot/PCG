using System;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public List<Node> nodes;
    public List<Arc> arcs;


    public Graph()
        {
        nodes = new List<Node>();
        arcs = new List<Arc>();
        }




        public Node AddNode(Vector2 n)
        {
            
            Node c = new Node(n);
            if (!nodes.Contains(c))
            {
            nodes.Add(c);
            }
            
        return c;
        }




        public void AddEdge(Node a, Node b)
        {
            if(nodes.Contains(a) || nodes.Contains(b))
            {
            //aggiungo l'arco ad entrabi i nodi senza fregarmene dell'ardine perche non e ordinato il grafo
            Arc temp = new Arc(2, a, b);
            arcs.Add(temp);

                for(int i=0;i<nodes.Count; i++)
                {
                    if (nodes[i] == a)
                    {
                        nodes[i].AddEdgeToNode(ref temp);
                    }
                    if (nodes[i] == b)
                    {
                        nodes[i].AddEdgeToNode(ref temp);
                    }
                }
            }
        }
    
    //si usa per gli archi sui bordi
    public void AddEdgeLimit(Node a, Node b)
    {
        if (nodes.Contains(a) || nodes.Contains(b))
        {
            Arc temp = new Arc(1, a, b);
            arcs.Add(temp);
            //aggiungo l'arco ad entrabi i nodi senza fregarmene dell'ardine perche non e ordinato il grafo

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] == a)
                {
                    nodes[i].AddEdgeToNode(ref temp);
                }
                if (nodes[i] == b)
                {
                    nodes[i].AddEdgeToNode(ref temp);
                }
            }
        }
    }




    public void Delete(Node c)
    {
        //DeleteEdge(c.ConnectedArc());
        //passando il riferimento all'arco non c'è bisogno che io lo cancelli
        //HAHAHAHHAHAHAHAH NO T.T
        nodes.Remove(c);
        
        
    }






    //cancella tutti gli archi del array c
    public void DeleteEdge(Arc[] c)
    {
        
        for (int i = 0; i < c.Length; i++)
        {
            arcs.Remove(c[i]);
        }
      
        
    }

    public void UpdateEdge(Arc c)
    {
        c.ReduceValue();
    }
}


