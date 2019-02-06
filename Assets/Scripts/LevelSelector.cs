using RAGE.Analytics;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Assets.Scripts.Constantes;

public class LevelSelector : MonoBehaviour
{
    #region Variables de Unity

    [SerializeField]
    private GameObject _climaButtons;
    [SerializeField]
    private GameObject _generoButtons;
    [SerializeField]
    private GameObject _nivelButtons;
    [SerializeField]
    private GameObject _panelList;
    [SerializeField]
    private GameObject _tutorialButton;

    #endregion

    #region Atributos

    public GameObject ClimaButtons { get => _climaButtons; set => _climaButtons = value; }
    public GameObject GeneroButtons { get => _generoButtons; set => _generoButtons = value; }
    public GameObject NivelButtons { get => _nivelButtons; set => _nivelButtons = value; }
    public GameObject PanelList { get => _panelList; set => _panelList = value; }
    public GameObject TutorialButton { get => _tutorialButton; set => _tutorialButton = value; }
    public static string LevelNameGlobal { get; set; } = string.Empty;
    public Text TextList { get; set; }
    public int Level { get; set; }

    #endregion

    #region Eventos

    private void Start()
    {
        GeneroButtons.SetActive(false);
        NivelButtons.SetActive(false);
        PanelList.SetActive(false);
        ClimaButtons.SetActive(true);
        TutorialButton.SetActive(true);
        TextList = PanelList.GetComponentInChildren<Text>();
    }

    #endregion

    #region Métodos públicos

    public void SetGenre(int g)
    {
        GM.Gm.Genero = (Genero)g;
        GeneroButtons.SetActive(false);
        NivelButtons.SetActive(true);
    }
    public void SetWeather(int w)
    {
        GM.Gm.Clima = (Clima)w;
        ClimaButtons.SetActive(false);
        GeneroButtons.SetActive(true);
    }

    public void SetLevel(int l)
    {
        Level = l;
        ClimaButtons.SetActive(false);
        NivelButtons.SetActive(false);
        GeneroButtons.SetActive(false);
        TutorialButton.SetActive(false);
        PanelList.SetActive(true);

        //reiniciar la variable
        LevelNameGlobal = string.Empty;
        if (l != 0)
        {
            switch (l)
            {
                case 1:
                    //LoadList("Level1Warm");
                    LevelNameGlobal = "Level1";
                    break;
                case 2:
                    //LoadList("Level2Cold");
                    LevelNameGlobal = "Level2";
                    break;
                case 3:
                    LevelNameGlobal = "Level3";
                    break;
            }


            switch (GM.Gm.Clima)
            {
                case Clima.CALIDO:
                    LevelNameGlobal = string.Concat(LevelNameGlobal, "Warm");
                    break;
                case Clima.FRIO:
                    LevelNameGlobal = string.Concat(LevelNameGlobal, "Cold");
                    break;
            }
            LoadList(LevelNameGlobal);
        }
        else
        {
            LevelNameGlobal = "LevelTutorial";
            GM.Gm.Genero = Genero.NEUTRAL;
            GM.Gm.Clima = Clima.AMBOS;
            GM.Gm.List = new List<string>
            {
                "Camiseta amarilla" + (char)13,
                "Deportivas" + (char)13,
                "Cepillo de dientes" + (char)13
            };

            TextList.text = "Deberás identificar los siguientes objetos y guardarlos en la maleta.\nMemorízalos y haz click en el botón play cuando estés listo:\n\n-Camiseta amarilla.\n-Deportivas.\n-Cepillo de dientes";
        }

        Tracker.T.Completable.Initialized(LevelNameGlobal, CompletableTracker.Completable.Level);
    }
    public void Play()
    {
        string levelPlay = (Level != 0) ? "Level" + Level.ToString() : "Tutorial";
        SceneManager.LoadScene(levelPlay);
    }

    #endregion

    #region Métodos privados

    private void LoadList(string name)
    {
        StringBuilder finalList = new StringBuilder();
        List<string> arrayList = new List<string>();
        TextAsset list = (TextAsset)Resources.Load("Lists/" + name, typeof(TextAsset));
        List<string> s = new List<string>(list.text.Split('\n'));
        int i = 1;
        while (s[i] != "F" + (char)13)
        {
            if (GM.Gm.Genero == Genero.HOMBRE)
            {
                arrayList.Add(s[i]);
                finalList.Append("- " + s[i] + "\n");
            }
            i++;
        }
        i++;
        while (s[i] != "N" + (char)13)
        {
            if (GM.Gm.Genero == Genero.MUJER)
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
        TextList.text = finalList.ToString();
    }

    #endregion

}
