using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RAGE.Analytics;

public class GM : MonoBehaviour {

    public static GM gm;


    public bool warm;
    public bool cold;
    public bool male;
    public bool female;

    string[] list;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
        list = new string[12];
    }

    private void Start()
    {
        
    }
    public int getGenre()
    {
        if (gm.male) return 1;
        else return 2;
    }
    public int getWeather()
    {
        if (gm.warm) return 0;
        else return 1;
    }

    public void setGenre(int g)
    {
        if (g == 1)
        {
            Tracker.T.setVar("Male", 1);
            gm.male = true;
            gm.female = false;
        }
        else if( g == 2)
        {
            Tracker.T.setVar("Female", 1);
            gm.male = false;
            gm.female = true;
        }
        else
        {
            Tracker.T.setVar("Male & Female", 1);
            gm.male = true;
            gm.female = true;
        }
    }
    public void setWeather(int w)
    {
        if (w == 1)
        {
            Tracker.T.setVar("Warm", 1);
            gm.warm = true;
            gm.cold = false;
        }
        else if(w == 2)
        {
            Tracker.T.setVar("Cold", 1);
            gm.warm = false;
            gm.cold = true;
        }
        else
        {
            Tracker.T.setVar("Warm & Cold", 1);
            gm.warm = true;
            gm.cold = true;
        }
    }

    public void setList(string [] list_)
    {
        gm.list = list_;
    }
    public string[] List
    {
        get { return gm.list; }
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
