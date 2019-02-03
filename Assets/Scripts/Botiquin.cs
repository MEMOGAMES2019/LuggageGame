using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Botiquin : MonoBehaviour
{

    public LevelManager lvlMngr;
    SpriteRenderer spRenderer;
    void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Cuando el ratón pasa por encima del objeto.
    /// </summary>
    private void OnMouseEnter()
    {
        spRenderer.enabled = true;
    }

    /// <summary>
    /// Cuando el ratón sale del objeto.
    /// </summary>
    private void OnMouseExit()
    {
        spRenderer.enabled = false;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            lvlMngr.GoToFirstAidKit();
        }
    }
}
