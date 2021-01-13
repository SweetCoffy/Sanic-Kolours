using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [Header("Totally necessary stuff")]
        public Rigidbody rb;
        public Transform floorDetection;
        public Animator anim;
    [Header("Movement")]
        public float speed = 20;
        public float drag = 5;
        public float jumpForce = 40;
        public float doubleJumpForce = 1;
        public float minSpeed = 50;
        public float rotationSpeed = 10;
        public float stompVelocity = 75;
        public float bounceMax = 15;
        public float bounceMultiplier = 3;
        public float homingAttackTimeout = 3;
        float homingAttackTime = 0;
        public float airDashForce = 25;
        public bool spinning = false;
        public float lightDashSpeed = 50;
        public bool isSuper = false;
        public float superSpeedMultiplier = 1.25f;
        public float superMaxSpeedMultiplier = 1.5f;
        public float superBoostSpeedMultiplier = 2;
        public float multiplier = 3;
        public float maxSpeed = 75;
        public float Speed {
            get {
                float m = 1;
                if (isSuper && !isBoosting) m = superSpeedMultiplier;
                if (CurrSpeed > minSpeed && isGrounded) return speed * multiplier * m;
                return speed * m;
            }
        }
    [Header("Surface Detection")]
        public float maxTimeSinceLastRaycast = 0.15f;
        public float distance = 0.07f;
        public float radius = 0.1f;
        public LayerMask groundMask;
    [Header("UI")]
        public float speedBarMax = 100;
        public Image outlineThing;
        public Image wispIcon;
        public RectTransform h;
        private Image hImage;
        public Image speedBar;
        public float barPercent = 1;
        public Image wispBar;
        public Image boostGauge2;
        public Image boostGauge3;
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
        public float boost {
            get {
                if (isSuper) return maxBoost;
                return _boost;
            }
            set {
                if (_boost - value < 0 && !isSuper) wispBar.color = Color.white;
                _boost = value;
            }
        }
        float _boost = 0;
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
        public ParticleSystem runningParticles;
        public TrailRenderer trail;
        public float baseFov = 70;
        public float fovLimit = 150;
    [Header("Other")]
        public bool wispsEnabled = true;
        public Transform graphics;
        public bool inputEnabled = true;
        public float homingAttackRadius = 10;
        Vector2 originalPos;
        Vector3 facing = Vector3.forward;
        public bool destroyEnemies;
        public Transform target;
        public HomingAttackTarget target1;
        bool didAirDash = false;
        private float scaleMultiplier = 0;
        public Transform lastCheckpoint;
        public Transform homingAttackTarget {
            get {
                return _target;
            }
            set {
                if (_target == value) return;
                scaleMultiplier = 0.5f;
                _target = value;
            }
        }
        private Transform _target;
        public Wisp currWisp; 
        public LayerMask homingAttackMask;
        public LayerMask lightSpeedDashMask;
        public float twoDModeZ = 0;
        public bool TwoDMode = false;
        public Transform camHolder;
        private int _rings = 0;
        public bool stomp = false;
        public float lightSpeedDashRange = 4;
        public float homingAttackForce = 50;
        private float fillAmount = 0;
        float superStartRings = 0;
        public Color superDefaultBarColor = Color.yellow;
        private float fallTime = 0;
        private bool didDoubleJump = false;
        public float Drag {
            get {
                if (movement.magnitude > 0) return drag * 0.3f;
                if (spinning) return 0;
                else return drag;
            }
        }
        public bool doingHomingAttack = false;
        public bool BounceOffEnemies {
            get {
                return doingHomingAttack || spinning;
            }
        }
        public bool DestroyEnemies {
            get {
                return doingHomingAttack || stomp || isBoosting || destroyEnemies || spinning || isSuper;
            }
        }
        public float MaxSpeed {
            get {
                float m = 1;
                if (isSuper) m = superMaxSpeedMultiplier;
                if (isBoosting) return maxBoostSpeed * m;
                return maxSpeed * m;
            }
        }
        public Transform nearestRing;
        Vector3 facingRelative = Vector3.zero;
        public int rings {
            get => _rings;
            set {
                _rings = value;
                t = 0.8f;
                if (isSuper) wispBar.color = Color.white;
                if (value == 0) RingCounter.main.color = Color.red;
                else RingCounter.main.color = Color.white;
                RingCounter.main.text = value.ToString("000");
            }
        }
        bool doingLightSpeedDash = false;
        bool isBoosting = false;
        float ringCooldown = 1;
        Vector2 lastMovement = Vector2.zero;
    void UpdateHomingAttackTarget() {
        if (doingHomingAttack) return;
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
            target1 = null;
            return;
        }
        if (currTarget) target1 = new HomingAttackTarget(currTarget.transform, currTarget);
        homingAttackTarget = currTarget.transform;
    }
    void UpdateNearestRing() {
        float minDist = float.PositiveInfinity;
        Collider currTarget = null;
        Collider[] targets = Physics.OverlapSphere(rb.position, lightSpeedDashRange, lightSpeedDashMask, QueryTriggerInteraction.Collide);
        foreach( Collider target in targets ) {
            float dist = Vector3.Distance(rb.position, target.transform.position);
            if (dist < minDist) {
                minDist = dist;
                currTarget = target;
            }
        }
        if (!currTarget) {
            nearestRing = null;
            return;
        }
        nearestRing = currTarget.transform;
    }
    IEnumerator WispUpdate() {
        CameraThing.main.Shake(0.3f, 0.5f);
        //wispIcon.color = new Color(1, 1, 1, 1);
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        while (currWisp.timeLeft > 0) {
            currWisp.Update();
            glowThing.color += new Color(0, 0, 0, 1);
            yield return null;
        }
        //wispIcon.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
        currWisp.End();
    }
    IEnumerator OnSuper() {
        CameraThing.main.Shake(0.3f, 0.5f);
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
    }
    void Start() {
        currWisp = null;
        originalPos = h.anchoredPosition;
        barColor = defaultBarColor;
        hImage = h.GetComponent<Image>();
        ConstantForce c = GetComponent<ConstantForce>();
        c.relativeForce *= ZoneInfo.current.gravityModifier;
        c.force *= ZoneInfo.current.gravityModifier;
    }
    public void ChangeWisp(Wisp wisp) {

        if (this.currWisp != null) {
            if (wisp.barColor.Equals(this.currWisp.barColor) && this.currWisp.beingUsed && this.currWisp.timeLeft > 0) {
                currWisp.timeLeft = currWisp.duration;
                return;
            }
            if (this.currWisp.timeLeft > 0) return;
        }
        
        this.currWisp = wisp.Clone();
    }
    void FixedUpdate() {
        Vector3 scaled = rb.velocity;
        scaled.Scale(transform.up);
        if (isGrounded) fallTime = 0;
        if (!isGrounded && rb.velocity.y < 0) fallTime -= (scaled.x + scaled.y + scaled.z) * Time.deltaTime;
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
        if (!(spinning && isGrounded)) {
            rb.AddRelativeForce(movement.x * Speed * Time.deltaTime * camHolder.right);
            rb.AddRelativeForce(movement.y * Speed * Time.deltaTime * camHolder.forward);
        } 
        if (didHit) {
            Debug.DrawRay(transform.position, hit.point - transform.position, Color.green);
            Quaternion h = Quaternion.FromToRotation(Vector3.up, hit.normal.normalized).normalized;
            Vector3 requiredSpeed = new Vector3(h.x, h.y, h.z) * (minSpeed / 2);
            if (CurrSpeed > requiredSpeed.magnitude) {
                Quaternion r = Quaternion.Lerp(rb.rotation, h, rotationSpeed * Time.deltaTime);
                rb.rotation = r;
            } else {
                rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            }
            secondsSinceLastRaycast = 0;
        } else if (secondsSinceLastRaycast > maxTimeSinceLastRaycast){
            Debug.DrawRay(transform.position, -transform.up * distance, Color.red);
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }
        var target = rb.velocity;
        target.Scale(transform.up);
        rb.velocity = Vector3.Lerp(rb.velocity, target, Drag * Time.deltaTime);
        secondsSinceLastRaycast += Time.deltaTime;
        if (!isGrounded && stomp && Input.GetButton("Stomp")) {
            rb.velocity = transform.up * -stompVelocity;
        }
        if (isGrounded && stomp) {
            stomp = false;
            //inputEnabled = true;
            if (Input.GetButton("Stomp")) {
                rb.velocity = Vector3.zero;
                CameraThing.main.Shake(0.2f, ((0.2f / 50) * stompVelocity) );
            } else {
                Vector3 v = (transform.up * fallTime) * bounceMultiplier;
                rb.velocity = new Vector3(Mathf.Clamp(v.x, -bounceMax, bounceMax), Mathf.Clamp(v.y, -bounceMax, bounceMax), Mathf.Clamp(v.z, -bounceMax, bounceMax));
            }
        }
        if (inputEnabled && Input.GetButton("Light Speed Dash")) {
            UpdateNearestRing();
            if (nearestRing) {
                Vector3 dir = nearestRing.position - rb.position;
                dir.Normalize();
                Vector3 veloc = dir * lightDashSpeed;
                rb.velocity = veloc;
                //rb.position = nearestRing.position;
            }
        }
    }
    void AnimationUpdate() {
        anim.SetBool("Spinning", spinning);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsSuper", isSuper);
    }
    void Update()
    {
        speedBar.fillAmount = CurrSpeed / speedBarMax;
        if (rings > superStartRings) superStartRings = rings;
        if (isSuper) {
            if (rings <= 0) {
                isSuper = false;
                h.anchoredPosition = originalPos;
            } else {
                if (ringCooldown > 0) ringCooldown -= Time.deltaTime;
                if (ringCooldown <= 0) {rings--;ringCooldown = 1;}
                h.anchoredPosition = originalPos + (new Vector2(Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity), Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity)) / 1.5f);
            }
        }
        if (anim) AnimationUpdate();
        if (doingHomingAttack) {
            homingAttackTime += Time.deltaTime;
            if (homingAttackTime >= homingAttackTimeout) {
                homingAttackTime = 0;
                doingHomingAttack = false;
            }
        }
        if (doingHomingAttack && homingAttackTarget) {
            Vector3 dir = target1.center - transform.position;
            dir.Normalize();
            Vector3 veloc = dir * homingAttackForce;
            rb.velocity = veloc;
        }
        target.localScale = Vector3.one * (1 + scaleMultiplier);
        if (scaleMultiplier > 0) {
            scaleMultiplier -= Time.deltaTime * 5;
        }
        iconHolder.gameObject.SetActive(wispsEnabled);
        target.gameObject.SetActive(homingAttackTarget);
        if (homingAttackTarget) {
            target.position = target1.center;
        }
        if (nearestRing) {
            Vector3 dir = nearestRing.position - rb.position;
            Debug.DrawLine(rb.position, dir, Color.yellow);
        }
        if (!homingAttackTarget) doingHomingAttack = false;
        /*if (doingLightSpeedDash) {
            UpdateNearestRing();
            if (nearestRing) {
                Vector3 dir = nearestRing.position - rb.centerOfMass;
                dir.Normalize();
                Vector3 veloc = dir * lightDashSpeed;
                rb.velocity = veloc;
            }
        }*/
        UpdateHomingAttackTarget();
        isBoosting = false;
        boostEffectTransform.LookAt(transform.position + rb.velocity);
        wispBar.color = Color.Lerp(wispBar.color, barColor, 10 * Time.deltaTime);
        float barTarget = barPercent;
        float barValue = fillAmount;
        if (float.IsNaN(barValue)) barValue = 0;
        if (float.IsNaN(barTarget)) barTarget = 0;
        fillAmount = Mathf.Lerp(barValue, barTarget, 6 * Time.deltaTime);
        wispBar.fillAmount = fillAmount;
        boostGauge2.fillAmount = fillAmount - 1;
        boostGauge3.fillAmount = fillAmount - 2;
        boostThing.localPosition = (Vector3)new Vector2(wispBar.fillAmount * wispBarRect.sizeDelta.x, 0);
        //if (TwoDMode) rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        //else rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (movement.magnitude > 0.1f) {
            facing = transform.TransformDirection((camHolder.forward * movement.y) + (camHolder.right * movement.x));
            facingRelative = (camHolder.forward * movement.y) + (camHolder.right * movement.x);
        }
        Debug.DrawRay(transform.position, facing * 7);
        Debug.DrawRay(transform.position, facingRelative * 7, Color.gray);
        if (facingRelative != -Vector3.forward) graphics.localRotation = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, facingRelative), Vector3.Cross(Vector3.forward, facingRelative));
        else graphics.localRotation = Quaternion.Euler(0, -180, 0);
        //h.gameObject.SetActive(boostEnabled || (currWisp != null && currWisp.timeLeft > 0));
        glowThing.fillAmount = wispBar.fillAmount;
        glowThing.color = new Color(wispBar.color.r, wispBar.color.g, wispBar.color.b, glowThing.color.a);
        if (isGrounded) didAirDash = false;
        if (currWisp == null) barPercent = boost / maxBoost;
        if (currWisp != null) {
            if (!currWisp.beingUsed) {
                wispIcon.sprite = currWisp.icon;
                //wispIcon.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
                if (!isSuper)barPercent = boost / maxBoost;
                else barPercent = ((float)rings + ringCooldown) / (float)superStartRings;
                if (!isSuper) barColor = defaultBarColor;
                if (isSuper) barColor = superDefaultBarColor;
                iconHolderImage.sprite = wispBarActive;
                iconHolderImage.color = Color.white;
            }
            if (currWisp.beingUsed && currWisp.timeLeft <= 0) {
                if (!isSuper)barPercent = boost / maxBoost;
                else barPercent = (float)rings / (float)superStartRings;
                if (!isSuper) barColor = defaultBarColor;
                if (isSuper) barColor = superDefaultBarColor;
                iconHolderImage.sprite = wispBarIdle;
                iconHolderImage.color = Color.white * 0.5f + Color.black;
            }
        } else {
            iconHolderImage.sprite = wispBarIdle;
            iconHolderImage.color = Color.white * 0.5f + Color.black;
            if (!isSuper)barPercent = boost / maxBoost;
            else barPercent = (float)rings / (float)superStartRings;
            if (!isSuper) barColor = defaultBarColor;
            if (isSuper) barColor = superDefaultBarColor;
        }
        if (inputEnabled) {
            if (Input.GetButtonDown("Stomp") && !isGrounded && !stomp) {
                //inputEnabled = false;
                rb.velocity = -transform.up * stompVelocity + (rb.velocity / 1.4f);
                stomp = true;
                spinning = true;
            }
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (movement.magnitude > 0.1f) lastMovement = movement.normalized;
            jump = Input.GetButtonDown("Jump");
            //if (!homingAttackTarget) doingHomingAttack = false;
            
            if (isGrounded) {
                doingHomingAttack = false;
                didAirDash = false;
                didDoubleJump = false;
                if ( rb.velocity.y <= 0f && !Input.GetButton("Stomp")) spinning = false;
                else if (Input.GetButton("Stomp")) {
                    if (CurrSpeed > 2) spinning = true;
                    else spinning = false;
                }
            }
            if (!isGrounded && jump) {
                
                if (homingAttackTarget && !doingHomingAttack) {
                    doingHomingAttack = true;
                    spinning = true;
                    Vector3 dir = target1.center - transform.position;
                    dir.Normalize();
                    Vector3 veloc = dir * homingAttackForce;
                    rb.velocity = veloc;
                } else if (!didDoubleJump) {
                    rb.AddForce(transform.up * doubleJumpForce, ForceMode.Impulse);
                    doingHomingAttack = false;
                    didDoubleJump = true;
                }

                  
            } 
            if (Input.GetButton("Boost") && boostEnabled && CurrSpeed > (minSpeed / 2)) {
                if (movement.magnitude != 0 && boost > 0) {
                    if (isSuper) ringCooldown -= Time.deltaTime * 3;
                    float m = 1;
                    if (isSuper) m = superBoostSpeedMultiplier;
                    Vector3 hh = Vector3.zero;
                    isBoosting = true;
                    outlineThing.color = Color.white;
                    Vector3 scaledVelocity = rb.velocity;
                    scaledVelocity.Scale(transform.right + transform.forward);
                    rb.AddRelativeForce(lastMovement.normalized.x * boostSpeed * Time.deltaTime * camHolder.right * m, ForceMode.Acceleration);
                    rb.AddRelativeForce(lastMovement.normalized.y * boostSpeed * Time.deltaTime * camHolder.forward * m, ForceMode.Acceleration);
                    boostThing.sizeDelta = new Vector2(3, 25);
                    glowThing.color += new Color(0, 0, 0, 1);
                    h.anchoredPosition = originalPos + (new Vector2(Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity), Random.Range(-Wisps.main.shakeIntensity, Wisps.main.shakeIntensity)) / 2);
                    if (boost > maxBoost) boost -= Time.deltaTime * boostConsumeRate * 2;
                    else boost -= Time.deltaTime * boostConsumeRate;
                    
                } else {
                    h.anchoredPosition = originalPos;
                }
            } else {
                h.anchoredPosition = originalPos;
            }
            if (Input.GetButtonDown("Boost") && boostEnabled) {
                if (boost > 0) {
                    Vector3 hh = Vector3.zero;
                    if (isSuper) rings--;
                    isBoosting = true;
                    outlineThing.color = Color.white;
                    Vector3 scaledVelocity = rb.velocity;
                    scaledVelocity.Scale(transform.right + transform.forward);
                    rb.AddRelativeForce(lastMovement.normalized.x * boostSpeed * Time.deltaTime * camHolder.right, ForceMode.VelocityChange);
                    rb.AddRelativeForce(lastMovement.normalized.y * boostSpeed * Time.deltaTime * camHolder.forward, ForceMode.VelocityChange);
                    boost = Mathf.Clamp(boost - 12.5f, 0, maxBoost * 3);
                    CameraThing.main.Shake(0.5f, 1f);
                }
            } 
            if (Input.GetButtonDown("Boost") && !didAirDash && !isGrounded) {
                Vector3 scaled = rb.velocity;
                scaled.Scale(transform.right + transform.forward);
                rb.velocity = scaled;
                rb.AddRelativeForce(((camHolder.forward * lastMovement.normalized.y) + (camHolder.right * lastMovement.normalized.x)) * airDashForce, ForceMode.Impulse);
                didAirDash = true;
                spinning = false;
            }
            if (Input.GetButtonDown("Wisp Power") && !isSuper) {
                var h = !wispsEnabled;
                if (currWisp == null) h = true;
                if (currWisp != null && currWisp.beingUsed && currWisp.timeLeft <= 0) h = true;
                if (h && rings >= 50) {
                    isSuper = true;
                    superStartRings = rings;
                    StartCoroutine(OnSuper());
                }
            }
            if (currWisp != null && Input.GetButtonDown("Wisp Power") && wispsEnabled) {
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
            spinning = true;
        }
        if (doingHomingAttack) spinning = true;
        if (trail) {
            trail.emitting = isBoosting || doingHomingAttack || stomp || didAirDash || spinning || CurrSpeed > minSpeed;
        }
        if (runningParticles) {
            var e = runningParticles.emission;
            e.enabled = isGrounded && (isBoosting || CurrSpeed > minSpeed);
        }
        if ((boost > 0 && boostEnabled) || (currWisp != null && currWisp.timeLeft > 0)) {
            hImage.color = Color.white;
        } else {
            hImage.color = Color.white * 0.5f + Color.black;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Clamp(baseFov + (CurrSpeed * fovIntensity), 20, fovLimit), fovSpeed * Time.deltaTime);
        if (glowThing.color.a > 1) glowThing.color = new Color(glowThing.color.r, glowThing.color.g, glowThing.color.b, 1);
        outlineThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        boostThing.sizeDelta *= new Vector2(1, 0.9f);
        glowThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        if (ringThing) ringThing.material.SetColor("_Color", new Color(t, t, t, 0));
        if (t > 0) t -= Time.deltaTime * 2;
        if (t < 0) t = 0;
        boostEffect.gameObject.SetActive(isBoosting);
        if (boost > maxBoost) boost = maxBoost;
    } 
    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, homingAttackRadius);
        if (!floorDetection) return;
        Gizmos.DrawWireSphere(floorDetection.position, radius);
    }
}
public class HomingAttackTarget {
    public Transform transform;
    public Vector3 center {
        get {
            if (col) return col.bounds.center;
            return transform.position;
        }
    }
    public Collider col;
    public HomingAttackTarget(Transform transform) {
        this.transform = transform;
    }
    public HomingAttackTarget(Transform transform, Collider col) {
        this.transform = transform;
        this.col = col;
    }
}