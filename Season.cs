using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Season : MonoBehaviour
{
    int day = 0;

    float summer = 0.5f;
    float spring = 0.2f;
    float winter = -0.5f;
    float fall = -0.2f;

    /// <summary>
    /// ogni stagione avrà 4 "giorni".
    /// Questi giorni avranno 2 secondo di distanza tra di loro ed ad ogni iterazione aumenterà o diminuirà la temperatura.
    /// La mappa si aggiornerà solo dopo il secondo e il quarto giorno 
    /// </summary>
    public float Summer()
    {
        //aumenta di 0.5 il calore gradualmente
        if (day < 4)
        {
            //StartCoroutine(TimeWait());
            day++;
            if (day == 2)
            {
                return summer / 2;
            }
            if (day == 4)
            {
                day = 0;
                return summer / 2;
            }

        }
        return 0f;
    }

    /// <summary>
    /// ogni stagione avrà 4 "giorni".
    /// Questi giorni avranno 2 secondo di distanza tra di loro ed ad ogni iterazione aumenterà o diminuirà la temperatura.
    /// La mappa si aggiornerà solo dopo il secondo e il quarto giorno 
    /// </summary>
    public float Spring()
    {
        //aumenta di 0.2 il calore gradualmente
        if (day < 4)
        {
            //StartCoroutine(TimeWait());
            day++;
            if (day == 2)
            {
                return spring / 2;
            }
            if (day == 4)
            {
                day = 0;
                return spring / 2;
            }

        }
        return 0f;
    }

    /// <summary>
    /// ogni stagione avrà 4 "giorni".
    /// Questi giorni avranno 2 secondo di distanza tra di loro ed ad ogni iterazione aumenterà o diminuirà la temperatura.
    /// La mappa si aggiornerà solo dopo il secondo e il quarto giorno 
    /// </summary>
    public float Winter()
    {
        //diminuisci di 0.5 il calore gradualmente
        if (day < 4)
        {
            //StartCoroutine(TimeWait());
            day++;
            if (day == 2)
            {
                return winter / 2;
            }
            if (day == 4)
            {
                day = 0;
                return winter / 2;
            }

        }
        return 0f;
    }

    /// <summary>
    /// ogni stagione avrà 4 "giorni".
    /// Questi giorni avranno 2 secondo di distanza tra di loro ed ad ogni iterazione aumenterà o diminuirà la temperatura.
    /// La mappa si aggiornerà solo dopo il secondo e il quarto giorno 
    /// </summary>
    public float Fall()
    {
        //diminuisci di 0.2 il calore gradualmente
        if (day < 4)
        {
            
            //StartCoroutine(TimeWait());
            day++;
            if (day == 2)
            {
                return fall / 2;
            }
            if (day == 4)
            {
                day = 0;
                return fall / 2;
            }
            
        }
        return 0f;

    }

    /*
    IEnumerator TimeWait()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);

        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    */
}
