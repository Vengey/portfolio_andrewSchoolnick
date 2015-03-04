using UnityEngine;
using System.Collections;

public class enemySoundDetection : MonoBehaviour
{	

	GameObject stuff;
	Player playerStuff;
    Archer archer;
    private bool checkIfPlayerIsStillThere = false;
	void Start()
	{
		stuff = GameObject.Find ("playerObject");
		playerStuff = stuff.GetComponent<Player>();
	}
	
			
	void OnTriggerStay2D(Collider2D other)
	{
        if (!playerStuff.Walking)
        {
            if (other.gameObject.name == "archerEnemy")
            {
                StartCoroutine("WaitBeforeChasing");
                archer = other.GetComponent<Archer>();
                if (checkIfPlayerIsStillThere)
                {
                    if (this.transform.position.x < other.transform.position.x)
                    {
                        archer.soundDetected(true);
                        playerStuff.arrowLeft = true;
                    }
                    else
                    {
                        archer.soundDetected(false);
                        playerStuff.arrowLeft = false;
                    }
                }
            }


        }
        else
        {
            checkIfPlayerIsStillThere = false;
            
        }
	}

    IEnumerator WaitBeforeChasing()
    {
        
        yield return new WaitForSeconds(2.5f);
        checkIfPlayerIsStillThere = true;
        StopCoroutine("WaitBeforeChasing");
    }
}
