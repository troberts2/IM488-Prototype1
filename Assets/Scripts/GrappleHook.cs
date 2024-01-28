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


    //Time stuff
    private bool isSlowed = false;
    [SerializeField] private float timeSlowPercentage = .2f; //use values 0-1 (think of it as percentage)

    //grapple ability
    [Header("For Grappling")]
    [SerializeField] private Transform cam;
    public Transform shootPt;
    [SerializeField] private LayerMask grapplable;

    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grapplableDelayTime;
    [SerializeField] private float overshootYAxis;
    [SerializeField] private float grappleFov = 80f;

    internal Vector3 grapplePoint;

    [SerializeField] private float grapplingCd;
    private float grapplingCdTimer;

    internal bool grappling;

    //temp before combine with other player move script
    private bool freeze;
    private bool activeGrapple;
    private bool enableMovementOnNextTouch;


    private void Start() {
        rb = GetComponent<Rigidbody>();
        playerFreeLook = playerCam.GetComponent<CinemachineFreeLook>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        RollingCameraSettings();
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0) && isSlowed) StartGrapple();

        if(grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;

        if(freeze) rb.velocity = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.Space) && !isSlowed){
            StopCoroutine(GrappleTimeNormal());
            StartCoroutine(GrappleTimeSlow());
            isSlowed = true;
        } 
        if(Input.GetKeyUp(KeyCode.Space) && isSlowed){
            StopCoroutine(GrappleTimeSlow());
            StartCoroutine(GrappleTimeNormal());
            isSlowed = false;
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
        for(float i = 1; i >= timeSlowPercentage; i -= Time.deltaTime){
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

    private void RollingCameraSettings(){
        playerFreeLook.m_XAxis.m_MinValue = 0;
        playerFreeLook.m_XAxis.m_MaxValue = 0;
        playerFreeLook.m_YAxis.m_MaxSpeed = 0f;
        playerFreeLook.m_YAxis.Value = 1f;
        playerFreeLook.m_RecenterToTargetHeading.m_enabled = true;
        playerFreeLook.m_XAxis.m_Wrap = false;
        //playerFreeLook.m_RecenterToTargetHeading.RecenterNow();
    }

    private void StartGrapple(){
        if(grapplingCdTimer > 0) return;

        if(Time.timeScale < 1) {
            StopCoroutine(GrappleTimeSlow());
            StartCoroutine(GrappleTimeNormal());
            isSlowed = false;
        } 

        grappling = true;
        freeze = true;
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grapplable)){
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grapplableDelayTime);
        }
        else{
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grapplableDelayTime);
        }

    }

    private void ExecuteGrapple(){
        freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y -1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);

    }

    private void StopGrapple(){
        grappling = false;
        grapplingCdTimer = grapplingCd;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

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
        activeGrapple = false;
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
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}
