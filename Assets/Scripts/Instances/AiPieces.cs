using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPieces : Pieces {

    public BoardManager board;
    //public MatchController matchController;


    private void Awake() {

        board = GameManager.Instance.BoardManager;
        Debug.Log(board);
        //matchController = MatchController.Instance;

    }

    private void Start() {
        //currentPosition = new Vector2(transform.position.x, transform.position.z);
        //GetCurrentSpace();
    }

    public  Spaces GetCurrentSpace() {

        currentPosition = new Vector2(transform.position.x, transform.position.z);
        if (currentPosition.x > -1 & currentPosition.x < board.mapSize.x & currentPosition.y > -1 & currentPosition.y < board.mapSize.y) {
            return currentSpace = board.spacePositions[(int)currentPosition.x, (int)currentPosition.y];
        }
        Debug.Log("Piece not found");
        return board.spacePositions[0,0];
    }
}
