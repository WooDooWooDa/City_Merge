using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private LayerMask buildingLayer;
    [SerializeField]
    private LayerMask snapLayer;

    private InputManager m_inputManager;
    private Building m_building;

    private Vector3 lastPosition;
    private Vector3 targetPosition;

    private bool isActive;
    private bool isDragging;
    private float threshold = 0.1f;
    private float timePassed;

    private void Start()
    {
        m_building = GetComponent<Building>();
        m_inputManager = Main.Instance.GetManager<InputManager>();

        m_inputManager.OnTouchStart += TouchStart;
        m_inputManager.OnDragStart += DragStart;
        m_inputManager.OnTouchEnded += TouchStop;
    }

    void Update()
    {
        if (isActive && isDragging) {
            Dragging();
        }
        Animate();
    }

    private void OnDestroy()
    {
        m_inputManager.OnTouchStart -= TouchStart;
        m_inputManager.OnDragStart -= DragStart;
        m_inputManager.OnTouchEnded -= TouchStop;
    }

    private void TouchStart(InputType inputType)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputType == InputType.Touch 
            ? Input.touches[0].position
            : (Vector2)Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (!hit.transform.IsChildOf(transform)) return;
            if (buildingLayer == (buildingLayer | (1 << hit.transform.gameObject.layer))) {
                FindObjectOfType<ScrollAndPinch>().ForceBlocked = true;
                isActive = true;
            }
        }
    }

    private void DragStart(InputType inputType)
    {
        if (!isActive) return;

        isDragging = true;
    }

    private void TouchStop(InputType inputType)
    {
        if (!isActive) return;

        if (!isDragging) {
            Tap(); //open building info
        } else {
            Lifted();
        }

        FindObjectOfType<ScrollAndPinch>().ForceBlocked = false;
        isActive = false;
        isDragging = false;
    }

    private void Tap()
    {
        //open info 
    }

    private void Dragging()
    {
        Animate();

        if (lastPosition == Vector3.zero) {
            lastPosition = transform.position;
            targetPosition = transform.position;
        }
        Vector3 pos = Vector3.zero;
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved) {
            pos = Input.GetTouch(0).position;
        } else {
            pos = Input.mousePosition;
        }

        if (pos != Vector3.zero) {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, snapLayer)) {
                Vector3 newPos = hit.transform.position;

                transform.position = newPos;
                targetPosition = newPos;

                //TODO Add a outline to the target building at location if there is
            }
        }
    }

    private void Lifted()
    {
        if (lastPosition != transform.position) {
            if (TargetIsOtherBuilding(out Building other)) {
                if (other == null || !Main.Instance.GetManager<BuildingManager>().
                    TryMerge(other, m_building)) {
                    transform.position = lastPosition;
                }
                return;
            }

            if (TargetIsGrid(out Tile tile)) {
                Main.Instance.GetManager<IslandManager>().MoveObjectTo(tile, m_building);
                transform.position = targetPosition;
            }
        }

        lastPosition = Vector3.zero;
    }

    private bool TargetIsOtherBuilding(out Building other)
    {
        Collider[] hits = (Physics.OverlapSphere(targetPosition, 0.25f, buildingLayer));
        foreach (var hit in hits) {
            if (!hit.transform.IsChildOf(transform)) {
                other = hit.gameObject.GetComponentInParent<Building>();
                return true;
            }
        }
        other = null;
        return false;
    }

    private bool TargetIsGrid(out Tile tile)
    {
        tile = null;
        Debug.DrawRay(targetPosition, Vector3.up, Color.red, 2f);
        Collider[] snaps = Physics.OverlapSphere(targetPosition, 0.5f, snapLayer);
        if (snaps.Length > 0) {
            tile = snaps[0].gameObject.GetComponentInParent<Tile>();
            //if (tile.has)
            return true;
        }
        return false;
    }

    private void Animate()
    {
        animator.SetBool("isDragging", isActive && isDragging);
    }
}
