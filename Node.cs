using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2f position;
    public Arc[] edges;
    public int countArc;
    public Node(Vector2f position)
    {
        this.position = position;
        edges = new Arc[99];
        countArc = 0;
    }


    public void AddEdge(Node a, Node b)
    {
        edges[countArc] = new Arc(2, a, b);

    }

    public void RemuveEdge()
    {

    }
}
