using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    public float swipeDeadZone=200;
    public float swipeDeadZone3D = 5;
    public float testMag;
    public int touchLayer = 1 << 11;
    public Ray touchRay;
    public RaycastHit touchRayHitInfo;

    public Vector2 startTouchPoint, swipeDelta, currTouchPoint;
    public Vector3 startTouchPoint3, swipeDelta3, currTouchPoint3;
    public bool tLeft, tRight, tUp, tDown;
    public bool isDragging = false;

    private void Update() {
        if (Input.touchCount > 0) {
            Get3DTouchPoints();
            currTouchPoint = Input.touches[0].position;
            if (Input.touches[0].phase == TouchPhase.Began) {
                isDragging = true;
                startTouchPoint = Input.touches[0].position;
                startTouchPoint3 = currTouchPoint3;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
                ResetTouches();
                ResetTouches3D();
            }
        } else {
            currTouchPoint = Vector2.zero;
            currTouchPoint3 = Vector3.zero;
        }

        //Calculate SwipeDelta
        swipeDelta = Vector2.zero;
        swipeDelta3 = Vector3.zero;
        if (isDragging) {
            
            if (Input.touchCount > 0) {
                swipeDelta = Input.touches[0].position - startTouchPoint;
                swipeDelta3 = currTouchPoint3 - startTouchPoint3;
            }
        }

        //if (swipeDelta.magnitude > swipeDeadZone) {
        //    SetSwipeDirection();
        //} else {
        //    tLeft = tRight = tUp = tDown = false;
        //}
        testMag = swipeDelta3.magnitude;
        if (swipeDelta3.magnitude > swipeDeadZone3D) {
            SetSwipeDirection3D();
        }
        else {
            tLeft = tRight = tUp = tDown = false;
        }



    }

    public void ResetTouches() {
        startTouchPoint = swipeDelta = currTouchPoint= Vector2.zero;
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

    public void SetSwipeDirection3D() {
        float x = swipeDelta3.x;
        float y = swipeDelta3.z;

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

    public void Get3DTouchPoints() {
        touchRay = Camera.main.ScreenPointToRay(Input.touches[0].position);  
        if(Physics.Raycast(touchRay,out touchRayHitInfo, 1000f, touchLayer)) {
            currTouchPoint3 = touchRayHitInfo.point;
        } else {
            currTouchPoint3 = Vector3.zero;
        }
}

    private void ResetTouches3D() {
        startTouchPoint3 = swipeDelta3 = currTouchPoint3 = Vector3.zero;
    }
}
