using RAGE.Analytics;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Assets.Scripts.Constantes;
// para leer de txt
using System.IO;
using System.Linq;

public class LevelSelector : MonoBehaviour
{
    #region Variables de Unity

    [SerializeField]
    private GameObject _climaButtons;
    [SerializeField]
    private GameObject _decoration;
    [SerializeField]
    private GameObject _levelButtons;
    [SerializeField]
    private GameObject _panelList;
    [SerializeField]
    private GameObject _tutorialButton;

    #endregion

    #region Atributos

    /// <summary>
    /// Objeto que contiene los elementos decorativos del menú. Imagen del hombre, panel, bocadillo...
    /// </summary>
    public GameObject decoration { get => _decoration; set => _decoration = value; }

    /// <summary>
    /// Botones para seleccionar la dificultad.
    /// </summary>
    public GameObject levelButtons { get => _levelButtons; set => _levelButtons = value; }

    /// <summary>
    /// Botones para elegir clima.
    /// </summary>
    public GameObject ClimaButtons { get => _climaButtons; set => _climaButtons = value; }

    /// <summary>
    /// Panel donde se encuentra la lista de objetos a recoger.
    /// </summary>
    public GameObject PanelList { get => _panelList; set => _panelList = value; }

    /// <summary>
    /// Nombre completo del nivel.
    /// </summary>
    public static string LevelNameGlobal { get; set; } = string.Empty;

    /// <summary>
    /// Lista de objetos a recoger.
    /// </summary>
    public Text TextList { get; set; }

    /// <summary>
    /// Nivel establecido por el jugador.
    /// </summary>
    public int Level { get; set; }

    private int levelSelected;

    #endregion

    #region Eventos

    private void Start()
    {
        levelSelected = -1;
        PanelList.SetActive(false);
        ClimaButtons.SetActive(false);
        levelButtons.SetActive(true);
        TextList = PanelList.GetComponentInChildren<Text>();

        GM.Gm.Genero = (Genero)PlayerPrefs.GetInt("genre", -1);
        if (!PlayerPrefs.HasKey("firstime"))
        {
            PlayerPrefs.SetInt("firstime", 1);
            SelectWeather(0);
        }

    }

    #endregion

    #region Métodos públicos

    /// <summary>
    /// Establece el clima en el que se cargarán los datos.
    /// </summary>
    /// <param name="w">Valor númerico del clima: 0 -> AMBOS, 1 -> CÁLIDO, 2 -> FRÍO</param>
    public void SetWeather(int w, int level)
    {
        GM.Gm.Clima = (Clima)w;
        ClimaButtons.SetActive(false);
        SetLevel(level);
    }

    /// <summary>
    /// Establece el nivel de dificultad en el que se cargaran los datos del fichero.
    /// </summary>
    /// <param name="l">Nivel de dificultad del juego: 0 -> Tutorial</param>
    public void SetLevel(int l)
    {
        _decoration.SetActive(false);

        Level = l;
        ClimaButtons.SetActive(false);
       
        
        PanelList.SetActive(true);
        

        //reiniciar la variable
        LevelNameGlobal = string.Empty;
        if (l != 0)
        {
            switch (l)
            {
                case 1:
                    LevelNameGlobal = "Level1";
                    break;
                case 2:
                    LevelNameGlobal = "Level2";
                    break;
                case 3:
                    LevelNameGlobal = "Level3";
                    break;
                case 4:
                    LevelNameGlobal = "Level4Global";
                    break;
            }

            if (LevelNameGlobal != "Level4Global")
            {
                switch (GM.Gm.Clima)
                {
                    case Clima.CALIDO:
                        LevelNameGlobal = string.Concat(LevelNameGlobal, "Warm");
                        break;
                    case Clima.FRIO:
                        LevelNameGlobal = string.Concat(LevelNameGlobal, "Cold");
                        break;
                }
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
                "Camiseta amarilla",
                "Deportivas",
                "Cepillo de dientes"
            };
            StringBuilder cad = new StringBuilder();
            cad.AppendLine("Lea atentamente e intente memorizar los siguientes objetos que debe introducir en la maleta...");
            cad.AppendLine("Cuando se sienta preparado haga click en el botón play");
            cad.AppendLine(" ");
            cad.AppendLine("- Camiseta amarilla");
            cad.AppendLine("- Deportivas");
            cad.AppendLine("- Cepillo de dientes");
            TextList.text = cad.ToString();
            TextList.alignment = TextAnchor.MiddleLeft;
        }

        Tracker.T.Completable.Initialized(LevelNameGlobal, CompletableTracker.Completable.Level);
    }

    /// <summary>
    /// Comienza el juego según los parámetros establecidos.
    /// </summary>
    public void Play()
    {
        string levelPlay = (Level != 0) ? "Level" + Level.ToString() : "Tutorial";
        SceneManager.LoadScene(levelPlay);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Créditos");
    }

    public void SelectWeather(int level)
    {

        if (level != 0)
        {
            levelButtons.SetActive(false);
            ClimaButtons.SetActive(true);
        }
        else
        {
            SetLevel(level);
            levelButtons.SetActive(false);
            ClimaButtons.SetActive(false);
            
        }
        levelSelected = level;
       
    }
    public void BackToLevels()
    {
        levelButtons.SetActive(true);
        ClimaButtons.SetActive(false);
    }
    #endregion

    #region Métodos privados

    /// <summary>
    /// Carga la lista de objetos a poner en la maleta.
    /// </summary>
    /// <param name="name">Nombre del fichero donde se van a carar los datos.</param>
    private void LoadList(string name)
    {

        TextList.text = string.Concat("Lea atentamente e intente memorizar los siguientes objetos que debe introducir en la maleta...\n", Environment.NewLine);
        GM.Gm.List = new List<string>();

        TextAsset list = (TextAsset)Resources.Load(string.Concat("Lists/", name), typeof(TextAsset));
        string txt = Encoding.UTF8.GetString(list.bytes);
        Queue<string> cola = new Queue<string>(txt.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));
        cola.Dequeue();
        if (LevelNameGlobal != "Level4Global")
        {

            GetPrendas(cola, Genero.HOMBRE, "F");
            GetPrendas(cola, Genero.MUJER, "N");
            GetPrendas(cola, Genero.NEUTRAL, "Fin");
        }
        // Level 4
        else
        {
            string listObjetos = LoadListLevel4(cola, GM.Gm.List, "Ropa para obstaculizar");

            TextList.text = string.Concat(TextList.text, listObjetos);
            string listObstaculos = LoadListLevel4(cola, GM.Gm.ObstaculosList, null);

        }
    }

    /// <summary>
    /// Busca en el txt los objetos que el usuario debe guardar en la maleta
    /// </summary>
    private string LoadListLevel4(Queue<string> cola, List<string> list, string fin)
    {
        string line;
        StringBuilder finalList = new StringBuilder();
        bool vacio = false;

        // Tramo de la ropa que el usuario debe buscar
        line = cola.Dequeue();
        while ((fin != null && !line.Equals(fin)) || (fin == null && !vacio))
        {
            List<string> entries = line.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            entries.ForEach(objeto =>
            {
                list.Add(objeto);
                finalList.AppendLine(string.Concat("- ", objeto));
            });

            if (cola.Count > 0)
                line = cola.Dequeue();
            else
                vacio = true;
        }
        return finalList.ToString();
    }

    /// <summary>
    /// Recoge del fichero las prendas según los parámetros.
    /// </summary>
    /// <param name="cola">Cola con la lista de objetos a procesar.</param>
    /// <param name="genero">Genero de la prenda a recoger.</param>
    /// <param name="fin">Hasta donde leemos del fichero.</param>
    private void GetPrendas(Queue<string> cola, Genero genero, string fin)
    {
        StringBuilder finalList = new StringBuilder();
        string objeto = cola.Dequeue();
        while (!objeto.Equals(fin))
        {
            if (GM.Gm.Genero == genero || genero == Genero.NEUTRAL)
            {
                GM.Gm.List.Add(objeto);
                finalList.AppendLine(string.Concat("- ", objeto));
            }
            objeto = cola.Dequeue();
        }

        TextList.text = string.Concat(TextList.text, finalList.ToString());
    }

    
    #endregion

}
