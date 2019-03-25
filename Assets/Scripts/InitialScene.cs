using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Constantes;

public class InitialScene : MonoBehaviour
{
    public GameObject title;
    public GameObject play;
    public GameObject playMini;
    public GameObject genre;
    public GameObject men;
    public GameObject[] speechBubbles;
    public bool saveGame;

    int speech;
    void Start()
    {
#if UNITY_EDITOR
        if(!saveGame)PlayerPrefs.DeleteAll();
#endif
        speech = 0;
        playMini.SetActive(false);
        men.SetActive(false);
        genre.SetActive(false);
        foreach (GameObject go in speechBubbles) go.SetActive(false);
    }

   public void SetSpeechBubble()
   {
        if (speech == 0)
        {
            title.SetActive(false);
            play.SetActive(false);
            men.SetActive(true);
            speechBubbles[speech].SetActive(true);
            speech++;
        }
        else
        {
            speechBubbles[speech - 1].SetActive(false);
            speechBubbles[speech].SetActive(true);
            speech++;
        }

        if (speech == speechBubbles.Length) playMini.SetActive(true);
   }

    public void ConfigGenre()
    {
        Debug.Log((Genero)PlayerPrefs.GetInt("genre"));
        if (!PlayerPrefs.HasKey("genre"))
        {
            men.SetActive(false);
            playMini.SetActive(false);
            speechBubbles[speech - 1].SetActive(false);
            genre.SetActive(true);
        }
        else
            GM.Gm.LoadScene("Menu");
    }
}
