using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flora : MonoBehaviour
{
    public int risorse;
    public int limite;
    int spawMax;//1 se (0-10) 2 se (4-20) 3 se (10-30)
    int spawMin;
    public int season;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MobVSFood(int n)
    {
        risorse -= n;
    }


    /// <summary>
    /// genera una pianta nel poligono
    /// </summary>
    /// <param name="umidità">umidtà relativa alla zona</param>
    /// <returns>stagione di crescita</returns>
    public int GeneratePlants(int riproduzione, int limite)
    {
        switch (riproduzione)
        {
            case 1:
                spawMin = 10;
                spawMax = 20;
                break;
            case 2:
                spawMin = 15;
                spawMax = 30;
                break;
            case 3:
                spawMin = 40;
                spawMax = 50;
                break;
        }
        risorse = UnityEngine.Random.Range(spawMin, spawMax);
        this.limite = limite;
        SetSeason();
        return 0;
    }

    private void SetSeason()
    {
        season = UnityEngine.Random.Range(1, 4);
    }

    public void Spaw()
    {
        risorse += UnityEngine.Random.Range(spawMin, spawMax);
        if (risorse > limite)
        {
            risorse = limite;
        }
    }
}
