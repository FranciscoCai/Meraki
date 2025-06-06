using UnityEngine;
using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;

public class level2Dialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public DialogCameraManager cameraManager; // asigna en el inspector
    public GameObject dialogCamera;
    public GameObject turnsUI;
    public GameObject[] Example;

    void Start()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Ya casi estamos/speed:down/...", "Painter"));

        dialogTexts.Add(new DialogData("/trigger:CastleCamera1/ ¿C-Cómo vamos a salir de aquí,  /color:purple/Lucy/color:white/?", "Painter"));

        dialogTexts.Add(new DialogData("/trigger:CastleCamera2/ La puerta está cerrada/speed:down/...", "Painter"));

        dialogTexts.Add(new DialogData("/trigger:CastleCamera4/ Creo que necesitamos conseguir algo antes/speed:down/...", "Lucy"));

        dialogTexts.Add(new DialogData("/trigger:CastleCamera5/ ¡Eso, los /color:green/botes de pintura/color:white/!", "Lucy"));

        dialogTexts.Add(new DialogData("Sigamos, /color:yellow/Picassin/color:white/, ya casi lo hemos logrado.", "Lucy"));

        dialogManager.Show(dialogTexts);

        StartCoroutine(WaitUntilDialogEnds());
    }

    private IEnumerator WaitUntilDialogEnds()
    {
        yield return new WaitUntil(() => dialogManager.state == State.Active);
        yield return new WaitUntil(() => dialogManager.state == State.Deactivate);
        dialogCamera.SetActive(false);

        turnsUI.SetActive(true);
        FindObjectOfType<UITurnManager>().InicializarTurnos();
    }
}