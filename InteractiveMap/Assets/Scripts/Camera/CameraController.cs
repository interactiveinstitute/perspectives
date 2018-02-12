using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Utils.Geom;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public TransformGesture PanGesture;
    public TransformGesture ScaleAndRotateGesture;
    public ScreenTransformGesture TiltGestureFullscreen;

    //public float cameraHeightLimitMin;
    //public float cameraHeightLimitMax;

    public Transform pivot;
    public Camera cam;

    [Range(1.0f, 20.0f)]
    public float lerpSpeed;

    [Range(100.0f, 2000.0f)]
    public float flySpeed;

    [Range(0.0f, 1.0f)]
    public float scrollSpeed;

    public float zoomScale;
    public AnimationCurve heightFactorCurve;
    public float tiltScale;

    [Header("Follow target")]
    Transform followTarget = null;
    private bool follow = false;
    [Range(0.1f, 1000.0f)]
    public float targetStartDistance;
    [Range(0.0f, 1.0f)]
    public float followTargetLocationSpeed;
    [Range(0.0f, 1.0f)]
    public float followPivotOrientationSpeed;

    [Header("Move to")]
    [Range(0f, 1f)]
    public float moveToLocationStartSpeed;
    [Range(0f, 1f)]
    public float moveToOrientationStartSpeed;
    public float moveToTime;
    private bool isMoving;

    private Vector3 followStartPosition;

    private Vector3 targetLocation;

    private Vector3 pivotTargetPosition;
    private Quaternion pivotTargetRotation;

    private Vector3 lastWorldHit;
    private Vector3 mousePrevPos;
    private Vector3 rotationWorldPos;
    private bool validRotationLocation;

    private Vector2 mouseScreenPos;
    private Plane transformPlane;

    [Tooltip("The point that gestures will focus on, rotate around, zoom towards, drag to, etc... Do not set this manually")]
    public Vector3 interactionPoint;

    private void Start()
    {
        isMoving = false;
        targetLocation = transform.localPosition;
        pivotTargetPosition = pivot.localPosition;
        pivotTargetRotation = pivot.localRotation;

        transformPlane = new Plane(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
    }

    private void OnEnable()
    {
        PanGesture.Transformed += PanGesture_Transformed;

        ScaleAndRotateGesture.Transformed += ScaleAndRotateGesture_Transformed;
        ScaleAndRotateGesture.TransformStarted += ScaleAndRotateGesture_TransformStarted;
        ScaleAndRotateGesture.TransformCompleted += ScaleAndRotateGesture_TransformCompleted;

        TiltGestureFullscreen.Transformed += TiltGesture_Transformed;
    }

    private void ScaleAndRotateGesture_TransformCompleted(object sender, System.EventArgs e)
    {
        //Debug.Log("Completed");
    }

    private void ScaleAndRotateGesture_TransformStarted(object sender, System.EventArgs e)
    {
        //Debug.Log("Started");
    }

    private void OnDisable()
    {
        PanGesture.Transformed -= PanGesture_Transformed;

        ScaleAndRotateGesture.Transformed -= ScaleAndRotateGesture_Transformed;
        ScaleAndRotateGesture.TransformStarted -= ScaleAndRotateGesture_TransformStarted;
        ScaleAndRotateGesture.TransformCompleted -= ScaleAndRotateGesture_TransformCompleted;

        TiltGestureFullscreen.Transformed += TiltGesture_Transformed;
    }

    void Update()
    {
        if (followTarget == null && follow)
        {
            StopFollow();
        }
        else if (followTarget != null && !follow)
        {
            StartFollow();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (follow)
            {
                StopFollow();
            }
            else
            {
                StartFollow();
            }
        }

        if (follow)
        {
            Vector3 viewDirTarget = followTarget.position - cam.transform.position;
            pivotTargetRotation = Quaternion.LookRotation(viewDirTarget, Vector3.up);
            //pivotTargetPosition = followTarget.position - viewDirTarget.normalized * targetStartDistance;
        }

        var forward = cam.transform.forward;
        forward.y = 0.0f;
        if (Input.GetKey(KeyCode.W))
        {
            targetLocation += forward * Time.deltaTime * flySpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            targetLocation -= forward * Time.deltaTime * flySpeed;
        }

        if (Input.GetMouseButtonDown(1))
        {
            mousePrevPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            var mouseCurrentPos = Input.mousePosition;
            //Tilt((mousePrevPos.y - mouseCurrentPos.y) * 0.15f);
            //Rotate((mousePrevPos.y - mouseCurrentPos.y) * 0.15f, rotationWorldPos, Vector3.Cross( pivot.forward, Vector3.up )); // Tilt
            if (validRotationLocation)
            {
                Rotate((mouseCurrentPos.y - mousePrevPos.y) * 0.15f, rotationWorldPos, Vector3.Cross(pivot.forward, Vector3.up)); // Tilt
                Rotate((mouseCurrentPos.x - mousePrevPos.x) * 0.2f, rotationWorldPos, Vector3.up);
            }
            else
            {
                Tilt((mousePrevPos.y - mouseCurrentPos.y) * 0.15f);
            }
            mousePrevPos = mouseCurrentPos;
        }

        var lerpFactor = Time.deltaTime * lerpSpeed;
        float targetLocationIP = lerpFactor;
        float pivotTargetPositionIP = lerpFactor;
        float pivotTargetRotationIP = lerpFactor;
        if (follow)
        {
            targetLocationIP = followTargetLocationSpeed;
            pivotTargetPositionIP = followPivotOrientationSpeed;
            pivotTargetRotationIP = followPivotOrientationSpeed;
        }

        if (follow)
        {
            targetLocation = followTarget.position - followStartPosition;
            interactionPoint = followTarget.position;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocation, targetLocationIP);
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, pivotTargetPosition, pivotTargetPositionIP);
        pivot.localRotation = Quaternion.Lerp(pivot.localRotation, pivotTargetRotation, pivotTargetRotationIP);

        // Keep straight
        Vector3 pivotTargetRotationEuler = pivotTargetRotation.eulerAngles;
        pivotTargetRotation = Quaternion.Euler(pivotTargetRotationEuler.x, pivotTargetRotationEuler.y, 0.0f);

        // Distance to focus
        //float focusDistance = Vector3(
    }

    private void StartFollow()
    {
        follow = true;
        Vector3 viewDirTarget = followTarget.position - cam.transform.position;
        pivotTargetRotation = Quaternion.LookRotation(viewDirTarget, Vector3.up);
        pivotTargetPosition = followTarget.position - viewDirTarget.normalized * targetStartDistance;
        targetLocation = Vector3.zero;
        followStartPosition = followTarget.position;
    }

    public bool MoveTo(Transform moveToTransform)
    {
        //followTarget = moveToTransform;
        //follow = true;
        //StartFollow();

        //if (!isMoving)
        //{
        //    StartCoroutine(InterpolateMoveToSpeed(moveToTime));
        //}

        if (isMoving)
        {
            Debug.LogWarning("Can't move to a new location before finished moving to current");

            return false;
        }
        else
        {
            followTarget = moveToTransform;
            follow = true;
            StartFollow();

            StartCoroutine(InterpolateMoveToSpeed(moveToTime));

            return true;
        }
    }

    IEnumerator InterpolateMoveToSpeed(float time)
    {
        isMoving = true;
        float followTargetLocationSpeedTmp = followTargetLocationSpeed;
        float followPivotOrientationSpeedTmp = followPivotOrientationSpeed;

        float t = 0;
        while (t < time)
        {
            followTargetLocationSpeed = Mathf.Lerp(moveToLocationStartSpeed, followTargetLocationSpeedTmp, t / time);
            followPivotOrientationSpeed = Mathf.Lerp(moveToOrientationStartSpeed, followPivotOrientationSpeedTmp, t / time);

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        followTargetLocationSpeed = followTargetLocationSpeedTmp;
        followPivotOrientationSpeed = followPivotOrientationSpeedTmp;
        isMoving = false;
    }

    public void StopFollow()
    {
        follow = false;
        followTarget = null;
        //StopAllCoroutines();
        //isMoving = false;
    }

    public void SetTargetWorldLocation(Vector3 worldLocation)
    {
        Vector3 pivotWorldPosition = pivot.TransformPoint( Vector3.zero );
        Vector3 backVec = pivotWorldPosition - worldLocation;
        targetLocation -= backVec * 1.0f;
    }

    private void TiltGesture_Transformed(object sender, System.EventArgs e)
    {
        //Debug.Log("TiltGesture_Transformed");
        Tilt(((ScreenTransformGesture)sender).DeltaPosition.y * 0.5f);
    }

    private void Zoom(float value)
    {
        if (follow)
        {
            var tempPos = pivot.localPosition;
            pivot.localPosition = pivotTargetPosition;
            var camPos = pivot.position;
            var dirVec = followTarget.position - camPos;
            pivot.position += dirVec * value * scrollSpeed;
            pivotTargetPosition = pivot.localPosition;
            pivot.localPosition = tempPos;
        }
        else
        {
            RaycastHit hit;
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                var tempPos = pivot.localPosition;
                pivot.localPosition = pivotTargetPosition;
                var camPos = pivot.position;
                var dirVec = hit.point - camPos;
                pivot.position += dirVec * value * scrollSpeed;
                pivotTargetPosition = pivot.localPosition;
                pivot.localPosition = tempPos;

                interactionPoint = hit.point;
            }
        }
    }

    private void Tilt(float value)
    {
        var tempRot = pivot.localRotation;
        pivot.localRotation = pivotTargetRotation;
        pivot.Rotate(Vector3.right, value, Space.Self);
        pivotTargetRotation = pivot.localRotation;
        pivot.localRotation = tempRot;
    }

    private void Rotate(float value, Vector3 position, Vector3 axis)
    {
        var tempPos = pivot.localPosition;
        var tempRot = pivot.localRotation;

        pivot.localPosition = pivotTargetPosition;
        pivot.localRotation = pivotTargetRotation;

        pivot.RotateAround(position, axis, value);

        pivotTargetPosition = pivot.localPosition;
        pivotTargetRotation = pivot.localRotation;

        pivot.localPosition = tempPos;
        pivot.localRotation = tempRot;
    }

    private void ScaleAndRotateGesture_Transformed(object sender, System.EventArgs e)
    {
        Vector2 midPoint = Vector2.zero;
        Vector2 midPointPrev = Vector2.zero;
        int numPoints = ScaleAndRotateGesture.ActivePointers.Count;
        for (int i = 0; i < numPoints; i++)
        {
            midPoint += ScaleAndRotateGesture.ActivePointers[i].Position;
            midPointPrev += ScaleAndRotateGesture.ActivePointers[i].PreviousPosition;
        }
        midPoint /= numPoints;
        midPointPrev /= numPoints;

        float dY = midPointPrev.y - midPoint.y;


        float distanceToMid = 0.0f;
        float distanceToMidPrev = 0.0f;
        for (int i = 0; i < numPoints; i++)
        {
            distanceToMid += Vector2.Distance( midPoint, ScaleAndRotateGesture.ActivePointers[i].Position);
            distanceToMidPrev += Vector2.Distance(midPointPrev, ScaleAndRotateGesture.ActivePointers[i].PreviousPosition);
        }
        distanceToMid /= numPoints;
        distanceToMidPrev /= numPoints;
        float deltaDistanceToMid = distanceToMidPrev - distanceToMid;


        lastWorldHit = ProjectionUtils.CameraToPlaneProjection(midPoint, cam, transformPlane);

        var tempPos = pivot.localPosition;
        var tempRot = pivot.localRotation;

        pivot.localPosition = pivotTargetPosition;
        pivot.localRotation = pivotTargetRotation;

        var camPos = pivot.position;
        var dirVec = lastWorldHit - camPos;

        float distanceToLastWorldHit = dirVec.magnitude;

        //Debug.Log("Eval, distanceToLastWorldHit: " + heightFactorCurve.Evaluate(distanceToLastWorldHit) + ", " + distanceToLastWorldHit);
        float customScale = 1.0f + deltaDistanceToMid / zoomScale *  heightFactorCurve.Evaluate( distanceToLastWorldHit );
        pivot.position = lastWorldHit - dirVec * customScale;

        pivot.RotateAround(lastWorldHit, Vector3.up, -ScaleAndRotateGesture.DeltaRotation);

        pivot.RotateAround(lastWorldHit, cam.transform.right, dY * tiltScale);

        pivotTargetPosition = pivot.localPosition;
        pivotTargetRotation = pivot.localRotation;

        pivot.localPosition = tempPos;
        pivot.localRotation = tempRot;
    }

    private void PanGesture_Transformed(object sender, System.EventArgs e)
    {
        //Debug.Log("PanGesture_Transformed, DeltaPosition: " + PanGesture.DeltaPosition);
        targetLocation = targetLocation - PanGesture.DeltaPosition;
    }

    public void MouseDown(BaseEventData eventData)
    {
        //Debug.Log("MouseDown");
        var pointerEvent = eventData as PointerEventData;

        if (pointerEvent != null)
        {
            transformPlane.SetNormalAndPosition(new Vector3(0.0f, 1.0f, 0.0f), pointerEvent.pointerCurrentRaycast.worldPosition);

            if (pointerEvent.button == PointerEventData.InputButton.Left)
            {
                mouseScreenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                RaycastHit hit;
                Ray rayFromScreenPoint = cam.ScreenPointToRay(mouseScreenPos);
                if (Physics.Raycast(rayFromScreenPoint, out hit, 10000.0f))
                {
                    interactionPoint = hit.point;
                }
#if false
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = pointerEvent.pointerCurrentRaycast.worldPosition;
                go.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
#endif
            }
            else if (pointerEvent.button == PointerEventData.InputButton.Right)
            {
                if (follow)
                {
                    mousePrevPos = Input.mousePosition;
                    rotationWorldPos = followTarget.position;
                    validRotationLocation = true;
                }
                else
                {
                    mousePrevPos = Input.mousePosition;
                    RaycastHit hit;
                    Ray rayFromScreenPoint = cam.ScreenPointToRay(Input.mousePosition);
                    //if (Physics.Raycast(new Ray(cam.transform.position, cam.transform.forward), out hit, 5000.0f))
                    //{
                    if (Physics.Raycast(rayFromScreenPoint, out hit, 10000.0f))
                    {
                        rotationWorldPos = hit.point;
                        //Debug.Log("rotationWorldPos: " + rotationWorldPos);
                        validRotationLocation = true;
                        interactionPoint = rotationWorldPos;
                    }
                    else
                    {
                        validRotationLocation = false;
                    }
                }
            }
        }
    }

    public void MouseDrag(BaseEventData eventData)
    {
        //Debug.Log("MouseDrag");
        var pointerEvent = eventData as PointerEventData;

        if (pointerEvent != null)
        {
            if (!follow)
            {
                if (pointerEvent.button == PointerEventData.InputButton.Left)
                {
                    var currentMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                    var delta = ProjectionUtils.CameraToPlaneProjection(mouseScreenPos, cam, transformPlane) -
                                ProjectionUtils.CameraToPlaneProjection(currentMousePos, cam, transformPlane);

                    targetLocation = targetLocation + delta;

                    interactionPoint += delta;

                    mouseScreenPos = currentMousePos;
                }
            }
        }
    }

    public void MouseScroll(BaseEventData eventData)
    {
        //Debug.Log("MouseScroll");
        var pointerEvent = eventData as PointerEventData;
        if (pointerEvent != null)
        {
            Zoom(pointerEvent.scrollDelta.y);
        }
    }
}
