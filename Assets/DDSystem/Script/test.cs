using UnityEngine;
using Doublsb.Dialog;
using System.Collections.Generic;
public class test : MonoBehaviour
{
    public DialogManager dialogManager;

    public GameObject[] Example;
    void Start()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/size:up/Hi, /size:init/my name is Pu.", "Painter"));

        dialogTexts.Add(new DialogData("I am Ta. Popped out to let you know Asset can show other characters.", "Constructor"));

        dialogTexts.Add(new DialogData("This Asset, The D'Dialog System has many features.", "Painter"));


        dialogManager.Show(dialogTexts); //pa mostrarlo
    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }
}
