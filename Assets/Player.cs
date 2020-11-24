using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [Header("Totally necessary stuff")]
        public Rigidbody rb;
        public Transform floorDetection;
    [Header("Movement")]
        public float speed = 20;
        public float drag = 5;
        public float jumpForce = 40;
        public float minSpeed = 50;
        public float rotationSpeed = 10;
        public float stompVelocity = 75;
        public float airDashForce = 25;
        public float multiplier = 3;
        public float maxSpeed = 75;
        public float Speed {
            get {
                if (CurrSpeed > minSpeed) return speed * multiplier;
                return speed;
            }
        }
    [Header("Surface Detection")]
        public float maxTimeSinceLastRaycast = 0.15f;
        public float distance = 0.07f;
        public float radius = 0.1f;
        public LayerMask groundMask;
    [Header("UI")]
        public Image outlineThing;
        public Image wispIcon;
        public RectTransform h;
        public Image speedBar;
        public float barPercent = 1;
        public Image wispBar;
        public Sprite wispBarIdle;
        public Sprite wispBarActive;
        public RectTransform wispBarRect;
        public Image glowThing;
        public RectTransform iconHolder;
        public Image iconHolderImage;
        public RectTransform boostThing;
        public Color defaultBarColor;
        public Color barColor;
        public Image ringThing;
        public Sprite placeholderIcon;
        public float CurrSpeed {
            get {
                Vector3 veloc = rb.velocity;
                veloc.Scale(transform.right + transform.forward);
                return veloc.magnitude;
            }
        }
        public float RealCurrSpeed {
            get {
                Vector3 veloc = rb.velocity;
                veloc.Scale(transform.right + transform.forward + transform.up);
                return veloc.magnitude;
            }
        }
    [Header("Gotta go fast")]
        public float maxBoost = 100;
        public float boost = 100;
        public float boostSpeed = 225;
        public float maxBoostSpeed = 200;
        public bool boostEnabled = true;
        public float boostConsumeRate = 10;
        public bool isGrounded {get; protected set;} = true;
        float secondsSinceLastRaycast = 0;
        Vector2 movement;
        bool jump = false;
        private float t = 0;
    [Header("Fancy effects")]
        public float fovIntensity = 0.5f;
        public float fovSpeed = 10;
        public Renderer boostEffect;
        public Transform boostEffectTransform;
        public Transform fakeShadow;
        public TrailRenderer trail;
        public float baseFov = 70;
        public float fovLimit = 150;
    [Header("Other")]
        public Transform graphics;
        public bool inputEnabled = true;
        public float homingAttackRadius = 10;
        Vector2 originalPos;
        Vector3 facing = Vector3.forward;
        public Transform target;
        bool didAirDash = false;
        public Transform homingAttackTarget;
        public Wisp currWisp; 
        public LayerMask homingAttackMask;
        public float twoDModeZ = 0;
        public bool TwoDMode = false;
        private int _rings = 0;
        public bool stomp = false;
        public float homingAttackForce = 50;
        private float fallTime = 0;
        public bool doingHomingAttack = false;
        public bool DestroyEnemies {
            get {
                return doingHomingAttack || stomp;
            }
        }
        public float MaxSpeed {
            get {
                if (isBoosting) return maxBoostSpeed;
                return maxSpeed;
            }
        }
        Vector3 facingRelative = Vector3.zero;
        public int rings {
            get => _rings;
            set {
                _rings = value;
                t = 0.5f;
                RingCounter.main.text = value.ToString("000");
            }
        }
        bool isBoosting = false;
        Vector2 lastMovement = Vector2.zero;
    void UpdateHomingAttackTarget() {
        float minDist = float.PositiveInfinity;
        Collider currTarget = null;
        Collider[] targets = Physics.OverlapSphere(floorDetection.position, homingAttackRadius, homingAttackMask, QueryTriggerInteraction.Collide);
        foreach( Collider target in targets ) {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < minDist) {
                minDist = dist;
                currTarget = target;
            }
        }
        if (!currTarget) {
            homingAttackTarget = null;
            return;
        }
        homingAttackTarget = currTarget.transform;
    }
    IEnumerator WispUpdate() {
        CameraThing.main.Shake(0.1f, 0.4f);
        //wispIcon.color = new Color(1, 1, 1, 1);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
        while (currWisp.timeLeft > 0) {
            currWisp.Update();
            glowThing.color += new Color(0, 0, 0, 1);
            yield return null;
        }
        //wispIcon.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
        currWisp.End();
    }
    void Start() {
        currWisp = null;
        originalPos = h.anchoredPosition;
        barColor = defaultBarColor;
    }
    public void ChangeWisp(Wisp wisp) {
        if (this.currWisp != null) if (this.currWisp.timeLeft > 0) return;
        
        this.currWisp = wisp.Clone();
    }
    void FixedUpdate() {
        if (!isGrounded && rb.velocity.y < 0) fallTime += Time.deltaTime;
        if (isGrounded) fallTime = 0;
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -MaxSpeed, MaxSpeed));
        if (TwoDMode) {
            Vector3 veloc = rb.velocity;
            veloc.Scale(Vector3.right + Vector3.up);
            rb.velocity = veloc;
            if (twoDModeZ != 0) rb.position = Vector3.Lerp(rb.position, new Vector3(rb.position.x, rb.position.y, twoDModeZ), 7 * Time.deltaTime);
        }
        RaycastHit hit;
        Collider[] hhhh = Physics.OverlapSphere(floorDetection.position, radius, groundMask);
        isGrounded = hhhh.Length > 0;
        bool didHit = Physics.Raycast(transform.position, -transform.up, out hit, distance, groundMask);
        rb.AddForce(movement.x * Speed * Time.deltaTime * transform.right);
        rb.AddForce(movement.y * Speed * Time.deltaTime * transform.forward);
        //
        //
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
        if (isGrounded && stomp) {
            stomp = false;
            //inputEnabled = true;
            CameraThing.main.Shake(0.2f, (0.2f / 50) * (stompVelocity + fallTime));
            rb.velocity = (transform.up * stompVelocity * (1 + fallTime) / 1.75f) + (rb.velocity / 2);
            fallTime = 0;
        }
    }
    void Update()
    {
        target.gameObject.SetActive(homingAttackTarget && !isGrounded);
        if (homingAttackTarget && !isGrounded) {
            target.position = homingAttackTarget.position;
        }
        UpdateHomingAttackTarget();
        isBoosting = false;
        boostEffectTransform.LookAt(transform.position + rb.velocity);
        wispBar.color = Color.Lerp(wispBar.color, barColor, 10 * Time.deltaTime);
        float barTarget = barPercent;
        if (float.IsNaN(barTarget)) barTarget = 0;
        wispBar.fillAmount = Mathf.Lerp(wispBar.fillAmount, barTarget, 10 * Time.deltaTime);
        speedBar.fillAmount = CurrSpeed / maxSpeed;
        boostThing.localPosition = (Vector3)new Vector2(wispBar.fillAmount * wispBarRect.sizeDelta.x, 0);
        //if (TwoDMode) rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        //else rb.constraints = RigidbodyConstraints.FreezeRotation;
        facing = ((transform.right * lastMovement.x) + (transform.forward * lastMovement.y)).normalized;
        facingRelative = (Vector3.right * lastMovement.x) + (Vector3.forward * lastMovement.y);
        Debug.DrawRay(transform.position, facing * 7);
        Debug.DrawRay(transform.position, facingRelative * 7, Color.gray);
        //
        if (facingRelative != -Vector3.forward) graphics.localRotation = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, facingRelative), Vector3.Cross(Vector3.forward, facingRelative));
        else graphics.localRotation = Quaternion.Euler(0, -180, 0);
        h.gameObject.SetActive(boost > 0 && boostEnabled || (currWisp != null && currWisp.timeLeft > 0));
        glowThing.fillAmount = wispBar.fillAmount;
        glowThing.color = new Color(wispBar.color.r, wispBar.color.g, wispBar.color.b, glowThing.color.a);
        if (isGrounded) didAirDash = false;
        if (currWisp == null) barPercent = boost / maxBoost;
        if (currWisp != null) {
            if (!currWisp.beingUsed) {
                wispIcon.sprite = currWisp.icon;
                //wispIcon.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
                barPercent = boost / maxBoost;
                iconHolderImage.sprite = wispBarActive;
            }
            if (currWisp.beingUsed && currWisp.timeLeft <= 0) {
                barPercent = boost / maxBoost;
                 iconHolderImage.sprite = wispBarIdle;
            }
        } else {
            iconHolderImage.sprite = wispBarIdle;
        }
        if (inputEnabled) {
            if (Input.GetButtonDown("Stomp") && !isGrounded && !stomp) {
                //inputEnabled = false;
                rb.velocity = -transform.up * stompVelocity + (rb.velocity / 2);
                stomp = true;
            }
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (movement.magnitude > 0.1f) lastMovement = movement.normalized;
            jump = Input.GetButtonDown("Jump");
            if (!homingAttackTarget) doingHomingAttack = false;
            
            if (!isGrounded && jump) {
                
                if (homingAttackTarget && !doingHomingAttack) {
                    doingHomingAttack = true;
                    Vector3 dir = homingAttackTarget.position - transform.position;
                    dir.Normalize();
                    Vector3 veloc = dir * homingAttackForce;
                    rb.velocity = veloc;
                } else if (!didAirDash) {
                    rb.AddForce(new Vector3(lastMovement.x, 0, lastMovement.y) * airDashForce, ForceMode.Impulse);
                    didAirDash = true;
                }    
            } 
            if (isGrounded) doingHomingAttack = false;
            
            if (Input.GetButton("Boost")) {
                if (lastMovement.magnitude != 0 && boost > 0) {
                    Vector3 hh = Vector3.zero;
                    isBoosting = true;
                    outlineThing.color = Color.white;
                    Vector3 scaledVelocity = rb.velocity;
                    scaledVelocity.Scale(transform.right + transform.forward);
                    rb.AddForce(((transform.right * lastMovement.x * boostSpeed) + (transform.forward * lastMovement.y * boostSpeed)) - scaledVelocity, ForceMode.Acceleration);
                    boostThing.sizeDelta = new Vector2(3, 25);
                    glowThing.color += new Color(0, 0, 0, 1);
                    h.anchoredPosition = originalPos + (new Vector2(Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity), Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity)) / 4);
                    boost -= Time.deltaTime * boostConsumeRate;
                } else {
                    h.anchoredPosition = originalPos;
                }
            } else {
                h.anchoredPosition = originalPos;
            }
            if (Input.GetButtonDown("Boost")) {
                Camera.main.fieldOfView = 25;
                boost = Mathf.Clamp(boost - 12.5f, 0, maxBoost);
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
        if (jump && isGrounded) {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
        if (trail) {
            trail.emitting = RealCurrSpeed > minSpeed;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Clamp(baseFov + (CurrSpeed * fovIntensity), 20, fovLimit), fovSpeed * Time.deltaTime);
        if (glowThing.color.a > 1) glowThing.color = new Color(glowThing.color.r, glowThing.color.g, glowThing.color.b, 1);
        outlineThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        boostThing.sizeDelta *= new Vector2(1, 0.9f);
        glowThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        if (ringThing) ringThing.material.SetColor("_Color", new Color(t, t, t, 0));
        if (t > 0) t -= Time.deltaTime * 3;
        boostEffect.gameObject.SetActive(isBoosting);
    } 
    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, homingAttackRadius);
        if (!floorDetection) return;
        Gizmos.DrawWireSphere(floorDetection.position, radius);
    }
}
