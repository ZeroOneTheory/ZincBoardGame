using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour {


    private BoardManager board;

    public List<Teams> teams = new List<Teams>();
    public int turnIndex = 0;
    public int teamCount;
    
    public Teams currentTeamsTurn;
    public Teams nextTeamsTurn;



    private void Awake() {
        board = GameManager.Instance.BoardManager;
    }
    public void Start() {

        if(teams.Count > 0) {
            teamCount = teams.Count;
            turnIndex = 0;

            currentTeamsTurn = teams[0];
            nextTeamsTurn = teams[1];

            board.selectedUnit = currentTeamsTurn.pieces[0];
        }
        
    }


    private void Update() {
        if (teams.Count > 0) {
            if (currentTeamsTurn.turnState == TurnState.WaitingToStart) {
                if (currentTeamsTurn.team.teamType == TeamType.PlayerControlled) {
                    EnablePlayerControlledTeam(currentTeamsTurn);
                }
                else if (currentTeamsTurn.team.teamType == TeamType.AiControlled) {
                    EnableAiControlledTeam(currentTeamsTurn);
                }
                currentTeamsTurn.turnState = TurnState.Moving;
                nextTeamsTurn.turnState = TurnState.WaitingToStart;
            }

            if (currentTeamsTurn.turnState == TurnState.TurnEnded) {
                currentTeamsTurn = nextTeamsTurn;
                if (turnIndex == teamCount) {
                    turnIndex = 0;
                }
                else {
                    turnIndex += 1;
                }
                nextTeamsTurn = teams[turnIndex + 1];
            }
        }
    }

    public void EnablePlayerControlledTeam(Teams team) {
        // Enable the player controlled team interface
        Debug.Log(team.team.teamName+" Team Up!");
    }

    public void EnableAiControlledTeam(Teams team) {
        //Start the Ai controlled team IEnumerator event
        Debug.Log(team.team.teamName + " Team Up!");
    }

}
