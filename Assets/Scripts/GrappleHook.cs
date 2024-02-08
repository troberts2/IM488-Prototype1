using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GrappleHook : MonoBehaviour
{
    //component ref
    private Rigidbody rb;
    private CinemachineFreeLook playerFreeLook;

    [SerializeField] private GameObject playerCam;
    [SerializeField] private Image crossHair;
    [SerializeField] private GameObject speedParticles;
    [SerializeField] private float speedUIMagnitude = 10f;


    //Time stuff
    internal bool isSlowed = false;
    [SerializeField] private float timeSlowPercentage = .2f; //use values 0-1 (think of it as percentage)
    [SerializeField] private float maxSlowTime = 3f;
    [SerializeField] private float currentSlowTimeLeft;
    [SerializeField] private Image slowTimeUI;

    //grapple ability
    [Header("For Grappling")]
    [SerializeField] private Transform cam;
    public Transform shootPt;
    private Transform spotToSendPlayer;
    [SerializeField] private LayerMask grapplable;

    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grapplableDelayTime;
    [SerializeField] private float overshootYAxis;
    [SerializeField] private float grappleFov = 80f;
    [SerializeField] private float grappleSpeedMultiplier = 1f;

    internal Vector3 grapplePoint;

    [SerializeField] private float grapplingCd;
    private float grapplingCdTimer = 0f;

    internal bool grappling;

    public AudioClip timeStop;
    public AudioClip grapple;

    Vector3 camPos;

    //temp before combine with other player move script
    private bool freeze;
    private bool enableMovementOnNextTouch;


    private void Start() {
        rb = GetComponent<Rigidbody>();
        playerFreeLook = playerCam.GetComponent<CinemachineFreeLook>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        RollingCameraSettings();
        currentSlowTimeLeft = maxSlowTime;
        slowTimeUI.fillAmount = currentSlowTimeLeft/maxSlowTime;
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0) && isSlowed) StartGrapple();

        camPos = Camera.main.transform.position;

        if (grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;

        if(freeze) rb.velocity = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.Space) && !isSlowed && currentSlowTimeLeft >= 0 && grapplingCdTimer <= 0){
            StopAllCoroutines();
            StartCoroutine(GrappleTimeSlow());
            isSlowed = true;
        } 
        if(Input.GetKeyUp(KeyCode.Space) && isSlowed){
            StopCoroutine(GrappleTimeSlow());
            StartCoroutine(GrappleTimeNormal());
            isSlowed = false;
        } 
        UseSlowTime();

        //Gabe code (hi) for crossheir color
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grapplable)) crossHair.GetComponent<Image>().color = Color.red;
        else crossHair.GetComponent<Image>().color = Color.green;

        if(rb.velocity.magnitude > speedUIMagnitude){
            speedParticles.SetActive(true);
        }else{
            speedParticles.SetActive(false);
        }
    }

    /// <summary>
    /// slows time to timeSlowPercentage
    /// </summary>
    /// <returns></returns>
    private IEnumerator GrappleTimeSlow(){
        isSlowed = true;
        crossHair.enabled = true;
        playerFreeLook.m_Lens.FieldOfView = grappleFov;
        FreeLookCameraSettings();
        AudioSource.PlayClipAtPoint(timeStop, camPos);
        for (float i = 1; i >= timeSlowPercentage; i -= Time.deltaTime){
            SetTimeScale(i);
            yield return null;
        }
    }
    private IEnumerator GrappleTimeNormal(){
        isSlowed = false;
        crossHair.enabled = false;
        RollingCameraSettings();
        for(float i = timeSlowPercentage; i <= 1; i += Time.deltaTime){
            SetTimeScale(i);
            yield return null;
        }
    }
    private void UseSlowTime(){
        if(isSlowed && currentSlowTimeLeft > 0){
            currentSlowTimeLeft -= Time.unscaledDeltaTime;
            slowTimeUI.enabled = true;
        }else if(!isSlowed && currentSlowTimeLeft < maxSlowTime){
            currentSlowTimeLeft += Time.unscaledDeltaTime;
        }
        slowTimeUI.fillAmount = currentSlowTimeLeft/maxSlowTime;
        if(currentSlowTimeLeft <= 0 && isSlowed){
            StopAllCoroutines();
            StartCoroutine(GrappleTimeNormal());
            isSlowed = false;
        }
        if(currentSlowTimeLeft >= maxSlowTime){
            slowTimeUI.enabled = false;
        } 
    }

    private void SetTimeScale(float scale){
        Time.timeScale = scale;
        Time.fixedDeltaTime = scale * .02f;

    }

    private void FreeLookCameraSettings(){
        playerFreeLook.m_XAxis.m_MinValue = -90f;
        playerFreeLook.m_XAxis.m_MaxValue = 90f;
        playerFreeLook.m_YAxis.m_MaxSpeed = 5f;
        playerFreeLook.m_XAxis.m_Wrap = false;
        playerFreeLook.m_RecenterToTargetHeading.m_enabled = false;
    }
    private float freeLookYAxisVal;
    private void RollingCameraSettings(){
        StartCoroutine(ChangeYValue(playerFreeLook.m_YAxis.Value, 1, .5f));
        StartCoroutine(ChangeXValue(playerFreeLook.m_XAxis.m_MaxValue, 0, .5f));
        playerFreeLook.m_YAxis.m_MaxSpeed = 0f;
        playerFreeLook.m_XAxis.m_Wrap = false;
        
    }
    public IEnumerator ChangeYValue(float oldValue, float newValue, float duration) {
        for (float t = 0f; t < duration; t += Time.deltaTime) {
        playerFreeLook.m_YAxis.Value = Mathf.Lerp(oldValue, newValue, t / duration);
        yield return null;
        }
        playerFreeLook.m_YAxis.Value = newValue;
    }
    public IEnumerator ChangeXValue(float oldValue, float newValue, float duration) {
        for (float t = 0f; t < duration; t += Time.deltaTime) {
        playerFreeLook.m_XAxis.m_MinValue = Mathf.Lerp(-oldValue, newValue, t / duration);
        playerFreeLook.m_XAxis.m_MaxValue = Mathf.Lerp(oldValue, newValue, t / duration);
        yield return null;
        }
        playerFreeLook.m_XAxis.m_MinValue = -newValue;
        playerFreeLook.m_XAxis.m_MaxValue = newValue;
    }

    private void StartGrapple(){
        if(grapplingCdTimer > 0 || currentSlowTimeLeft <= 0) return;

        if(Time.timeScale < 1) {
            StopAllCoroutines();
            isSlowed = false;
            crossHair.enabled = false;
            Time.timeScale = 1f;
            RollingCameraSettings();
        }

        grappling = true;
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grapplable)){
            grapplePoint = hit.point;
            spotToSendPlayer = hit.transform.GetChild(0);
            freeze = true;
            Invoke(nameof(ExecuteGrapple), grapplableDelayTime);
        }
        else{
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grapplableDelayTime);
        }

    }

    private void ExecuteGrapple(){
        freeze = false;
        AudioSource.PlayClipAtPoint(grapple, camPos);
        grapplingCdTimer = grapplingCd;
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y -1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        JumpToPosition(spotToSendPlayer.position, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);

    }

    private void StopGrapple(){
        grappling = false;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        playerFreeLook.m_Lens.FieldOfView = grappleFov;
    }

    public void ResetRestrictions()
    {
        playerFreeLook.m_Lens.FieldOfView = 60f;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            StopGrapple();
        }
    }


    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ * grappleSpeedMultiplier / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}
