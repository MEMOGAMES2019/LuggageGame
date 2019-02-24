using UnityEngine;

public class Level4Object : MonoBehaviour
{

    void Start()
    {
        if (!GM.Gm.List.Contains(name) && !GM.Gm.ObstaculosList.Contains(name))
        {
            gameObject.SetActive(false);
        }
    }
}