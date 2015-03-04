using UnityEngine;
using System.Collections;

public class arrowScript : MonoBehaviour {
	
	public Vector3 moveSpeed;
	public bool left;
	
	GameObject stuff;
	Player playerStuff; 
	
	// Use this for initialization
	void Start ()
	{
		stuff = GameObject.Find ("playerObject");
		playerStuff = stuff.GetComponent<Player>();
		left = playerStuff.arrowLeft;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(left)
		{
			transform.position -= moveSpeed;
		}
		else
		{
			transform.position += moveSpeed;
		}
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.name == "playerObject")
		{
			playerStuff.Hitpoints -= 1;
			Debug.Log(playerStuff.Hitpoints);
            if (playerStuff.Hitpoints <= 0)
            {
                playerStuff.killPlayer();
            }
			Destroy (this.gameObject);
		}
	}
}
