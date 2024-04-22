using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndPinch : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
    private Camera Camera;
    private bool Rotate = true;
    private Plane Plane;

    private bool m_isMoving = false;
    private bool m_isClamped = true;

    private InputManager m_inputManager = null;
    public bool ForceBlocked { get { return m_forceBlocked; } set { m_forceBlocked = value; } }
    private bool m_forceBlocked = false;
    private bool canMove = false;

    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;

        Main.Instance.OnInitialized += Initialize;
    }

    private void Initialize()
    {
        m_inputManager = Main.Instance.GetManager<InputManager>();
        if (m_inputManager != null) {
            m_inputManager.OnDragStart += ToggleMove;
            m_inputManager.OnDragStop += ToggleMove;
        }
    }

    private void ToggleMove(InputType inputType)
    {
        canMove = !canMove;
    }

    private void Update()
    {
        m_isMoving = false;

        if (!canMove || m_forceBlocked) return;

        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        //Scroll
        if (Input.touchCount >= 1) {
            var Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved) {
                Camera.transform.Translate(Delta1, Space.World);
            }

            m_isMoving = true;
            m_isClamped = !m_isMoving;
        }

        //Pinch
        if (Input.touchCount >= 2) {
            //TODO add a param to reduce speed of scroll and pinch
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);

            //edge case
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);

            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));

            m_isMoving = true;
            m_isClamped = !m_isMoving;
        }

        if (!m_isMoving && !m_isClamped) {
            ClampToIslandSize();
        }
    }

    public void ReCenter()
    {
        Camera.transform.LookAt(Main.Instance.GetManager<IslandManager>().Center);
    }

    private void ClampToIslandSize()
    {
        uint islandLevel = Main.Instance.GetManager<IslandManager>().Data.Level;

        Vector3 position = Camera.transform.position;
        var cameraMaxSize = islandLevel + 10;
        Camera.transform.position = new Vector3(
                Mathf.Clamp(position.x, -cameraMaxSize, cameraMaxSize), 
                position.y, 
                Mathf.Clamp(position.z, -cameraMaxSize, cameraMaxSize)
            );

        if (!position.Equals(Camera.transform.position))
            ReCenter();

        m_isClamped = true;
    }

    private Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    private Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}
