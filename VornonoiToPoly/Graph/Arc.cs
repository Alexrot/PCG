using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc
{
    public Node a;
    public Node b;
    public int value;

    public Arc(int value, Node a, Node b)
    {
        this.value = value;
        this.a = a;
        this.b = b;
    }

    public void ReduceValue()
    {
        value -= 1;
    }

    public Node GetA() { return a; }
    public Node GetB() { return b; }
    public int GetValue() { return value; }
}
