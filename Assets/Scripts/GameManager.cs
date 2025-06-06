using UnityEngine;
using System.Collections;
using StarterAssets;
using UnityEngine.InputSystem;

public enum PintorEfect
{
    Move,Stun
}
public enum DinoEfect
{
    Follow, Howl, Stun, Angry
}
public enum ConstructorEfect
{
    Move,TurtleWalk, Stun
}
public enum TurtleEfect
{
    Move, Down, Up, Stop
}
public class GameManager : MonoBehaviour
{
    public delegate void ChangeTurnDelegate();
    public static event ChangeTurnDelegate OnStartTurn;
    public static event ChangeTurnDelegate OnPassTurn;
    public static event ChangeTurnDelegate OnStopTurn;

    public GameObject[] objects;
    public GameObject g_Dino;
    public GameObject g_Pintor;
    public GameObject Camera;
    public GameObject dialogTester;
    public GameObject cameraFollowPlayer;

    public ActivateDialog activateDialogScript;

    public bool changeCamera = false;
    public bool joseMiguel = false;
    public bool activateDialog = false;
    public bool tutorial = false;
    public bool nivel1 = false;
    public bool mando = false;

    public float distance;

    private GameObject Player;
    private StarterAssetsInputs _input;
    

    public PintorEfect g_pintorEfect;
    public DinoEfect g_dinoEfect;
    public ConstructorEfect g_constructorEfect;
    public TurtleEfect g_turtleEfect;
    public static GameManager Instance { get; private set; }


    //private Vector3[] previousPositions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if (tutorial)
        {
            activateDialog = true;
        }

        Player = GameObject.FindWithTag("Player");
        _input = Player.GetComponent<StarterAssetsInputs>();

        objects = GameObject.FindGameObjectsWithTag("MovebleObject");
        g_Dino = GameObject.FindGameObjectWithTag("Dino");
        g_Pintor = GameObject.FindGameObjectWithTag("Pintor");
        
        
        //if (objects != null && objects.Length > 0)
        //{
        //    previousPositions = new Vector3[objects.Length];
        //    for (int i = 0; i < objects.Length; i++)
        //    {
        //        if (objects[i] != null)
        //        {
        //            previousPositions[i] = objects[i].transform.position;
        //        }

        //    }
        //}
    }
    public void ChangeTurn()
    {
        if(joseMiguel)
        {
            return;
        }
        StartCoroutine(ResetChangeStateAfterDelay(2));
    }

    private void Update()
    {
        if (nivel1 && activateDialogScript.activateDialogByCode)
        {
            activateDialog = true;
        }
        if(activateDialog)
        {
            dialogTester.SetActive(true);
            joseMiguel = true;
            //cameraFollowPlayer.SetActive(false);
        }
        else
        {
            //cameraFollowPlayer.SetActive(true);
        }

        //for (int i = 0; i < objects.Length; i++)
        //{
        //    if (objects[i].transform.position != previousPositions[i])
        //    {
        //        turno = !turno;


        //        previousPositions[i] = objects[i].transform.position;
        //        StartCoroutine(ResetChangeStateAfterDelay(2f));

        //        break;
        //    }
        //}

        //Calcula la distancia entre el dino y el pintor pa q se asuste el pintor
        if (g_Dino != null && g_Pintor != null)
        {

            distance = Vector3.Distance(g_Dino.transform.position, g_Pintor.transform.position);
        }

        if (joseMiguel)
        {
            _input.changeCamera = false;
        }
        if (_input.changeCamera && joseMiguel == false)
        {
            _input.changeCamera = false;
            changeCamera = !changeCamera;
        }
        Camera.SetActive(changeCamera);

        //Detectar si se usa mando

        if (Gamepad.current != null)
        {
            mando = true;
        }
        else
        {
            mando = false;
        }
    }
    public IEnumerator ResetChangeStateAfterDelay(float delay)
    {
        StartTurn();
        MoveTurn();
        yield return new WaitForSeconds(delay);
        StopTurn();
    }
    private void StartTurn()
    {
        OnStartTurn?.Invoke();
    }
    private void MoveTurn()
    {
        changeCamera = true;
        joseMiguel = true;
        //PintorStartEfect();
        OnPassTurn?.Invoke();
        //DinoStartEfect();
    }
    private void StopTurn()
    {
        changeCamera = false;
        joseMiguel = false;
        //PintorStopEfect();
        OnStopTurn?.Invoke();
        //DinoStopEfect();
    }
    //private void PintorStartEfect()
    //{
    //    Animator APintor = g_Pintor.GetComponent<Animator>();
    //    if (g_pintorEfect == PintorEfect.Stun)
    //    {
    //        g_stunTimer++;
    //        if (g_stunTimer == 1)
    //        {
    //            APintor.GetComponent<Data_Pintor>().ChangePintorStunColor();
    //            APintor.SetTrigger("Stun");
    //        }
    //        if (g_stunTimer == 2)
    //        {
    //            g_pintorEfect = PintorEfect.Move;
    //            g_stunTimer = 0;
    //        }
    //    }
    //    else if (g_pintorEfect == PintorEfect.Move)
    //    {
    //        APintor.SetTrigger("Move");
    //    }
    //}
    //private void DinoStartEfect()
    //{
    //    Animator ADino = g_Dino.GetComponent<Animator>();
    //    if (g_dinoEfect == DinoEfect.Follow)
    //    {
    //        ADino.GetComponent<DataWolf>().ChangePintorOriginalColor();
    //        ADino.SetTrigger("Follow");
    //    }
    //    else if (g_dinoEfect == DinoEfect.Stun)
    //    {
    //        g_dinoEfect = DinoEfect.Howl;
    //        g_pintorEfect = PintorEfect.Stun;
    //        ADino.GetComponent<DataWolf>().ChangePintorStunColor();
    //        ADino.SetTrigger("Stun");
    //    }
    //    else if (g_dinoEfect == DinoEfect.Howl)
    //    {
    //        g_dinoEfect = DinoEfect.Angry;
    //        ADino.GetComponent<DataWolf>().ChangePintorOriginalColor();
    //        ADino.SetTrigger("Howl");
    //    }
    //    else if (g_dinoEfect == DinoEfect.Angry)
    //    {
    //        ADino.GetComponent<DataWolf>().ChangePintorAngryColor();
    //        g_dinoEfect = DinoEfect.Follow;
    //        ADino.SetTrigger("Angry");
    //    }
    //}
    //private void PintorStopEfect()
    //{
    //    Animator APintor = g_Pintor.GetComponent<Animator>();
    //    APintor.SetTrigger("Idle");
    //    if( g_pintorEfect == PintorEfect.Move)
    //    {
    //        APintor.GetComponent<Data_Pintor>().ChangePintorOriginalColor();
    //    }
    //}
    //private void DinoStopEfect()
    //{
    //    Animator ADino = g_Dino.GetComponent<Animator>();
    //    ADino.SetTrigger("Idle");
    //    //if (g_pintorEfect == PintorEfect.Move)
    //    //{
    //    //    ADino.GetComponent<Data_Pintor>().ChangePintorOriginalColor();
    //    //}
    //}
    public PintorEfect GetPintorEfect()
    {
        return g_pintorEfect;
    }
    public void SetPintorEfect(PintorEfect p_pintorEfect)
    {
        g_pintorEfect = p_pintorEfect;
    }
    public DinoEfect GetDinoEfect()
    {
        return g_dinoEfect;
    }
    public void SetDinoEfect(DinoEfect d_dinoEfect)
    {
        g_dinoEfect = d_dinoEfect;
    }
    public ConstructorEfect GetConstructorEfect()
    {
        return g_constructorEfect;
    }
    public void SetConstructorEfect(ConstructorEfect c_constructorEfect)
    {
        g_constructorEfect = c_constructorEfect;
    }
    public TurtleEfect GetTurtleEfect()
    {
        return g_turtleEfect;
    }
    public void SetTurtleEfect(TurtleEfect t_turtleEfect)
    {
        g_turtleEfect = t_turtleEfect;
    }
}
