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
    public string Check()
    {
        List<string> objetosNoGuardados = new List<string>();

        ObjetosList.ForEach(objeto =>
        {
            if (!ObjetosGuardados.Contains(objeto))
            {
                objetosNoGuardados.Add(objeto);
            }
        });
        if (objetosNoGuardados.Count == 0) return ("Felicidades\n ¡Has superado el nivel con éxito!");

        StringBuilder cad = new StringBuilder();
        cad.AppendLine("Ups... Te has olvidado de estos objetos:");

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
        cad.AppendLine("Ouch... Has metido estos objetos que no tenías que meter:");

        ObjetosErroneosGuardados.ForEach(objeto => cad.AppendLine(string.Concat("- ", objeto)));

        return cad.ToString();
    }

    #endregion

}
