using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public List<Arco> ar;
    public Text tRef;
    public Text tBase;
    //int number=0;
    //int ra = 0;
    // Start is called before the first frame update
    void Start()
    {
        ar = new List<Arco>();
        Arco a = new Arco(5);
       
            Method(ref a);
        a.ReduceValue();
        tBase.text = a.GetValue() + "";
        a.ReduceValue();
        Node[] prossimiDaControllare = { };
        a = null;
        tRef.text = ar[0].GetValue() + "";

        //GameObject newGO = new GameObject("myTextGO");
        //newGO.transform.SetParent(this.transform);

        //Text myText = newGO.AddComponent<Text>();


    }

    public void Method(ref Arco refArgument)
    {
        ar.Add(refArgument);
        ar[0].ReduceValue();
        tRef.text= ar[0].GetValue()+"";
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    public class Arco
    {
   
        public int value;

        public Arco(int value)
        {
            this.value = value;

        }
        public void ReduceValue()
        {
            value -= 1;
        }
        public int GetValue() { return value; }
    }

}
