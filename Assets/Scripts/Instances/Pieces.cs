using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Pieces : MonoBehaviour {

    public int moveCount = 0;
    public bool isActive = false;
    public float moveSpeed=1;

    public Piece piece;
    public Vector2 currentPosition = new Vector3(0,0);
    public Spaces currentSpace;
    public Spaces goalSpace;
    public List<Spaces> path = new List<Spaces>();
    public List<Spaces> reachable = new List<Spaces>();

    public BoardManager board;
    public MatchController mControl;

    public void Awake() {
        board = GameManager.Instance.BoardManager;
        mControl = FindObjectOfType<MatchController>();
    }

    public void LateUpdate() {
        if (mControl.selectedPiece != this) {
            isActive = false;
        }
    }


    public Spaces GetCurrentSpace() {
        SetValidCurrentPosition(transform.position);

        if (board != null & board.spacePositions[(int)currentPosition.x, (int)currentPosition.y] != null) {
            return board.spacePositions[(int)currentPosition.x, (int)currentPosition.y];
        }
        else {
            Debug.LogWarning("Board var has null value");
            Spaces failed = new Spaces();
            return failed;
        }

    }

    public Vector2 SetValidCurrentPosition(Vector3 pos) {
        if (pos.x >-1 & pos.x < board.mapSize.x & pos.z > -1 & pos.z < board.mapSize.y) {
            return new Vector2(pos.x, pos.z);
        }
        else {
            return new Vector2(0, 0);
        }
    }

    public void GetReachableSpaces() {
        int count = 1;
        reachable.Clear();
        foreach (Spaces s in currentSpace.edges) {
            reachable.Add(s);
        }
        while (count > 1) {
            
            List<Spaces> edgesInList = new List<Spaces>();
            for(int i=0; i<reachable.Count; i++) {
                foreach(Spaces ss in reachable[i].edges) {
                    edgesInList.Add(ss);
                }
            }
            foreach(Spaces sss in edgesInList) {
                if (!reachable.Contains(sss)) { reachable.Add(sss); }
            }
            count -= 1;
        }

    }

    public void MovePlayer() {
       transform.position = Vector3.Lerp(transform.position, goalSpace.GetPositionInWorldCoord(), Time.deltaTime * moveSpeed);
    }

}
