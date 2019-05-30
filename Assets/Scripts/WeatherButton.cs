using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Constantes;

public class WeatherButton : MonoBehaviour
{
    public int _level;
    public GameObject[] stars;
    public LevelSelector lvlSelector;
    public Clima clima;
    int numStars = 0;
   
    public void SetStars(int level)
    {
        foreach (GameObject go in stars) go.transform.GetChild(0).gameObject.SetActive(false);
        _level = level;
        numStars = 0;
        if (clima == Clima.CALIDO)
            numStars = PlayerPrefs.GetInt("level" + _level.ToString() + "W", 0);
        else numStars = PlayerPrefs.GetInt("level" + _level.ToString() + "C", 0);

        for (int i = 0; i < numStars; i++)
        {
            stars[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
   
    public void SetWeather()
    {
       
        lvlSelector.SetWeather((int)clima, _level);

    }
}
