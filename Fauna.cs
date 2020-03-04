using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna : MonoBehaviour
{
    bool passive;//true passivo false aggressivo
    public int numEntità;
    int movimento;
    int ciboNecessario;
    Zone accampamento;
    List<Zone> visitato;

    GodsEye god;

    public Fauna Spawn(Zone nido)
    {
        numEntità = UnityEngine.Random.Range(2, 5);
        if (UnityEngine.Random.Range(0, 1) == 0)
        {
            passive = true;
            ciboNecessario = numEntità;
        }
        else
        {
            passive = false;
            ciboNecessario = numEntità*2;
        }     
        accampamento = nido;
        visitato.Add(nido);
        /////
        //////
        ///ERROREEEEEEEEEEEEEEEEEEEEEEE
        return this;
    }

    public void NewGeneration()
    {
        numEntità += numEntità / 2;
        if (passive)
        {
            ciboNecessario = numEntità;
        }
        else
        {
            ciboNecessario = numEntità * 2;
        }
    }

    void Eat()
    {
        while(accampamento.GetFood() == 0|| !accampamento.food)
        {
            if(!passive&& accampamento.mob.Count > 1)
            {
                break;
            }
            if (movimento == 0)
            {
                Starvation();
            }
            Explore();
            movimento--;           
        }
        ///////////////WIP
        if (ciboNecessario<accampamento.GetFood()&& passive)
        {
            Consume();
        }
        else
        {

            int poor =  accampamento.GetFood()- ciboNecessario;
            Die(poor);
        }
        if (!passive)
        {
            if (accampamento.mob.Count > 1)
            {
                int food =Hunt();
                ciboNecessario -= food;
            }
            if (ciboNecessario > 0)
            {
                if(ciboNecessario < accampamento.GetFood())
                {
                    Consume();
                }
                else
                {
                    int poor = accampamento.GetFood() - ciboNecessario;
                    Die(poor);
                }
            }
        }
    }

    private void Die(int poor)
    {
        numEntità -= UnityEngine.Random.Range(0,poor);
    }

    private void Consume()
    {
        accampamento.risorse.MobVSFood(ciboNecessario);
    }

    int Hunt()
    {
        Fauna atk = this;
        Fauna def;

        if (accampamento.mob[1] != this)
        {
                def = accampamento.mob[1];

        }
        else
        {
            def = accampamento.mob[0];
        }
 
        int atkDice = atk.numEntità / 4;
        int defDice = def.numEntità / 4;
        int diff=Mathf.Abs(atkDice-defDice);
        int food = 0;
        if (atkDice == defDice)
        {
            
            
            int warResult = UnityEngine.Random.Range(0, 10);
            if ( warResult>= 5)
            {
                def.numEntità -= atk.numEntità;
                food = atk.numEntità;
                god.CheckEaten(def);
            }
            else if(warResult >=3)
            {
                def.numEntità -= atk.numEntità/2;
                food = atk.numEntità / 2;
                atk.numEntità -= atk.numEntità / 4;
                god.CheckEaten(def);
            }
            else if (warResult >= 1)
            {
                def.numEntità -= atk.numEntità / 4;
                food = atk.numEntità / 4;
                atk.numEntità -= atk.numEntità / 2;
                god.CheckEaten(def);
            }
        }
        else if (atkDice > defDice)
        {
            int warResult = UnityEngine.Random.Range(0, 10);
            if (warResult >= 3)
            {
                def.numEntità -= atk.numEntità;
                food = atk.numEntità;
                god.CheckEaten(def);
            }
            else if (warResult >= 2)
            {
                def.numEntità -= atk.numEntità / 2;
                food = atk.numEntità / 2;
                atk.numEntità -= atk.numEntità / 4;
                god.CheckEaten(def);
            }
            else if (warResult >= 1)
            {
                def.numEntità -= atk.numEntità / 4;
                food = atk.numEntità / 4;
                atk.numEntità -= atk.numEntità / 2;
                god.CheckEaten(def);
            }
        }
        else if (atkDice < defDice)
        {
            int warResult = UnityEngine.Random.Range(0, 10);
            if (warResult >= 8)
            {
                def.numEntità -= atk.numEntità;
                food = atk.numEntità;
                god.CheckEaten(def);
            }
            else if (warResult >= 5)
            {
                def.numEntità -= atk.numEntità / 2;
                food = atk.numEntità / 2;
                atk.numEntità -= atk.numEntità / 4;
                god.CheckEaten(def);
            }
            else if (warResult >= 3)
            {
                def.numEntità -= atk.numEntità / 4;
                food = atk.numEntità / 4;
                atk.numEntità -= atk.numEntità / 2;
                god.CheckEaten(def);
            }
            else if (warResult >= 1)
            {
                food = 0;
            }
        }
        return food;
    }

    private void Starvation()
    {
        if (numEntità == 1)
        {
            Death();
        }
        else
        {
            numEntità = numEntità / 2;
        }
    }

    private void Death()
    {
        god.Estinzione(this);
    }

    void Explore()
    {
        List<Zone> a = accampamento.vicini;
        if (passive)
        {   
            foreach (Zone b in a)
            {
                if (!b.food || b.mobIn)
                {
                    a.Remove(b);
                }           
            }
            if (a.Count == 0)
            {
                a = accampamento.vicini;
            }         
            Spostamento(a[UnityEngine.Random.Range(0, a.Count - 1)]);
        }
        else
        {
            Spostamento(a[UnityEngine.Random.Range(0, a.Count - 1)]);
        }
    }





    
    void Spostamento(Zone a)
    {
            accampamento.UscitaMob(this);
            accampamento = a;
            visitato.Add(a);
            accampamento.EntrataMob(this);

    }
    
}
