using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{

    GameObject twin;
    public GameObject panelInfo;
    Text nameInfo;
    Vector3 StartPoint;
    Vector3 Offset;
    bool hasExit;
    Luggage luggage;
    void Start()
    {
        StartPoint = transform.localPosition;
        nameInfo = panelInfo.GetComponentInChildren<Text>();
        hasExit = false;
        luggage = transform.parent.GetComponent<Luggage>();
    }

    public void SetTwin(GameObject ob)
    {
        twin = ob;
    }
    private void OnMouseEnter()
    {
        panelInfo.SetActive(true);
        nameInfo.text = name;

    }
    private void OnMouseExit()
    {
        panelInfo.SetActive(false);
    }
    private void OnMouseOver()
    {
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x == Screen.width - 1 || Input.mousePosition.y == Screen.height - 1)
        {
            hasExit = false;
            transform.localPosition = StartPoint;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hasExit)
            {
                panelInfo.SetActive(false);
                luggage.RemoveObject(this);
                twin.SetActive(true);
            }
            hasExit = false;
            transform.localPosition = StartPoint;

        }
    }

    /// <summary>
    /// Evento cuando se clicka el objeto.
    /// </summary>
    private void OnMouseDown()
    {
        //StartPoint = transform.localPosition;
        Offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
    }
    /// <summary>
    /// Evento cuando se mantiene pulsado el objeto.
    /// </summary>
    private void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + Offset;
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
        {
            throw new System.ArgumentNullException(nameof(collision));
        }

        hasExit = true;
    }
}
