using UnityEngine;
using UnityEngine.VFX;

public class LaserMouseControl : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("L�ser VFX")]
    public VisualEffect laserVFX;
    public string scaleParam = "AlphaClip";
    public float fadeSpeed = 2f;
    public float maxScale = 0.6f;
    private float currentScale = 0f;
    private bool laserOn = false;

    [Header("C�mara")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0f, 5f, -7f);
    public float smoothSpeed = 5f;

    [Header("Interacci�n")]
    public float rayDistance = 10f;
    public LayerMask interactableLayer;
    private Rigidbody grabbedRb;
    public float pullForce = 20f;
    public Transform holdPoint; // punto delante del jugador donde se mantendr� el objeto

    void Start()
    {
        if (laserVFX != null)
        {
            laserVFX.enabled = true;
            laserVFX.SetFloat(scaleParam, 0f);
        }

        // Crea un punto donde "sujetar" objetos si no hay uno
        if (holdPoint == null)
        {
            GameObject holder = new GameObject("HoldPoint");
            holder.transform.parent = transform;
            holder.transform.localPosition = new Vector3(0f, 1f, 2f);
            holdPoint = holder.transform;
        }
    }

    void Update()
    {
        // Movimiento b�sico
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // Rat�n: activar o desactivar l�ser
        if (Input.GetMouseButtonDown(0))
        {
            laserOn = true;
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            laserOn = false;
            DropObject();
        }

        float targetScale = laserOn ? maxScale : 0f;
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * fadeSpeed);

        if (laserVFX != null)
        {
            laserVFX.SetFloat(scaleParam, currentScale);
        }

        // Si tenemos un objeto agarrado, lo movemos al punto de sujeci�n
        if (grabbedRb != null)
        {
            Vector3 directionToHold = holdPoint.position - grabbedRb.position;
            grabbedRb.linearVelocity = directionToHold * pullForce;
        }
    }

    void TryGrabObject()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.linearDamping = 10f;
                    grabbedRb = rb;
                }
            }
        }
    }

    void DropObject()
    {
        if (grabbedRb != null)
        {
            grabbedRb.useGravity = true;
            grabbedRb.linearDamping = 0f;
            grabbedRb = null;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            Vector3 desiredPosition = transform.position + cameraOffset;
            Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            cameraTransform.position = smoothedPosition;
            cameraTransform.LookAt(transform);
        }
    }
}