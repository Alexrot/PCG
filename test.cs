using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Text t;
    // Start is called before the first frame update
    void Start()
    {
        Node primo = new Node(new Vector2(0f, 10f));
        Node sec = new Node(new Vector2(1f, 10f));
        Node ter = new Node(new Vector2(10f, 0f));
        Node qua = new Node(new Vector2(10f, 10f));

        List<Node> a = new List<Node>();
        a.Add(primo);
        a.Add(sec);
        a.Add(ter);
        a.Add(qua);


        a.Sort();
        //string b = a[0].position.x.ToString();
       /* 
        t.text = "primo valore"+ a[0].position.x.ToString()+"x"+ a[0].position.y.ToString() + "y"+
            "secondo valore" + a[1].position.x.ToString() + "x" + a[1].position.y.ToString() + "y"+
            "terzo valore" + a[2].position.x.ToString() + "x" + a[2].position.y.ToString() + "y"+
            "quarto valore" + a[3].position.x.ToString() + "x" + a[3].position.y.ToString() + "y"
            ;
        //Debug.Log("" +b );*/
    }

    public void Method()
    {

    }





}
