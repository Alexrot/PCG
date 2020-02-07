using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age : MonoBehaviour
{
    GodsEye god;
    Season time;

    int year;
    /// <summary>
    /// 1 = spring |
    /// 2 = summer |
    /// 3 = fall |
    /// 4 = winter
    /// </summary>
    int currentSeason;

    bool next;
    
    // Start is called before the first frame update
    void Set()
    {
        time = new Season();
        year=0;
        currentSeason = 1;
        next = false;
    }

    public void SetGod(GodsEye god)
    {
        this.god = god;
        Set();
    }

    public void GoNext()
    {
        
        switch (currentSeason)
        {
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    
                    if (i == 1 || i == 3)
                    {
                        god.HeatUpdate(time.Spring());
                        Debug.Log("iterazione hum");
                        god.HumMapEvo();
                    }
                }
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    
                    if (i == 1 || i == 3)
                    {
                        god.HeatUpdate(time.Summer());
                        Debug.Log("iterazione hum");
                        god.HumMapEvo();
                    }
                }
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    
                    if (i == 1 || i == 3)
                    {
                        god.HeatUpdate(time.Fall());
                        Debug.Log("iterazione hum");
                        god.HumMapEvo();
                    }
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    
                    if (i == 1 || i == 3)
                    {
                        god.HeatUpdate(time.Winter());
                        Debug.Log("iterazione hum");
                        god.HumMapEvo();
                    }
                }
                break;

        }
        currentSeason++;
        if (currentSeason > 4)
            currentSeason = 1;
    }



    public string GetSeason()
    {
        switch (currentSeason)
        {
            case 1:
                return "spring";
            case 2:
                return "summer";
            case 3:
                return "fall";
            case 4:
                return "winter";
            default:
                return "";
        }
    }
}
