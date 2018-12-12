using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType {
    PlayerControlled,
    AiControlled
}

[CreateAssetMenu(fileName = "New Team", menuName = "Board/Teams")]
public class Team : ScriptableObject {

    public string teamName;
    public TeamType teamType;
    public int movesPerTurn;
    public bool shareMoves = false;

}
