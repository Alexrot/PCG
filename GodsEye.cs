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
    }
    
    public void HumMapUpdate()
    {

    }


    public void UpdateInfoText(float c, float a, float u, int t)
    {
        calore.text = c+"°";
        //Debug.Log(c);
        altezza.text = a+"m";
        umidità.text = u+"%";
        switch (t)
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
