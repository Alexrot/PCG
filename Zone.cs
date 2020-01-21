using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone 
{

    Transform segmento;
    List<Vector2> poligono;
    Vector2 centro;
    float altezza;
    float umidità;
    float calore;
    int typeBiome;

    public Zone(List<Vector2> poligono, Vector2 centro, Transform segmento, float noise)
    {
        
        this.poligono = poligono;
        this.centro = centro;
        this.segmento = segmento;
        CalcolateHeat();
    }

    void CalcolateHeat()
    {
        float pos =centro.y;
        if (pos <= 300 && pos >= 200)
        {
            calore = 1;//caldo
        }
        else if(pos <= 500 && pos >= 450 || pos <= 50 && pos >= 0)
        {
            calore = 0;//freddp
        }
        else if (pos <= 450 && pos >= 350 || pos <= 150 && pos >= 50)
        {
            calore = 0.4f;//temperato freddo
        }
        else if (pos <= 350 && pos >= 300|| pos <= 200 && pos >= 150)
        {
            calore = 0.7f;//temperato caldo
        }
    }
}
