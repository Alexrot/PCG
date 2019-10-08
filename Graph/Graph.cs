using System;
using System.Collections.Generic;

class Graph
{
        List<Node> nodes;


        public Graph(int max)
        {
            nodes = new List<Node>();
        }




        public void AddNode(Vector2f n)
        {
        Node c = new Node(n);
            nodes.Add(c);
        }





        private bool FindNode(Node c)
        {
            return nodes.Contains(c);
        }






        public void AddEdge(Node a, Node b)
        {
            if(FindNode(a) || FindNode(b))
            {
            //aggiungo l'arco ad entrabi i nodi senza fregarmene dell'ardine perche non e ordinato il grafo
            
                for(int i=0;i<nodes.Count; i++)
                {
                    if (nodes[i] == a)
                    {
                        nodes[i].AddEdge(a, b);
                    }
                    if (nodes[i] == b)
                    {
                        nodes[i].AddEdge(a, b);
                    }
                }
            }
        }



    public void Delete(Node c)
    {
        nodes.Remove(c);
        DeleteEdge(c);
        
        
    }


    //cancella tutti gli archi del nodo c
    public void DeleteEdge(Node c)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            for(int j = 0; j < nodes[i].countArc; j++)
            {
                if (nodes[i].edges[j].a == c || nodes[i].edges[j].b == c)
                {
                    nodes[i].RemoveArc(nodes[i].edges[j]);
                }
            }
        }
    }


}


