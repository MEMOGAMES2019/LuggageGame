using RAGE.Analytics;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Luggage : MonoBehaviour
{
    #region Variables de Unity

    [SerializeField]
    private List<LuggageTarget> _targets;
    public Sprite emptyLuggage, fullLuggage;

    #endregion

    #region Atributos
    /// <summary>
    /// Objetos que hay que meter en la maleta.
    /// </summary>
    private List<string> ObjetosList { get; set; }

    /// <summary>
    /// Objetos guardados en la maleta y correctos.
    /// </summary>
    private List<string> ObjetosGuardados { get; set; }

    /// <summary>
    /// Objetos guardados en la maleta e incorrectos.
    /// </summary>
    private List<string> ObjetosErroneosGuardados { get; set; }

    /// <summary>
    /// Número de objetos guardados.
    /// </summary>
    private int NumItemsSaved { get; set; } = 0;

    /// <summary>
    /// Maletas en diferentes escenas.
    /// </summary>
    private List<LuggageTarget> Targets { get => _targets; set => _targets = value; }

    /// <summary>
    /// Variable que almacena las estrellas que consigue el usuario.
    /// </summary>
    private int stars = 0;
    public int Stars { get => stars; set => stars = value; }
    #endregion

    #region Eventos

    void Start()
    {
        ObjetosList = GM.Gm.List;
        ObjetosGuardados = new List<string>();
        ObjetosErroneosGuardados = new List<string>();
    }

    void Update() { }

    #endregion

    #region Métodos públicos

    /// <summary>
    /// Guarda un objeto en la maleta.
    /// </summary>
    /// <param name="obj">Objeto a guardar.</param>
    public void SaveObject(Item obj)
    {
        NumItemsSaved++;
        obj.gameObject.SetActive(true);
        if (NumItemsSaved == 1)
            Targets.ForEach(target => target.ChangeSprite(fullLuggage));

        if (ObjetosList.Contains(obj.name))
        {
            ObjetosGuardados.Add(obj.name);
            Tracker.T.setVar("Objeto guardado", 1);
            Debug.Log("Correcto");
        }
        else
        {
            ObjetosErroneosGuardados.Add(obj.name);
            Tracker.T.setVar("Objeto erróneo guardado", 1);
            Debug.Log("Correcto");
        }
    }

    /// <summary>
    /// Quita un objeto de la maleta.
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObject(Item obj)
    {
        NumItemsSaved--;
        obj.gameObject.SetActive(false);
        if (NumItemsSaved == 0)
            Targets.ForEach(target => target.ChangeSprite(emptyLuggage));

        if (ObjetosGuardados.Contains(obj.name))
        {
            ObjetosGuardados.Remove(obj.name);
            Tracker.T.setVar("Objeto quitado", 1);
        }
        else
        {
            ObjetosErroneosGuardados.Remove(obj.name);
            Tracker.T.setVar("Objeto erróneo quitado", 1);
        }
    }

    /// <summary>
    /// Comprueba si ha metido todos los objetos en la maleta.
    /// </summary>
    /// <returns>Acierto o los objetos que le ha faltaba por meter.</returns>
    public string Check(int level)
    {
        string clima = "C";
        if (GM.Gm.Clima == Assets.Scripts.Constantes.Clima.CALIDO) clima = "W";
        List<string> objetosNoGuardados = new List<string>();

        ObjetosList.ForEach(objeto =>
        {
            if (!ObjetosGuardados.Contains(objeto))
            {
                objetosNoGuardados.Add(objeto);
            }
        });
        if (objetosNoGuardados.Count == 0 && ObjetosErroneosGuardados.Count == 0)
        {
            stars = 3;
            PlayerPrefs.SetInt("level" + level.ToString()+ clima, 3);
            return ("Felicidades\n ¡Ha superado el nivel con éxito!");
        }

        int erroneos = ObjetosErroneosGuardados.Count;
        if (erroneos >= 3) stars = 1;
        else if (erroneos >= 1) stars = 2;
        else
        {
            float objTotales = ObjetosList.Count;
            float objMetidos = objTotales - objetosNoGuardados.Count;
            objMetidos -= ObjetosErroneosGuardados.Count / 2;


            if (objMetidos / objTotales >= 0.8f) stars = 3;
            else if (objMetidos / objTotales >= 0.4f) stars = 2;
            else if (objMetidos >= 1) stars = 1;
            else stars = 0;
        }

        
        if(PlayerPrefs.GetInt("level" + level.ToString() + clima)<= stars)
            PlayerPrefs.SetInt("level" + level.ToString()+clima, stars);

        if (objetosNoGuardados.Count == 0) return string.Empty;
        StringBuilder cad = new StringBuilder();
        cad.AppendLine("Ups... Se ha olvidado de estos objetos:\n");

        objetosNoGuardados.ForEach(objeto => cad.AppendLine(string.Concat("- ", objeto)));

        return cad.ToString();
    }

    /// <summary>
    /// Devuelve si hay objetos erróneos metidos en la maleta.
    /// </summary>
    /// <returns>Los objetos erróneos metidos en la maleta.</returns>
    public string GetObjetosErroneos()
    {
        if (ObjetosErroneosGuardados.Count == 0) return string.Empty;
        StringBuilder cad = new StringBuilder();
        cad.AppendLine("Ouch... Ha metido estos objetos que no tenía que meter:\n");

        ObjetosErroneosGuardados.ForEach(objeto => cad.AppendLine(string.Concat("- ", objeto)));

        return cad.ToString();
    }

    #endregion

}
