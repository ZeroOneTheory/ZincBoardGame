using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxArrow : MonoBehaviour {

    [SerializeField]
    private Transform planeTransform;
    public float arrowWidth =.1f;
    public float maxScale;

    public void ScaleArrowFromPosition(Vector2 startPosition, Vector2 endPosition, Vector2 deltaPosition) {
        Vector3 setScale = planeTransform.localScale;

        if( deltaPosition.x > deltaPosition.y) {
            // Left-right scale
            setScale.z = Mathf.InverseLerp(Vector2.Distance(startPosition, endPosition),1, maxScale);
            setScale.x = arrowWidth;
        } else {
            //Up-down scale
            setScale.x = Mathf.InverseLerp(Vector2.Distance(startPosition, endPosition),1, maxScale);
            setScale.z = arrowWidth;
        }
        planeTransform.localScale = setScale;
        

    }

   
}
