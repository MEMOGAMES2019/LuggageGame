using RAGE.Analytics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    enum State { BATHROOM, ROOM, DRAWER, LUGGAGE, FIRSTAIDKIT, END };

    public Camera roomCam;
    public Camera bathroomCam;
    public Camera drawerCam;
    public Camera firstAidKitCam;
    public GameObject buttonBathroom;
    public Sprite[] bttnBathroom;
    public GameObject buttonBackToRoom;
    public GameObject bttnEnd;
    public GameObject endPanel;
    public Luggage luggage;
    public GameObject drawerImage;
    Vector3 initialLuggagePos;
    Vector3 initialLuggageScale;
    GameObject currentDrawer = null;
    State state;
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
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
            Tracker.T.Accessible.Accessed("FirstAidKit", AccessibleTracker.Accessible.Screen);
        }
    }

    public void GoToDrawer(GameObject drawer)
    {
        if (state != State.END)
        {
            bttnEnd.gameObject.SetActive(false);
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
            state = State.DRAWER;
            roomCam.gameObject.SetActive(false);
            drawerCam.gameObject.SetActive(true);
            if (drawer != null)
            {
                currentDrawer = drawer;
                currentDrawer.SetActive(true);
                Tracker.T.Accessible.Accessed("Drawer", AccessibleTracker.Accessible.Screen);
            }
            buttonBackToRoom.SetActive(true);
            luggage.transform.position = initialLuggagePos;
            luggage.transform.localScale = initialLuggageScale;
            drawerImage.SetActive(true);
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
        if (state != State.END)
        {
            state = State.ROOM;
            roomCam.gameObject.SetActive(true);
            bathroomCam.gameObject.SetActive(false);
            drawerCam.gameObject.SetActive(false);
            if (currentDrawer != null) currentDrawer.SetActive(false);
            buttonBackToRoom.SetActive(false);
            bttnEnd.gameObject.SetActive(true);
        }
    }

    void GoToBathroom()
    {
        if (state != State.END)
        {
            bttnEnd.gameObject.SetActive(false);
            state = State.BATHROOM;
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[1];
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
        if (state == State.BATHROOM)
        {
            Tracker.T.setVar("RoomButtom", 1);
            BackToRoom();
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
        }
        else
        {
            Tracker.T.setVar("BathRoomButton", 1);
            GoToBathroom();
        }
    }

    public void End()
    {
        state = State.END;
        StringBuilder cad = new StringBuilder();

        cad.AppendLine(luggage.Check());
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
