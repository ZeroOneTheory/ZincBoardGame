using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Pieces : MonoBehaviour {

    public int moveCount = 0;
    public bool isActive = false;

    public Piece piece;
    public Vector2 currentPosition = new Vector2(0,0);
    public Spaces currentSpace;
    public List<Spaces> path = new List<Spaces>();
    

    // -------- Editor -------------
    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}
