using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spaces : MonoBehaviour {

    public Space space;
    public List<Spaces> edges = new List<Spaces>();
    public Vector2 position;
    public bool isTraversable = true;
    public bool isHighlighted = false;
    public bool isClickable = false;


    private MeshRenderer meshRenderer;



    private void Awake() {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update() {
        UpdateMaterials();
    }


    private void UpdateMaterials() {
        if (isHighlighted) {
            if (meshRenderer.material != space.highlightedMaterial) {
                meshRenderer.material = space.highlightedMaterial;
            }
        }
        else {
            if (meshRenderer.material != space.baseMaterial) {
                meshRenderer.material = space.baseMaterial;
            }
        }
    }

    public float DistanceToSpace(Spaces s) {
        return Vector2.Distance(position, s.position);
    }
    public Vector3 GetPositionInWorldCoord() {
        return new Vector3(position.x, 0, position.y);
    }

}
