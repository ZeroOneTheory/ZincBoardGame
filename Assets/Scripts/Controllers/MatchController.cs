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

         public bool playerTeamEnabled = false;
         public bool aiTeamEnabled = false;

        public Pieces selectedPiece;
        private int piecesLayer = 1 << 10; // Set to Player Pieces Layer

    private void Awake() {
            board = GameManager.Instance.BoardManager;
            
        }
        public void Start() {

            if (teams.Count > 0) {
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
                    currentTeamsTurn.turnState = TurnState.Choosing;
                    nextTeamsTurn.turnState = TurnState.WaitingToStart;
                }

                if (currentTeamsTurn.turnState == TurnState.TurnEnded) {
                    currentTeamsTurn = nextTeamsTurn;
                    if (turnIndex == teamCount-1) {
                        turnIndex = 0;
                        nextTeamsTurn = teams[1];
                }
                    else {
                    nextTeamsTurn = teams[turnIndex];
                    turnIndex += 1;
                        
                }
                    
                }
            }
        if(currentTeamsTurn.teamMoveCount==0 && currentTeamsTurn.turnState != TurnState.TurnEnded) {
            selectedPiece = null;
            board.selectedUnit = null;
            currentTeamsTurn.turnState = TurnState.TurnEnded;
        }

        if (playerTeamEnabled) {

            if (Input.touchCount > 1) {
                PlayerPieces detPiece = GetPieceAtTouchPoint();
                if (detPiece != null) {
                    selectedPiece = detPiece;
                    board.selectedUnit = selectedPiece;
                }
            }

        }
            
        }

    public void EnablePlayerControlledTeam(Teams team) {
        // Enable the player controlled team interface
        Debug.Log(team.team.teamName + " Team Up!");
        currentTeamsTurn.teamMoveCount = currentTeamsTurn.team.movesPerTurn;
        playerTeamEnabled = true;
        aiTeamEnabled = false;
    }

        public void EnableAiControlledTeam(Teams team) {
            //Start the Ai controlled team IEnumerator event
            Debug.Log(team.team.teamName + " Team Up!");
        playerTeamEnabled = false;
        aiTeamEnabled = true;
    }

    public PlayerPieces GetPieceAtTouchPoint() {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
        RaycastHit hitinfo;
        if(Physics.Raycast(ray,out hitinfo, 1000f, piecesLayer)) {
            return hitinfo.transform.gameObject.GetComponent<PlayerPieces>();
        } else {
            return null;
        }
    }
    

    }



