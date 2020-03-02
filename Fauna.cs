using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna : MonoBehaviour
{
    bool passive;//true passivo false aggressivo
    int numEntità;
    int movimento;
    int ciboNecessario;
    Zone accampamento;
    List<Zone> visitato;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Eat()
    {
        while(accampamento.GetFood() < ciboNecessario)
        {
            if (movimento == 0)
            {
                Starvation();
            }
            Explore();
            movimento--;
            
        }
    }

    private void Starvation()
    {
        
    }

    void Explore()
    {
        List<Zone> a = accampamento.vicini;



        if (passive)
        {
            foreach (Zone b in a)
            {
                if (!b.food)
                {
                    a.Remove(b);
                }
            }
            int nextZone = UnityEngine.Random.Range(0, a.Count - 1);
            Spostamento(a[nextZone]);


            /*
            if (a[nextZone].food && !a[nextZone].mobIn && passive)
            {
                accampamento.UscitaMob();
                accampamento = a[nextZone];
                visitato.Add(a[nextZone]);
                accampamento.EntrataMob(this);
            }
            else if (!passive)
            {

            }
            a.Remove(a[nextZone]);

        }

        */

        }
    }





    
    void Spostamento(Zone a)
    {
        if(!a.mobIn)
        {
            accampamento.UscitaMob();
            accampamento = a;
            visitato.Add(a);
            accampamento.EntrataMob(this);
        }
        else
        {
            //a.Remove(a[nextZone]);

        }
    }
    
}
