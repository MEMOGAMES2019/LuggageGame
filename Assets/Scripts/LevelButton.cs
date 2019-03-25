using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int level;
    public GameObject[] stars;
    public GameObject blocked;
    public LevelSelector lvlSelector;

    bool isBlocked;
    int numStars = 0;
    void Start()
    {
        isBlocked = true;
        CalcStars(level - 1);
        if ((numStars >= 2)|| level == 0)
        {
            if (level != 0) blocked.SetActive(false);

            CalcStars(level);
            for (int i = 0; i < numStars; i++)
            {
                stars[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            isBlocked = false;
        }
    }

    void CalcStars(int level)
    {
        int suma = -1;
        if (level != 0)
            suma = PlayerPrefs.GetInt("level" + level.ToString() + "W", 0) + PlayerPrefs.GetInt("level" + level.ToString() + "C", 0);
        else suma = PlayerPrefs.GetInt("level" + level.ToString() + "C", 0)*2;

        if (suma >= 5) numStars = 3;
        else if (suma >= 4) numStars = 2;
        else if (suma >= 1) numStars = 1;
        else numStars = 0;
    }
    public void SelectWeather()
    {
        if (!isBlocked)
        {
            lvlSelector.SelectWeather(level);
        }
    }

}
