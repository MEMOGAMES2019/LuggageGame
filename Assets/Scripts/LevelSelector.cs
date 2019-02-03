using RAGE.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        GM.gm.SetGenre(g);
        genre = g;
        genreB.SetActive(false);
        levelsB.SetActive(true);
    }
    public void Weather(int w)
    {
        weatherB.SetActive(false);
        weather = w;
        GM.gm.SetWeather(w);
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
                GM.gm.SetGenre(0);
                GM.gm.SetWeather(0);
                string[] arrayList = new string[4];
                arrayList[0] = "Camiseta amarilla" + (char)13;
                arrayList[1] = "Deportivas" + (char)13;
                arrayList[2] = "Cepillo de dientes" + (char)13;

                GM.gm.SetList(arrayList);
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
        string finalList = "";
        string[] arrayList = new string[12];
        TextAsset list = (TextAsset)Resources.Load("Lists/" + name, typeof(TextAsset));
        string[] s = list.text.Split('\n');
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
        GM.gm.SetList(arrayList);
        textList.text = finalList;
    }
}
