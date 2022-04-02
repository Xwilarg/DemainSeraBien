using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostMouse : MonoBehaviour
{
    private void Update()
    {
        transform.position = Camera.main.ViewportToWorldPoint(Mouse.current.position.ReadValue());
    }
}
