using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int polygonNumber;
    public bool useSeed;
    public int seed;

    public int isola;

    public void SetNoiseData(bool useSeed, int seed)
    {
        this.useSeed = useSeed;
        this.seed = seed;
    }

    public void SetVoronoiData( int polygonNumber, int isola)
    {
        
        this.polygonNumber = polygonNumber;
        this.isola = isola;

    }
}
