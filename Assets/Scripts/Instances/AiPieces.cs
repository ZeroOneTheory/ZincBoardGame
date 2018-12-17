using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPieces : Pieces {

    public Pieces targetPiece;

    public void Start() {
        currentPosition = SetValidCurrentPosition(transform.position);
        currentSpace= GetCurrentSpace();
        goalSpace = currentSpace;
    }

    public void Update() {

        if (mControl.selectedPiece == this) {
            
                if (goalSpace != currentSpace) {
                    mControl.currentTeamsTurn.turnState = TurnState.Moving;
                    if (Vector3.Distance(transform.position, goalSpace.GetPositionInWorldCoord()) > .125f) {
                        MovePlayer();
                    }
                    else {
                        currentPosition = SetValidCurrentPosition(goalSpace.GetPositionInWorldCoord());
                        currentSpace = goalSpace;
                        transform.position = currentSpace.GetPositionInWorldCoord();

                        if (mControl.currentTeamsTurn.turnState == TurnState.Moving) {
                            mControl.currentTeamsTurn.turnState = TurnState.Choosing;
                            mControl.currentTeamsTurn.teamMoveCount -= 1;
                        }
                    }
                } else {
                if(mControl.currentTeamsTurn.turnState == TurnState.Choosing && goalSpace==currentSpace) {
                    goalSpace = ChooseGoalSpace();
                }
            }
            
        }
    }

    public Spaces ChooseGoalSpace() {
        GetReachableSpaces();
        var players = FindObjectsOfType<PlayerPieces>();

        foreach (PlayerPieces plys in players) {
            if (targetPiece == null) {
                targetPiece = plys;
            }
            else {
                if (Vector2.Distance(targetPiece.currentPosition, currentPosition) > Vector2.Distance(plys.currentPosition, currentPosition)) {
                    targetPiece = plys;
                }
            }
        }
        Spaces chosenSpace = null;
        foreach (Spaces s in reachable) {
            if (chosenSpace == null) {
                chosenSpace = s;
            }
            else {
                if (Vector2.Distance(chosenSpace.position, targetPiece.currentSpace.position) > Vector2.Distance(s.position, targetPiece.currentSpace.position)) {
                    chosenSpace = s;
                }
            }
        }

        return chosenSpace;


    }



}
