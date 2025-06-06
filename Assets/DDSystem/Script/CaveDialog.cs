using UnityEngine;
using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;

public class CaveDialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject dialogCamera;

    public GameObject[] Example;

    public GameObject turnsUI;

    void Start()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/speed:down/...", "Painter"));

        dialogTexts.Add(new DialogData("/color:purple/L-Lucy/color:white//speed:down/.../speed:init//size:down/¿dónde estamos?", "Painter"));

        dialogTexts.Add(new DialogData("No lo sé, /color:yellow/Picassin/color:white//speed:down/...", "Lucy"));

        dialogTexts.Add(new DialogData("Pero vamos a encontrar la forma de salir de aquí, no te preocupes.", "Lucy"));

        dialogTexts.Add(new DialogData("Pues no sé cómo/speed:down/...", "Painter"));

        dialogTexts.Add(new DialogData("Tiene que haber una forma/speed:down/...", "Lucy"));

        dialogTexts.Add(new DialogData("/trigger:Animation//speed:down/...", "Lucy"));

        dialogTexts.Add(new DialogData("¿Y si coloco una caja encima?", "Lucy"));

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
