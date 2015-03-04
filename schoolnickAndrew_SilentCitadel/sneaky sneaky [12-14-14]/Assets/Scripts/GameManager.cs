///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// THIS
// IS
// ALSO
// FROM
// LAST
// PLATFORMER
//
// None of the code AS I KNOW are specific to this program, I am keeping the code up on here because of reference
// purposes, so use this if you need a reference and modify it up to work for our game
// you'll probably delete a good majority of the stuff, but you may find stuff you want to keep I dunno
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


    public GUIStyle myStyle = new GUIStyle();
    public GameObject player;
    Player playerStuff;
    

    public Camera sceneCamera;



	// Use this for initialization
	void Start () 
    {
        GameObject playerOBJ = GameObject.Find("Player");
        playerStuff = player.GetComponent<Player>();
        sceneCamera = Camera.main;
        

	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}
    void OnGUI()    //Initialize GUI
    {
        
        //GUI.TextField(new Rect(15, 15, 150, 38), "HP: " + playerStuff.Hitpoints , myStyle);
        
        //GUI.Box(new Rect(0, 60, Screen.width, 0.2f), GUIContent.none);
    }

    void OnApplicationQuit() 
    { 
        
    }

    

  
    
    
}
