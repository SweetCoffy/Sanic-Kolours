using UnityEngine;

public class CameraThing : MonoBehaviour
{
    public Rigidbody target;
    Player p;
    public static CameraThing main;
    public Vector3 offset;
    public float rotationSpeed = 2;
    public float surfaceAngleRotationSpeed = 7;
    public Vector3 rotationOffset;
    public float speed = 10;
    float shakeDuration = 0;
    float originalShakeDuration = 0;
    float shakeIntensity = 0;
    Vector3 targetPos;
    float curSpeed;
    public float rSpeed = 180;
    public float mouseSensibility = 25;
    public float resetSpeed = 10;
    float yRotation = 0;
    float defaultYRotation = 0;
    float xRotation = 0;
    public float minX = -60;
    public float maxX = 60;
    [Range(0, 1)]
    public float rotationMultiplier = 1;
    Vector3 currVeloc = Vector3.zero;
    public Vector3 intensity = Vector3.one;
    public Transform xAxis;
    public Transform yetAnotherAxis;
    public Transform yAxis;
    public bool fixedUpdate = false;
    public bool controllable = true;
    public bool shouldBeControllable = true;
    public bool shouldMove = true;
    public bool move = true;
    public bool updateDirections = true;
    public Vector3 right;
    public Vector3 forward;
    public bool shouldRotate = true;
    public float freecamSpeed = 10;
    public bool rotate = true;
    public bool freecam = false;
    public bool firstPerson = false;
    Vector3 m;
    Vector3 xPos;
    Vector3 yPos;
    Vector3 cPos;
    public bool strafe = false;
    void Start() {/**/
        main = this;
        yRotation = transform.localEulerAngles.y;
        p = target.GetComponent<Player>();
        xPos = xAxis.localPosition;
        yPos = yAxis.localPosition;
        cPos = Camera.main.transform.localPosition;
        m = intensity;
    }
    void UpdateCamera() {
        mouseSensibility = Game.mouseSensitivity;
        if (Input.GetMouseButtonDown(0)) {
            curSpeed = mouseSensibility;
            Cursor.lockState = CursorLockMode.Locked;
        } else if (Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape)) {
            if (!Input.GetKeyDown(KeyCode.Escape)) curSpeed = rSpeed;
            Cursor.lockState = CursorLockMode.None;
        }
        bool hhh = true;
        if (p) {
            if (p.TwoDMode && !p.dead && !firstPerson) {
                hhh = false;
                yRotation = Mathf.Lerp(yRotation, 0, resetSpeed * Time.deltaTime);
                xRotation = Mathf.Lerp(xRotation, -26.082f, resetSpeed * Time.deltaTime);
            }
        }
        if (firstPerson) {
            intensity = Vector3.zero;
        } else intensity = m;
        if (updateDirections) {
            forward = transform.forward;
            right = transform.right;
        }
        if (hhh && controllable && rotate) {
            yRotation += Input.GetAxis("Camera Horizontal") * Time.deltaTime * curSpeed;
            xRotation = Mathf.Clamp(xRotation + (Input.GetAxis("Camera Vertical") * Time.deltaTime * curSpeed), minX, maxX);
        }
        Vector3 g = new Vector3(0, yRotation, 0);
        currVeloc = Vector3.Lerp(currVeloc, target.velocity, speed * Time.deltaTime);
        Vector3 h = currVeloc;
        h.Scale(intensity);
        targetPos = target.position + h + offset;
        if (shakeDuration > 0) {
            shakeDuration -= Time.deltaTime;
            targetPos += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * (shakeIntensity * (shakeDuration / originalShakeDuration));
        }
        if (shakeDuration <= 0) {
            shakeDuration = 0;
            originalShakeDuration = 0;
            shakeIntensity = 0;
        }
        if (Input.GetKeyDown(KeyCode.J)) strafe = !strafe;
        if (rotate) {
            xAxis.localRotation = Quaternion.Euler(xRotation, 0, 0);
            Quaternion r = Quaternion.Euler(0, yRotation, 0);
            yetAnotherAxis.rotation = Quaternion.Lerp(yetAnotherAxis.localRotation, Quaternion.Lerp(Quaternion.identity, target.rotation, rotationMultiplier), surfaceAngleRotationSpeed * Time.deltaTime);
            Quaternion _r = Quaternion.Lerp(transform.rotation, r, rotationSpeed * Time.deltaTime);
            transform.localRotation = _r;
            if (yAxis) yAxis.localRotation = _r;
        }
        if (move && !freecam) {
            transform.position = targetPos;
            yetAnotherAxis.position = transform.position;
        } 
        if (freecam) {
            Vector3 mov = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (Input.GetButton("Jump")) {
                mov.y += 1;
            } else if (Input.GetButton("Boost")) {
                mov.y -= 1;
            }
            transform.position += ((Camera.main.transform.forward * mov.z) + (Camera.main.transform.right * mov.x) + (Vector3.up * mov.y)) * Time.deltaTime * freecamSpeed;
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            firstPerson = !firstPerson;
        }
        if (firstPerson) {
            Camera.main.transform.localPosition = Vector3.zero;
            yAxis.localPosition = Vector3.up * 1.5f;
            xAxis.localPosition = Vector3.zero;
            yetAnotherAxis.position = transform.position;
        } else {
            yAxis.localPosition = yPos;
            xAxis.localPosition = xPos;
            Camera.main.transform.localPosition = cPos;
        }
        if (p.dead && !firstPerson && !freecam) {
            Camera.main.transform.LookAt(p.transform);
        }
    }
    void FixedUpdate()
    {
        if(!fixedUpdate) return;
        UpdateCamera();
    }
    void Update() {
        if (fixedUpdate) return;
        UpdateCamera();
    }
    public void Shake(float duration, float intensity) {
        if (intensity < shakeIntensity) return;
        originalShakeDuration = duration;
        shakeDuration = duration;
        shakeIntensity = intensity;
    }
}
