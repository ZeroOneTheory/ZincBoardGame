using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager  {

    private GameObject gameObject;
    private static GameManager m_Instance;
    public static GameManager Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new GameManager();
                m_Instance.gameObject = new GameObject("_GameManager");
                m_Instance.gameObject.AddComponent<TouchController>();
                m_Instance.gameObject.AddComponent<InputController>();
                m_Instance.gameObject.AddComponent<BoardManager>();
            }
            return m_Instance;
        }
    }

    private TouchController m_touchController;
    public TouchController TouchController {
        get {
            if (m_touchController == null) {
                m_touchController = gameObject.GetComponent<TouchController>();
            }
            return m_touchController;
        }
    }

    private InputController m_inputController;
    public InputController InputController {
        get {
            if (m_inputController == null) {
                m_inputController = gameObject.GetComponent<InputController>();
            }
            return m_inputController;
        }
    }

    private BoardManager m_boardManager;
    public BoardManager BoardManager {
        get {
            if(m_boardManager == null){
                    m_boardManager = gameObject.GetComponent<BoardManager>();
                }
            return m_boardManager;
        }
        
    }

}
