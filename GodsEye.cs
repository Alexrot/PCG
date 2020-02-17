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
    public Text seasonText;
    public GameObject showAtStart;
    public GameObject hideAtStart;
    public Text seedShow;

    List<Zone> mappa;
    List<Zone> seaAndSnow;
    public Button newMap;
    public Button nextSeason;
    public Button btnStart;
    public Dropdown styleMap;
    MapData data;
    Pcg mondo;

    Age time;


    public bool autoSkip=false;
    public int polygonNumber;
    public bool useSeed;
    public int seed;
     bool isolaGrande;
     bool isolaMedia;
     bool isolaPiccola;


    // Start is called before the first frame update

     void Start()
    {
        showAtStart.gameObject.SetActive(false);


        data = new MapData();
        mappa = new List<Zone>();
        seaAndSnow = new List<Zone>();
        newMap.onClick.AddListener(NewMap);
        btnStart.onClick.AddListener(NewMap);
        nextSeason.onClick.AddListener(NextSeason);
        mondo = new Pcg();
        time = new Age();
        time.SetGod(this);



    }
    
    public void NewMap()
    {
        //show text and button
        showAtStart.gameObject.SetActive(true);
        hideAtStart.gameObject.SetActive(false);
        

        

        SendData();
        foreach(Zone a in mappa)
        {
            Destroy(a.polyGO);
        }
        mondo.Generate(this, poligono, data);
        seedShow.text = "" + data.seed;

    }

    void NextSeason()
    {
        seasonText.text = time.GetSeason();
        time.GoNext();
        UpdateData();
    }



    private void SendData()
    {
        data.SetNoiseData(useSeed, seed);

        data.SetVoronoiData(polygonNumber, styleMap.value);
    }

    public void UpdateData()
    {
        foreach (Zone a in mappa)
        {
            a.DefineZoneByHum();
            a.polyGO.GetComponent<PolygonInteraction>().UpdateData();
        }
    }

    public void SetZone(List<Zone> a, List<Zone> humStart)
    {
        mappa = a;
        seaAndSnow = humStart;
        HumMapUpdate();
        ErrorHumUp();
    }

    
    public void HumMapUpdate()
    {


        List<Zone> nextToHum = new List<Zone>();
        Debug.Log(seaAndSnow.Count);
        foreach (Zone a in seaAndSnow)
        {

            foreach(Zone b in a.vicini)
            {
                if (b.humCheck == false)
                {
                    if (b.calore <= 0.4f && b.calore > 0f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);
                    }
                    else if (b.calore <= 0.7f && b.calore > 0.4f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);
           
                    }
                    else if (b.calore <= 0 && b.umidità < a.umidità - 0.2f)
                    {
                        b.SetUmidità(a.umidità - 0.2f);
                        
                    }
                    else if (b.calore >= 1 && b.umidità < a.umidità - 0.4f)
                    {
                        b.SetUmidità(a.umidità - 0.4f);
                        
                    }else
                    {
                        
                        b.SetUmidità(a.umidità - 0.4f);
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
    /// <summary>
    /// Da effettuare 2 volta a stagione
    /// </summary>
    /// <param name="next">prossimi da controllare</param>
    private void HumMapUpdate(List<Zone> next)
    {
        List<Zone> nextToHum = new List<Zone>();
        foreach (Zone a in next)
        {
            foreach (Zone b in a.vicini)
            {
                if (b.humCheck == false)
                {
                    if (b.calore <= 0.4f && b.calore > 0f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);
                    }
                    else if (b.calore <= 0.7f && b.calore > 0.4f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);

                    }
                    else if (b.calore <= 0 && b.umidità < a.umidità - 0.2f)
                    {
                        b.SetUmidità(a.umidità - 0.2f);

                    }
                    else if (b.calore >= 1 && b.umidità < a.umidità - 0.4f)
                    {
                        b.SetUmidità(a.umidità - 0.4f);

                    }
                    else
                    {
                        
                            b.SetUmidità(a.umidità - 0.4f);
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

    /// <summary>
    /// Da effettuare 2 volta a stagione
    /// </summary>
    /// <param name="humZone"></param>
    public void HumMapEvo()
    {
        foreach (Zone a in mappa)
        {
            if(!seaAndSnow.Contains(a))
            a.humCheck = false;
        }
        HumMapUpdate();
    }


    private void ErrorHumUp()
    {
        float max = 0f;
        foreach (Zone a in mappa)
        {
            if (a.umidità == 0)
            {
                foreach (Zone b in a.vicini)
                {
                    if (b.umidità - 0.3 > max)
                    {
                        max = b.umidità - 0.3f;
                    }
                }
            }
        }
    }

    public void HeatUpdate(float change)
    {
        foreach(Zone a in mappa)
        {
            a.ChangeHeat(change);
        }
    }

    public void UpdateInfoText(Zone c)
    {
        calore.text = c.calore+"°";
        //Debug.Log(c);
        altezza.text = (c.altezza*1000)+"m";
        umidità.text = (c.umidità*100)+"%";
        switch (c.typeBiome)
        {
            case 0:
                tipo.text = "Mare profondo";
                break;
            case 1:
                tipo.text = "Mare";
                break;
            case 2:
                tipo.text = "Ghiaccio";
                break;
            case 3:
                tipo.text = "Iceberg";
                break;
            case 4:
                tipo.text = "Sabbia";
                break;
            case 5:
                tipo.text = "Pianura";
                break;
            case 6:
                tipo.text = "Palude";
                break;
            case 7:
                tipo.text = "Collina brulla";
                break;
            case 8:
                tipo.text = "Pianura in collina";
                break;
            case 9:
                tipo.text = "Foresta";
                break;
            case 10:
                tipo.text = "Mesa";
                break;
            case 11:
                tipo.text = "Taiga";
                break;
            case 12:
                tipo.text = "Picco Innevato";
                break;
        }
        
    }
}
