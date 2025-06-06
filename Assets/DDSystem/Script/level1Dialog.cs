using UnityEngine;
using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;

public class level1Dialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject dialogCamera;

    public GameObject cuadroSprite;
    public GameObject[] Example;

    private bool cuadroBool = false;

    public GameObject turnsUI;

    void Start()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/color:purple/L-Lucy/color:white//speed:down/.../speed:init//size:down/¿qué haces aquí?", "Painter"));

        dialogTexts.Add(new DialogData("Te traigo el pincel, /color:yellow/Picassin/color:white/.", "Lucy"));

        dialogTexts.Add(new DialogData("G-Gracias/speed:down/...", "Painter"));

        dialogTexts.Add(new DialogData("Ahora vendrá el /color:red/dino/color:white/.", "Painter"));

        dialogTexts.Add(new DialogData("Ayúdame/speed:down/.../speed:init/ /size:down/por favor/speed:down/...", "Painter", () =>
        {
            cuadroBool = true;
            cuadroSprite.SetActive(true);
        }));

        dialogTexts.Add(new DialogData("Tengo que pintar el cuadro, cumplir la profecía.", "Painter"));

        dialogTexts.Add(new DialogData("Lo sé, y te voy a ayudar, no te preocupes.", "Lucy", () =>
        {
            cuadroBool = false;
            cuadroSprite.SetActive(false);
        }));

        dialogTexts.Add(new DialogData("/trigger:Animation//speed:down/...", "Lucy"));

        dialogTexts.Add(new DialogData("¿Qué le pasa a las cajas?", "Lucy"));

        dialogManager.Show(dialogTexts); //pa mostrarlo

        StartCoroutine(WaitUntilDialogEnds());
    }

    private IEnumerator WaitUntilDialogEnds()
    {
        // Esperar a que comience el diálogo
        yield return new WaitUntil(() => dialogManager.state == State.Active);

        // Esperar hasta que el estado cambie a Deactivate
        yield return new WaitUntil(() => dialogManager.state == State.Deactivate);

        dialogCamera.SetActive(false);

        turnsUI.SetActive(true);
        FindObjectOfType<UITurnManager>().InicializarTurnos();
       
    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }
}
