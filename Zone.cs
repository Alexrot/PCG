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

    public Zone(List<Vector2> poligono, Vector2 centro, Transform segmento)
    {
        this.poligono = poligono;
        this.centro = centro;
        this.segmento = segmento;
    }


}
