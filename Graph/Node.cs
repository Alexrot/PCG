using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2f position;
    public List<Arc> edges;
    public int countArc;
    public Node(Vector2f position)
    {
        this.position = position;
        edges = new List<Arc>();
        countArc = 0;
    }


    public void AddEdgeToNode(Node a, Node b)
    {
        Arc c = new Arc(2, a, b);
        if (!FindEdge(c))
        {
            edges[countArc] = c;
        }
        
        countArc++;
    }

    public void RemoveArc(Arc c)
    {

                edges.Remove(c);
                countArc--;

    }


    private bool FindEdge(Arc c)
    {
        return edges.Contains(c);
    }
}
