using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node 
{
    public Vector2 position;
    public List<Arc> edges;

    public Vector2 GetPosition()
    {
        return position;
    }
    
    public Node(Vector2 position)
    {
        this.position = position;
        edges = new List<Arc>();
        
    }



    public void AddEdgeToNode(ref Arc a)
    {
        edges.Add(a);
        
    }

    public void RemoveArc(Arc c)
    {

        edges.Remove(c);
        

    }



    public List<Arc> ConnectedArc()
    {
        return edges;
    }

    public List<Node> NearNodes(List<Arc> a)
    {
        List<Node> n= new List<Node>();
        for(int i=0;i<a.Count; i++)
        {
            if (a[i].a == this)
            {
                n.Add(a[i].b);
            }
            else
            {
                n.Add(a[i].a);
            }
        }
        
        return n;
    }


    /// <summary>
    /// trova un arco che collega 2 nodi
    /// </summary>
    /// <param name="a">nodo 1</param>
    /// <param name="b">nodo 2</param>
    public Arc FindArc(Node a, Node b)
    {
        List<Arc> archiA = a.ConnectedArc();
        List<Arc> archiB = b.ConnectedArc();
        List<Arc> archi = new List<Arc>();
        archi.AddRange(archiA);
        archi.AddRange(archiB);
        Arc[] u = archi
                    .GroupBy(i => i)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key).ToArray();
        
        return u[0];
    }

    public bool EsisteArco(Node a, Node b)
    {
        List<Arc> archiA = a.ConnectedArc();
        List<Arc> archiB = b.ConnectedArc();
        List<Arc> archi = new List<Arc>();
        archi.AddRange(archiA);
        archi.AddRange(archiB);
        Arc[] u = archi
                    .GroupBy(i => i)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key).ToArray();

        if (u.Count()>0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
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
  
    private bool FindEdge(Arc c)
    {
        return edges.Contains(c);
    }
    */



    public List<Arc> GetArcs()
    {
        return edges;
    }


    
}
