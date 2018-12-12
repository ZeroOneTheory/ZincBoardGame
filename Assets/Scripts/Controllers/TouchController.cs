using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    public float swipeDeadZone;

    //[HideInInspector]
    public Vector2 startTouchPoint, swipeDelta;
    public bool isDragging = false;

    private void Update() {
        if (Input.touchCount > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                isDragging = true;
                startTouchPoint = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
                ResetTouches();
            }
        }

        //Calculate SwipeDelta
        swipeDelta = Vector2.zero;
        if (isDragging) {
            if (Input.touchCount > 0) {
                swipeDelta = Input.touches[0].position - startTouchPoint;
            }
        }

        if (swipeDelta.magnitude > swipeDeadZone) {
            SetSwipeDirection();
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
                Debug.Log("Left");
            }
            else {
                //Right
                Debug.Log("Right");
            }
        }
        else {
            //Up and Down
            if (y < 0) {
                //Down
                Debug.Log("Down");
            }
            else {
                //Up
                Debug.Log("Up");
            }
        }
        //ResetTouches();
    }




}
