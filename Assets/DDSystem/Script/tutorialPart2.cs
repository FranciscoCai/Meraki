using UnityEngine;
using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;

public class tutorialPart2 : MonoBehaviour
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

        if (GameManager.Instance.mando)
        {
            dialogTexts.Add(new DialogData("/size:up/  ¡/color:purple/Lucy /color:white/! /size:init/¡Que alegría verte!", "Constructor"));
            //Activa el pincel
            dialogTexts.Add(new DialogData("Rápido, necesito que me ayudes.", "Constructor", () =>
            {
                pincelBool = true;
                pincelSprite.SetActive(true);
            }));
            dialogTexts.Add(new DialogData("Tengo que llevarle este pincel a /color:yellow/Picassin/color:white/.", "Constructor"));

            dialogTexts.Add(new DialogData("Sí... ¿Te persigue el /color:red/dino/color:white/?", "Lucy"));
            //Desactiva el pincel
            dialogTexts.Add(new DialogData("Bien adivinado, el pincel lo atrae.", "Constructor", () =>
            {
                pincelBool = false;
                pincelSprite.SetActive(false);
            }));

            dialogTexts.Add(new DialogData("Necesito llegar al otro lado, pero no puedo cruzar con estas /color:blue/tortugas/color:white/ moviéndose todo el rato.", "Constructor"));

            dialogTexts.Add(new DialogData("Déjamelo a mí, /color:orange/Hefestín/color:white/.", "Lucy"));

            dialogTexts.Add(new DialogData("Tengo que conseguir que avance el tiempo sin congelar a la tortuga/size:up/(R)/size:init/.", "Lucy"));
        }
        else
        {
            dialogTexts.Add(new DialogData("/size:up/  ¡/color:purple/Lucy /color:white/! /size:init/¡Que alegría verte!", "Constructor"));
            //Activa el pincel
            dialogTexts.Add(new DialogData("Rápido, necesito que me ayudes.", "Constructor", () =>
            {
                pincelBool = true;
                pincelSprite.SetActive(true);
            }));
            dialogTexts.Add(new DialogData("Tengo que llevarle este pincel a /color:yellow/Picassin/color:white/.", "Constructor"));

            dialogTexts.Add(new DialogData("Sí... ¿Te persigue el /color:red/dino/color:white/?", "Lucy"));
            //Desactiva el pincel
            dialogTexts.Add(new DialogData("Bien adivinado, el pincel lo atrae.", "Constructor", () =>
            {
                pincelBool = false;
                pincelSprite.SetActive(false);
            }));

            dialogTexts.Add(new DialogData("Necesito llegar al otro lado, pero no puedo cruzar con estas /color:blue/tortugas/color:white/ moviéndose todo el rato.", "Constructor"));

            dialogTexts.Add(new DialogData("Déjamelo a mí, /color:orange/Hefestín/color:white/.", "Lucy"));

            dialogTexts.Add(new DialogData("Tengo que conseguir que avance el tiempo sin congelar a la tortuga /size:up/(R)/size:init/.", "Lucy"));
        }

       

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
