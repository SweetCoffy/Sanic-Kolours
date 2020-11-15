using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Transform floorDetection;
    public float speed = 20;
    public float drag = 5;
    public Wisp currWisp; 
    public float distance = 0.07f;
    public LayerMask groundMask;
    public Image outlineThing;
    public Image wispIcon;
    public float maxBoost = 100;
    public float boost = 100;
    public RectTransform h;
    public Image wispBar;
    public Sprite placeholderIcon;
    public Transform fakeShadow;
    public float rotationSpeed = 10;
    bool isGrounded = true;
    float secondsSinceLastRaycast = 0;
    public float fovIntensity = 0.5f;
    Vector2 movement;
    bool jump = false;
    public TrailRenderer trail;
    public float minSpeed = 50;
    public float radius = 0.1f;
    private float t = 0;
    public float boostSpeed = 225;
    public float boostConsumeRate = 10;
    public float baseFov = 70;
    public Image ringThing;
    public float fovLimit = 150;
    public Transform graphics;
    public Image glowThing;
    Vector2 originalPos;
    Vector3 facing = Vector3.forward;
    bool didAirDash = false;
    public float jumpForce = 40;
    public Color defaultBarColor;
    public float airDashForce = 25;
    public bool inputEnabled = true;
    private int _rings = 0;
    Vector3 facingRelative = Vector3.zero;
    public int rings {
        get => _rings;
        set {
            _rings = value;
            t = 1;
            RingCounter.main.text = value.ToString("000");
        }
    }
    public float fovSpeed = 10;
    Vector2 lastMovement = Vector2.zero;
    public float maxTimeSinceLastRaycast = 0.15f;
    IEnumerator WispUpdate() {
        while (currWisp.timeLeft > 0) {
            currWisp.Update();
            glowThing.color += new Color(0, 0, 0, 1);
            yield return null;
        }
        currWisp.End();
    }
    void Start() {
        ChangeWisp(Wisps.main.rocket);
        originalPos = h.anchoredPosition;
    }
    public void ChangeWisp(Wisp wisp) {
        if (this.currWisp != null) if (this.currWisp.timeLeft > 0) return;
        
        this.currWisp = wisp.Clone();
    }
    void Update()
    {
        facing = ((transform.right * lastMovement.x) + (transform.forward * lastMovement.y)).normalized;
        facingRelative = (Vector3.right * lastMovement.x) + (Vector3.forward * lastMovement.y);
        Debug.DrawRay(transform.position, facing * 7);
        Debug.DrawRay(transform.position, facingRelative * 7, Color.gray);
        graphics.forward = Vector3.Slerp(graphics.forward, facing, rotationSpeed * Time.deltaTime);
        glowThing.fillAmount = wispBar.fillAmount;
        glowThing.color = new Color(wispBar.color.r, wispBar.color.g, wispBar.color.b, glowThing.color.a);
        if (isGrounded) didAirDash = false;
        if (currWisp == null) wispBar.fillAmount = boost / maxBoost;
        if (currWisp != null) {
            if (!currWisp.beingUsed) {
                wispIcon.sprite = currWisp.icon;
                wispBar.fillAmount = boost / maxBoost;
            }
            if (currWisp.beingUsed && currWisp.timeLeft <= 0) wispBar.fillAmount = boost / maxBoost;
        }
        if (inputEnabled) {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (movement.magnitude > 0.9f) lastMovement = movement;
            jump = Input.GetButtonDown("Jump");
            if (!isGrounded && jump && !didAirDash) {
                rb.AddForce(new Vector3(lastMovement.x, 0, lastMovement.y) * airDashForce, ForceMode.Impulse);
                didAirDash = true;
            }
            if (Input.GetButton("Boost")) {
                if (movement.magnitude != 0 && boost > 0) {
                    Vector3 hh = Vector3.zero;
                    outlineThing.color = Color.white;
                    Vector3 scaledVelocity = rb.velocity;
                    scaledVelocity.Scale(transform.up);
                    hh += transform.right * movement.x * boostSpeed; 
                    hh += transform.forward * movement.y * boostSpeed;
                    hh += scaledVelocity; 
                    rb.velocity = hh;
                    glowThing.color += new Color(0, 0, 0, 1);
                    h.anchoredPosition = originalPos + new Vector2(Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity), Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity));
                    boost -= Time.deltaTime * boostConsumeRate;
                } else {
                    h.anchoredPosition = originalPos;
                }
            } else {
                h.anchoredPosition = originalPos;
            }
            if (Input.GetButtonDown("Boost")) {
                Camera.main.fieldOfView = 25;
            }
            if (currWisp != null && Input.GetButtonDown("Wisp Power")) {
                if (!currWisp.beingUsed) {
                    currWisp.player = this;
                    currWisp.Start();
                    StartCoroutine(WispUpdate());
                }
            }
        } else {
            jump = false;
            movement = Vector2.zero;
        }
        RaycastHit hit;
        Collider[] hhhh = Physics.OverlapSphere(floorDetection.position, radius, groundMask);
        isGrounded = hhhh.Length > 0;
        bool didHit = Physics.Raycast(transform.position, -transform.up, out hit, distance, groundMask);
        rb.AddForce(movement.x * speed * Time.deltaTime * transform.right);
        rb.AddForce(movement.y * speed * Time.deltaTime * transform.forward);
        //Debug.Log(didHit);
        //Debug.Log(hhhh.Length > 0 && jump);
        if (didHit) {
            fakeShadow.gameObject.SetActive(true);
            Debug.DrawRay(transform.position, hit.point - transform.position, Color.green);
            //isGrounded = true;
            Quaternion h = Quaternion.FromToRotation(Vector3.up, hit.normal.normalized);
            rb.rotation = Quaternion.Lerp(rb.rotation, h, rotationSpeed * Time.deltaTime);
            fakeShadow.position = hit.point;
            fakeShadow.rotation = h;
            secondsSinceLastRaycast = 0;
        } else if (secondsSinceLastRaycast > maxTimeSinceLastRaycast){
            Debug.DrawRay(transform.position, -transform.up * distance, Color.red);
            //isGrounded = false;
            fakeShadow.gameObject.SetActive(false);
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }
        var target = rb.velocity;
        target.Scale(transform.up);
        rb.velocity = Vector3.Lerp(rb.velocity, target, drag * Time.deltaTime);
        secondsSinceLastRaycast += Time.deltaTime;
        if (jump && isGrounded) {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
        if (trail) {
            Vector3 veloc = rb.velocity;
            veloc.Scale(transform.right + transform.forward);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Clamp(baseFov + (veloc.magnitude * fovIntensity), 20, fovLimit), fovSpeed * Time.deltaTime);
            trail.emitting = veloc.magnitude > minSpeed;
        }
        if (glowThing.color.a > 1) glowThing.color = new Color(glowThing.color.r, glowThing.color.g, glowThing.color.b, 1);
        outlineThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        glowThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        if (ringThing) ringThing.material.SetColor("_Color", new Color(t, t, t, 0));
        if (t > 0) t -= Time.deltaTime * 3;
    } 
    void OnDrawGizmos() {
        if (!floorDetection) return;
        Gizmos.DrawWireSphere(floorDetection.position, radius);
    }
}
