using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Piece",menuName ="Board/Pieces")]
public class Piece : ScriptableObject {

    public string pieceName;
    public int defaultMoveRange;
    public bool allowDiagnols;
}
