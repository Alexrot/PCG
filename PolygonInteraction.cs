using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonInteraction : MonoBehaviour
{
    public GodsEye god;
    public Zone data;
    public float umidità;
    public float altezza;
    public float calore;



    public void SetData(Zone a)
    {
        data = a;
    }

    public void UpdateData()
    {
        umidità = data.umidità;
        altezza = data.altezza;
        calore = data.calore;
    }


    private void OnMouseDown()
    {
        //Debug.Log(data.centro);
        god.UpdateInfoText(data);
    }
}

