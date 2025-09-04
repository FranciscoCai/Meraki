using UnityEngine;
using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;

public class newTutorialDialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject pincelSprite;
    public GameObject dialogCamera;
    public GameObject turnsUI;

    private bool pincelBool = false;

    public GameObject[] Example;

    void Start()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/size:up/ ¡¿Cómo?! /size:init/¿Dónde estoy?", "Lucy"));

        dialogTexts.Add(new DialogData("/size:up/ ¡/color:purple/Lucy/color:white/! /size:init/¡Al fin has llegado!", "Constructor"));

        dialogTexts.Add(new DialogData("/color:purple/Lucy/color:white//speed:down/... /speed:init/¿Cómo que /color:purple/Lucy/color:white/?", "Lucy"));

        dialogTexts.Add(new DialogData("No puede ser/speed:down/.../speed:init/ ¡Estoy atrapada en el libro!", "Lucy"));

        dialogTexts.Add(new DialogData("¡Rápido, necesito que me ayudes a salir de aquí!", "Constructor"));

        dialogTexts.Add(new DialogData("¡Y ese es /color:orange/Hefestín/color:white/!", "Lucy"));

        dialogTexts.Add(new DialogData("/size:up/¡¿A qué estás esperando?!/size:init/ ¡No nos queda tiempo!", "Constructor"));

        dialogTexts.Add(new DialogData("/speed:down/...", "Lucy"));

        dialogTexts.Add(new DialogData("Supongo que no tengo elección/speed:down/...", "Lucy"));

        dialogManager.Show(dialogTexts);

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
