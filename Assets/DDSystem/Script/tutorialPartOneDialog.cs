using UnityEngine;
using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;

public class tutorialPartOneDialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject dialogCamera;
    public GameObject turnsUI;
    public GameObject[] Example;

    void Start()
    {
        var dialogTexts = new List<DialogData>();

        if (GameManager.Instance.mando)
        {
            dialogTexts.Add(new DialogData("/size:up/ ¡¿Cómo?! /size:init/¡Estás usando mando!", "Lucy"));
            dialogTexts.Add(new DialogData("¡Y soy /color:purple/Lucy/color:white/!", "Lucy"));
            dialogTexts.Add(new DialogData("/trigger:Animation/Tiene que haber alguna forma de que pueda salir de aquí/speed:down/...", "Lucy"));
            dialogTexts.Add(new DialogData("¿A lo mejor puedo congelar a la /color:blue/tortuga/color:white//size:up/(R2)/size:init/?", "Lucy"));
        }
        else
        {
            dialogTexts.Add(new DialogData("/size:up/ ¡¿Cómo?! /size:init/¡Estoy atrapada en el libro!", "Lucy"));
            dialogTexts.Add(new DialogData("¡Y soy /color:purple/Lucy/color:white/!", "Lucy"));
            dialogTexts.Add(new DialogData("/trigger:Animation/Tiene que haber alguna forma de que pueda salir de aquí/speed:down/...", "Lucy"));
            dialogTexts.Add(new DialogData("¿A lo mejor puedo congelar a la /color:blue/tortuga/color:white//size:up/(Click izquierdo)/size:init/?", "Lucy"));
        }

        dialogManager.Show(dialogTexts);

        // Corutina que espera que el estado se vuelva Deactivate
        StartCoroutine(WaitUntilDialogEnds());
    }

    private IEnumerator WaitUntilDialogEnds()
    {
        // Esperar a que comience el diálogo
        yield return new WaitUntil(() => dialogManager.state == State.Active);

        // Esperar hasta que el estado cambie a Deactivate
        yield return new WaitUntil(() => dialogManager.state == State.Deactivate);

        // Desactivar la cámara cuando termina
        dialogCamera.SetActive(false);

        turnsUI.SetActive(true);
        FindObjectOfType<UITurnManager>().InicializarTurnos();
    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }
}