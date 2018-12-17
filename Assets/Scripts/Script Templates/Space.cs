using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tile",menuName ="Board/Tiles")]
public class Space : ScriptableObject {

    public string tileName;
    public GameObject tilePrefab;
    public Material baseMaterial;
    public Material highlightedMaterial;
    public Material clickableMaterial;
    public int tileCost;

    
}
