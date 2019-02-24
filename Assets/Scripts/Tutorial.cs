using UnityEngine;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{

    public GameObject mano;
    public GameObject panel;
    public GameObject camisetaAmarilla;
    public GameObject panelInfoObject;
    public Animator manoAnimator;
    public GameObject camara;

    Text texto;

    enum State { CLICK, DRAGNDROP, LUGGAGE, OVERINFO, PULLOVER, BACKTOROOM, DRAWER, BATHROOM, BACKTHROOM, END, NULL }
    State state;
    // Use this for initialization
    void Start()
    {
        state = State.CLICK;
        texto = panel.GetComponentInChildren<Text>();
        texto.text = "Haz click sobre la camiseta amarilla.";
        manoAnimator.SetInteger("step", 0);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state == State.CLICK)
            {
                state = State.DRAGNDROP;
                texto.text = "Sin soltar, arrastra la camiseta hasta la maleta y luego suelta.";
                manoAnimator.SetInteger("step", 1);
            }
            else if (state == State.LUGGAGE)
            {
                state = State.OVERINFO;
                texto.text = "Si pones el cursor sobre el objeto, aparecerá su nombre.";
            }
            else if (state == State.DRAWER)
            {
                texto.text = "No todos los cajones tendrán algo dentro.";
            }
            else if (state == State.BACKTHROOM)
            {
                state = State.END;
                panel.SetActive(false);
                manoAnimator.SetInteger("step", 8);
            }
            else if (state == State.NULL)
            {
                panel.SetActive(false);
                mano.SetActive(false);
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (state == State.DRAGNDROP)
            {
                if (camisetaAmarilla.activeSelf)
                {
                    state = State.CLICK;
                    texto.text = "Haz click sobre la camiseta amarilla.";
                    manoAnimator.SetInteger("step", 0);
                }
                else
                {
                    state = State.LUGGAGE;
                    texto.text = "Haz click en la maleta para ver lo que has metido.";
                    manoAnimator.SetInteger("step", 2);
                }
            }
            if (state == State.PULLOVER)
            {
                if (camisetaAmarilla.activeSelf)
                {
                    state = State.BACKTOROOM;
                    texto.text = "Haz click aquí para cerrar la maleta y volver atrás.";
                    manoAnimator.SetInteger("step", 4);
                }
            }
        }

        if (state == State.OVERINFO)
        {
            if (panelInfoObject.activeSelf)
            {
                state = State.PULLOVER;
                texto.text = "Puedes arrastrar el objeto fuera de la maleta para sacarlo.";
                manoAnimator.SetInteger("step", 3);
            }
        }
        else if (state == State.END && camara.gameObject.activeSelf)
        {
            state = State.NULL;
            texto.text = "Haz click aquí cuando creas que has terminado.";
            panel.SetActive(true);
        }
    }

    public void ButtonBackToRoom()
    {
        if (state == State.BACKTOROOM)
        {
            state = State.DRAWER;
            texto.text = "Puedes ver el contenido de los cajones haciendo click en ellos.";
            manoAnimator.SetInteger("step", 5);

        }
        else if (state == State.DRAWER)
        {
            state = State.BATHROOM;
            texto.text = "Puedes hacer click aquí para ir al baño.";
            manoAnimator.SetInteger("step", 6);
        }
    }
    public void ButtonBathRoom()
    {
        if (state == State.BATHROOM)
        {
            state = State.BACKTHROOM;
            texto.text = "Revisa los cajones y haz click aquí para volver a la habitación";
            manoAnimator.SetInteger("step", 7);

        }

    }
    public void End()
    {
        panel.SetActive(false);
        mano.SetActive(false);
    }
}
