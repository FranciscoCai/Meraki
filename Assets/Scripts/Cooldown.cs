using UnityEngine;
using System.Collections;
using StarterAssets;

public class Cooldown : MonoBehaviour
{
    
    public GameObject colliderOne;
    public Color newColor = Color.red;
    public GameObject player;
    public bool bigObject = false;

    private ThirdPersonController thirdPersonController;


    private Vector3 previousPosition; //Para el cooldown
    private bool initialTurn;
    [SerializeField] private Color originalColor;
    [SerializeField] private Renderer objectRenderer;
    private int c_numberCooldown = 0;
    [SerializeField] private int c_maxCooldown;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        thirdPersonController = player.GetComponent<ThirdPersonController>();
        previousPosition = transform.position;
        originalColor = objectRenderer.material.color;
        colliderOne.SetActive(false);
    }

    private void Update()
    {
       
        //if (transform.position != previousPosition && bigObject == false)
        //{
        //    cooldown = cooldown + 1;
        //    previousPosition = transform.position;
        //    initialTurn = thirdPersonController._turn;
        //}
        //if (thirdPersonController._turn != initialTurn && bigObject == false)
        //{
        //    cooldown = 0;
        //    initialTurn = GameManager.Instance.turno;
        //}
        //if (transform.position != previousPosition && bigObject && cooldown == 0)
        //{
        //    cooldown = cooldown + 1;
        //    previousPosition = transform.position;
        //    initialTurn = thirdPersonController._turn;
        //}
        //if (thirdPersonController._turn != initialTurn && bigObject && cooldown == 1)
        //{
        //    cooldown = cooldown + 1;
        //    previousPosition = transform.position;
        //    initialTurn = thirdPersonController._turn;
        //}
        //if (thirdPersonController._turn != initialTurn && bigObject && cooldown == 2)
        //{
        //    cooldown = 0;
        //    initialTurn = GameManager.Instance.turno;
        //}
        //if (cooldown == 1 && bigObject == false)
        //{
        //    objectRenderer.material.color = newColor;
        //    colliderOne.SetActive(true);
        //}
        //if (cooldown == 1 && bigObject)
        //{
            
        //    objectRenderer.material.color = newColor;
        //    colliderOne.SetActive(true);
        //}
        //if (cooldown == 2 && bigObject)
        //{
           
        //    objectRenderer.material.color = newColor;
        //    colliderOne.SetActive(true);
        //}
        //if (cooldown == 0)
        //{
            
        //    objectRenderer.material.color = originalColor;
        //    colliderOne.SetActive(false);

        //}
    }

    public void MoveableObjectChange()
    {
        if(c_numberCooldown == 0)
        {
            objectRenderer.material.color = newColor;
            colliderOne.SetActive(true);
        }
        c_numberCooldown++;
        if (c_numberCooldown > c_maxCooldown) 
        {
            objectRenderer.material.color = originalColor;
            colliderOne.SetActive(false);
            GameManager.OnStartTurn -= MoveableObjectChange;
            c_numberCooldown = 0;
        }
    }
    private void OnDisable()
    {
        GameManager.OnStartTurn -= MoveableObjectChange;
    }
}
