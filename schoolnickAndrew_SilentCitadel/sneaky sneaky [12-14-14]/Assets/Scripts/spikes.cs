using UnityEngine;
using System.Collections;

public class spikes : MonoBehaviour {
   
    public GameObject player;
    Player playerStuff;
    private bool hurtPlayer = true;
    
	// Use this for initialization
	void Start () 
    {
        playerStuff = player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name == "playerObject")
        {
            if (hurtPlayer)
            {
                hurtPlayer = false;
                playerStuff.Hitpoints -= 1;
                if (playerStuff.Hitpoints <= 0)
                {
                    playerStuff.killPlayer();
                }
                Debug.Log(playerStuff.Hitpoints);
                StartCoroutine("waitBeforeHurtingAgain");
                
            }
            
        }
    }

    IEnumerator waitBeforeHurtingAgain()
    {
       
        
        
        

        yield return new WaitForSeconds(1.5f);
        hurtPlayer = true;
        StopCoroutine("waitBeforeHurtingAgain");

    }
}
