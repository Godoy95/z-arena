using UnityEngine;

[ExecuteInEditMode]
public class CameraRig : MonoBehaviour
{
    public Transform target;
    public bool autoTargetPlayer;
    public LayerMask wallLayers;
    public GameObject crosshair;
    public GameObject canoDaArma;

    public enum Shoulder
    {
        Right, Left
    }
    public Shoulder shoulder;

    [System.Serializable]
    public class CameraSettings
    {
        [Header("-Positioning-")]
        public Vector3 camPositionOffsetLeft;
        public Vector3 camPositionOffsetRight;

        [Header("-Camera Options-")]
        public float mouseXSensitivity = 5.0f;
        public float mouseYSensitivity = -5.0f;
        public float minAngle = -30.0f;
        public float maxAngle = 70.0f;
        public float rotationSpeed = 5.0f;
        public float maxCheckDist = 0.1f;

        [Header("-Zoom-")]
        public float fieldOfView = 70.0f;
        public float zoomFieldOfView = 30.0f;
        public float zoomSpeed = 3.0f;

        [Header("-Visual Options-")]
        public float hideMeshWhenDistance = 0.5f;

    }
    [SerializeField]
    public CameraSettings cameraSettings;

    [System.Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Mouse X";
        public string horizontalAxis = "Mouse Y";
        public string aimButton = "Fire2";
        public string switchShoulderButton = "Fire4";
    }
    [SerializeField]
    InputSettings input;

    [System.Serializable]
    public class MovementSettings
    {
        public float movementLerpSpeed = 5.0f;
    }
    [SerializeField]
    MovementSettings movement;

    Transform pivot;
    Camera mainCamera;
    float newX = 0.0f;
    float newY = 0.0f;
    bool isAiming;



    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
        pivot = transform.GetChild(0);
        if(Application.isPlaying)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            
        }

        canoDaArma = GameObject.FindGameObjectWithTag("CanoDeArma");
        isAiming = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            if (Application.isPlaying)
            {
                if (Input.GetButton(input.aimButton))
                {
                    isAiming = true;
                }
                else
                {
                    isAiming = false;
                }
                Zoom(isAiming);
                //Zoom(Input.GetButton(input.aimButton));
                RotateCamera();
                CheckWall();
                CheckMeshRenderer();
                

                if (Input.GetButtonDown(input.switchShoulderButton))
                    SwitchShoulders();

            }
        }

        
    }

    private void LateUpdate()
    {
        if(!target)
        {
            TargetPlayer();
        }
        else
        {
            Vector3 targetPosition = target.position;
            Quaternion targetRotation = target.rotation;

            FollowTarget(targetPosition, targetRotation);
        }

    }

    //Deixa o player como alvo
    void TargetPlayer()
    {
        if(autoTargetPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Jogador");

            if(player)
            {
                Transform playerT = player.transform;

                target = playerT;
            }
            
        }
    }

    //Segue o alvo
    void FollowTarget(Vector3 targetPosition, Quaternion targetRotation)
    {
        if(!Application.isPlaying)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
        else
        {
            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movement.movementLerpSpeed);
            transform.position = newPos;
        }
    }

    //Rotaciona a Camera com Input
    void RotateCamera()
    {
        if (!pivot)
            return;

        if(isAiming)
        {
            newY = 0f;
        }
        else
        {
            newY += cameraSettings.mouseYSensitivity * Input.GetAxis(input.horizontalAxis);
        }

        newX += cameraSettings.mouseXSensitivity * Input.GetAxis(input.verticalAxis);

        Vector3 eulerAngleAxis = new Vector3();
        eulerAngleAxis.x = newY;
        eulerAngleAxis.y = newX;

        newX = Mathf.Repeat(newX, 360);
        newY = Mathf.Clamp(newY, cameraSettings.minAngle, cameraSettings.maxAngle);

        Quaternion newRotantion = Quaternion.Slerp(pivot.localRotation, Quaternion.Euler(eulerAngleAxis), Time.deltaTime * cameraSettings.rotationSpeed);

        pivot.localRotation = newRotantion;

    }

    //Checa se encontrou parede e desvia
    void CheckWall()
    {
        if (!pivot || !mainCamera)
            return;

        RaycastHit hit;

        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 pivotPos = pivot.position;

        Vector3 start = pivotPos;
        Vector3 dir = mainCamPos - pivotPos;

        float dist = Mathf.Abs(shoulder == Shoulder.Left ? cameraSettings.camPositionOffsetLeft.z : cameraSettings.camPositionOffsetRight.z);

        if (Physics.SphereCast(start, cameraSettings.maxCheckDist, dir, out hit, dist, wallLayers))
        {
            MoveCamUp(hit, pivotPos, dir, mainCamT);
        }
        else
        {
            switch(shoulder)
            {
                case Shoulder.Left:
                    PositionCamera(cameraSettings.camPositionOffsetLeft);
                    break;
                case Shoulder.Right:
                    PositionCamera(cameraSettings.camPositionOffsetRight);
                    break;

            }
        }
    }

    //Desvia a camera quando acerta uma parede
    void MoveCamUp(RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT)
    {
        float hitDist = hit.distance;
        Vector3 sphereCastCenter = pivotPos + (dir.normalized * hitDist);
        cameraT.position = sphereCastCenter;

    }

    //Posiciona a camera
    void PositionCamera(Vector3 cameraPos)
    {
        if (!mainCamera)
            return;

        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.localPosition;
        Vector3 newPos = Vector3.Lerp(mainCamPos, cameraPos, Time.deltaTime * movement.movementLerpSpeed);
        mainCamT.localPosition = newPos;
    }

    //Esconde o mesh quando a camera esta muito proxima
    void CheckMeshRenderer()
    {
        if (!mainCamera || !target)
            return;

        SkinnedMeshRenderer[] meshes = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 targetPos = target.position;
        float dist = Vector3.Distance(mainCamPos, targetPos + target.up);

        if(meshes.Length > 0)
        {
            for(int i = 0; i < meshes.Length; i++)
            {
                if (dist <= cameraSettings.hideMeshWhenDistance)
                    meshes[i].enabled = false;
                else
                    meshes[i].enabled = true;
            }
        }
    }

    //Aplica zoom quando mirar
    public void Zoom(bool isZooming)
    {
        if (!mainCamera)
            return;

        if(isZooming)
        {
            float newFieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.zoomFieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = newFieldOfView;
        }
        else
        {
            float originalFieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.fieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = originalFieldOfView;
        }
    }

    //Troca o ombro onde fica a camera
    public void SwitchShoulders()
    {
        switch(shoulder)
        {
            case Shoulder.Left:
                shoulder = Shoulder.Right;
                break;
            case Shoulder.Right:
                shoulder = Shoulder.Left;
                break;
        }
    }

    
}