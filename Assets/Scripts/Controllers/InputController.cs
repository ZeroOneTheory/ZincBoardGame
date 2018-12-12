using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public bool left, right, up, down, check, leftMouse;

    private void Update() {

        up = Input.GetKeyDown(KeyCode.W);
        down = Input.GetKeyDown(KeyCode.S);
        left = Input.GetKeyDown(KeyCode.A);
        right = Input.GetKeyDown(KeyCode.D);
        check = Input.GetKeyDown(KeyCode.Space);
        leftMouse = Input.GetMouseButtonDown(0);
    }
}
