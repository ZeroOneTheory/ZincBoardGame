using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPieces : Pieces {

    private InputController input;
    private TouchController tInput;
 
    public bool nextMoveChoosen;
    public Spaces NextSpace;


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

            if (nextMoveChoosen && tInput.isDragging == false) {
                if (goalSpace != currentSpace) {
                    mControl.currentTeamsTurn.turnState = TurnState.Moving;
                    if (Vector3.Distance(transform.position, goalSpace.GetPositionInWorldCoord()) > .125f) {
                        MovePlayer();
                    }
                    else {
                        currentPosition = SetValidCurrentPosition(goalSpace.GetPositionInWorldCoord());
                        currentSpace = goalSpace;
                        transform.position = currentSpace.GetPositionInWorldCoord();
                        nextMoveChoosen = false;
                        SendMatchTurnFinished();
                    }
                }
            }
        }
        else {nextMoveChoosen = false;}

            if (path != null) {
                DrawPathDebug();
            }
        

    }

    private void SendMatchTurnFinished() {
        if (mControl.currentTeamsTurn.turnState == TurnState.Moving) {
            mControl.currentTeamsTurn.turnState = TurnState.Choosing;
            mControl.currentTeamsTurn.teamMoveCount -= 1;
        }
    }

    private void TelegraphNextMovePosition() {
        if (nextMoveChoosen) {
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
            if (GetMoveToSpace() != null) {
                NextSpace = GetMoveToSpace();
                if (tInput.CheckDeadzone3D()) {
                    goalSpace = NextSpace;
                    nextMoveChoosen = true;
                } else {
                    goalSpace = currentSpace;
                }
            }
            if (nextMoveChoosen) {
                Debug.DrawLine(currentSpace.GetPositionInWorldCoord(), goalSpace.GetPositionInWorldCoord(), Color.red); // temp Telegraph move selection
            }
        }
    }


    private Spaces GetMoveToSpace() {
        Vector3 currTouchPoint = tInput.currTouchPoint3;
        float checkDistance = Mathf.Infinity;
        float distanceFromTouch = 0;
        Spaces returnSpace = null;
        foreach(Spaces s in currentSpace.edges) {
            distanceFromTouch = Vector3.Distance(s.transform.position, currTouchPoint);
            if (distanceFromTouch < checkDistance) {
                returnSpace = s;
                checkDistance = distanceFromTouch;
            }
        }
        if (!board.CheckForWalkableSpace(returnSpace)) {
            returnSpace = null;
        }
        return returnSpace;
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
