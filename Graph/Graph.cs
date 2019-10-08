using System;
using System.Collections.Generic;

class Graph
{
        Node[] nodes;
        //Arc[] edge;
        int count;
        //int countArc;
        public Graph(int max)
        {
            nodes = new Node[max];
            count = 0;
            countArc = 0;
        }
        public void AddNode(Vector2f n)
        {
            nodes[count] = new Node(n);
            count++; 

        }
        private int FindNode(Node c)
        {
            for(int i=0;i<nodes.Length;i++)
                if (nodes[i] == c)
                {
                    return i;
                }
            return -1;
        }
        public void AddEdge(Node a, Node b)
        {
            int aIndex = FindNode(a);
            int bIndex = FindNode(b);
            if(aIndex != -1 || bIndex != -1)
            {
            //aggiungo l'arco ad entrabi i nodi senza fregarmene dell'ardine perche non e ordinato il grafo
            nodes[aIndex].AddEdge(a, b);
            nodes[bIndex].AddEdge(a, b);
            }
        }

    public void Swap(Node a, Node b)
    {
        int aIndex = FindNode(a);
        int bIndex = FindNode(b);
        Node temp = nodes[aIndex];
        nodes[aIndex] = nodes[bIndex];
        nodes[bIndex] = temp;

    }


    public void Delete(Node c)
    {
        int Charpos = FindNode(c);
        Node[] temp = new Node[nodes.Length - 1];
        for(int i = 0; i < nodes.Length; i++)
        {
            if (i < Charpos)
            {
                temp[i] = nodes[i];
            }
            if (i > Charpos)
            {
                temp[i - 1] = nodes[i];
            }
        }
        DeleteEdge(c);
        nodes = temp;
        count--;
    }
    public void AddNewNode(Vector2f c)
    {
        Node[] temp = new Node[nodes.Length + 1];
        for (int i = 0; i < nodes.Length; i++)
        {
            temp[i] = nodes[i];
        }
        temp[nodes.Length] = new Node(c);
        count++;
        nodes = temp;

    }

    public void DeleteEdge(Node c)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            for(int j = 0; j < nodes[i].countArc; j++)
            {
                if (nodes[i].edges[j] == c)
                {
                    nodes[i].edges.Remove(c);
                }
            }
        }
    }

    public void Print()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            Console.Write("({0})-", nodes[i].value);
            for(int j = 0; j < nodes[i].edges.Count; j++)
            {
                Console.Write("({0})-", nodes[i].edges[j]);
            }
        }
    }
}