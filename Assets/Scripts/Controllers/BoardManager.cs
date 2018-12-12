using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BoardManager: MonoBehaviour {

    public int spacesLayer = 1 << 9; // Set LayerMask to Spaces Layer (9)
    public Vector2 mapSize = new Vector2(10, 10);
    public Spaces targetSpace;
    public Spaces[,] spacePositions;
    public Spaces[,] graph;
    public Pieces selectedUnit;
    public LevelManager lvlManager;
    public List<Spaces> movePath = null;

    [SerializeField]
    private GameObject setTileTEMP; // temp variable name for default tile
    private InputController input;


    // -------- Initialization -------------
    private void Awake() {
        lvlManager = FindObjectOfType<LevelManager>();
        input = GameManager.Instance.InputController;
        LoadLevels();
        GenerateBoard();
        GeneratePathGraph();

    }

    //-------- Updates -------------
    private void Update() {
        if (input.leftMouse) {
            targetSpace = GetSpaceFromCameraRay(Input.mousePosition);
            GeneratePathToPosition((int)targetSpace.position.x,(int) targetSpace.position.y);

        }
    }

    //-------- Behaviours -------------
    public void LoadLevels() {
        if( lvlManager.loadedLevels.Count > 0) {
            setTileTEMP = lvlManager.loadedLevels[0].levelPiece;
        }
    }

    public void GenerateBoard() {

        spacePositions = new Spaces[(int)mapSize.x, (int)mapSize.y];

        for(int x=0; x<mapSize.x; x++) {
            for(int y=0; y<mapSize.y; y++) {
                GameObject go = (GameObject)Instantiate(setTileTEMP, new Vector3(x,0,y), Quaternion.identity); // replace with read from map file
                string setName = setTileTEMP.GetComponent<Spaces>().space.tileName + x.ToString() + " , " + y.ToString();
                go.name = setName;
                go.transform.SetParent(this.transform);
                Spaces curSpace = go.gameObject.GetComponent<Spaces>();
                curSpace.position = new Vector2(x, y);
                spacePositions[x, y] = curSpace;
               
            }
        }
    }

    public void GeneratePathGraph() {

        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {

                if (x > 0) { spacePositions[x, y].edges.Add(spacePositions[x - 1, y]); }
                if (x < mapSize.x-1) { spacePositions[x, y].edges.Add(spacePositions[x + 1, y]); }
                if (y > 0) { spacePositions[x, y].edges.Add(spacePositions[x, y-1]); }
                if (y < mapSize.x - 1) { spacePositions[x, y].edges.Add(spacePositions[x, y+1]); }

            }
        }
        
    }

    public void UnHighlightAllPieces() {
        foreach(Spaces s in spacePositions) {
            s.isHighlighted = false;
        }
    }
    /*
    //  Add a function to generate path which creates a list of nodes reachable within alloted moves for the selected piece
    //
    */
    public void GeneratePathToPosition(int x, int y) {
        movePath = null;
        selectedUnit.path = null;

        if(selectedUnit == null) {
            Debug.Log("Selected Unit was null");
            return;
        }
        if(selectedUnit.currentSpace == null) {
            Debug.Log("current space was null");
            return;
        }

        Dictionary<Spaces, float> dist = new Dictionary<Spaces, float>();
        Dictionary<Spaces, Spaces> prev = new Dictionary<Spaces, Spaces>();
        List<Spaces> unvisited = new List<Spaces>();

        Spaces source = selectedUnit.currentSpace;
        Spaces target = spacePositions[x, y];
        

        dist[source] = 0;
        prev[source] = null;

        // Step 1 - Create a set of all nodes and mark them as unvisited and set the origin position to a dist cost of 0
        foreach(Spaces v in spacePositions) {
            if (v != source) {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        
        while (unvisited.Count > 0) {
            // Sort the unvisited nodes by distance
            Spaces u = unvisited.OrderBy(n => dist[n]).First();

            if (u == target) {
                break;
            }

            unvisited.Remove(u);
            // Step 2 - Compare all unvisited nodes and calculate their costs and assign the smaller value
            foreach (Spaces v in u.edges) {
                float alt = dist[u]+ u.DistanceToSpace(v); 
                if (alt < dist[v]) {
                    dist[v] = alt;
                    prev[v] = u;  
                }
            }
        }

        if(prev[target] == null) {
            // no route found
            Debug.Log("No route Found");
            return;
        }
        movePath = new List<Spaces>();
        Spaces curr = target;

        while (curr!= null) {
            movePath.Add(curr);
            curr = prev[curr];
        }
   
        movePath.Reverse();
        movePath.RemoveAt(0);
        selectedUnit.path = movePath;
        Debug.Log("Move Path:Length "+movePath.Count);
    }

    public Spaces GetSpaceAtLocation(Vector3 pos) {
        Debug.Log("No Space Detected");
        return null;
    }

    public Spaces GetSpaceFromCameraRay(Vector3 pos) {

        RaycastHit hitinfo;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hitinfo, 10000f, spacesLayer)) {
            hitinfo.transform.gameObject.GetComponent<Spaces>().isHighlighted = true;
            return hitinfo.transform.gameObject.GetComponent<Spaces>();
        } else {
            Debug.Log("No space captured");
        }
        return null;
    }
}
