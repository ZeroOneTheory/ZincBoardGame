using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPieces : Pieces {


    public  void Start() {
        currentPosition = SetValidCurrentPosition(transform.position);
        currentSpace= GetCurrentSpace();
    }

}
