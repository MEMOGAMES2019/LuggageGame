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
    private GameObject _generoButtons;
    [SerializeField]
    private GameObject _nivelButtons;
    [SerializeField]
    private GameObject _panelList;
    [SerializeField]
    private GameObject _tutorialButton;
    [SerializeField]
    private Text _pregunta;

    #endregion

    #region Atributos

    /// <summary>
    /// Botones para elegir clima.
    /// </summary>
    public GameObject ClimaButtons { get => _climaButtons; set => _climaButtons = value; }

    /// <summary>
    /// Botones para elegir género.
    /// </summary>
    public GameObject GeneroButtons { get => _generoButtons; set => _generoButtons = value; }

    /// <summary>
    /// Botones para elegir nivel de dificultad.
    /// </summary>
    public GameObject NivelButtons { get => _nivelButtons; set => _nivelButtons = value; }

    /// <summary>
    /// Panel donde se encuentra la lista de objetos a recoger.
    /// </summary>
    public GameObject PanelList { get => _panelList; set => _panelList = value; }

    /// <summary>
    /// Pregunta que se le hace al jugador.
    /// </summary>
    public Text PreguntaText { get => _pregunta; set => _pregunta = value; }

    /// <summary>
    /// Botones para dar comienzo al tutorial.
    /// </summary>
    public GameObject TutorialButton { get => _tutorialButton; set => _tutorialButton = value; }

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
        PreguntaText.text = "¿En qué clima quieres jugar?";
    }

    #endregion

    #region Métodos públicos

    /// <summary>
    /// Establece el género en el que se cargarán los datos.
    /// </summary>
    /// <param name="g">Valor númerico del género: 0 -> NEUTRAL, 1 -> HOMBRE, 2 -> MUJER</param>
    public void SetGenre(int g)
    {
        GM.Gm.Genero = (Genero)g;
        GeneroButtons.SetActive(false);
        NivelButtons.SetActive(true);
        PreguntaText.text = "¿En qué nivel de dificultad quieres jugar?";
    }

    /// <summary>
    /// Establece el clima en el que se cargarán los datos.
    /// </summary>
    /// <param name="w">Valor númerico del clima: 0 -> AMBOS, 1 -> CÁLIDO, 2 -> FRÍO</param>
    public void SetWeather(int w)
    {
        GM.Gm.Clima = (Clima)w;
        ClimaButtons.SetActive(false);
        GeneroButtons.SetActive(true);
        PreguntaText.text = "¿Con qué género quieres jugar?";
    }

    /// <summary>
    /// Establece el nivel de dificultad en el que se cargaran los datos del fichero.
    /// </summary>
    /// <param name="l">Nivel de dificultad del juego: 0 -> Tutorial</param>
    public void SetLevel(int l)
    {
        Level = l;
        ClimaButtons.SetActive(false);
        NivelButtons.SetActive(false);
        GeneroButtons.SetActive(false);
        TutorialButton.SetActive(false);
        PanelList.SetActive(true);
        PreguntaText.text = string.Empty;

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
            cad.AppendLine("Deberás identificar los siguientes objetos y guardarlos en la maleta.");
            cad.AppendLine("Memorízalos y haz click en el botón play cuando estés listo");
            cad.AppendLine();
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

    #endregion

    #region Métodos privados

    /// <summary>
    /// Carga la lista de objetos a poner en la maleta.
    /// </summary>
    /// <param name="name">Nombre del fichero donde se van a carar los datos.</param>
    private void LoadList(string name)
    {

        TextList.text = string.Concat("Tienes que meter estos objetos en la maleta:", Environment.NewLine);
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
