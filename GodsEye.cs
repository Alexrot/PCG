using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodsEye : MonoBehaviour
{
    public Transform poligono;
    public Text calore;
    public Text altezza;
    public Text umidità;
    public Text tipo;
    List<Zone> mappa;
    List<Zone> seaAndSnow;

    


    // Start is called before the first frame update

    private void Start()
    {
        mappa = new List<Zone>();
        seaAndSnow = new List<Zone>();

    }

    public void SetZone(List<Zone> a, List<Zone> humStart)
    {
        mappa = a;
        seaAndSnow = humStart;
        HumMapUpdate(humStart);
        ErrorHumUp();
    }

    private void ErrorHumUp()
    {
        float max = 0f;
        foreach(Zone a in mappa)
        {
            if (a.umidità == 0)
            {
                foreach(Zone b in a.vicini)
                {
                    if (b.umidità - 0.3 > max)
                    {
                        max = b.umidità - 0.3f;
                    }
                }
            }
        }
    }

    public void HumMapUpdate(List<Zone> humZone)
    {


        List<Zone> nextToHum = new List<Zone>();
        foreach(Zone a in humZone)
        {
            foreach(Zone b in a.vicini)
            {
                if (b.humCheck == false)
                {
                    if (b.calore == 0.4f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);
                    }
                    else if (b.calore == 0.7f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);
           
                    }
                    else if (b.calore == 0 && b.umidità < a.umidità - 0.2f)
                    {
                        b.SetUmidità(a.umidità - 0.2f);
                        
                    }
                    else if (b.calore == 1 && b.umidità < a.umidità - 0.3f)
                    {
                        b.SetUmidità(a.umidità - 0.3f);
                        
                    }else
                    {
                        b.SetUmidità(a.umidità - 0.3f);
                    }
                    b.humCheck = true;
                    nextToHum.Add(b);
                }
                
            }
        }
        if (nextToHum.Count != 0)
        {
            Debug.Log("iterazione hum"+ nextToHum.Count);
            HumMapUpdate(nextToHum);
        }

    }


    public void UpdateInfoText(Zone c)
    {
        calore.text = c.calore+"°";
        //Debug.Log(c);
        altezza.text = (c.altezza/**1000*/)+"m";
        umidità.text = (c.umidità/**100*/)+"%";
        switch (c.typeBiome)
        {
            case 0:
                tipo.text = "Mare profondo";
                break;
            case 1:
                tipo.text = "Mare";
                break;
            case 2:
                tipo.text = "Spiaggia";
                break;
            case 3:
                tipo.text = "Pianura";
                break;
            case 4:
                tipo.text = "Collina bassa";
                break;
            case 5:
                tipo.text = "Collina alta";
                break;
            case 6:
                tipo.text = "Montagna";
                break;
            case 7:
                tipo.text = "Picco innevato";
                break;
        }
        
    }
}
