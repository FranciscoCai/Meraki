 using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class wallMoveable : MonoBehaviour
{

    private Vector3 moveGizmoPosition;
    [SerializeField] private Transform moveTransform;
    [SerializeField] private Vector3 initialPosition;
    private Vector3 toMove;

    private bool isMove = false;
    private Coroutine moveToCorutine;
    private void OnEnable()
    {
        GameManager.OnPassTurn += changeTurnMove;
    }
    private void OnDisable()
    {
        GameManager.OnPassTurn -= changeTurnMove;
    }
    private void Start()
    {
        initialPosition = gameObject.transform.position;
        moveGizmoPosition = moveTransform.position;
        toMove = initialPosition;
    }
    public void wallEnterEfect()
    {
        //isMove = true;
        //if (moveToCorutine != null)
        //{
        //    StopCoroutine(moveToCorutine);
        //}
        toMove = moveGizmoPosition;
        //moveToCorutine = StartCoroutine(MoveTo(moveGizmoPosition));

    }
    public void wallExitEfect()
    {
        //isMove = false;
        //if (moveToCorutine != null)
        //{
        //    StopCoroutine(moveToCorutine);
        //}
        toMove = initialPosition;
        //moveToCorutine = StartCoroutine(MoveTo(initialPosition));
    }
    private void changeTurnMove()
    {
        StartCoroutine(MoveTo(toMove));
    }
    private IEnumerator MoveTo(Vector3 movePosition)
    {
        while (Vector3.Distance(gameObject.transform.position, movePosition) > 0.01f)
        {
            // Mueve el objeto hacia la posición objetivo
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePosition, 10f * Time.deltaTime);
            yield return null;
        }

        // Asegura que la posición final sea exactamente la deseada
        gameObject.transform.position = movePosition;

        // Llama al evento o acción al completar el movimiento (si se proporciona)
    }
}
