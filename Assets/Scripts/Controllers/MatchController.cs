using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour {


    private BoardManager board;
    private TouchController tInput;


    public List<Teams> teams = new List<Teams>();
    public int turnIndex = 0;
    public int teamCount;
    public int selePieceIndex = 0;

    public Teams currentTeamsTurn;
    public Teams nextTeamsTurn;

    public bool playerTeamEnabled = false;
    public bool aiTeamEnabled = false;

    public Pieces selectedPiece;
    private int piecesLayer = 1 << 10; // Set to Player Pieces Layer

    private void Awake() {
        board = GameManager.Instance.BoardManager;
        tInput = GameManager.Instance.TouchController;
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

        if (playerTeamEnabled) {

                if (Input.touchCount > 0 && tInput.startTouchPoint==Vector2.zero) {
                    PlayerPieces detPiece = GetPieceAtTouchPoint();
                    if (detPiece != null) {
                        selectedPiece = detPiece;
                        board.selectedUnit = selectedPiece;
                    }
                }

            if (currentTeamsTurn.teamMoveCount == 0 && currentTeamsTurn.turnState != TurnState.TurnEnded) {
                selectedPiece = null;
                board.selectedUnit = null;
                currentTeamsTurn.turnState = TurnState.TurnEnded;
                currentTeamsTurn.teamMoveCount = currentTeamsTurn.team.movesPerTurn;
            }

        }

        if (aiTeamEnabled) {
            if (currentTeamsTurn.teamMoveCount == 0) {
                if (selePieceIndex < currentTeamsTurn.pieces.Count - 1) {
                    selePieceIndex += 1;
                    selectedPiece = currentTeamsTurn.pieces[selePieceIndex];
                    currentTeamsTurn.teamMoveCount = selectedPiece.piece.defaultMoveRange;
                } 
            }

            if (currentTeamsTurn.teamMoveCount == 0 && currentTeamsTurn.turnState != TurnState.TurnEnded && selePieceIndex==currentTeamsTurn.pieces.Count-1) {
                selectedPiece = null;
                board.selectedUnit = null;
                currentTeamsTurn.turnState = TurnState.TurnEnded;
                currentTeamsTurn.teamMoveCount = currentTeamsTurn.team.movesPerTurn;
                selePieceIndex = 0;
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
            selectedPiece = currentTeamsTurn.pieces[selePieceIndex];
            currentTeamsTurn.teamMoveCount = selectedPiece.piece.defaultMoveRange;
    }

    public PlayerPieces GetPieceAtTouchPoint() {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hitinfo;
        if(Physics.Raycast(ray,out hitinfo, 1000f, piecesLayer)) {
            return hitinfo.transform.gameObject.GetComponent<PlayerPieces>();
        } else {
            return null;
        }
    }
    

    }



