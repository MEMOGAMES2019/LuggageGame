using RAGE.Analytics;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Luggage : MonoBehaviour
{

    int numItemsSaved = 0;
    public LuggageTarget[] targets;

    public Sprite emptyLuggage, fullLuggage;
    List<string> list;
    bool[] itemSaved;
    void Start()
    {
        list = GM.Gm.List;
        itemSaved = new bool[list.Count];
    }

    // Update is called once per frame
    void Update() { }

    public void SaveObject(Item obj)
    {
        numItemsSaved++;
        obj.gameObject.SetActive(true);
        if (numItemsSaved == 1)
        {
            foreach (LuggageTarget lgT in targets) lgT.ChangeSprite(fullLuggage);
        }

        int i = 0;
        while (i < list.Count && list[i].ToUpper() != obj.name.ToUpper())
        {
            i++;
        }
        if (i < list.Count)
        {
            Tracker.T.setVar("Objeto guardado", 1);
            Debug.Log("Correcto");
            itemSaved[i] = true;
        }
    }
    public void RemoveObject(Item obj)
    {
        numItemsSaved--;
        obj.gameObject.SetActive(false);
        if (numItemsSaved == 0)
        {
            foreach (LuggageTarget lgT in targets) lgT.ChangeSprite(emptyLuggage);
        }

        int i = 0;
        while (i < list.Count && list[i].ToUpper() != obj.name.ToUpper()) i++;
        if (i < list.Count) itemSaved[i] = false;

        Tracker.T.setVar("Objeto quitado", 1);
    }

    public string Check()
    {
        string[] itemsNotSaved = new string[list.Count];
        int j = 0;
        bool allSaved = true;
        for (int i = 0; i < list.Count; i++)
        {
            if (!itemSaved[i])
            {
                allSaved = false;
                itemsNotSaved[j] = list[i];
                j++;
            }
        }
        if (allSaved) return "Chachi";
        else
        {
            StringBuilder cad = new StringBuilder();
            cad.AppendLine("Te has dejado estos objetos:");
            int i = 0;
            while (i < itemsNotSaved.Length && itemsNotSaved[i] != null)
            {
                cad.AppendLine(string.Concat("- ", itemsNotSaved[i]));
                i++;
            }
            return cad.ToString();
        }
    }
}
