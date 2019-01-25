using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    enum State { BATHROOM, ROOM, DRAWER, LUGGAGE, FIRSTAIDKIT, END};

    public Camera roomCam;
    public Camera bathroomCam;
    public Camera drawerCam;
    public Camera firstAidKitCam;
    public GameObject buttonBathroom;
    public Sprite [] bttnBathroom;
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
	void Start () {
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
        }

    }
    public void BathRoomButton()
    {
        if (currentDrawer != null) currentDrawer.SetActive(false);
        if (state == State.BATHROOM)
        {
            BackToRoom();
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
        }
        else 
        {
            GoToBathroom();
        }
    }

   public void End()
    {
        state = State.END;
        string sol = luggage.Check();
        bttnEnd.gameObject.SetActive(false);
        buttonBathroom.SetActive(false);
        endPanel.gameObject.SetActive(true);
        endPanel.GetComponentInChildren<Text>().text = sol;
    }
}
