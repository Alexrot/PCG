using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Text t;
    // Start is called before the first frame update
    void Start()
    {

        List<int> b = new List<int>();
        b.Add(1);
        b.Add(2);
        b.Add(4);
        b.Add(5);
        b.Add(7);
        b.Add(1);
        b.Add(4);
        foreach (int value in b)
        {
            Debug.Log("{0} "+ 5/3);
        }
        
    }

    public void Method()
    {

    }



    static bool IsEven(int value)
    {
        return value % 2 == 0;
    }
}





