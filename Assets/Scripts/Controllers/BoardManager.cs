using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BoardManager: MonoBehaviour {

    public int spacesLayer = 1 << 9; // Set LayerMask to Spaces Layer (9)
    public Vector2 mapSize = new Vector2(10, 10);
    public Spaces targetSpace;
    public Spaces[,] spacesGrid;
    public Spaces[,] spacesGraph;
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
            //targetSpace = GetSpaceFromCameraRay(Input.mousePosition);
            //GeneratePathToPosition((int)targetSpace.position.x,(int) targetSpace.position.y);

        }
    }

    //-------- Behaviours -------------
    public void LoadLevels() {
        if( lvlManager.loadedLevels.Count > 0) {
            setTileTEMP = lvlManager.loadedLevels[0].levelPiece;
        }
    }

    public void GenerateBoard() {

        spacesGrid = new Spaces[(int)mapSize.x, (int)mapSize.y];

        for(int x=0; x<mapSize.x; x++) {
            for(int y=0; y<mapSize.y; y++) {
                GameObject go = (GameObject)Instantiate(setTileTEMP, new Vector3(x,0,y), Quaternion.identity); // replace with read from map file
                string setName = setTileTEMP.GetComponent<Spaces>().space.tileName + x.ToString() + " , " + y.ToString();
                go.name = setName;
                go.transform.SetParent(this.transform);
                Spaces curSpace = go.gameObject.GetComponent<Spaces>();
                curSpace.position = new Vector2(x, y);
                spacesGrid[x, y] = curSpace;
               
            }
        }
    }

    public void GeneratePathGraph() {

        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {

                if (x > 0) { spacesGrid[x, y].edges.Add(spacesGrid[x - 1, y]); }
                if (x < mapSize.x-1) { spacesGrid[x, y].edges.Add(spacesGrid[x + 1, y]); }
                if (y > 0) { spacesGrid[x, y].edges.Add(spacesGrid[x, y-1]); }
                if (y < mapSize.x - 1) { spacesGrid[x, y].edges.Add(spacesGrid[x, y+1]); }

            }
        }
        
    }

    public void UnHighlightAllPieces() {
        foreach(Spaces s in spacesGrid) {
            s.ResetSpaces();
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
        Spaces target = spacesGrid[x, y];
        

        dist[source] = 0;
        prev[source] = null;

        // Step 1 - Create a set of all nodes and mark them as unvisited and set the origin position to a dist cost of 0
        foreach(Spaces v in spacesGrid) {
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
        // Clean up
        dist.Clear();
        prev.Clear();
        unvisited.Clear();
    }

    public Spaces GetSpaceAtLocation(Vector2 pos) {
        if (pos.x > -1 & pos.x < mapSize.x & pos.y > -1 & pos.y < mapSize.y) {
            return spacesGrid[(int)pos.x, (int)pos.y];
        }
        Debug.LogWarning("No Space Detected at:" + pos);
        return null;
        
    }

    public Spaces GetSpaceFromCameraRay(Vector3 pos) {

        UnHighlightAllPieces();
        RaycastHit hitinfo;
        Ray ray = Camera.main.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, spacesLayer)) {
            Debug.DrawRay(pos, hitinfo.point);
            hitinfo.transform.gameObject.GetComponent<Spaces>().SetSpaceClickable();
            return hitinfo.transform.gameObject.GetComponent<Spaces>();
        } else {
            Debug.Log("No space captured");

        }
        return null;
    }

    public bool CheckForValidSpace(Vector2 pos) {
        if (pos.x > -1 & pos.x < mapSize.x & pos.y > -1 & pos.y < mapSize.y) {
            return true;
        }
        return false;
    }
}
