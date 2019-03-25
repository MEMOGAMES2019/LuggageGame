using RAGE.Analytics;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    enum State { BATHROOM, ROOM, DRAWER, LUGGAGE, FIRSTAIDKIT, END };

    public int level;
    public Camera roomCam;
    public Camera bathroomCam;
    public Camera drawerCam;
    public Camera firstAidKitCam;
    public GameObject buttonBathroom;
    public Sprite[] bttnBathroom;
    [SerializeField]
    private List<Sprite> _tiposSuelos;
    [SerializeField]
    private GameObject _suelo;
    public GameObject buttonBackToRoom;
    public GameObject bttnEnd;
    public GameObject endPanel;
    public Luggage luggage;
    public GameObject drawerImage;
    Vector3 initialLuggagePos;
    Vector3 initialLuggageScale;
    GameObject currentDrawer = null;
    State state;
    public List<Sprite> TiposSuelos { get => _tiposSuelos; set => _tiposSuelos = value; }
    public GameObject Suelo { get => _suelo; set => _suelo = value; }

    // 0 = Room & 1 = Bathroom
    int myActualRoom = 0;
    //public Image
    void Start()
    {
        state = State.ROOM;
        roomCam.gameObject.SetActive(true);
        bathroomCam.gameObject.SetActive(false);
        drawerCam.gameObject.SetActive(false);
        firstAidKitCam.gameObject.SetActive(false);
        buttonBathroom.SetActive(true);
        buttonBackToRoom.SetActive(false);
        initialLuggagePos = luggage.transform.position;
        initialLuggageScale = luggage.transform.localScale;
        bttnEnd.gameObject.SetActive(true);
    }

    public void GoToFirstAidKit()
    {
        if (state != State.END)
        {
            state = State.FIRSTAIDKIT;
            bathroomCam.gameObject.SetActive(false);
            firstAidKitCam.gameObject.SetActive(true);

            bttnEnd.gameObject.SetActive(false);
            buttonBackToRoom.SetActive(true);

            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[1];
            Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[1];
            Tracker.T.Accessible.Accessed("FirstAidKit", AccessibleTracker.Accessible.Screen);
        }
    }

    public void GoToDrawer(GameObject drawer)
    {
        if (state != State.END)
        {
            bttnEnd.gameObject.SetActive(false);
            state = State.DRAWER;
            roomCam.gameObject.SetActive(false);
            drawerCam.gameObject.SetActive(true);

            if (myActualRoom == 0)
            {
                buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
                Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[0];
            }
            else if (myActualRoom == 1)
            {
                buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[1];
                Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[1];
            }
            buttonBackToRoom.SetActive(true);
            drawerImage.SetActive(true);

            if (drawer != null)
            {
                currentDrawer = drawer;
                currentDrawer.SetActive(true);
                luggage.transform.position = initialLuggagePos;
                luggage.transform.localScale = initialLuggageScale;
                Tracker.T.Accessible.Accessed(drawer.name, AccessibleTracker.Accessible.Screen);
            }
        }
    }

    public void GoToLuggage()
    {
        if (state != State.END)
        {
            if (currentDrawer != null) currentDrawer.SetActive(false);
            GoToDrawer(null);
            drawerImage.SetActive(false);
            state = State.LUGGAGE;
            luggage.gameObject.SetActive(true);
            luggage.transform.position = new Vector3(0, 19f, luggage.transform.position.z);
            luggage.transform.localScale = new Vector3(3.5f, 3.5f, 1);
            Tracker.T.Accessible.Accessed("Luggage", AccessibleTracker.Accessible.Screen);
        }

    }

    public void BackToRoom()
    {
        // boton salir de la maleta
        if (state != State.END)
        {
            if (myActualRoom == 0)
            {
                state = State.ROOM;
                roomCam.gameObject.SetActive(true);
                bathroomCam.gameObject.SetActive(false);
            }
            else if (myActualRoom == 1)
            {
                state = State.BATHROOM;
                roomCam.gameObject.SetActive(false);
                bathroomCam.gameObject.SetActive(true);
            }
            drawerCam.gameObject.SetActive(false);
            if (currentDrawer != null)
                currentDrawer.SetActive(false);
            buttonBackToRoom.SetActive(false);
            bttnEnd.gameObject.SetActive(true);
        }
    }

    void GoToBathroom()
    {
        if (state != State.END)
        {
            bttnEnd.gameObject.SetActive(true);
            state = State.BATHROOM;
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[1];
            Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[1];
            drawerCam.gameObject.SetActive(false);
            roomCam.gameObject.SetActive(false);
            bathroomCam.gameObject.SetActive(true);
            buttonBackToRoom.SetActive(false);
            Tracker.T.Accessible.Accessed("Bathroom", AccessibleTracker.Accessible.Screen);
        }

    }
    public void BathRoomButton()
    {
        if (currentDrawer != null) currentDrawer.SetActive(false);
        if (myActualRoom == 1)
        {
            myActualRoom = 0;
            Tracker.T.setVar("RoomButtom", 1);
            BackToRoom();
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
            Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[0];
        }
        else
        {
            myActualRoom = 1;
            Tracker.T.setVar("BathRoomButton", 1);
            GoToBathroom();
        }
    }

    public void End()
    {
        state = State.END;
        StringBuilder cad = new StringBuilder();

        cad.AppendLine(luggage.Check(level));
       
        string s = luggage.GetObjetosErroneos();
        if (s.Length > 0)
            cad.Append(s);

        bttnEnd.gameObject.SetActive(false);
        buttonBathroom.SetActive(false);
        endPanel.gameObject.SetActive(true);
        endPanel.GetComponentInChildren<Text>().text = cad.ToString();

        Tracker.T.setVar("EndButton", 1);
        Tracker.T.setVar("Resultado: " + cad.Length, cad.ToString());
        Tracker.T.Completable.Completed(LevelSelector.LevelNameGlobal, CompletableTracker.Completable.Level, true);
    }
}
