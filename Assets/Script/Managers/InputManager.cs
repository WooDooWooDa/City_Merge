using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputType
{
    Mouse,
    Touch
}

public class InputManager : Manager
{
    public override string LoadingInfo => "";

    private bool m_hasPressed = false;
    private bool m_hasStartedDrag = false;
    private bool m_hasMoved = false;

    public Action<InputType> OnTouchStart;
    public Action<InputType> OnTouchEnded;
    public Action<InputType, Vector3> OnTap;
    public Action<InputType> OnDragStart;
    public Action<InputType> OnDragStop;

    private float m_elapsedTime = 0f;
    private float m_maxTapTime = 0.2f;

    private Vector3 lastMousePos = Vector3.zero;

    public override void Initialize()
    {
        base.Initialize();

        //TODO on tap event, add an effect like azurlane, !make it a param!
        Events.Loading.OnLoadingPageClose += Ready;

        IsInitialize = false;
    }

    private void Ready()
    {
        IsInitialize = true;
        Events.Loading.OnLoadingPageClose -= Ready;
    }

    public override void OnUpdate(float delta)
    {
        if (!IsInitialize) return;

        CheckTouch();
        CheckClick();

        m_elapsedTime += delta;
    }

    private void CheckClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            m_elapsedTime = 0f;
            if (!PointerIsOverUI(Input.mousePosition)) {
                OnTouchStart?.Invoke(InputType.Mouse);
                m_hasPressed = true;
                m_hasMoved = false;

                lastMousePos = Input.mousePosition;
            }
        }
        if (m_hasPressed && !m_hasStartedDrag && Input.GetKey(KeyCode.Mouse0)) {
            if (!PointerIsOverUI(Input.mousePosition)) {
                if (!m_hasStartedDrag && lastMousePos != Input.mousePosition) {
                    OnDragStart?.Invoke(InputType.Mouse);
                    if (m_hasStartedDrag) {
                        OnDragStop?.Invoke(InputType.Mouse);
                    }
                    m_hasStartedDrag = true;
                    m_hasMoved = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            if (!PointerIsOverUI(Input.mousePosition)) {
                OnTouchEnded?.Invoke(InputType.Mouse);
                if (!m_hasMoved && m_elapsedTime <= m_maxTapTime) {
                    OnTap?.Invoke(InputType.Mouse, (Vector2)Input.mousePosition);
                }
                if (m_hasStartedDrag) {
                    OnDragStop?.Invoke(InputType.Mouse);
                }

                m_hasStartedDrag = m_hasPressed = m_hasMoved = false;
            }
        }
    }

    private void CheckTouch()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began) {
            m_elapsedTime = 0f;
            if (!PointerIsOverUI(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y))) {
                debugger.Log("Touch Started");
                OnTouchStart?.Invoke(InputType.Touch);
                m_hasPressed = true;
                m_hasMoved = false;
            }
        }

        if (m_hasPressed && !m_hasStartedDrag && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved) {
            if (!PointerIsOverUI(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y))) {
                debugger.Log("Dragging started");
                OnDragStart?.Invoke(InputType.Touch);
                m_hasMoved = true;
                m_hasStartedDrag = true;
            }
        }

        if (m_hasPressed && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended) {
            debugger.Log("Touch Ended, Elapsed Time : " + m_elapsedTime);
            if (!PointerIsOverUI(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y))) {
                OnTouchEnded?.Invoke(InputType.Touch);
                if (!m_hasMoved && m_elapsedTime <= m_maxTapTime) {
                    OnTap?.Invoke(InputType.Touch, Input.touches[0].position);
                }
                if (m_hasStartedDrag) {
                    OnDragStop?.Invoke(InputType.Touch);
                }

                m_hasStartedDrag = m_hasPressed = m_hasMoved = false;
            }
        }
    }

    public override bool Save(ref SaveData data)
    {
        return true;
    }

    public override bool Load(ref SaveData data)
    {
        return true;
    }

    public bool PointerIsOverUI(Vector2 screenPos)
    {
        var hitObject = UIRaycast(ScreenPosToPointerData(screenPos));
        return hitObject != null && hitObject.layer == LayerMask.NameToLayer("UI");
    }

    private GameObject UIRaycast(PointerEventData pointerData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count < 1 ? null : results[0].gameObject;
    }

    private PointerEventData ScreenPosToPointerData(Vector2 screenPos)
    {
        return new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };
    }
}
