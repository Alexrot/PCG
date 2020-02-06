using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int polygonNumber;
    public bool useSeed;
    public int seed;
    public bool isolaGrande;
    public bool isolaMedia;
    public bool isolaPiccola;

    public void SetNoiseData(bool useSeed, int seed)
    {
        this.useSeed = useSeed;
        this.seed = seed;
    }

    public void SetVoronoiData( int polygonNumber, bool isolaGrande, bool isolaMedia, bool isolaPiccola)
    {
        
        this.polygonNumber = polygonNumber;
        this.isolaGrande = isolaGrande;
        this.isolaMedia = isolaMedia;
        this.isolaPiccola = isolaPiccola;
    }
}
