using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieces : Pieces {

    private InputController input;

    public BoardManager board;
    //public MatchController matchController;


    private void Awake() {
        input = GameManager.Instance.InputController;
        board = GameManager.Instance.BoardManager;
        Debug.Log(board);
        //matchController = MatchController.Instance;

    }

    private void Start() {

        currentPosition = SetValidCurrentPosition();
        currentSpace = GetCurrentSpace();
    }

    private void LateUpdate() {

        if (board != null) {
            if (board.selectedUnit == this) {
                GetInputs();
            }

            if (path != null) {
                DrawPathDebug();
            }
        }

    }

    private void GetInputs() {

        if (input.left || input.right || input.up || input.down) {
            path = null;
            board.UnHighlightAllPieces();
        }
            if (input.left) { this.transform.position = this.transform.position + new Vector3(0, 0, -1); currentSpace = GetCurrentSpace(); }
            if (input.right) { this.transform.position = this.transform.position + new Vector3(0, 0, 1); currentSpace = GetCurrentSpace(); }
            if (input.up) { this.transform.position = this.transform.position + new Vector3(-1, 0, 0); currentSpace = GetCurrentSpace(); }
            if (input.down) { this.transform.position = this.transform.position + new Vector3(1, 0, 0); currentSpace = GetCurrentSpace(); }

    }

    public Spaces GetCurrentSpace() {
        SetValidCurrentPosition();
        if (board != null & board.spacePositions[(int)currentPosition.x, (int)currentPosition.y] != null) {
            return board.spacePositions[(int)currentPosition.x,(int)currentPosition.y];
        } else {
            Debug.LogWarning("Board var has null value");
            Spaces failed = new Spaces();
            return failed;
        }
        
    }

    public Vector2 SetValidCurrentPosition() {
        if (transform.position.x > -1 & transform.position.x < board.mapSize.x & transform.position.z > -1 & transform.position.z < board.mapSize.y) {
            return new Vector2(transform.position.x, transform.position.z);
        } else {
            return new Vector2(0,0);
        }
    }

    public void DrawPathDebug() {
        if (path != null) {
            int currNode = 0;
            while (currNode < path.Count - 1) {
                Vector3 start = path[currNode].GetPositionInWorldCoord();
                Vector3 end = path[currNode + 1].GetPositionInWorldCoord();
                Debug.DrawLine(start, end, Color.blue);
                currNode++;
            }
        }
    }
}
