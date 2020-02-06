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
    public bool humCheck =false;


    public Zone(List<Vector2> poligono, Vector2 centro, Transform segmento, float noise)
    {
        vicini = new List<Zone>();
        altezza = noise;
        this.poligono = poligono;
        this.centro = centro;
        poly = segmento;
        CalcolateHeat();
        DefineZoneByHum();
        polyGO =poly.gameObject;

        
        
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
        else
        {
            this.umidità = umidità;
        }
        
        humCheck = true;
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
        else if (altezza > 0.9 )
        {
            //type 7 ice
            typeBiome = 7;
            poly.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    /// <summary>
    /// Cambia il tipo della zona in base a temperatura e umidità
    /// 
    /// </summary>
    public void DefineZoneByHum()
    {
       
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
            if (calore == 0)
            {
                if (altezza <0.10)
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
        if (altezza > 0.45&&calore==0)
        {
            poly.GetComponent<Renderer>().material.color = Color.Lerp(poly.GetComponent<Renderer>().material.color, Color.Lerp(Color.blue, Color.white  , 0.45f), 0.55f);
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
