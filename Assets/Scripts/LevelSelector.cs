using RAGE.Analytics;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Assets.Scripts.Constantes;

public class LevelSelector : MonoBehaviour
{

    public GameObject weatherB;
    public GameObject genreB;
    public GameObject levelsB;
    public GameObject listPanel;
    public GameObject tutorialButton;

    // Tracker
    public static string LevelNameGlobal = string.Empty;

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
        tutorialButton.SetActive(true);
        textList = listPanel.GetComponentInChildren<Text>();
    }
    public void Genre(int g)
    {
        GM.Gm.Genero=(Genero)g;
        genre = g;
        genreB.SetActive(false);
        levelsB.SetActive(true);
    }
    public void Weather(int w)
    {
        weatherB.SetActive(false);
        weather = w;
        GM.Gm.Clima = (Clima)w;
        genreB.SetActive(true);
    }

    public void Level(int l)
    {
        level = l;
        weatherB.SetActive(false);
        levelsB.SetActive(false);
        listPanel.SetActive(true);
        genreB.SetActive(false);
        tutorialButton.SetActive(false);

        //reiniciar la variable
        LevelNameGlobal = string.Empty;

        switch (l)
        {
            case 1:
                if (weather == 1)
                {
                    LoadList("Level1Warm");
                    LevelNameGlobal = "Level1Warm";
                }
                else
                {
                    LoadList("Level1Cold");
                    LevelNameGlobal = "Level1Cold";
                }
                break;
            case 2:
                if (weather == 1)
                {
                    LoadList("Level2Warm");
                    LevelNameGlobal = "Level2Warm";
                }
                else
                {
                    LoadList("Level2Cold");
                    LevelNameGlobal = "Level2Cold";
                }

                break;
            case 3:
                if (weather == 1)
                {
                    LoadList("Level3Warm");
                    LevelNameGlobal = "Level3Warm";
                }
                else
                {
                    LoadList("Level3Cold");
                    LevelNameGlobal = "Level3Cold";
                }

                break;
            case 4: //Tutorial
                LevelNameGlobal = "LevelTutorial";
                GM.Gm.Genero = Genero.NEUTRAL;
                GM.Gm.Clima = Clima.AMBOS;
                List<string> arrayList = new List<string>
                {
                    "Camiseta amarilla" + (char)13,
                    "Deportivas" + (char)13,
                    "Cepillo de dientes" + (char)13
                };

                GM.Gm.List = arrayList;
                textList.text = "Deberás identificar los siguientes objetos y guardarlos en la maleta.\nMemorízalos y haz click en el botón play cuando estés listo:\n\n-Camiseta amarilla.\n-Deportivas.\n-Cepillo de dientes";
                break;
        }

        Tracker.T.Completable.Initialized(LevelNameGlobal, CompletableTracker.Completable.Level);
    }
    public void Play()
    {
        if (level != 4)
            SceneManager.LoadScene("Level" + level.ToString());
        else
            SceneManager.LoadScene("Tutorial");

    }
    void LoadList(string name)
    {
        StringBuilder finalList = new StringBuilder();
        List<string> arrayList = new List<string>();
        TextAsset list = (TextAsset)Resources.Load("Lists/" + name, typeof(TextAsset));
        List<string> s = new List<string>(list.text.Split('\n'));
        int i = 1;
        while (s[i] != "F" + (char)13)
        {
            if (genre == 1)
            {
                arrayList.Add(s[i]);
                finalList.Append("- " + s[i] + "\n");
            }
            i++;
        }
        i++;
        while (s[i] != "N" + (char)13)
        {
            if (genre == 2)
            {
                arrayList.Add(s[i]);
                finalList.Append("- " + s[i] + "\n");
            }
            i++;
        }
        i++;
        while (s[i] != "Fin")
        {
            arrayList.Add(s[i]);
            finalList.Append("- " + s[i] + "\n");
            i++;
        }
        GM.Gm.List = arrayList;
        textList.text = finalList.ToString();
    }
}
