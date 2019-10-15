using System;
using System.Collections.Generic;

public class Graph
{
    List<Node> nodes;
    List<Arc> arcs;


    public Graph(int max)
        {
            nodes = new List<Node>();
        }




        public Node AddNode(Vector2f n)
        {
            
            Node c = new Node(n);
            if (!FindNode(c))
            {
            nodes.Add(c);
            }
            
        return c;
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
        if (FindNode(a) || FindNode(b))
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
        DeleteEdge(c.connectedArc(c));
        nodes.Remove(c);
        
        
    }

    public Node FindClose(Node position, bool vertORorizon)
    {
        Vector2f newNode = position.position;
        bool searchForNode = true;
        Node temp;
        while (searchForNode)
        {
            if (vertORorizon)
            {//x++
                newNode.x+=1f;
                //genero un nodo temporaneo per controllare l'esistenza di un nodo in quella posizione
                 temp = new Node(newNode);
                if (FindNode(temp))
                {
                    return temp;
                }
            }
            else
            {//y++
                newNode.y += 1f;
                //genero un nodo temporaneo per controllare l'esistenza di un nodo in quella posizione
                temp = new Node(newNode);
                if (FindNode(temp))
                {
                    return temp;
                }
            }
        }
        return null;
    }





    //cancella tutti gli archi del nodo c
    public void DeleteEdge(Arc[] c)
    {
        
        for (int i = 0; i < c.Length; i++)
        {
            arcs.Remove(c[i]);
        }
      
        
    }


}


