using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostMouse : MonoBehaviour
{
    [SerializeField]
    private Vector3 PlaneSpaceHeight;

    private Plane PositionPlane;

    private void Start()
    {
        PositionPlane = new Plane(Vector3.up, PlaneSpaceHeight);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (PositionPlane.Raycast(ray, out float Enter))
        {
            Vector3 hitPoint = ray.GetPoint(Enter);

            transform.position = hitPoint;
        }
    }

}
