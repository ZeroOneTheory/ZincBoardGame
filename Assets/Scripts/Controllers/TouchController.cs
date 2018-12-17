using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    public float swipeDeadZone=200;

    public Vector2 startTouchPoint, swipeDelta;
    public bool tLeft, tRight, tUp, tDown;
    public bool isDragging = false;

    private void Update() {
        if (Input.touchCount > 1) {
             
            if (Input.touches[1].phase == TouchPhase.Began) {
                isDragging = true;
                startTouchPoint = Input.touches[1].position;
            }
            else if (Input.touches[1].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Canceled) {
                ResetTouches();
            }
        }

        //Calculate SwipeDelta
        swipeDelta = Vector2.zero;
        if (isDragging) {
            if (Input.touchCount > 0) {
                swipeDelta = Input.touches[1].position - startTouchPoint;
            }
        } 

        if (swipeDelta.magnitude > swipeDeadZone) {
            SetSwipeDirection();
        } else {
            tLeft = tRight = tUp = tDown = false;
        }



    }

    public void ResetTouches() {
        startTouchPoint = swipeDelta = Vector2.zero;
        isDragging = false;
    }

    public void SetSwipeDirection() {
        float x = swipeDelta.x;
        float y = swipeDelta.y;

        if (Mathf.Abs(x) > Mathf.Abs(y)) {
            //Left or Right
            if (x < 0) {
                //Left
                if (!CheckForSetDir()) { tLeft = true; }
            }
            else {
                //Right
                if (!CheckForSetDir()) { tRight = true; }
            }
        }
        else {
            //Up and Down
            if (y < 0) {
                //Down
                if (!CheckForSetDir()) { tDown = true; }
            }
            else {
                //Up
                if (!CheckForSetDir()) { tUp = true; }
            }
        }
        

    }

    public bool CheckForSetDir() {
        if(tLeft || tRight || tUp || tDown) {
            return true;
        } else {
            return false;
        }
    }

    public int GetDirectAsInt() {
        if (CheckForSetDir()) {
            if (tLeft) return 1;
            if (tRight) return 2;
            if (tUp) return 3;
            if (tDown) return 4;
        }
        return 0;
    }




}
