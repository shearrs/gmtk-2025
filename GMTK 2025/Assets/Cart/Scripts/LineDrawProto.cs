using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class LineDrawProto : MonoBehaviour
{
    InputAction moveAction;
    InputAction driftAction;

    LineRenderer lineDraw;

    private List<Vector3> points = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        driftAction = InputSystem.actions.FindAction("Sprint");
        GetComponent<Renderer>().material.color = Color.red;
        lineDraw = GetComponent<LineRenderer>();
        lineDraw.startWidth = 2f;
        lineDraw.endWidth = 2f;
    }

    // Update is called once per frame
    void Update()
    {

        //if(moveAction.IsPressed())
        //{
        //    Vector2 moveDir = moveAction.ReadValue<Vector2>();
        //    gameObject.transform.Translate(Vector3.forward * -moveDir.y * Time.deltaTime * 20f);
        //    gameObject.transform.Rotate(new Vector3(0f, moveDir.x / 5, 0f));
        //}

        if(driftAction.WasPressedThisFrame())
        {
            points.Clear();
        }

        if (driftAction.IsPressed())
        {
            Ray ray = new Ray(gameObject.transform.position, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 10f))
            {
                if(PointDistance(hitInfo.point) > 1f)
                {
                    points.Add(hitInfo.point);

                    lineDraw.positionCount = points.Count;
                    lineDraw.SetPositions(points.ToArray());
                }
            }
        }

        if(driftAction.WasReleasedThisFrame())
        {
            lineDraw.positionCount = 0;
        }
    }

    private float PointDistance(Vector3 point)
    {
        if(!points.Any())
        {
            return Mathf.Infinity;
        } else
        {
            return Vector3.Distance(points.Last(), point);
        }
    }

}
