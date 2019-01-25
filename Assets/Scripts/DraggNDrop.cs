using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DraggNDrop : MonoBehaviour
{
    private float OFFSET_Z { get { return 10.0f; } }

    public Item twin;
    Luggage luggage;
    bool itsInTarget;
    private Vector3 StartPoint;
    private Vector3 Offset;
    public bool warm;
    public bool cold;
    public bool female;
    public bool male;
    int genre = 1; //0 --> neutral, 1 --> male, 2 --> female;
    int weather = 0; //0--> warm, 1-->cold

    void Start()
    {
        genre = GM.gm.getGenre();
        weather = GM.gm.getWeather();
        if ((cold && weather == 0) || (warm && weather == 1)) gameObject.SetActive(false);
        else
        {
            if ((female && genre == 1) || (male && genre == 2)) gameObject.SetActive(false);
        }
        itsInTarget = false;
        luggage = twin.transform.parent.gameObject.GetComponent<Luggage>();
    }

    /// <summary>
    /// Evento cuando se clicka el objeto.
    /// </summary>
    private void OnMouseDown()
    {
        StartPoint = transform.position;
        Offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, OFFSET_Z));
    }

    /// <summary>
    /// Evento cuando se mantiene pulsado el objeto.
    /// </summary>
    private void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, OFFSET_Z);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + Offset;
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    /// <summary>
    /// Evento cuando se deja de clickar el objeto.
    /// </summary>
    private void OnMouseUp()
    {
       
        transform.position = StartPoint;
        if (itsInTarget)
        {
            luggage.saveObject(twin);
            twin.setTwin(gameObject);
            gameObject.SetActive(false);
        }
        itsInTarget = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        itsInTarget = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        itsInTarget = false;
    }


}