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


    public Zone(List<Vector2> poligono, Vector2 centro, Transform segmento, float noise)
    {
        vicini = new List<Zone>();
        altezza = noise;
        this.poligono = poligono;
        this.centro = centro;
        poly = segmento;
        CalcolateHeat();
        DefineZoneType();
        polyGO =poly.gameObject;

        
        //polyGO.AddComponent(vicini[1]);
    }




    /// <summary>
    /// Inserisco nel poligono le informazioni sui poligoni vicini tramite i loro punti di voronoi
    /// </summary>
    /// <param name="vicini"></param>
    public void AddNeighbor(Zone vicino)
    {
        vicini.Add(vicino);
    }

    public void SetUmidità(int umidità)
    {
        this.umidità = umidità;
    }

    /// <summary>
    /// Definisce il poligono in base all'altezza
    /// </summary>
    private void DefineZoneType()
    {
        if (altezza <= 0.25)
        {
            //type 0 deepOcean
            typeBiome = 0;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.gray, 0.41f);
        }
        else if (altezza > 0.25 && altezza <= 0.4)
        {
            //type 1 Ocean
            typeBiome = 1;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.white, 0.31f);
        }
        else if (altezza > 0.4 && altezza <= 0.45)
        {
            //type 2 Coast
            typeBiome = 2;
            poly.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.white, 0.41f);
        }
        else if (altezza > 0.45 && altezza <= 0.55)
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
        else if (altezza > 0.9 && altezza <= 1)
        {
            //type 7 ice
            typeBiome = 7;
            poly.GetComponent<Renderer>().material.color = Color.white;
        }
    }


    /// <summary>
    /// Calcola la temperatura del poligono in base alla sua posizione nella mappa
    /// </summary>
    void CalcolateHeat()
    {
        float pos =centro.y;
        if (pos <= 1200 && pos >= 800)
        {
            calore = 1;//caldo
        }
        else if(pos <= 2000 && pos >= 1800 || pos <= 200 && pos >= 0)
        {
            calore = 0;//freddo
        }
        else if (pos <= 1800 && pos >= 1400 || pos <= 600 && pos >= 200)
        {
            calore = 0.4f;//temperato freddo
        }
        else if (pos <= 1400 && pos >= 1200|| pos <= 800 && pos >= 600)
        {
            calore = 0.7f;//temperato caldo
        }
    }
}
