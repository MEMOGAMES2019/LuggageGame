using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

    public GameObject weatherB;
    public GameObject genreB;
    public GameObject levelsB;
    public GameObject listPanel;
    Text textList;

    int genre;
    int weather;
    int level;

    private void Start()
    {
        weatherB.SetActive(true);
        genreB.SetActive(false);
        levelsB.SetActive(false);
        listPanel.SetActive(false);
        textList = listPanel.GetComponentInChildren<Text>();
    }
    public void Genre(int g)
    {
        GM.gm.setGenre(g);
        genre = g;
        genreB.SetActive(false);
        levelsB.SetActive(true);
    }
    public void Weather(int w)
    {
        weatherB.SetActive(false);
        weather = w;
        GM.gm.setWeather(w);
        genreB.SetActive(true);
    }
   
    public void Level(int l)
    {
        level = l;
        levelsB.SetActive(false);
        listPanel.SetActive(true);
        switch (l)
        {
            case 1:
                if (weather == 1) loadList("Level1Warm",6);
                else loadList("Level1Cold",6);
                break;
            case 2:
                if (weather == 1) loadList("Level2Warm",9);
                else loadList("Level2Cold",9);
              
                break;
            case 3:
                if (weather == 1) loadList("Level3Warm",12);
                else loadList("Level3Cold",12);
                break;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene("Level" + level.ToString());
    }
    void loadList(string name, int numWords)
    {
        string finalList = "";
        string [] arrayList = new string[12];
        TextAsset list = (TextAsset)Resources.Load("Lists/" + name, typeof(TextAsset));
        string [] s = list.text.Split('\n');
        int i = 1;
        int wordsSaved = 0;
        while (s[i] != "F" + (char)13)
        {
            if (genre == 1)
            {
                arrayList[wordsSaved] = s[i];
                wordsSaved++;
                finalList += "- " + s[i] + "\n";
            }
            i++;
        }
        i++;
        while (s[i] != "N" + (char)13)
        {
            if (genre == 2)
            {
                arrayList[wordsSaved] = s[i];
                wordsSaved++;
                finalList += "- " + s[i] + "\n";
            }
            i++;
        }
        i++;
        while (s[i] != "Fin")
        {
            arrayList[wordsSaved] = s[i];
            wordsSaved++;
            finalList += "- " + s[i] + "\n";
            i++;
        }
        GM.gm.setList(arrayList);
        textList.text = finalList;
    }
}
