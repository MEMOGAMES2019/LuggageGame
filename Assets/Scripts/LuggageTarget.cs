using UnityEngine;

public class LuggageTarget : MonoBehaviour
{
    bool over = false;
    public LevelManager lvlMngr;
    SpriteRenderer spR;
    private void Start()
    {
        spR = GetComponent<SpriteRenderer>();
    }
    private void OnMouseEnter()
    {
        over = true;
    }
    private void OnMouseExit()
    {
        over = false;
    }
    private void OnMouseUp()
    {
        if (over)
        {
            lvlMngr.GoToLuggage();
        }

    }
    public void ChangeSprite(Sprite sp)
    {
        spR.sprite = sp;
    }
}
