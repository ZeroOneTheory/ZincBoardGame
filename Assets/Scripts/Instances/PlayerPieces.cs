using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPieces : Pieces {

    private InputController input;
    private TouchController tInput;
 
    public int moveIndex;
    public Vector2 checkPosition;


    private void Start() {
        tInput = GameManager.Instance.TouchController;
        // Get MatchController
        currentPosition = SetValidCurrentPosition(transform.position);
        currentSpace = GetCurrentSpace();
    }

    private void Update() {

        if (mControl.selectedPiece == this) {
            GetTouchInputs();
            TelegraphNextMovePosition();

            if (moveIndex != 0 && tInput.isDragging == false) {
                if (goalSpace != currentSpace) {
                    mControl.currentTeamsTurn.turnState = TurnState.Moving;
                    if (Vector3.Distance(transform.position, goalSpace.GetPositionInWorldCoord()) > .125f) {
                        MovePlayer();
                    }
                    else {
                        currentPosition = SetValidCurrentPosition(goalSpace.GetPositionInWorldCoord());
                        currentSpace = goalSpace;
                        transform.position = currentSpace.GetPositionInWorldCoord();
                        moveIndex = 0;
                        if (mControl.currentTeamsTurn.turnState == TurnState.Moving) {
                            mControl.currentTeamsTurn.turnState = TurnState.Choosing;
                            mControl.currentTeamsTurn.teamMoveCount -= 1;
                        }
                    }
                }
            }
        }
        else {moveIndex = 0;}

            if (path != null) {
                DrawPathDebug();
            }
        

    }

    private void TelegraphNextMovePosition() {
        if (moveIndex != 0) {
            goalSpace.HighlighSpace();
        } else {
            board.UnHighlightAllPieces();
        }
    }

    public void HighlightRechableSpaces() {
        if (reachable.Count > 0) {
            foreach(Spaces s in reachable) {
                s.HighlighSpace();
            }
        }
    }

    private void GetTouchInputs() {

        if (tInput.isDragging) {
            moveIndex = tInput.GetDirectAsInt();
            tInput.startTouchPoint = Camera.main.WorldToScreenPoint(transform.position);
            tInput.startTouchPoint3 = transform.position;
            
            
            switch (moveIndex) {
                case 1: checkPosition = new Vector2(currentPosition.x, currentPosition.y-1); break;
                case 2: checkPosition = new Vector2(currentPosition.x, currentPosition.y+1); break;
                case 3: checkPosition = new Vector2(currentPosition.x-1, currentPosition.y); break;
                case 4: checkPosition = new Vector2(currentPosition.x+1, currentPosition.y); break;
                case 0: goalSpace = currentSpace; checkPosition = new Vector2(-1,-1); break;
            }
            if (board.CheckForValidSpace(checkPosition)) {
                goalSpace = board.GetSpaceAtLocation(checkPosition);
            }
            if (moveIndex != 0) {
                Debug.DrawLine(currentSpace.GetPositionInWorldCoord(), goalSpace.GetPositionInWorldCoord(), Color.red); // temp Telegraph move selection
            }
        } 
    }

    public void DrawPathDebug() {
        if (path != null) {
            int currNode = 0;
            while (currNode < path.Count - 1) {
                Vector3 start = path[currNode].GetPositionInWorldCoord();
                Vector3 end = path[currNode + 1].GetPositionInWorldCoord();
                Debug.DrawLine(start, end, Color.magenta);
                currNode++;
            }
        }
    }
}
