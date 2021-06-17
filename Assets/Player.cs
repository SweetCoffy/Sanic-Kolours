using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public static float time = 0;
    [Header("Totally necessary stuff")]
        public Rigidbody rb;
        ConstantForce c;
        public Transform floorDetection;
        public Animator anim;
    [Header("Movement")]
        public float speed = 20;
        public bool weirdAirDash = false;
        public float drag = 5;
        public float jumpForce = 40;
        public float doubleJumpForce = 1;
        [HideInInspector] public bool spring = false;
        public float globalGravity = -5;
        public float spinningGravityMultiplier = 5;
        public float minSpeed = 50;        
        public float runningParticlesStart = 40;
        public float speedEffectStart = 30;
        public float surfaceMinSpeed = 40;
        public float rotationSpeed = 10;
        public float stompVelocity = 75;
        public float bounceMax = 15;
        public float bounceMin = 15;
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
        float airDashTimeLeft = 0;
        public float airDashTime = 1;
        public float spindashSpeed = 100;
        public float spindashChargeTime = 5;
        float spindashCharge = 0;
        bool spindash = false;
        public bool didJump = false;
        public float Speed {
            get {
                float m = 1;
                if (isSuper && !isBoosting) m = superSpeedMultiplier;
                if (boostMode && isGrounded) return speed * multiplier * m;
                return speed * m;
            }
        }
    [Header("Surface Detection")]
        public float maxTimeSinceLastRaycast = 0.15f;
        public float distance = 0.07f;
        public float radius = 0.1f;
        public float raycastOffset = 0.05f;
        public LayerMask groundMask;
    [Header("UI")]
        //public float speedBarMax = 100;
        //public Image outlineThing;
        //public Image ringsThing;
        //public Gradient ringsThingGradient;
        //public float ringsThingSpeed = 1f;
        //float ringsThingTime = 0;
        //public Image wispIcon;
        //public RectTransform h;
        //private Image hImage;
        //public Image speedBar;
        //public float barPercent = 1;
        //public Image boostBarBackground;
        //public ParticleSystem particle1;
        //public ParticleSystem particle2;
        //public ParticleSystem particle3;
        //public Image wispBar;
        //public Image boostGauge2;
        //public Image boostGauge3;
        //public Sprite wispBarIdle;
        //public Sprite wispBarActive;
        //public RectTransform wispBarRect;
        //public Image glowThing;
        //public RectTransform iconHolder;
        //public Image iconHolderImage;
        //public RectTransform boostThing;
        public Color defaultBarColor;
        public HUD hud;
        public Color barColor;
        //public Image ringThing;
        public bool boostMode = false;
        public bool enteredBoostMode = false;
        //public Sprite placeholderIcon;
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
                //if (isSuper) return maxBoost;
                return Mathf.Clamp(_boost, 0, MaxBoost * 3);
            }
            set {
                float diff = value - _boost;
                if (diff > 7) {
                    //wispBar.color = Color.white;
                }
                if (diff > 0) {
                    //if (boostRecoverEffect) boostRecoverEffect.Play(true);
                }
                _boost = value;
            }
        }
        [SerializeField] float _boost = 0;
        public float boostSpeed = 225;
        public float maxBoostSpeed = 200;
        public bool boostEnabled = true;
        public float boostConsumeRate = 10;
        public bool isGrounded {get; protected set;} = true;
        float secondsSinceLastRaycast = 0;
        Vector2 movement;
        bool jump = false;
    [Header("Fancy effects")]
        public float fovIntensity = 0.5f;
        public GameObject boostShockwave;
        public float fovSpeed = 10;
        public Renderer boostEffect;
        public Transform boostEffectTransform;
        public Transform fakeShadow;
        public ParticleSystem runningParticles;
        public ParticleSystem speedEffect;
        public TrailRenderer trail;
        //public ParticleSystem boostRecoverEffect;
        public float baseFov = 70;
        public float fovLimit = 150;
    [Header("Other")]
        public LayerMask wallMask;
        public bool lostWorldHomingAttack = false;
        public GameObject droppedRing;
        [HideInInspector] public bool dead = false;
        public float superDamage = 5;
        public float superBoostRegenRate = 50;
        public float superBoostRegenRateBySpeed = 0.5f;
        public float stompDamage = 3;
        public float boostDamage = 2;
        public bool wispsEnabled = true;
        public Transform graphics;
        public AttractRings attractRings;
        public bool inputEnabled = true;
        public float homingAttackRadius = 10;
        Vector2 originalPos;
        Vector2 rOriginalPos;
        public Vector3 facing = Vector3.forward;
        public bool destroyEnemies;
        public Transform target;
        public HomingAttackTarget target1;
        bool didAirDash = false;
        private float scaleMultiplier = 0;
        public float invincibility = 1;
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
        bool s = false;
        public LayerMask homingAttackMask;
        public LayerMask lightSpeedDashMask;
        public float twoDModeZ = 0;
        public bool TwoDMode = false;
        public Transform camHolder;
        [SerializeField] int _rings = 0;
        public bool stomp = false;
        public float lightSpeedDashRange = 4;
        public float homingAttackForce = 50;
        private float fillAmount = 0;
        public float superStartRings = 0;
        public Color superDefaultBarColor = Color.yellow;
        public Gradient superColorGradient;
        public Vector3 camForward;
        public Vector3 camRight;
        public float gradientSpeed = 0.01f;
        public static int score = 0;
        private float fallTime = 0;
        private bool didDoubleJump = false;
        float lastBoostTime = 0;
        Color co;
        public float Drag {
            get {
                if ((spinning && isGrounded) || spring) return 0;
                if (movement.magnitude > 0) return drag * 0.3f;
                return drag;
            }
        }
        public bool doingHomingAttack = false;
        public bool BounceOffEnemies {
            get {
                return doingHomingAttack || (spinning && !isGrounded);
            }
        }
        public bool DestroyEnemiesNoSuper {
            get {
                return doingHomingAttack || stomp || isBoosting || destroyEnemies || spinning;
            }
        }
        public bool DestroyEnemies {
            get {
                return DestroyEnemiesNoSuper || isSuper;
            }
        }
        public float MaxBoost {
            get {
                return maxBoost + (maxBoost * Game.boostUpgrade);
            }
        }
        public float airDashExtraSpeedMultiplier = 0.075f;
        public float MaxSpeed {
            get {
                float m = 1;
                if (isSuper) m = superMaxSpeedMultiplier;
                if (isBoosting && isGrounded) return maxBoostSpeed * m;
                if (spinning && isGrounded) return float.MaxValue;
                return maxSpeed * m;
            }
        }
        public Transform nearestRing;
        Vector3 facingRelative = Vector3.zero;
        public int rings {
            get => _rings;
            set {
                float diff = value - _rings;
                if (diff > 0) {
                    for (int i = 0; i < diff; i++) {
                        if ((boost + 5) >= MaxBoost) break;
                        boost += 5;
                    }
                }
                _rings = value;
                //RingCounter.main.text = value + ""; 
            }
        }
        float hTime = 0;
        bool doingLightSpeedDash = false;
        public bool isBoosting = false;
        public float ringCooldown = 1;
        float timeThing = 0;
        Vector3 wallDir = Vector3.zero;
        public float wallDist = 1.5f;
        float rTime = 0;
        int targetAmount = 100;
        public static Player main;
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
            if (target.transform == nearestRing) continue;
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
            yield return null;
        }
        //wispIcon.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
        currWisp.End();
        yield return null;
    }
    IEnumerator OnSuper() {
        if (!s) {
            Vector3 startVeloc = rb.velocity;
            Time.timeScale = 0.1f;
            rb.isKinematic = true;
            if (boostShockwave) Instantiate(boostShockwave, rb.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.1f);
            rb.isKinematic = false;
            rb.velocity = startVeloc;
            Time.timeScale = 1;
            while (!isGrounded) {
                yield return null;
            }
            Time.timeScale = 0.1f;
            Light[] lights = Object.FindObjectsOfType<Light>();
            Color ogCol          = RenderSettings.ambientLight       ;
            Color ogSkyColor     = RenderSettings.ambientSkyColor    ;
            Color ogEquatorColor = RenderSettings.ambientEquatorColor;
            Color ogGroundColor  = RenderSettings.ambientGroundColor ;
            float ogIntensity    = RenderSettings.ambientIntensity   ;
            //foreach (Light l in lights) {
            //    l.enabled = false;
            //}
            startVeloc = rb.velocity;
            //RenderSettings.ambientSkyColor     = Color.black;
            //RenderSettings.ambientEquatorColor = Color.black;
            //RenderSettings.ambientGroundColor  = Color.black;
            //RenderSettings.ambientLight        = Color.black;
            //RenderSettings.ambientIntensity    = 0          ;
            s = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            inputEnabled = false;
            CameraThing.main.Shake(0.5f, 0.7f);
            float m = 1;
            while (boost > 0) {
                boost -= Time.unscaledDeltaTime * boostConsumeRate * 15 * m;
                invincibility = 1;
                m += Time.unscaledDeltaTime * 1.5f;
                yield return null;
            }
            boost = 0;
            m = 1;
            CameraThing.main.Shake(0.3f, 0.7f);
            isSuper = true;
            while (boost < MaxBoost) {
                boost += Time.unscaledDeltaTime * boostConsumeRate * 15 * m;
                m += Time.unscaledDeltaTime * 1.1f;
                invincibility = 1;
                yield return null;
            }
            boost = MaxBoost;
            if (boostShockwave) Instantiate(boostShockwave, rb.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(1.25f);
            //foreach (Light l in lights) {
            //    l.enabled = true;
            //}
            Time.timeScale = 1;
            //RenderSettings.ambientLight = ogCol;
            //RenderSettings.ambientSkyColor     = ogSkyColor;
            //RenderSettings.ambientEquatorColor = ogEquatorColor;
            //RenderSettings.ambientGroundColor  = ogGroundColor;
            //RenderSettings.ambientIntensity = ogIntensity;
            inputEnabled = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            s = false;
            rb.velocity = startVeloc;
        }
    }
    IEnumerator OnSuperEnd() {
        if (!s) {
            s = true;
            float m = 1;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            inputEnabled = false;
            while (boost > 0) {
                boost -= Time.unscaledDeltaTime * boostConsumeRate * 50 * m;
                m += Time.deltaTime * 1.5f;
                yield return null;
            }
            boost = 0;
            m = 1;
            CameraThing.main.Shake(0.3f, 0.7f);
            isSuper = false;
            while (boost < MaxBoost) {
                boost += Time.unscaledDeltaTime * boostConsumeRate * 50 * m;
                m += Time.deltaTime * 1.1f;
                yield return null;
            }
            boost = MaxBoost;
            inputEnabled = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            s = false;
        }
    }
    public void SuperTransform(bool force = false) {
        if (isSuper) return;
        if (!Game.superEnabled && !force) {
            rTime = .3f;
            return;
        };
        s = false;
        superStartRings = rings;
        StartCoroutine(OnSuper());
    }
    void Start() {/**/
        main = this;
        hud.player = this;
        //if (boostBarBackground) co = boostBarBackground.color;
        currWisp = null;
        //originalPos = h.anchoredPosition;
        //rOriginalPos = ringsThing.rectTransform.anchoredPosition;
        barColor = defaultBarColor;
        //hImage = h.GetComponent<Image>();
        gfxStartPos = graphics.localPosition;
        trailOffset = trail.transform.localPosition - gfxStartPos;
        c = GetComponent<ConstantForce>();
        c.relativeForce *= ZoneInfo.current.gravityModifier;
        c.force *= ZoneInfo.current.gravityModifier;
    }
    public float bTime = 0;
    float wCooldown = 0;
    public float airDashVerticalSpeedMultiplier = 0.05f;
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
    Vector3 gfxStartPos;
    Vector3 trailOffset;
    void LateUpdate() {
        enteredBoostMode = false;
    }
    void FixedUpdate() {
        hud.color = barColor;
        if (ControlsThing.main != null) {
            if (isGrounded) {
                ControlsThing.main["jump"] = true;
                ControlsThing.main["boost"] = true;
                ControlsThing.main["doublejump"] = false;
                ControlsThing.main["airdash"] = false;
                ControlsThing.main["airboost"] = false;
                ControlsThing.main["bounce"] = false;
                ControlsThing.main["stomp"] = false;
            } else {
                ControlsThing.main["boost"] = false;
                ControlsThing.main["jump"] = false;
                if (!stomp) {
                    ControlsThing.main["bounce"] = true;
                    ControlsThing.main["stomp"] = false;
                }
                else {
                    ControlsThing.main["bounce"] = false;
                    ControlsThing.main["stomp"] = true;
                }
                if (!didAirDash) {
                    if (boost > 0) {
                        ControlsThing.main["airdash"] = false;
                        ControlsThing.main["airboost"] = true;
                    } else {
                        ControlsThing.main["airdash"] = true;
                        ControlsThing.main["airboost"] = false;
                    }
                } else {
                    ControlsThing.main["airdash"] = false;
                    ControlsThing.main["airboost"] = false;
                }
                if (!didDoubleJump) {
                    ControlsThing.main["doublejump"] = true;
                } else {
                    ControlsThing.main["doublejump"] = false;
                }
            }
        }
        graphics.localScale = Vector3.Lerp(graphics.localScale, Vector3.one, 9 * Time.deltaTime);
        graphics.localPosition = Vector3.Lerp(graphics.localPosition, gfxStartPos, 13 * Time.deltaTime);
        trail.transform.localPosition = graphics.localPosition + trailOffset;
        camForward = CameraThing.main.forward;
        camRight = CameraThing.main.right;
        if (spinning && isGrounded) c.force = Vector3.up * globalGravity * spinningGravityMultiplier;
        else c.force = Vector3.up * globalGravity;
        if (boostMode) gameObject.layer = 17;
        else gameObject.layer = 15;
        if (dead) return;
        if (rb.position.y < -600) Kill();
        RaycastHit hit;
        bool didHit = Physics.Raycast(rb.position + (transform.up * raycastOffset), -transform.up, out hit, distance, groundMask);
        if (didHit) {
            Debug.DrawRay(rb.position + (transform.up * raycastOffset), hit.point - (rb.position + (transform.up * raycastOffset)), Color.green);
            Quaternion h = Quaternion.FromToRotation(Vector3.up, hit.normal.normalized).normalized;
            Vector3 requiredSpeed = new Vector3(h.x, h.y, h.z) * (surfaceMinSpeed);
            if (CurrSpeed > requiredSpeed.magnitude || boostMode || (requiredSpeed / surfaceMinSpeed).magnitude < .2f || (spinning && isGrounded)) {
                //Quaternion r = Quaternion.Lerp(rb.rotation, h, rotationSpeed * Time.deltaTime);
                Quaternion r = h;
                rb.rotation = r;
                secondsSinceLastRaycast = 0;
                //if (isGrounded && h != Quaternion.identity) rb.MovePosition(hit.point);
            } else if (secondsSinceLastRaycast > maxTimeSinceLastRaycast){
                //rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
                rb.rotation = Quaternion.identity;
            }
        } else if (secondsSinceLastRaycast > maxTimeSinceLastRaycast){
            Debug.DrawRay(rb.position + (transform.up * raycastOffset), -transform.up * distance, Color.red);
            //rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            rb.rotation = Quaternion.identity;
        }
        Vector3 scaled = rb.velocity;
        scaled.Scale(transform.up);
        if (isGrounded) fallTime = 0;
        if (!isGrounded && rb.velocity.y < 0) fallTime -= (scaled.x + scaled.y + scaled.z) * Time.deltaTime;
        // Old speed limit
        //rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed), Mathf.Clamp(rb.velocity.y, -MaxSpeed, MaxSpeed), Mathf.Clamp(rb.velocity.z, -MaxSpeed, MaxSpeed));
        
        // New speed limit
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
        if (TwoDMode) {
            Vector3 veloc = rb.velocity;
            veloc.Scale(Vector3.right + Vector3.up);
            rb.velocity = veloc;
            if (twoDModeZ != 0) rb.position = Vector3.Lerp(rb.position, new Vector3(rb.position.x, rb.position.y, twoDModeZ), 7 * Time.deltaTime);
        }
        if (didJump && !isGrounded && !didAirDash) spinning = true;
        else if (isGrounded && !Input.GetButton("Stomp")) spinning = false;
        if (isGrounded && rb.velocity.y < 0.1f) didJump = false;
        if (inputEnabled) {
            if (jump && isGrounded) {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                graphics.localScale = new Vector3(1.5f, .5f, 1.5f);
                jump = false;
                didJump = true;
                isGrounded = false;
                spinning = true;
                airDashTimeLeft = airDashTime;
            }
            if (!isGrounded && stomp && Input.GetButton("Stomp")) {
                graphics.localScale = new Vector3(.5f, 1.5f, .5f);
                spinning = true;
                rb.velocity = transform.up * -stompVelocity;
            }
        }
        if (isGrounded) spring = false;
        Collider[] hhhh = Physics.OverlapSphere(floorDetection.position, radius, groundMask);
        isGrounded = hhhh.Length > 0;
        Vector3 m = ((camForward * movement.y) + (camRight * movement.x)).normalized;
        if (!(spinning && isGrounded)) {
            rb.AddRelativeForce(m * Speed);
        }  else if (spinning && isGrounded) {
            rb.AddRelativeForce(m * Speed * 0.25f);
        }
        var target = rb.velocity;
        target.Scale(transform.up);
        rb.velocity = Vector3.Lerp(rb.velocity, target, Drag * Time.deltaTime);
        secondsSinceLastRaycast += Time.deltaTime;
        if (CurrSpeed > minSpeed && !doingHomingAttack && !boostMode && (isGrounded || (!isGrounded && didAirDash))) {
            boostMode = true;
            if (!isSuper) CameraThing.main.Shake(0.3f, 0.4f);
            else CameraThing.main.Shake(0.3f, 0.6f);
            enteredBoostMode = true;
            if (boostShockwave) {
                Instantiate(boostShockwave, graphics.position, graphics.rotation);
            }
        }
        if (CurrSpeed < minSpeed && isGrounded && boostMode) {
            boostMode = false;
        }
        if (stomp) {
            graphics.localScale = new Vector3(.6f, 1.4f, .6f);
        }
        if (isGrounded && stomp && !(!isGrounded && isSuper && isBoosting)) {
            stomp = false;
            //inputEnabled = true;
            if (Input.GetButton("Stomp")) {
                if (boostShockwave) {
                    Instantiate(boostShockwave, graphics.position, graphics.rotation);
                } 
                CameraThing.main.Shake(0.2f, 0.3f);
                rb.velocity = Vector3.zero;
                graphics.localScale = new Vector3(2f, .25f, 2f);
            } else {
                CameraThing.main.Shake(0.06f, 0.08f);
                graphics.localScale = new Vector3(1.5f, .5f, 1.5f);
                float y = Mathf.Clamp(fallTime * bounceMultiplier, bounceMin, bounceMax);
                Vector3 v = (transform.up * y);
                rb.velocity = v;
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
        RaycastHit rhit;
        bool ohno = false;
        bool didHith = Physics.Raycast(rb.position, graphics.forward, out rhit, wallDist, wallMask);
        if (wCooldown > 0) wCooldown -= Time.deltaTime;
        if (didHith) {
            if (wallDir.magnitude <= 0 && wCooldown <= 0) {
                if (movement.magnitude > 0) {
                    wallDir = graphics.forward;
                    rb.velocity = (Vector3.down * .1f) + (wallDir * 50);
                    isGrounded = true;
                    didDoubleJump = false;
                    didAirDash = false;
                    if (Input.GetButtonDown("Jump")) {
                        wCooldown = .1f;
                        rb.velocity = -wallDir * airDashForce * 1.5f;
                    }
                    //rb.position = rhit.point;
                } else {
                    ohno = true;
                }
            }
        } else {
            ohno = true;
        }
        if (isGrounded) {
            ohno = true;
        }
        if (ohno) {
            wallDir = Vector3.zero;
        }
    }
    void AnimationUpdate() {
        anim.SetBool("Spinning", spinning);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsSuper", isSuper);
    }
    void Update()
    {
        time += Time.deltaTime;
        if (dead) return;
        if (inputEnabled) {
            /**/
            if (CurrSpeed > 15) {
                /**/
                bool l = Input.GetButtonDown("Quickstep Left");
                bool r = Input.GetButtonDown("Quickstep Right");
                /**/
                if (l) {
                    rb.MovePosition(rb.position + (graphics.right * -5));
                    graphics.localPosition += graphics.right * 5;
                    score += 100;
                }
                if (r) {
                    rb.MovePosition(rb.position + (graphics.right * 5));
                    graphics.localPosition -= graphics.right * 5;
                    score += 100;
                }
            }
        }
        //ringsThingTime += Time.deltaTime * ringsThingSpeed;
        //if (ringsThingTime > 1) ringsThingTime = 0;
        //if ((isSuper && rings > 9 && !isBoosting) || (!isSuper && rings > 0)) ringsThingTime = 0;
        //ringsThing.color = ringsThingGradient.Evaluate(ringsThingTime);
        if (invincibility > 0) invincibility -= Time.deltaTime;
        if (attractRings) {
            attractRings.enabled = isBoosting;
        }
        //speedBar.fillAmount = CurrSpeed / speedBarMax;
        if (rings > superStartRings) superStartRings = rings;
        if (lastBoostTime < 15) lastBoostTime += Time.deltaTime;
        if (isSuper && rings > 0) {
            if (rb.constraints == RigidbodyConstraints.FreezeRotation) boost = maxBoost;
            //glowThing.color += new Color(0, 0, 0, 1);
            if (ringCooldown > 0) ringCooldown -= Time.deltaTime;
            if (ringCooldown <= 0) {rings--;ringCooldown = 1;}
            if (rings <= 0 || Input.GetButtonDown("Wisp Power")) {
                StartCoroutine(OnSuperEnd());
                
            }
            timeThing += Time.deltaTime * gradientSpeed;
            if (timeThing > 1) timeThing = 0;
            if (superColorGradient != null) {
                superDefaultBarColor = superColorGradient.Evaluate(timeThing);
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
            if (lostWorldHomingAttack) rb.velocity = Vector3.Slerp(rb.velocity.normalized, dir, 15 * Time.deltaTime) * homingAttackForce;
            else rb.velocity = veloc;

            
        }
        target.localScale = Vector3.one * (1 + scaleMultiplier);
        if (scaleMultiplier > 0) {
            scaleMultiplier -= Time.deltaTime * 5;
        }
        //iconHolder.gameObject.SetActive(wispsEnabled);
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
        //isBoosting = false;
        boostEffectTransform.LookAt(transform.position + rb.velocity);
        //wispBar.color = Color.Lerp(wispBar.color, barColor, 10 * Time.deltaTime);
        //float barTarget = barPercent;
        //float barValue = fillAmount;
        //if (float.IsNaN(barValue)) barValue = 0;
        //if (float.IsNaN(barTarget)) barTarget = 0;
        //fillAmount = Mathf.Lerp(barValue, barTarget, 50 * Time.deltaTime);
        //if (boostBarBackground)boostBarBackground.color = co * wispBar.color;
        //wispBar.fillAmount = fillAmount;
        //boostGauge2.fillAmount = fillAmount - 1;
        //boostGauge3.fillAmount = fillAmount - 2;
        //boostThing.localPosition = (Vector3)new Vector2((fillAmount % 1.01f) * wispBarRect.sizeDelta.x, 0);
        //if (TwoDMode) rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        //else rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (movement.magnitude > 0.1f) {
            Vector3 h = (camForward * movement.y) + (camRight * movement.x);
            if (CameraThing.main.firstPerson || CameraThing.main.strafe) {
                h = camForward;
            }
            facing = transform.TransformDirection(h);
            facingRelative = h;
        }
        Debug.DrawRay(transform.position, facing * 7);
        Debug.DrawRay(transform.position, facingRelative * 7, Color.gray);
        if (facingRelative != -Vector3.forward) graphics.localRotation = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, facingRelative), Vector3.Cross(Vector3.forward, facingRelative));
        else graphics.localRotation = Quaternion.Euler(0, -180, 0);
        //h.gameObject.SetActive(boostEnabled || (currWisp != null && currWisp.timeLeft > 0));
        //glowThing.fillAmount = wispBar.fillAmount;
        //glowThing.color = new Color(wispBar.color.r, wispBar.color.g, wispBar.color.b, glowThing.color.a);
        if (isGrounded) didAirDash = false;
        //if (currWisp == null) barPercent = boost / maxBoost;
        if (currWisp != null) {
            if (!currWisp.beingUsed) {
                //wispIcon.sprite = currWisp.icon;
                //wispIcon.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
                //barPercent = boost / maxBoost;
                if (!isSuper) barColor = defaultBarColor;
                if (isSuper) barColor = superDefaultBarColor;
                //iconHolderImage.sprite = wispBarActive;
                //iconHolderImage.color = Color.white;
            }
            if (currWisp.beingUsed && currWisp.timeLeft <= 0) {
                //barPercent = boost / maxBoost;
                if (!isSuper) barColor = defaultBarColor;
                if (isSuper) barColor = superDefaultBarColor;
                //iconHolderImage.sprite = wispBarIdle;
                //iconHolderImage.color = Color.white * 0.5f + Color.black;
            }
        } else {
            //iconHolderImage.sprite = wispBarIdle;
            //iconHolderImage.color = Color.white * 0.5f + Color.black;
            //barPercent = boost / maxBoost;
            if (!isSuper) barColor = defaultBarColor;
            if (isSuper) barColor = superDefaultBarColor;
        }
        if (inputEnabled) {
            if (Input.GetButtonDown("Stomp") && !isGrounded && !stomp && !(!isGrounded && isSuper && isBoosting)) {
                //inputEnabled = false;
                rb.velocity = -transform.up * stompVelocity + (rb.velocity / 1.4f);
                graphics.localScale = new Vector3(.5f, 1.5f, .5f);
                stomp = true;
                spinning = true;
            }
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            if (movement.magnitude > 0.1f) lastMovement = movement.normalized;
            jump = Input.GetButtonDown("Jump");
            //if (!homingAttackTarget) doingHomingAttack = false;
            
            if (isGrounded) {
                didAirDash = false;
                didDoubleJump = false;
                if (!Input.GetButton("Stomp")) spinning = false;
                if (Input.GetButton("Stomp")) {
                    if (CurrSpeed > 2) spinning = true;
                    else spinning = false;
                }
            }
            if (!isGrounded && jump) {
                
                if (homingAttackTarget && !doingHomingAttack && wallDir.magnitude <= 0 && wCooldown <= 0) {
                    doingHomingAttack = true;
                    spinning = true;
                    //Vector3 dir = target1.center - transform.position;
                    //dir.Normalize();
                    //Vector3 veloc = dir * homingAttackForce;
                    //rb.velocity = veloc;
                } else if (!didDoubleJump) {
                    Vector3 vel = rb.velocity;
                    vel.y = doubleJumpForce;
                    didJump = true;
                    spinning = true;
                    didAirDash = false;
                    stomp = false;
                    rb.velocity = vel;
                    graphics.localScale = new Vector3(1.5f, .5f, 1.5f);
                    doingHomingAttack = false;
                    didDoubleJump = true;
                }

                  
            } 
            if (isBoosting && isSuper && !isGrounded && didAirDash) {
                c.force = Vector3.zero;
                Vector3 v = rb.velocity;
                if (v.y < 0) v.Scale(Vector3.forward + Vector3.right);
                rb.velocity = v;
                ringCooldown -= Time.deltaTime * .5f;
            }
            if (isSuper) {
                if (!Input.GetButtonDown("Boost")) didAirDash = false;
                didDoubleJump = false;
            }
            if (Input.GetButtonDown("Boost") && (boost <= 0 || (!isGrounded && didAirDash))) {
                hTime = .2f;
            }
            if (hTime > 0) {
                hTime -= Time.deltaTime;
                float i = Settings.main.shakeIntensity;
                //h.anchoredPosition = originalPos + new Vector2(Random.Range(-i, i), Random.Range(-i, i));
            } else {
                //h.anchoredPosition = originalPos;
            }
            if (rTime > 0) {
                rTime -= Time.deltaTime;
                float i = Settings.main.ringsShakeIntensity;
                //ringsThing.rectTransform.anchoredPosition = rOriginalPos + new Vector2(Random.Range(-i, i), Random.Range(-i, i));
            } else {
               //ringsThing.rectTransform.anchoredPosition = rOriginalPos;
            }
            if (Input.GetButton("Boost") && boostEnabled) {
                if (lastMovement.magnitude != 0 && boost > 0 && bTime > 0 && isBoosting) {
                    float m = 1; 
                    if (isSuper) m = superBoostSpeedMultiplier;
                    Vector3 hh = Vector3.zero;
                    lastBoostTime = 0;
                    if (Input.GetKey(KeyCode.F)) {
                        boost -= Time.unscaledDeltaTime * (boostConsumeRate);
                        Time.timeScale = .2f;
                    } else Time.timeScale = 1;
                    //outlineThing.color = Color.white;
                    Vector3 scaledVelocity = rb.velocity;
                    scaledVelocity.Scale(transform.right + transform.forward);
                    Vector2 mo = ((camForward * lastMovement.normalized.y) + (camRight * lastMovement.normalized.x)).normalized;
                    if (isSuper) ringCooldown -= Time.deltaTime;
                    if (isGrounded || isSuper) rb.AddRelativeForce(mo * boostSpeed * m, ForceMode.Acceleration);
                    //boostThing.sizeDelta = new Vector2(3, 25);
                    if (boost > MaxBoost) boost -= Time.deltaTime * boostConsumeRate * 2;
                    else boost -= Time.deltaTime * boostConsumeRate;
                    /**/
                } else {
                    
                    isBoosting = false;
                    Time.timeScale = 1;
                }
            } else {
                
                isBoosting = false;
                Time.timeScale = 1;
            }
            if (Input.GetButtonDown("Wisp Power") && rings < 50) {
                rTime = 0.2f;
            }
            if (CurrSpeed >= minSpeed / 2) {
                bTime = .5f;
            }
            if (bTime > 0) {
                bTime -= Time.deltaTime;
            }
            if (Input.GetButtonDown("Boost") && boostEnabled && !isBoosting && (isGrounded ? true : !didAirDash)) {
                if (boost > 0) {
                    Vector3 hh = Vector3.zero;
                    if (isSuper) {
                        rTime = .1f;
                        rings--;
                    }
                    bTime = .5f;
                    isBoosting = true;
                    if (boostShockwave) {
                        Instantiate(boostShockwave, graphics.position, graphics.rotation);
                    }
                    //outlineThing.color = Color.white;
                    Vector3 scaledVelocity = rb.velocity;
                    scaledVelocity.Scale(transform.right + transform.forward);
                    Vector3 mo = ((camForward * lastMovement.normalized.y) + (camRight * lastMovement.normalized.x)).normalized;
                    rb.AddRelativeForce(mo * boostSpeed * 2.5f, ForceMode.VelocityChange);
                    Camera.main.fieldOfView += 30;
                    boost -= 12.5f;
                    //CameraThing.main.Shake(0.5f, 1f);
                    /**/
                }
            } 
            if (Input.GetButtonDown("Boost") && !didAirDash && !isGrounded) {
                Vector3 mov = ((camForward * lastMovement.normalized.y) + (camRight * lastMovement.normalized.x)).normalized;
                Vector3 scaled = mov * (airDashForce + (CurrSpeed * airDashExtraSpeedMultiplier));
                scaled.Scale(transform.right + transform.forward);
                if (boost > 0) {
                    scaled.y += 40;
                } else {
                    scaled.y += airDashVerticalSpeedMultiplier * CurrSpeed;
                }
                if (boostShockwave) {
                    Instantiate(boostShockwave, graphics.position, graphics.rotation);
                }   
                rb.velocity = scaled;
                didAirDash = true;
                stomp = false;
                spinning = true;
            }
            if (didAirDash && airDashTimeLeft > 0 && !isGrounded && weirdAirDash) {
                airDashTimeLeft -= Time.deltaTime;
                spinning = false;
                rb.velocity = Vector3.Slerp(rb.velocity, ((camForward * lastMovement.normalized.y) + (camRight * lastMovement.normalized.x)) * airDashForce, 6 * Time.deltaTime);
                //boostMode = true;
            }
            if (!weirdAirDash) airDashTimeLeft = 0;
            if (Input.GetButtonDown("Wisp Power") && !isSuper) {
                var h = !wispsEnabled;
                if (currWisp == null) h = true;
                if (currWisp != null && currWisp.beingUsed && currWisp.timeLeft <= 0) h = true;
                if (h && rings >= 50) {
                    SuperTransform();
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
        if (doingHomingAttack) spinning = true;
        if (trail) {
            trail.emitting = isBoosting || doingHomingAttack || stomp || didAirDash || spinning || boostMode;
        }
        if (runningParticles) {
            var e = runningParticles.emission;
            e.enabled = isGrounded && (isBoosting || CurrSpeed > runningParticlesStart);
        }
        if (speedEffect) {
            var e = speedEffect.emission;
            e.enabled = isBoosting || RealCurrSpeed > speedEffectStart || doingHomingAttack;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Clamp(baseFov + (CurrSpeed * fovIntensity), 20, fovLimit), fovSpeed * Time.deltaTime);
        //if (glowThing.color.a > 1) glowThing.color = new Color(glowThing.color.r, glowThing.color.g, glowThing.color.b, 1);
        //outlineThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        //boostThing.sizeDelta *= new Vector2(1, 0.8f);
        //glowThing.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
        boostEffect.gameObject.SetActive(isBoosting);
        //if (boost > maxBoost) boost = MaxBoost;
    } 
    [ContextMenu("Take damage")]
    public void TakeDamageNoKnockback() {
        TakeDamage(Vector3.zero);
    }
    public void TakeDamage(Vector3 knockback) {
        if (dead) return;
        if (invincibility > 0 || isSuper) return;
        rb.velocity = knockback;
        if (rings <= 0) {
            Kill();
            return;
        }
        var lostRings = Mathf.Clamp(rings, 0, 50);
        var droppedRings = Mathf.Clamp(lostRings, 0, 30);
        var angle = 360 / droppedRings;
        for (var i = 0; i < droppedRings; i++) {
            Rigidbody r = Instantiate(droppedRing, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            r.angularVelocity = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));
            r.velocity = (Quaternion.Euler(0, i * angle, 0) * Vector3.forward) * 7 + (Vector3.up * 25);
        }
        rings -= droppedRings;
        invincibility = 0.7f;
        rTime = .7f;
    }
    public void Kill() {
        if (dead) return;
        dead = true;
        rings = 0;
        boost = 0;
        currWisp = null;
        StartCoroutine(_Kill());
        isSuper = false;
        AnimationUpdate();
    }
    IEnumerator _Kill() {
        //GetComponent<Collider>().enabled = false;
        //rb.velocity = Vector3.up * 17;
        rTime = 99;
        rb.angularVelocity = new Vector3(Random.Range(-69, 69), Random.Range(-69, 69), Random.Range(-69, 69));
        rb.useGravity = true;
        GetComponent<ConstantForce>().enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        inputEnabled = false;
        float timer = 1.1f;
        while (timer > 0) {
            timer -= Time.deltaTime;
            CameraThing.main.freecam = true;
            if (Input.GetButtonDown("Stomp")) timer = 0;
            if (Input.GetKey(KeyCode.F)) timer = float.PositiveInfinity;
            yield return null;
        }
        StageLoader.main.RestartStage();
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