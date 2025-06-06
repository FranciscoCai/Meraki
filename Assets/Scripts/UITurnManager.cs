using UnityEngine;
using UnityEngine.UI;

public class UITurnManager : MonoBehaviour
{
    public Image [] onTurn; //elemnt 2 es on turn 1
    public Image[] notOnTurn; // elemnt 2 es not on turn 3     

    public Sprite Lucy;
    public Sprite Dino;
    public Sprite Hefestin;
    public Sprite Picassin;
    public Sprite Tortugas;


    public bool tutorial1 = false;
    public bool tutorial2 = false;
    public bool tutorial3 = false;
    public bool nivel1 = false;
    public bool cave = false;


    public bool playerTurn = false;

    private bool turnosInicializados = false;


    void Update()
    {
        // Supón que GameManager.Instance.joseMiguel activa esto
        if (!turnosInicializados && GameManager.Instance.joseMiguel)
        {
            InicializarTurnos();
            turnosInicializados = true;
        }

        if (!turnosInicializados) return;

        if (GameManager.Instance.joseMiguel)
        {
            playerTurn = false;
        }
        else
        {
            playerTurn = true;
        }

        if (playerTurn)
        {
            TurnoLucy();
        }
        else
        {
            TurnoNPC();
        }
    }

    public void InicializarTurnos()
    {
        GameObject[] onTurnObjects = GameObject.FindGameObjectsWithTag("OnTurn");
        GameObject[] notOnTurnObjects = GameObject.FindGameObjectsWithTag("NotOnTurn");

        onTurn = new Image[onTurnObjects.Length];
        notOnTurn = new Image[notOnTurnObjects.Length];

        for (int i = 0; i < onTurnObjects.Length; i++)
        {
            onTurn[i] = onTurnObjects[i].GetComponent<Image>();
        }

        for (int i = 0; i < notOnTurnObjects.Length; i++)
        {
            notOnTurn[i] = notOnTurnObjects[i].GetComponent<Image>();
        }

        Debug.Log("Turnos inicializados.");
    }

    void TurnoLucy()
    {
        if (nivel1 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            onTurn[1].gameObject.SetActive(false);
            onTurn[2].gameObject.SetActive(false);

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Dino;
            notOnTurn[1].gameObject.SetActive(true);
            notOnTurn[1].transform.GetChild(0).GetComponent<Image>().sprite = Picassin;
            notOnTurn[0].gameObject.SetActive(false);

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }
        if (tutorial1 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            onTurn[1].gameObject.SetActive(false);
            onTurn[2].gameObject.SetActive(false);

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Tortugas;
            notOnTurn[1].gameObject.SetActive(false);
            notOnTurn[0].gameObject.SetActive(false);

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }
        if (tutorial2 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            onTurn[1].gameObject.SetActive(false);
            onTurn[2].gameObject.SetActive(false);

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Dino;
            notOnTurn[1].gameObject.SetActive(true);
            notOnTurn[1].transform.GetChild(0).GetComponent<Image>().sprite = Picassin;
            notOnTurn[0].gameObject.SetActive(true);
            notOnTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Tortugas;

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }
        if (tutorial3 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            onTurn[1].gameObject.SetActive(false);
            onTurn[2].gameObject.SetActive(false);

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Dino;
            notOnTurn[1].gameObject.SetActive(true);
            notOnTurn[1].transform.GetChild(0).GetComponent<Image>().sprite = Hefestin;
            notOnTurn[0].gameObject.SetActive(false);

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }

    }

    void TurnoNPC()
    {
        if (nivel1 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Dino;
            onTurn[1].gameObject.SetActive(true);
            onTurn[1].transform.GetChild(0).GetComponent<Image>().sprite = Picassin;
            onTurn[2].gameObject.SetActive(false);

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            notOnTurn[1].gameObject.SetActive(false);
            notOnTurn[0].gameObject.SetActive(false);

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }
        if (tutorial1 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Tortugas;
            onTurn[1].gameObject.SetActive(false);
            onTurn[2].gameObject.SetActive(false);

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            notOnTurn[1].gameObject.SetActive(false);
            notOnTurn[0].gameObject.SetActive(false);

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }
        if (tutorial2 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Dino;
            onTurn[1].gameObject.SetActive(true);
            onTurn[1].transform.GetChild(0).GetComponent<Image>().sprite = Picassin;
            onTurn[2].gameObject.SetActive(true);
            onTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Tortugas;

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            notOnTurn[1].gameObject.SetActive(false);
            notOnTurn[0].gameObject.SetActive(false);

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }
        if (tutorial3 && onTurn.Length > 2 && notOnTurn.Length > 2)
        {
            onTurn[0].transform.GetChild(0).GetComponent<Image>().sprite = Dino;
            onTurn[1].gameObject.SetActive(true);
            onTurn[1].transform.GetChild(0).GetComponent<Image>().sprite = Hefestin;
            onTurn[2].gameObject.SetActive(false);

            notOnTurn[2].gameObject.SetActive(true);
            notOnTurn[2].transform.GetChild(0).GetComponent<Image>().sprite = Lucy;
            notOnTurn[1].gameObject.SetActive(false);
            notOnTurn[0].gameObject.SetActive(false);

            AplicarColorGris(notOnTurn);
            AplicarColorNormal(onTurn);
        }

    }

    void AplicarColorGris(Image[] images)
    {
        foreach (var img in images)
        {
            if (img != null && img.transform.childCount > 0)
            {
                var child = img.transform.GetChild(0).GetComponent<Image>();
                if (child != null)
                    child.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }
    }

    void AplicarColorNormal(Image[] images)
    {
        foreach (var img in images)
        {
            if (img != null && img.transform.childCount > 0)
            {
                var child = img.transform.GetChild(0).GetComponent<Image>();
                if (child != null)
                    child.color = Color.white;
            }
        }
    }
}