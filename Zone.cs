
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{
    public GameObject polyGO;
    Transform poly;
    List<Vector2> poligono;
    public Vector2 centro;
    public float altezza;
    public float umidità;
    public float calore;
    public int typeBiome;
    public List<Zone> vicini;
    public bool humCheck = false;

    public bool mobIn = false;
    public List<Fauna> mob;
    

    public Flora risorse;
    public bool food = false;
    public int seasonFood;


    public Zone(List<Vector2> poligono, Vector2 centro, Transform segmento, float noise)
    {
        vicini = new List<Zone>();
        altezza = noise;
        this.poligono = poligono;
        this.centro = centro;
        poly = segmento;
        CalcolateHeatStart();
        DefineZoneByHum();

        polyGO = poly.gameObject;

        mob = new List<Fauna>();
        
        risorse = new Flora();

    }



    private int CalcolateSize()
    {
        float size = 0;
        foreach (Vector2 a in poligono)
        {
            size += (Math.Abs(a.x - centro.x) + Math.Abs(a.y - centro.y));
        }
        size = size * 100;
        return (int)size;
    }

    /// <summary>
    /// Inserisco nel poligono le informazioni sui poligoni vicini tramite i loro punti di voronoi
    /// </summary>
    /// <param name="vicini"></param>
    public void AddNeighbor(Zone vicino)
    {
        vicini.Add(vicino);
    }

    public void SetUmidità(float umidità)
    {
        if (umidità <= 0)
        {
            umidità = 0.01f;
        }
        else if (umidità > 1)
        {
            umidità = 1f;
        }
        else
        {
            this.umidità = umidità;
        }

        humCheck = true;

    }

    /// <summary>
    /// Definisce il poligono in base all'altezza
    /// </summary>
    public void DefineZoneType()
    {
        if (altezza <= 0.25)
        {
            //type 0 deepOcean
            typeBiome = 0;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.gray, 0.41f);
        }
        else if (altezza > 0.25 && altezza <= 0.45)
        {
            //type 1 Ocean
            typeBiome = 1;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.white, 0.31f);
        }
        else if (altezza > 0.45 && altezza <= 0.50)
        {
            //type 2 Coast
            typeBiome = 2;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.white, 0.41f);
        }
        else if (altezza > 0.50 && altezza <= 0.55)
        {
            //type 3 plain
            typeBiome = 3;
            poly.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (altezza > 0.55 && altezza <= 0.6)
        {
            //type 4 hill
            typeBiome = 4;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.gray, 0.41f);
        }
        else if (altezza > 0.6 && altezza <= 0.7)
        {
            //type 5 highHill
            typeBiome = 5;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, 0.557f);
        }
        else if (altezza > 0.7 && altezza <= 0.9)
        {
            //type 6 mountain
            typeBiome = 6;
            poly.GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (altezza > 0.9)
        {
            //type 7 ice
            typeBiome = 7;
            poly.GetComponent<Renderer>().material.color = Color.white;
        }
    }


    /// <summary>
    /// Definisce il poligono in base all'umidità
    /// </summary>
    public void DefineHumZone()
    {
        poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.Lerp(Color.white, Color.blue, 0.01f), Color.blue, umidità);
    }



    /// <summary>
    /// Definisce il poligono in base alla noise
    /// </summary>
    public void DefineNoiseZone()
    {
        poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.black, altezza);
    }

    /// <summary>
    /// Definisce il poligono in base al calore
    /// </summary>
    public void DefineHeatZone()
    {
        poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.red, calore);
    }



    /// <summary>
    /// Cambia il tipo della zona in base a temperatura e umidità
    /// 
    /// </summary>
    public void DefineZoneByHum()
    {
        Debug.Log("colore");

        //3 layer
        if (altezza <= 0.45)
        {
            ///mare
            if (altezza < 0.25)
            {
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.gray, 0.41f);
                typeBiome = 0;
            }
            else
            {
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.white, 0.31f);
                typeBiome = 1;
            }
            if (calore <= 0.20)
            {
                if (altezza < 0.10)
                {
                    poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.Lerp(Color.gray, Color.blue, 0.55f), 0.35f);
                    typeBiome = 3;
                }
                else
                {
                    poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.blue, 0.35f);
                    typeBiome = 2;
                }
            }
        }
        else if (altezza <= 0.65 && altezza > 0.45)///altezza 0 sopra il livello del mare
        {
            if (umidità <= 0.32)
            {
                //coast
                typeBiome = 4;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.white, 0.41f);
            }
            else if (umidità <= 0.82 && umidità > 0.32)
            {
                //plain
                typeBiome = 5;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.gray, 0.52f);
            }
            else if (umidità > 0.82)
            {
                //palude
                typeBiome = 6;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.Lerp(Color.green, Color.red, 0.657f), 0.62f);
            }
        }
        else if (altezza <= 0.85 && altezza > 0.65)
        {
            typeBiome = 2;
            ///altezza 2 sopra il livello del mare
            if (umidità <= 0.32)
            {
                //collina
                typeBiome = 7;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, 0.557f);
            }
            else if (umidità <= 0.64 && umidità > 0.32)
            {
                //pianura
                typeBiome = 8;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.gray, 0.67f);
            }
            else if (umidità > 0.64)
            {
                //foresta
                typeBiome = 9;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.Lerp(Color.blue, Color.gray, 0.45f), 0.59f);
            }
        }
        else if (altezza > 0.85)
        {
            ///altezza 3 sopra il livello del mare
            if (umidità <= 0.32)
            {
                //mesa
                typeBiome = 10;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.Lerp(Color.green, Color.red, 0.557f), 0.55f);
            }
            else if (umidità <= 0.64 && umidità > 0.32)
            {
                //taiga
                typeBiome = 11;
                poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.Lerp(Color.blue, Color.gray, 0.45f), 0.65f);
            }
            else if (umidità > 0.64)
            {
                //snow
                typeBiome = 12;
                poly.GetComponent<Renderer>().material.color = Color.white;
            }

        }
        if (altezza > 0.45 && calore == 0)
        {
            poly.GetComponent<Renderer>().material.color = Color.Lerp(poly.GetComponent<Renderer>().material.color, Color.Lerp(Color.blue, Color.white, 0.65f), 0.65f);
        }
        Debug.Log("colore"+typeBiome);
    }

    /// <summary>
    /// Calcola la temperatura del poligono in base alla sua posizione nella mappa
    /// </summary>
    void CalcolateHeatStart()
    {
        float pos = centro.y;
        calore = Math.Abs(1 - (Math.Abs(pos - 1000) / 1000));

    }

    public void ChangeHeat(float change)
    {

        CalcolateHeatStart();
        if (typeBiome == 2 || typeBiome == 3)
        {
            change = change / 2;
        }
        if (calore + change > 1)
        {
            calore = 1;
        }
        else if (calore + change < 0)
        {
            calore = 0;
        }
        else
        {
            calore = calore + change;
        }


    }

    ///////////////////////////////////////////////FLORA////////////////////////////////////////////////////////////
    public void SpawnFood()
    {
        risorse.Spawn();
    }

    public int GetFood()
    {
        return risorse.risorse;
    }

    public void GeneraFlora()
    {

        if (altezza < 0.85 && altezza > 0.45)
        {

            if (umidità > 0.32 && umidità <= 0.64)
            {

                if (UnityEngine.Random.Range(1, 4) > 2)
                {

                    Plant(1);
                }
            }
            if (umidità > 0.64 && umidità <= 0.8)
            {
                if (UnityEngine.Random.Range(1, 4) > 1)
                {

                    Plant(2);
                }
            }
            if (umidità > 0.8)
            {

                Plant(3);
            }
        }
    }

    private void Plant(int riproduzione)
    {
        risorse.GeneratePlants(riproduzione, CalcolateSize());
        food = true;
        seasonFood = risorse.season;
    }

    ///////////////////////////////////////////////FAUNA//////////////////////////////////////////////////
    public void UscitaMob(Fauna a)
    {
        mob.Remove(a);
        if(mob.Count==0) mobIn = false;
    }

    public void EntrataMob(Fauna entity)
    {
        mobIn = true;
        mob.Add(entity);
    }

    public Fauna GenateMob(Fauna progenitore)
    {
        Fauna prole = progenitore.Spawn(this);
        mob.Add(prole);
        return prole;
    }

    public bool SpawnChance()
    {
        bool risp = false;
        if (altezza < 0.85 && altezza > 0.45)
        {
            if (umidità > 0.32)
            {
                risp = true;
            }
        }
        return risp;
    }


}