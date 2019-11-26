using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2 position;
    public List<Arc> edges;
    public int countArc;
    public Node(Vector2 position)
    {
        this.position = position;
        edges = new List<Arc>();
        countArc = 0;
    }



    public void AddEdgeToNode(ref Arc a)
    {
        edges.Add(a);
        countArc++;
    }

    public void RemoveArc(Arc c)
    {

        edges.Remove(c);
        countArc--;

    }



    public Arc[] ConnectedArc()
    {
        Arc[] n=edges.ToArray();
        return n;
    }

    public Node[] NearNodes(Arc[] a)
    {
        Node[] n= { };
        for(int i=0;i<a.Length; i++)
        {
            if (a[i].a == this)
            {
                n[i] = a[i].b;
            }
            else
            {
                n[i] = a[i].a;
            }
        }
        
        return n;
    }

    /*
    public void AddEdgeToNode(Node a, Node b)
    {
        Arc c = new Arc(2, a, b);
        if (!FindEdge(c))
        {
            edges[countArc] = c;
        }
        
        countArc++;
    }





            //si usa per gli archi sui bordi
    public void AddEdgeToNodeLimit(Node a, Node b)
    {
        Arc c = new Arc(1, a, b);
        if (!FindEdge(c))
        {
            edges[countArc] = c;
        }

        countArc++;
    }
    */

    private bool FindEdge(Arc c)
    {
        return edges.Contains(c);
    }



    public List<Arc> GetArcs()
    {
        return edges;
    }


    
}
