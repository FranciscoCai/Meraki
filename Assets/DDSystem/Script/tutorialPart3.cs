using UnityEngine;
using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;


public class tutorialPart3 : MonoBehaviour
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

        dialogTexts.Add(new DialogData("¡Vamos/color:purple/ Lucy/color:white/! Ya casi llegamos al castillo.", "Constructor"));
      
        dialogTexts.Add(new DialogData("Aunque el /color:red/dino/color:white/ nos pisa los talones, y ya no quedan /color:blue/tortugas/color:white/...", "Constructor"));

        dialogTexts.Add(new DialogData("¿Qué hacemos?", "Constructor"));

        dialogTexts.Add(new DialogData("/trigger:Animation/Tiene que haber alguna otra forma de pararle los pies/speed:down/...", "Lucy"));

        dialogTexts.Add(new DialogData("A lo mejor las cajas esas... /size:up/(A y D para rotar)", "Lucy"));

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
