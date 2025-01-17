using UnityEngine;

public class BaseTestingController : MonoBehaviour
{
    [SerializeField]
    Animator ShotgunAnimator;
    [Header("Animator Triggers")]
    [SerializeField] string firstShot = "firstShot";
    [SerializeField] string secondShot = "secondShot";
    [SerializeField] string doubleShot = "doubleShot";
    [SerializeField] string reload = "reloadHalf";
    [Space]
    [Header("Moving Controller")]
    [SerializeField] CharacterController characterController;
    [SerializeField] float MoveSpeed = 5f;
    bool isFirstShot = true;
    [Header("Mouse Sensivity")]
    [SerializeField]
    float mouseSensivity = 5f;
    const float mouseSensivityConst = 1f;
    Transform mainCamera;
    void Update()
    {
        ShootingInput();
        MovinInput();
        CameraInput(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
    }
    void ShootingInput()
    {
        //shooting input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isFirstShot)
            {
                ShotgunAnimator.SetTrigger(firstShot);
                isFirstShot = false;
            }
            else
            {
                ShotgunAnimator.SetTrigger(secondShot);
                isFirstShot = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (isFirstShot)
            {
                ShotgunAnimator.SetTrigger(doubleShot);
            }
            else
            {
                ShotgunAnimator.SetTrigger(secondShot);
                isFirstShot = true;
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (!isFirstShot)
            {
                isFirstShot = true;
                ShotgunAnimator.SetTrigger(reload);
            }
        }

    }
    void MovinInput()
    {
        Vector3 moving = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            moving.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moving.z = -1;
        }
        else moving.z = 0;
        if (Input.GetKey(KeyCode.A))
        {
            moving.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moving.x = 1;
        }
        else moving.x = 0;
        moving.Normalize();
        Vector3 finalMoving =
        transform.forward * moving.z +
        transform.right * moving.x;
        characterController.Move(finalMoving * Time.deltaTime * MoveSpeed);
    }
    void CameraInput(Vector2 lookVector)
    {
        //float x = Input.GetAxisRaw("Mouse X") * mouseSensivity * mouseSensivityConst;
        //characterController.transform.Rotate(Vector3.up, x);
        //
        //float vertical = Input.GetAxisRaw("Mouse Y") * mouseSensivity * mouseSensivityConst;
        //Debug.Log(vertical);
        //float currentY = mainCamera.transform.localEulerAngles.x - vertical;
        //mainCamera.transform.localEulerAngles = new Vector3(currentY, 0f, 0f);
        //float currentVertical = mainCamera.transform.localRotation.x - vertical;
        //mainCamera.transform.localRotation = Quaternion.Euler(currentVertical, 0f, 0f);

        var lookHor = transform.rotation.eulerAngles.y;
        var lookVert = mainCamera.transform.localEulerAngles.x;
        lookVert = lookVert > 180f ? lookVert - 360f : lookVert;

        lookHor += lookVector.x * mouseSensivity * mouseSensivityConst;
        lookVert += lookVector.y * mouseSensivity * mouseSensivityConst * -1f;
        lookVert = Mathf.Clamp(lookVert, -90, 90);

        transform.rotation = Quaternion.Euler(0, lookHor, 0);
        mainCamera.transform.localRotation = Quaternion.Euler(lookVert, 0, 0);
    }
    private void FixedUpdate()
    {

    }
    private void Start()
    {
        isFirstShot = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera = Camera.main.transform;
    }
}
