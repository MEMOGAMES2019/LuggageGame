using RAGE.Analytics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Scripts.Constantes;

public class GM : MonoBehaviour
{
    #region Variables Unity

    [SerializeField]
    private int _clima;
    [SerializeField]
    private int _genero;
    [SerializeField]
    private int _level;

    #endregion

    #region Atributos

    /// <summary>
    /// Lista de objetos a poner en la maleta.
    /// </summary>
    public List<string> List { get; set; }

    /// <summary>
    /// Manejado general del juego.
    /// </summary>
    public static GM Gm;
   
    public List<string> ObstaculosList { get; set; }

    /// <summary>
    /// Clima del juego.
    /// </summary>
    public Clima Clima
    {
        get => (Clima)_clima; set
        {
            switch (value)
            {
                case Clima.CALIDO:
                    Tracker.T.setVar("Warm", 1);
                    break;
                case Clima.FRIO:
                    Tracker.T.setVar("Cold", 1);
                    break;
                default:
                    Tracker.T.setVar("Warm & Cold", 1);
                    break;
            }
            _clima = (int)value;
        }
    }

    /// <summary>
    /// Género del juego.
    /// </summary>
    public Genero Genero
    {
        get => (Genero)_genero; set
        {
            switch (value)
            {
                case Genero.HOMBRE:
                    Tracker.T.setVar("Male", 1);
                    break;
                case Genero.MUJER:
                    Tracker.T.setVar("Female", 1);
                    break;
                default:
                    Tracker.T.setVar("Male & Female", 1);
                    break;
            }
            _genero = (int)value;
        }
    }

    #endregion

    #region Eventos

    private void Awake()
    {
        if (Gm == null)
        {
            Gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Gm != this)
        {
            Destroy(gameObject);
        }
        List = new List<string>();
        ObstaculosList = new List<string>();
    }

    #endregion

    #region Métodos públicos
    /// <summary>
    /// Método que setea en género del jugador dado un entero. 0 será neutral, 1 será hombre y 2 será mujer.
    /// </summary>
    /// <param name="g"></param>
    public void SetGenre(int g)
    {
        PlayerPrefs.SetInt("genre", g);
        Genero = (Genero)g;
        LoadScene("Menu");
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    /**
     * Método para el botón salir del menú
     */
    public void DoExitGame()
    {
        Application.Quit();
    }

    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        LoadScene("Intro");
    }
    #endregion

}
