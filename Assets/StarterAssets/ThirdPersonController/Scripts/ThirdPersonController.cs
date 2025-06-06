using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;
using static Unity.VisualScripting.Member;
using static UnityEngine.UI.Image;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */
public enum TypeShoot
{
    Default,
    ShootMoveObject,
}
namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        [SerializeField] private Animator _animator;
        private CharacterController _controller;
        [SerializeField] private MainPlayerInput _input;
        private GameObject _mainCamera;
        [SerializeField] private bool cameraMove;

        [SerializeField] private LineRenderer _laserLine;
        [SerializeField] private Transform _laserOrigin;
        [SerializeField] private Transform _laserTarget;
        public GameObject laserVFX;
        private Vector3 initialTransformLaser;


        [SerializeField] private float _turnSmoothVelocity;
        [SerializeField] private bool _getGameObject = false;
        private GameObject _copyOmogram;
        private GameObject _saveHitObject;
        private float _pickPlayerRotation;
        private float _pickObjectRotation;
        private float _pickPlusRotation;
        private ObjectCollisionCheck m_objectCollisionScript;
        [SerializeField] private Material m_hologram;
        [SerializeField] private LayerMask _RayLayer;

        private BoxCollider _boxCollider;

        [SerializeField] private float _copyOmogramDistance;
        [SerializeField] private float _maxCopyOmogramDistance = 10;
        [SerializeField] private float _minCopyOmogramDistance = 2;




        private const float _threshold = 0.01f;

        [SerializeField] private Coroutine m_NormalShootRoutine;
        [SerializeField] private Coroutine m_MovebleObjectShootRoutine;

        [SerializeField] private bool _hasAnimator;



        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }


        private void Start()
        {
            Cursor.visible = false;
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            if (gameObject.transform.childCount > 0)
            {
                Transform firstChild = gameObject.transform.GetChild(0);
                if (_animator != null)
                {
                    _hasAnimator = true;
                }
                else
                {
                    _hasAnimator = false;
                }
            }
            initialTransformLaser = laserVFX.transform.localScale;
            _controller = GetComponent<CharacterController>();
            //_input = GetComponent<StarterAssetsInputs>();
            _input = gameObject.GetComponent<MainPlayerInput>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            //shooting
            _input.OnBeginNormalShootEfect += BeginNormalShot;
            _input.OnEndNormalShootEfect += EndNormalShot;
            _input.OnBeginCheckMoveObjectEfect += EndMovebleObjectShot;
            _input.OnBeginCancelMoveObjectEfect += CancelMovebleObjectEfect;
            _input.OnPassTurnEfect += PassTurn;
        }

        //private void Shoot()
        //{
        //    if (!Grounded) return;
        //    if (_input.shoot == true && _getGameObject == false)
        //    {
        //        _laserLine.enabled = true;
        //        _targetRotation = _mainCamera.transform.eulerAngles.y;
        //        float rotation = _targetRotation;
        //        // rotate to face input direction relative to camera position
        //        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        //        _laserLine.SetPosition(0, _laserOrigin.position);
        //        Vector3 rayOrigin = _laserOrigin.transform.position;
        //        RaycastHit hit;
        //        if (Physics.Raycast(_laserOrigin.transform.position, _laserOrigin.transform.forward, out hit, _maxCopyOmogramDistance, _RayLayer))
        //        {
        //            _input.shootCancel = false;
        //            if (hit.collider.gameObject.TryGetComponent(out ObjectCollisionCheck objectCollisionCheck))
        //            {
        //                _typeShoot = TypeShoot.ShootMoveObject;
        //                _pickPlayerRotation = _mainCamera.transform.eulerAngles.y;
        //                _pickObjectRotation = hit.collider.transform.eulerAngles.y;
        //                _saveHitObject = hit.collider.gameObject;

        //                _boxCollider = hit.collider.gameObject.GetComponent<BoxCollider>();
        //                _boxCollider.enabled = false;

        //                _saveHitObject.GetComponent<Collider>().enabled = false;
        //                objectCollisionCheck.m_isCollision = false;
        //                _copyOmogram = Instantiate(hit.collider.gameObject, hit.collider.gameObject.transform.position, hit.collider.transform.rotation);
        //                _copyOmogram.GetComponent<Collider>().enabled = true;
        //                _copyOmogram.GetComponent<Collider>().isTrigger = true;
        //                _copyOmogram.GetComponent<NavMeshObstacle>().enabled = false;
        //                _copyOmogramDistance = Vector3.Distance(new Vector3(_laserOrigin.position.x, 0, _laserOrigin.position.z), new Vector3(_copyOmogram.transform.position.x, 0, _copyOmogram.transform.position.z));
        //                m_objectCollisionScript = _copyOmogram.GetComponent<ObjectCollisionCheck>();
        //                _getGameObject = true;
        //                return;
        //            }
        //            else if (hit.collider.gameObject.TryGetComponent(out DataTurtle dataTurtle))
        //            {
        //                dataTurtle.TurtlehitEfect();
        //            }
        //            else if (hit.collider.gameObject.TryGetComponent(out DataWolf dataWolf))
        //            {
        //                if (dataWolf.D_hadBeenStuned)
        //                { return; }
        //                dataWolf.D_hadBeenStuned = true;
        //                GameManager.Instance.ChangeDinoEfect(DinoEfect.Stun);
        //                GameManager.Instance.ChangeTurn();
        //            }
        //            else
        //            {
        //            }
        //            _laserLine.SetPosition(1, hit.point);
        //        }
        //        else
        //        {
        //            _laserLine.SetPosition(1, rayOrigin + (_laserOrigin.transform.forward * _maxCopyOmogramDistance));
        //        }

        //    }
        //    else if (_getGameObject == false)
        //    {
        //        _laserLine.enabled = false;
        //    }
        //    if (_getGameObject)
        //    {
        //        if (_input.stopShoot)
        //        {
        //            _laserLine.enabled = false;
        //            if (_typeShoot == TypeShoot.ShootMoveObject)
        //            {
        //                //if (m_objectCollisionScript == null)
        //                //{
        //                //    return;
        //                //}
        //                if (_copyOmogram != null && m_objectCollisionScript.m_isCollision == false)
        //                {
        //                    _boxCollider.enabled = true;
        //                    _saveHitObject.transform.position = _copyOmogram.transform.position;
        //                    _saveHitObject.transform.rotation = _copyOmogram.transform.rotation;
        //                    GameManager.OnStartTurn += _saveHitObject.GetComponent<Cooldown>().MoveableObjectChange;
        //                    GameManager.Instance.ChangeTurn();
        //                }
        //                else
        //                {

        //                }
        //                ShootStopEfect();
        //            }
        //        }
        //        else if (_input.shootCancel)
        //        {
        //            _laserLine.enabled = false;
        //            if (_typeShoot == TypeShoot.ShootMoveObject)
        //            {
        //                _input.shootCancel = false;
        //                ShootStopEfect();
        //            }
        //        }
        //    }

        //    if (_typeShoot == TypeShoot.ShootMoveObject)
        //    {
        //        _targetRotation = _mainCamera.transform.eulerAngles.y;
        //        float rotation = _targetRotation;
        //        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        //        _pickPlusRotation += _input.roteObject * Time.deltaTime * 60f;
        //        float rotateGrade = _mainCamera.transform.eulerAngles.y - _pickPlayerRotation + _pickPlusRotation;
        //        float moveGrade = _input.moveObject.y;
        //        _copyOmogramDistance -= (moveGrade * Time.deltaTime) * 10F;
        //        if (_copyOmogramDistance < _minCopyOmogramDistance)
        //        {
        //            _copyOmogramDistance = _minCopyOmogramDistance;
        //        }
        //        else if (_copyOmogramDistance > _maxCopyOmogramDistance)
        //        {
        //            _copyOmogramDistance = _maxCopyOmogramDistance;
        //        }
        //        Vector3 newPosition = new Vector3(_laserOrigin.transform.position.x, _saveHitObject.transform.position.y, _laserOrigin.transform.position.z) + _laserOrigin.transform.forward.normalized * _copyOmogramDistance;

        //        _copyOmogram.transform.position = newPosition;
        //        _copyOmogram.transform.rotation = Quaternion.Euler(0.0f, _pickObjectRotation + rotateGrade, 0.0f);

        //        _laserLine.SetPosition(1, newPosition);
        //    }
        //}

        private void Update()
        {
            if (gameObject.transform.childCount > 0)
            {
                Transform firstChild = gameObject.transform.GetChild(0);
            }

            if (GameManager.Instance.joseMiguel == false)
            {
                JumpAndGravity();
                GroundedCheck();
                //Shoot();
                Move();
            }
        }

        private void LateUpdate()
        {
            if (CanRotateCamera)
            {
                CameraRotation();
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                //_animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            Quaternion previousRotation = transform.rotation;
            if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                _cinemachineTargetYaw += _input.Look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        public TypeShoot _typeShoot;
        private GameObject shootObject;
        [SerializeField] private float shootTime = 2f;
        [SerializeField] private Renderer objectRenderer;
        private IEnumerator NormalShootEfect()
        {

            yield return new WaitForNextFrameUnit();
            if (GameManager.Instance.joseMiguel == true)
            {
                yield break;
            }

            _laserLine.enabled = true;
            laserVFX.SetActive(true);

            float shootObjectTime = 0;
            _animator.SetBool("OnShoot", true);
            Color initialColor = Color.white;
            Color targetColor = Color.blue;
            while (true)
            {
                if (_getGameObject == false)
                {
                    _targetRotation = _mainCamera.transform.eulerAngles.y;
                    float rotation = _targetRotation;
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                    _laserLine.SetPosition(0, _laserOrigin.position);
                    Vector3 rayOrigin = _laserOrigin.transform.position;

                    laserVFX.transform.localScale = new Vector3(initialTransformLaser.x, initialTransformLaser.y, _maxCopyOmogramDistance);
                    RaycastHit hit;
                    if (Physics.Raycast(_laserOrigin.transform.position, _laserOrigin.transform.forward, out hit, _maxCopyOmogramDistance, _RayLayer))
                    {
                        _input.ShootCancel = false;
                        if (hit.collider.gameObject.TryGetComponent(out ObjectCollisionCheck objectCollisionCheck))
                        {
                            _typeShoot = TypeShoot.ShootMoveObject;

                            _pickPlayerRotation = _mainCamera.transform.eulerAngles.y;
                            _pickObjectRotation = hit.collider.transform.eulerAngles.y;

                            _saveHitObject = hit.collider.gameObject;

                            _boxCollider = hit.collider.gameObject.GetComponent<BoxCollider>();
                            _boxCollider.enabled = false;

                            _saveHitObject.GetComponent<Collider>().enabled = false;
                            objectCollisionCheck.m_isCollision = false;

                            _copyOmogram = Instantiate(hit.collider.gameObject, hit.collider.gameObject.transform.position, hit.collider.transform.rotation);
                            _copyOmogram.GetComponent<Collider>().enabled = true;
                            _copyOmogram.GetComponent<Collider>().isTrigger = true;
                            _copyOmogram.GetComponent<NavMeshObstacle>().enabled = false;


                            _copyOmogramDistance = Vector3.Distance(new Vector3(_laserOrigin.position.x, 0, _laserOrigin.position.z), new Vector3(_copyOmogram.transform.position.x, 0, _copyOmogram.transform.position.z));
                            m_objectCollisionScript = _copyOmogram.GetComponent<ObjectCollisionCheck>();
                            if (m_objectCollisionScript.m_materialHologram == null)
                            {
                                m_objectCollisionScript.m_materialToHologram.material = m_hologram;
                            }
                            else
                            {
                                m_objectCollisionScript.m_materialToHologram.material = m_hologram;
                                m_objectCollisionScript.m_materialToHologram.material.color = objectCollisionCheck.m_materialHologram.color;
                            }
                            _getGameObject = true;


                            m_MovebleObjectShootRoutine = StartCoroutine(ShootMovebleObject());
                            EndNormalShot();
                            break;
                        }
                        else if (hit.collider.gameObject.TryGetComponent(out DataTurtle dataTurtle))
                        {
                            if (dataTurtle.T_CanShoot)
                            {
                                if (shootObject == null)
                                {
                                    shootObject = hit.collider.gameObject;
                                    objectRenderer = dataTurtle.D_objectRenderer;
                                }
                                if (shootObject == hit.collider.gameObject)
                                {
                                    shootObjectTime += Time.deltaTime;
                                    float t = Mathf.Clamp01(shootObjectTime / shootTime);
                                    objectRenderer.material.color = Color.Lerp(initialColor, targetColor, t);
                                }
                                else
                                {
                                    shootObjectTime = 0;
                                    float t = Mathf.Clamp01(shootObjectTime / shootTime);
                                    objectRenderer.material.color = Color.Lerp(initialColor, targetColor, t);
                                    shootObject = null;
                                }
                                if (shootObjectTime > shootTime)
                                {
                                    dataTurtle.T_CanShoot = false;
                                    dataTurtle.t_turtleEfect = TurtleEfect.Down;

                                    shootObjectTime = 0;
                                    objectRenderer.material.color = Color.white;
                                    shootObject = null;

                                    GameManager.Instance.ChangeTurn();
                                    EndNormalShot();

                                    shootObjectTime = 0;
                                    objectRenderer.material.color = Color.white;
                                    shootObject = null;
                                    break;
                                }
                            }
                        }
                        else if (hit.collider.gameObject.TryGetComponent(out DataWolf dataWolf))
                        {
                            if (!dataWolf.D_hadBeenStuned)
                            {
                                if (shootObject == null)
                                {
                                    shootObject = hit.collider.gameObject;
                                    objectRenderer = dataWolf.D_objectRenderer;
                                }
                                if (shootObject == hit.collider.gameObject)
                                {
                                    shootObjectTime += Time.deltaTime;
                                    float t = Mathf.Clamp01(shootObjectTime / shootTime);
                                    objectRenderer.material.color = Color.Lerp(initialColor, targetColor, t);
                                }
                                else
                                {
                                    shootObjectTime = 0;
                                    float t = Mathf.Clamp01(shootObjectTime / shootTime);
                                    objectRenderer.material.color = Color.Lerp(initialColor, targetColor, t);
                                    shootObject = null;
                                }
                                if (shootObjectTime > shootTime)
                                {
                                    dataWolf.D_hadBeenStuned = true;
                                    GameManager.Instance.SetDinoEfect(DinoEfect.Stun);


                                    GameManager.Instance.ChangeTurn();
                                    EndNormalShot();

                                    shootObjectTime = 0;
                                    objectRenderer.material.color = Color.white;
                                    shootObject = null;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (shootObject != null)
                            {
                                shootObjectTime = 0;
                                float t = Mathf.Clamp01(shootObjectTime / shootTime);
                                objectRenderer.material.color = Color.Lerp(initialColor, targetColor, t);
                                shootObject = null;
                            }
                        }
                        _laserLine.SetPosition(1, hit.point);
                        laserVFX.transform.localScale = new Vector3(initialTransformLaser.x, initialTransformLaser.y, Vector3.Distance(laserVFX.transform.position, hit.point));
                    }
                    else
                    {
                        if(shootObject != null)
                        {
                            shootObjectTime = 0;
                            objectRenderer.material.color = Color.white;
                            shootObject = null;
                        }
                        _laserLine.SetPosition(1, rayOrigin + (_laserOrigin.transform.forward * _maxCopyOmogramDistance));
                    }

                }
                else
                {
                    EndNormalShot();
                }
                yield return null;
            }
        }
        private void BeginNormalShot()
        {
            if (m_NormalShootRoutine != null)
            {
                return;
            }

            m_NormalShootRoutine = StartCoroutine(NormalShootEfect());
        }
        private void EndNormalShot()
        {
            if (m_NormalShootRoutine == null)
            {
                return;
            }
            StopCoroutine(m_NormalShootRoutine);
            if (_typeShoot == TypeShoot.Default)
            {
                if (shootObject != null)
                {
                    objectRenderer.material.color = Color.white;
                    shootObject = null;
                }
                    _laserLine.enabled = false;
                laserVFX.SetActive(false);
                _animator.SetBool("OnShoot", false);
            }
            else if (_typeShoot == TypeShoot.ShootMoveObject)
            {

            }
            m_NormalShootRoutine = null;
        }
        [SerializeField] private GameObject EmptyObject;
        [SerializeField] private LayerMask moveableColliderLayer;
        [SerializeField] private GameObject limitGround;
        private IEnumerator ShootMovebleObject()
        {
            bool isInstantiate = false;
            float signo = 0f;
            float CountTime = 0f;
            float Offset= 0.1f;
            CanRotateCamera = true;

            int layerMaskClosePoint = LayerMask.GetMask("MoveableObjectGround");
            ObjectCollisionCheck _saveHitObjectObjectCheck = _saveHitObject.GetComponent<ObjectCollisionCheck>();
            if (_saveHitObjectObjectCheck.m_limitGround != null) 
            {
                limitGround = _saveHitObjectObjectCheck.m_limitGround;

            }
            else
            {
                //if (Physics.Raycast(_copyOmogram.transform.position + Vector3.up * 2, Vector3.down, out RaycastHit hitGround, 10f, layerMaskClosePoint))
                //{
                //    limitGround = hitGround.collider.gameObject;
                //}
            }
            while (true)
            {
                if (_typeShoot == TypeShoot.ShootMoveObject)
                {
                    _targetRotation = _mainCamera.transform.eulerAngles.y;
                    float rotation = _targetRotation;
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                    _pickPlusRotation += _input.RoteObject * Time.deltaTime * 60f;
                    float rotateGrade = _mainCamera.transform.eulerAngles.y - _pickPlayerRotation + _pickPlusRotation;
                    float moveGrade = _input.MoveObject.y;

                    float _copyOmogramPreviusDistance = _copyOmogramDistance;
                    Vector3 _copyOmogramPreviusPosition = _copyOmogram.transform.position;

                    _copyOmogramDistance -= (moveGrade * Time.deltaTime) * 10F;
                    if (_copyOmogramDistance < _minCopyOmogramDistance)
                    {
                        _copyOmogramDistance = _minCopyOmogramDistance;
                    }
                    else if (_copyOmogramDistance > _maxCopyOmogramDistance)
                    {
                        _copyOmogramDistance = _maxCopyOmogramDistance;
                    }

                    Vector3 LaserStartPosition = new Vector3(_laserOrigin.transform.position.x, _saveHitObject.transform.position.y, _laserOrigin.transform.position.z);
                    Vector3 newPosition = LaserStartPosition + _laserOrigin.transform.forward.normalized * _copyOmogramDistance;

                    //GameObject emptyGameObject = Instantiate(EmptyObject, newPosition, Quaternion.Euler(0.0f, _pickObjectRotation + rotateGrade, 0.0f));
                    Collider newCollider = _copyOmogram.GetComponent<Collider>();
                    //if(newCollider is BoxCollider)
                    //{
                    //    BoxCollider boxCollider = (BoxCollider) newCollider;
                    //    emptyGameObject.AddComponent<BoxCollider>();
                    //    BoxCollider emptyCollider = emptyGameObject.GetComponent<BoxCollider>();

                    //    ((BoxCollider)emptyCollider).center = boxCollider.center;
                    //    ((BoxCollider)emptyCollider).size = boxCollider.size;
                    //}



                    bool isHitClosePoint = false;

                    RaycastHit[] hitsDown = Physics.RaycastAll(newPosition + Vector3.up * 2, Vector3.down, 10f, layerMaskClosePoint);

                    foreach (RaycastHit rayDown in hitsDown)
                    {

                        if (_saveHitObjectObjectCheck.m_limitGround != null)
                        {
                            if (rayDown.collider.gameObject == limitGround)
                            {
                                isHitClosePoint = true;
                                break;
                            }
                        }
                        else
                        {
                            limitGround = rayDown.collider.gameObject;
                            isHitClosePoint = true;
                            break;
                        }
                    }
                    //if (Physics.Raycast(newPosition + Vector3.up *2, Vector3.down, out RaycastHit hitGround, 10f, layerMaskClosePoint))
                    //{
                    //    _lastHitCollider = hitGround.collider;
                    //    isHitClosePoint = true;
                    //}
                    if (!isHitClosePoint)
                    {
                        Vector3 directionClosePoint = (gameObject.transform.position - newPosition).normalized;
                        float distanceClosePoint = 3f;
                        Vector3 HitPointForward = Vector3.zero;
                        Vector3 HitPointBehind = Vector3.zero;

                        RaycastHit[] hitsForward = Physics.RaycastAll(new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z), directionClosePoint, distanceClosePoint, layerMaskClosePoint);

                        foreach (RaycastHit hits in hitsForward)
                        {
                            if (hits.collider.gameObject == limitGround)
                            {
                                HitPointForward = new Vector3(hits.point.x, newPosition.y, hits.point.z);

                                Vector3 rayOrigin = new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z);
                                Vector3 rayDirection = directionClosePoint.normalized; // Asegúrate de que esté normalizado
                                float rayDistance = distanceClosePoint;

                                Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * rayDistance, Color.red);
                                break;
                            }
                        }
                        RaycastHit[] hitsBehind = Physics.RaycastAll(new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z), directionClosePoint * -1, distanceClosePoint, layerMaskClosePoint);

                        foreach (RaycastHit hits in hitsBehind)
                        {
                            if (hits.collider.gameObject == limitGround)
                            {
                                HitPointBehind = new Vector3(hits.point.x, newPosition.y, hits.point.z);

                                Vector3 rayOrigin = new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z);
                                Vector3 rayDirection = directionClosePoint.normalized;
                                float rayDistance = distanceClosePoint;

                                // Raycast hacia atrás (directionClosePoint * -1)
                                Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * -1f * rayDistance, Color.blue, 1f);
                                break;
                            }
                        }
                        //if (Physics.Raycast(new Vector3(newPosition.x, _lastHitCollider.transform.position.y, newPosition.z), directionClosePoint, out RaycastHit hitClosePointForward, distanceClosePoint, layerMaskClosePoint))
                        //{
                        //    if (hitClosePointForward.collider.gameObject == limitGround)
                        //    {


                        //        HitPointForward = new Vector3(hitClosePointForward.point.x, newPosition.y, hitClosePointForward.point.z);

                        //        Vector3 rayOrigin = new Vector3(newPosition.x, _lastHitCollider.transform.position.y, newPosition.z);
                        //        Vector3 rayDirection = directionClosePoint.normalized; // Asegúrate de que esté normalizado
                        //        float rayDistance = distanceClosePoint;

                        //        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * rayDistance, Color.red);
                        //    }
                        //}
                        //if (Physics.Raycast(new Vector3(newPosition.x, _lastHitCollider.transform.position.y, newPosition.z), directionClosePoint * -1, out RaycastHit hitClosePointBehind, distanceClosePoint, layerMaskClosePoint))
                        //{
                        //    if (hitClosePointBehind.collider.gameObject == limitGround)
                        //    {
                        //        HitPointBehind = new Vector3(hitClosePointBehind.point.x, newPosition.y, hitClosePointBehind.point.z);

                        //        Vector3 rayOrigin = new Vector3(newPosition.x, _lastHitCollider.transform.position.y, newPosition.z);
                        //        Vector3 rayDirection = directionClosePoint.normalized;
                        //        float rayDistance = distanceClosePoint;

                        //        // Raycast hacia atrás (directionClosePoint * -1)
                        //        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * -1f * rayDistance, Color.blue, 1f);
                        //    }
                        //}
                        //Quien distancia es menor
                        if (Vector3.Distance(newPosition, HitPointForward) < Vector3.Distance(newPosition, HitPointBehind))
                        {
                            CanRotateCamera = true;
                            newPosition = HitPointForward;
                            _copyOmogramDistance = Vector3.Distance(newPosition, new Vector3(gameObject.transform.position.x, newPosition.y, gameObject.transform.position.z));
                            Offset = 0.1f;
                        }
                        else if (Vector3.Distance(newPosition, HitPointBehind) < Vector3.Distance(newPosition, HitPointForward))
                        {
                            CanRotateCamera = true;
                            //if (Vector3.Distance(HitPointBehind, new Vector3(gameObject.transform.position.x, HitPointBehind.y, gameObject.transform.position.z)) > _maxCopyOmogramDistance && signo == 0)
                            //{
                            //        signo = Mathf.Sign(_input.Look.x);
                            //}
                            //if (signo == Mathf.Sign(_input.Look.x))
                            //{
                            //    if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
                            //    {
                            //        //Don't multiply mouse input by Time.deltaTime;
                            //        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                            //        _cinemachineTargetYaw -= _input.Look.x * deltaTimeMultiplier;
                            //        _cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier;
                            //    }

                            //    // clamp our rotations so our values are limited 360 degrees
                            //    _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                            //    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                            //    // Cinemachine will follow this target
                            //    CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                            //        _cinemachineTargetYaw, 0.0f);

                            //    CountTime = 0f;
                            //}
                            //else if (_input.Look.x < 0.2f && _input.Look.x > -0.2f)
                            //{
                            //    CountTime = 0f;
                            //}
                            //else
                            //{
                            //    CountTime += Time.deltaTime;
                            //    if (CountTime >= 0.1f)
                            //    {
                            //        signo = 0f;
                            //    }
                            //}
                            newPosition = HitPointBehind;
                            _copyOmogramDistance = Vector3.Distance(newPosition, new Vector3(gameObject.transform.position.x, newPosition.y, gameObject.transform.position.z));
                            Offset = 0.1f;
                        }
                        else
                        {
                            CanRotateCamera = false;
                            //if (signo == 0)
                            //{
                            //    signo = Mathf.Sign(_input.Look.x);
                            //}

                            //if (signo == Mathf.Sign(_input.Look.x))
                            //{
                            //    if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
                            //    {
                            //        //Don't multiply mouse input by Time.deltaTime;
                            //        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                            //        _cinemachineTargetYaw -= _input.Look.x * deltaTimeMultiplier;
                            //        _cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier;
                            //    }

                            //    // clamp our rotations so our values are limited 360 degrees
                            //    _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                            //    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                            //    // Cinemachine will follow this target
                            //    CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                            //        _cinemachineTargetYaw, 0.0f);

                            //    CountTime = 0f;
                            //}
                            //else if (_input.Look.x < 0.2f && _input.Look.x > -0.2f)
                            //{
                            //    CountTime = 0f;
                            //}
                            //else
                            //{
                            //    CountTime += Time.deltaTime;
                            //    if (CountTime >= 0.1f)
                            //    {
                            //        signo = 0f;
                            //    }
                            //}


                            //Vector3 right = Vector3.Cross(Vector3.up, directionClosePoint.normalized);
                            //Vector3 left = -right;

                            //Vector3 HitPointRight = Vector3.zero;
                            //Vector3 HitPointLeft = Vector3.zero;
                            //if (Physics.Raycast(new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z), right, out RaycastHit hitRightHit, distanceClosePoint, layerMaskClosePoint))
                            //{
                            //    HitPointRight = new Vector3(hitRightHit.point.x, newPosition.y, hitRightHit.point.z);

                            //    Vector3 rayOrigin = new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z);
                            //    Vector3 rayDirection = directionClosePoint.normalized; // Asegúrate de que esté normalizado
                            //    float rayDistance = distanceClosePoint;

                            //    Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * rayDistance, Color.red);
                            //}
                            //if (Physics.Raycast(new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z), left, out RaycastHit hitLeftHit, distanceClosePoint, layerMaskClosePoint))
                            //{
                            //    HitPointLeft = new Vector3(hitLeftHit.point.x, newPosition.y, hitLeftHit.point.z);

                            //    Vector3 rayOrigin = new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z);
                            //    Vector3 rayDirection = directionClosePoint.normalized;
                            //    float rayDistance = distanceClosePoint;

                            //    // Raycast hacia atrás (directionClosePoint * -1)
                            //    Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * -1f * rayDistance, Color.blue, 1f);
                            //}
                            //if (Vector3.Distance(newPosition, HitPointRight) < Vector3.Distance(newPosition, HitPointLeft))
                            //{
                            //    newPosition = HitPointRight;
                            //    _copyOmogramDistance = Vector3.Distance(newPosition, new Vector3(gameObject.transform.position.x, newPosition.y, gameObject.transform.position.z));
                            //}
                            //else if (Vector3.Distance(newPosition, HitPointLeft) < Vector3.Distance(newPosition, HitPointRight))
                            //{
                            //    newPosition = HitPointLeft;
                            //    _copyOmogramDistance = Vector3.Distance(newPosition, new Vector3(gameObject.transform.position.x, newPosition.y, gameObject.transform.position.z));
                            //}
                            Offset += 0.015f;
                            Vector3 DirectionPrevius = _copyOmogramPreviusPosition - newPosition;
                            // Normalizamos la dirección para que sea un vector unitario (no afecte la distancia del Raycast)
                            DirectionPrevius.Normalize();

                            // Lanza el Raycast
                            RaycastHit hitPreviusDirection;
                            Vector3 Origen = new Vector3(newPosition.x, limitGround.transform.position.y, newPosition.z);

                            if (Physics.Raycast(Origen, DirectionPrevius, out hitPreviusDirection, 10f, layerMaskClosePoint))
                            {
                                // Obtenemos el punto de impacto del Raycast
                                Vector3 impactPoint = hitPreviusDirection.point;

                                // Usamos la normal del hit para mover el impacto dentro del objeto
                                // Aseguramos que nos movemos en la dirección opuesta a la normal de la superficie para "enterrar" el impacto un poco
                                Vector3 adjustedImpactPoint = impactPoint - hitPreviusDirection.normal * Offset; // Ajuste muy pequeño para "entrar" en el objeto

                                // Actualizamos newPosition con el punto de impacto ajustado
                                newPosition = new Vector3(adjustedImpactPoint.x, _copyOmogram.transform.position.y, adjustedImpactPoint.z);
                            }

                            Debug.DrawRay(Origen, DirectionPrevius * 10f, Color.red);
                            CameraToPickObjectPosition(newPosition);


                        }
                    }
                    else
                    {
                        CanRotateCamera = true;
                        Offset = 0.1f;
                    }
                    //bool isColliding = Physics.CheckBox(newPosition, ((BoxCollider)newCollider).size / 1.95f, Quaternion.Euler(0.0f, _pickObjectRotation + rotateGrade, 0.0f), moveableColliderLayer);
                    Vector3 directionPlayer = (newPosition - gameObject.transform.position).normalized;
                    float distancePlayer = Vector3.Distance(newPosition, gameObject.transform.position);

                    if (/*!isColliding &&*/ !Physics.Raycast(gameObject.transform.position, directionPlayer, out RaycastHit hit, distancePlayer, moveableColliderLayer))
                    {
                        //Distnacia no atravesar objeto
                        _copyOmogram.transform.position = newPosition;
                            _copyOmogram.transform.rotation = Quaternion.Euler(0.0f, _pickObjectRotation + rotateGrade, 0.0f);

                            _laserLine.SetPosition(1, newPosition);
                            laserVFX.transform.localScale = new Vector3(initialTransformLaser.x, initialTransformLaser.y, _copyOmogramDistance);
                    }
                    else
                    {
                        if (isHitClosePoint)
                        {
                            newPosition = new Vector3(hit.point.x, newPosition.y, hit.point.z);
                            _copyOmogramDistance = Vector3.Distance(newPosition, new Vector3(gameObject.transform.position.x, newPosition.y, gameObject.transform.position.z));
                            _copyOmogram.transform.position = newPosition;
                            _laserLine.SetPosition(1, newPosition);
                            laserVFX.transform.localScale = new Vector3(initialTransformLaser.x, initialTransformLaser.y, _copyOmogramDistance);
                            _copyOmogram.transform.rotation = Quaternion.Euler(0.0f, _pickObjectRotation + rotateGrade, 0.0f);
                        }
                        else
                        {
                            _copyOmogram.transform.position = _copyOmogramPreviusPosition;
                            CameraToPickObjectPosition(_copyOmogramPreviusPosition);
                        }
                    }
                }
                yield return null;
            }
        }
        private void CameraToPickObjectPosition(Vector3 newPosition)
        {
            Vector3 direction = newPosition - gameObject.transform.position;
            direction.y = 0f; // Eliminar componente vertical para quedarnos solo con el ángulo en el plano horizontal

            _cinemachineTargetYaw = Quaternion.LookRotation(direction).eulerAngles.y;
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
            _copyOmogramDistance = Vector3.Distance(newPosition, new Vector3(gameObject.transform.position.x, newPosition.y, gameObject.transform.position.z));
        }
        private bool CanRotateCamera = true;
        private void EndMovebleObjectShot()
        {
            if (m_MovebleObjectShootRoutine == null)
            {
                return;
            }
            _animator.SetBool("OnShoot", false);
            StopCoroutine(m_MovebleObjectShootRoutine);
            CheckMovebleObjectEfect();
        }
        private void CheckMovebleObjectEfect()
        {
            _laserLine.enabled = false;
            laserVFX.SetActive(false);
            if (_copyOmogram != null && m_objectCollisionScript.m_isCollision == false)
            {
                _boxCollider.enabled = true;
                _saveHitObject.transform.position = _copyOmogram.transform.position;
                _saveHitObject.transform.rotation = _copyOmogram.transform.rotation;
                GameManager.OnStartTurn += _saveHitObject.GetComponent<Cooldown>().MoveableObjectChange;
                GameManager.Instance.ChangeTurn();
            }
            ShootStopEfect();
            _laserLine.enabled = false;
            laserVFX.SetActive(false);
            m_MovebleObjectShootRoutine = null;
        }
        private void CancelMovebleObjectEfect()
        {
            _laserLine.enabled = false;
            laserVFX.SetActive(false);
            ShootStopEfect();
        }
        //private IEnumerator Shoot()
        //{
        //    if (Grounded)
        //    {
        //        while (true)
        //        {
        //            yield return null;
        //            if (_input.shoot == true && _getGameObject == false)
        //            {
        //                _laserLine.enabled = true;
        //                _targetRotation = _mainCamera.transform.eulerAngles.y;
        //                float rotation = _targetRotation;
        //                // rotate to face input direction relative to camera position
        //                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        //                _laserLine.SetPosition(0, _laserOrigin.position);
        //                Vector3 rayOrigin = _laserOrigin.transform.position;
        //                RaycastHit hit;
        //                if (Physics.Raycast(_laserOrigin.transform.position, _laserOrigin.transform.forward, out hit, _maxCopyOmogramDistance, _RayLayer))
        //                {
        //                    _input.shootCancel = false;
        //                    if (hit.collider.gameObject.TryGetComponent(out ObjectCollisionCheck objectCollisionCheck))
        //                    {
        //                        _typeShoot = TypeShoot.ShootMoveObject;
        //                        _pickPlayerRotation = _mainCamera.transform.eulerAngles.y;
        //                        _pickObjectRotation = hit.collider.transform.eulerAngles.y;
        //                        _saveHitObject = hit.collider.gameObject;

        //                        _boxCollider = hit.collider.gameObject.GetComponent<BoxCollider>();
        //                        _boxCollider.enabled = false;

        //                        _saveHitObject.GetComponent<Collider>().enabled = false;
        //                        objectCollisionCheck.m_isCollision = false;
        //                        _copyOmogram = Instantiate(hit.collider.gameObject, hit.collider.gameObject.transform.position, hit.collider.transform.rotation);
        //                        _copyOmogram.GetComponent<Collider>().enabled = true;
        //                        _copyOmogram.GetComponent<Collider>().isTrigger = true;
        //                        _copyOmogram.GetComponent<NavMeshObstacle>().enabled = false;
        //                        _copyOmogramDistance = Vector3.Distance(new Vector3(_laserOrigin.position.x, 0, _laserOrigin.position.z), new Vector3(_copyOmogram.transform.position.x, 0, _copyOmogram.transform.position.z));
        //                        m_objectCollisionScript = _copyOmogram.GetComponent<ObjectCollisionCheck>();
        //                        _getGameObject = true;
        //                        continue;
        //                    }
        //                    else if (hit.collider.gameObject.TryGetComponent(out DataTurtle dataTurtle))
        //                    {
        //                        dataTurtle.TurtlehitEfect();
        //                    }
        //                    else if (hit.collider.gameObject.TryGetComponent(out DataWolf dataWolf))
        //                    {
        //                        if (dataWolf.D_hadBeenStuned)
        //                        { continue; }
        //                        dataWolf.D_hadBeenStuned = true;
        //                        GameManager.Instance.ChangeDinoEfect(DinoEfect.Stun);
        //                        GameManager.Instance.ChangeTurn();
        //                    }
        //                    else
        //                    {
        //                    }
        //                    _laserLine.SetPosition(1, hit.point);
        //                }
        //                else
        //                {
        //                    _laserLine.SetPosition(1, rayOrigin + (_laserOrigin.transform.forward * _maxCopyOmogramDistance));
        //                }

        //            }
        //            else if (_getGameObject == false)
        //            {
        //                _laserLine.enabled = false;
        //            }
        //            if (_getGameObject)
        //            {
        //                if (_input.stopShoot)
        //                {
        //                    _laserLine.enabled = false;
        //                    if (_typeShoot == TypeShoot.ShootMoveObject)
        //                    {
        //                        //if (m_objectCollisionScript == null)
        //                        //{
        //                        //    return;
        //                        //}
        //                        if (_copyOmogram != null && m_objectCollisionScript.m_isCollision == false)
        //                        {
        //                            _boxCollider.enabled = true;
        //                            _saveHitObject.transform.position = _copyOmogram.transform.position;
        //                            _saveHitObject.transform.rotation = _copyOmogram.transform.rotation;
        //                            GameManager.OnStartTurn += _saveHitObject.GetComponent<Cooldown>().MoveableObjectChange;
        //                            GameManager.Instance.ChangeTurn();
        //                        }
        //                        else
        //                        {

        //                        }
        //                        ShootStopEfect();
        //                    }
        //                }
        //                else if (_input.shootCancel)
        //                {
        //                    _laserLine.enabled = false;
        //                    if (_typeShoot == TypeShoot.ShootMoveObject)
        //                    {
        //                        _input.shootCancel = false;
        //                        ShootStopEfect();
        //                    }
        //                }
        //            }

        //            if (_typeShoot == TypeShoot.ShootMoveObject)
        //            {
        //                _targetRotation = _mainCamera.transform.eulerAngles.y;
        //                float rotation = _targetRotation;
        //                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        //                _pickPlusRotation += _input.roteObject * Time.deltaTime * 60f;
        //                float rotateGrade = _mainCamera.transform.eulerAngles.y - _pickPlayerRotation + _pickPlusRotation;
        //                float moveGrade = _input.moveObject.y;
        //                _copyOmogramDistance -= (moveGrade * Time.deltaTime) * 10F;
        //                if (_copyOmogramDistance < _minCopyOmogramDistance)
        //                {
        //                    _copyOmogramDistance = _minCopyOmogramDistance;
        //                }
        //                else if (_copyOmogramDistance > _maxCopyOmogramDistance)
        //                {
        //                    _copyOmogramDistance = _maxCopyOmogramDistance;
        //                }
        //                Vector3 newPosition = new Vector3(_laserOrigin.transform.position.x, _saveHitObject.transform.position.y, _laserOrigin.transform.position.z) + _laserOrigin.transform.forward.normalized * _copyOmogramDistance;

        //                _copyOmogram.transform.position = newPosition;
        //                _copyOmogram.transform.rotation = Quaternion.Euler(0.0f, _pickObjectRotation + rotateGrade, 0.0f);

        //                _laserLine.SetPosition(1, newPosition);
        //            }
        //        }
        //    }
        //}
        private void ShootStopEfect()
        {
            _animator.SetBool("OnShoot", false);
            _getGameObject = false;
            Destroy(_copyOmogram);
            _saveHitObject.GetComponent<Collider>().enabled = true;
            m_objectCollisionScript = null;
            _copyOmogram = null;
            _pickPlusRotation = 0;
            _typeShoot = TypeShoot.Default;
        }
        private void Move()
        {
            if ((_input.Shoot == true || _getGameObject))
            {
                return;
            }
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.Sprint ? SprintSpeed : MoveSpeed;
            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.Move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.AnalogMovement ? _input.Move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.Move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat("MoveSpeed", _input.Move.magnitude);
                //_animator.SetFloat(_animIDSpeed, _animationBlend);
                //_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
            //if (_input.Shoot == true || _getGameObject)
            //{
            //    _input.Jump = false;
            //    return;
            //}
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    //_animator.SetBool(_animIDJump, false);
                    //_animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        //_animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        //_animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.Jump = false;
            }

        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
        private void PassTurn()
        {
            GameManager.Instance.ChangeTurn();
        }
    }
}