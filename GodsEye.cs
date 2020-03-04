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
    public Dropdown styleMapStart;
    public Toggle seedMap;
    public Toggle seedMapStart;
    public Text seedInput;
    public Text seedInputStart;
    MapData data;
    Pcg mondo;

    Age time;

    Fauna adamoEdEva;
    List<Fauna> mob;
    int maxMob=0;


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
        seedMap.onValueChanged.AddListener(delegate {
            ToggleSeedText(seedMap);
        });
        seedMapStart.onValueChanged.AddListener(delegate {
            ToggleSeedTextStart(seedMapStart);
        });
         adamoEdEva= new Fauna();


    }


    public void CheckEaten(Fauna def)
    {
        if (def.numEntità < 1)
        {
            Estinzione(def);
        }
    }

    public void Estinzione(Fauna ex)
    {
        mob.Remove(ex);
    }
    
    
    public void NewMap()
    {
        //show text and button
        showAtStart.gameObject.SetActive(true);
        hideAtStart.gameObject.SetActive(false);
        

        

        SendData();
        styleMapStart.gameObject.SetActive(false);
        seedMapStart.gameObject.SetActive(false);
        foreach (Zone a in mappa)
        {
            Destroy(a.polyGO);
        }
        mondo.Generate(this, poligono, data);
        seedShow.text = "" + data.seed;

        mob = new List<Fauna>();
        GenerateEntity();
    }

    private void GenerateEntity()
    {
        maxMob = 0;
        foreach (Zone a in mappa)
            a.GeneraFlora();
        foreach(Zone a in mappa)
        if (UnityEngine.Random.Range(0, 4) == 0 && maxMob < 20 && a.SpawnChance())
        {
            mob.Add(a.GenateMob(adamoEdEva));
            maxMob++;
        }
    }

    void NextSeason()
    {
        seasonText.text = time.GetSeason();
        time.GoNext();
        UpdateData();
    }



    private void SendData()
    {
        if (seedMapStart.isOn)
        {
            data.SetNoiseData(useSeed, int.Parse(seedInputStart.text));
        }
        else if (seedMap.isOn)
        {
            data.SetNoiseData(useSeed, int.Parse(seedInput.text));
        }
        else
        {
            data.SetNoiseData(useSeed, seed);
        }

        if (styleMapStart.IsActive())
        {
            data.SetVoronoiData(polygonNumber, styleMapStart.value);
        }
        else
        {
            data.SetVoronoiData(polygonNumber, styleMap.value);
        }
        
    }


    void ToggleSeedText(Toggle t)
    {
        if (seedMap.isOn) seedInput.gameObject.SetActive(true); else seedInput.gameObject.SetActive(false);
        if (seedMap.isOn) useSeed = true; else useSeed = false;

    }

    void ToggleSeedTextStart(Toggle t)
    {
        if (seedMapStart.isOn) seedInputStart.gameObject.SetActive(true);else seedInputStart.gameObject.SetActive(false);
        if (seedMapStart.isOn) useSeed = true; else useSeed = false;
    }

    public void UpdateData()
    {
        foreach (Zone a in mappa)
        {
            a.DefineZoneByHum();
            //a.DefineZoneType();
            //a.DefineHumZone();
            //a.DefineNoiseZone();
            //a.DefineHeatZone();

            a.polyGO.GetComponent<PolygonInteraction>().UpdateData();
        }
    }

    public void SetZone(List<Zone> a, List<Zone> humStart)
    {
        mappa = a;
        seaAndSnow = humStart;
        HumMapUpdate();
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
                    if (b.calore <= 0.4f && b.calore > 0f /*&& b.umidità < a.umidità - 0.1f*/)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1
                    }
                    else if (b.calore <= 0.7f && b.calore > 0.4f/* && b.umidità < a.umidità - 0.1f*/)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1

                    }
                    else if (b.calore < 0.1f && b.calore > 0.7f/* && b.umidità < a.umidità - 0.1f*/)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1

                    }
                    else if (b.calore <= 0 /*&& b.umidità < a.umidità - 0.2f*/)
                    {
                        b.SetUmidità(a.umidità - 0.2f);//2

                    }
                    else if (b.calore >= 1/* && b.umidità < a.umidità - 0.4f*/)
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
                    if (b.calore <= 0.4f && b.calore > 0f /*&& b.umidità < a.umidità - 0.1f*/)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1
                    }
                    else if (b.calore <= 0.7f && b.calore > 0.4f/* && b.umidità < a.umidità - 0.1f*/)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1

                    }
                    else if (b.calore < 0.1f && b.calore > 0.7f/* && b.umidità < a.umidità - 0.1f*/)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1

                    }
                    else if (b.calore <= 0 /*&& b.umidità < a.umidità - 0.2f*/)
                    {
                        b.SetUmidità(a.umidità - 0.2f);//2

                    }
                    else if (b.calore >= 1/* && b.umidità < a.umidità - 0.4f*/)
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
            //Debug.Log("iterazione hum"+ nextToHum.Count);
            HumMapUpdate(nextToHum);
        }
    }
    //////////////////////////////////////////////////////////////////////hum season update
    public void HumMapSeasonUpdate()
    {


        List<Zone> nextToHum = new List<Zone>();
        Debug.Log(seaAndSnow.Count);
        foreach (Zone a in seaAndSnow)
        {

            foreach (Zone b in a.vicini)
            {
                if (b.humCheck == false)
                {
                    if (b.calore <= 0.4f && b.calore > 0f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1
                    }
                    else if (b.calore <= 0.7f && b.calore > 0.4f && b.umidità < a.umidità - 0.1f)
                    {
                        b.SetUmidità(a.umidità - 0.1f);//1

                    }
                    else if (b.calore <= 0 && b.umidità < a.umidità - 0.2f)
                    {
                        b.SetUmidità(a.umidità - 0.2f);//2

                    }
                    else if (b.calore >= 1 && b.umidità < a.umidità - 0.4f)
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
            Debug.Log("iterazione hum" + nextToHum.Count);
            HumMapSeasonUpdate(nextToHum);
        }

    }
    /// <summary>
    /// Da effettuare 2 volta a stagione
    /// </summary>
    /// <param name="next">prossimi da controllare</param>
    private void HumMapSeasonUpdate(List<Zone> next)
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

                    b.humCheck = true;
                    nextToHum.Add(b);
                }

            }
        }
        if (nextToHum.Count != 0)
        {
            Debug.Log("iterazione hum" + nextToHum.Count);
            HumMapSeasonUpdate(nextToHum);
        }
    }
    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Da effettuare 4 volta a stagione
    /// </summary>
    /// <param name="season">indica la stagione in cui generare la risorsa</param>
    public void PlantMapEvo(int season)
    {
        foreach (Zone a in mappa)
        {
            if (a.food || a.seasonFood == season)
            {
                a.SpawnFood();
            }
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
