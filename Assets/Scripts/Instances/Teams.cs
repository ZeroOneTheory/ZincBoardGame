using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState {
    Moving,
    TurnEnded,
    WaitingToStart,
    Paused
}

public class Teams: MonoBehaviour {

    public Team team;
    Pieces currentPieceTurn;
    public List<Pieces> pieces = new List<Pieces>();
    public TurnState turnState = TurnState.Paused;

    private void Start() {

        Pieces[] child;
        child = GetComponentsInChildren<Pieces>();

        foreach(Pieces p in child) {
            if (p != null) { pieces.Add(p); }
            
        }

    }



}
