using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour 
{
    //Variables and whatnot
    GameManager gameManager;
    private Animator animator;  //Animator attached to enemy
    public float speed = 5.0f;
    public GameObject arrow;

    private bool movementLeft = true;   //Checks to see if the enemy is CURRENTLY moving left
    public bool MovingLeft
    {
        get
        {
            return movementLeft;
        }
    }
    private bool moveEnemy = true;      //This is just a boolean to tell whether or not to stop the enemy from moving
    private bool allowedToFireAgain = true;     //A boolean telling the enemy to pause before firing another arrow

    //This is a variable used for doing actions on the enemy if the player is still unseen
    //This is used right now in adding additional damage to the enemy if attacked from behind
    public bool PlayerHasBeenDetected
    {
        get;
        set;
    }

    //Self explainatory
    private int health = 100;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    private bool dead = false;
    Transform[] gameObjectChildren; //This is an array to access the children or objects attached to this object
    
	// "Use this for initialization" -Quoth the Unity
	void Start () 
    {
        animator = this.GetComponent<Animator>();
        GameObject mainGO = GameObject.Find("Manager");
        gameManager = mainGO.GetComponent<GameManager>();

        gameObjectChildren = this.gameObject.GetComponentsInChildren<Transform>();      //Get the children attached at startup
	}
	
	// Update is called once per frame
	void Update () 
    {
        movement(); //Check to see whether or not to switch directions
        
        
        
        if (movementLeft && moveEnemy)      //If patrolling left AND is able to move
        {
            foreach(Transform t in gameObjectChildren)  //Change which spotlight component is active
            {
                if (t.gameObject.name == "SpotlightLeft")
                {
                    t.gameObject.SetActive(true);
                }
                else if (t.gameObject.name == "SpotlightRight")
                {
                    t.gameObject.SetActive(false);
                }
            }

            if (playerDetected() && allowedToFireAgain) //If enemy can shoot an arrow again and player is still in sight
            {
                animator.SetBool("moveLeft", true);
                animator.SetBool("fire", true);
                StartCoroutine("disableMovement");      //Begin shooting arrow
            }
            else    //If the enemy is just patrolling without spotting player at all
            {
                this.transform.Translate(Vector3.right * speed * Time.deltaTime * -1);
                animator.SetBool("moveLeft", true);
            }

        }
        else if (movementLeft == false && moveEnemy)    //If patrolling right AND is able to move
        {
            foreach (Transform t in gameObjectChildren)
            {
                if (t.gameObject.name == "SpotlightLeft")
                {
                    t.gameObject.SetActive(false);
                }
                else if (t.gameObject.name == "SpotlightRight")
                {
                    t.gameObject.SetActive(true);
                }
            }
            if (playerDetected())
            {
                animator.SetBool("moveLeft", false);
                animator.SetBool("fire", true);
                StartCoroutine("disableMovement");
            }
            else
            {
                this.transform.Translate(Vector3.right * speed * Time.deltaTime * 1);
                animator.SetBool("moveLeft", false);
            }
        }


        //Once again pretty dang self-explainatory
        if (health <= 0)
        {
            killEnemy();
        }
        
        

        
	}


    //Raycasting kicks so much a**
    private void movement()
    {
        bool result;
        Vector2 originEnemy;            //Start point of linecast
        Vector2 distanceFromEnemy;      //End point

        //Provides an offset to the origin and distance depending on which way enemy is patrolling
        if (movementLeft)
        {
            originEnemy = new Vector2(transform.position.x - 2.0f, transform.position.y);
            distanceFromEnemy = new Vector2(transform.position.x - 2.0f, transform.position.y - 5);
        }
        else
        {
            originEnemy = new Vector2(transform.position.x + 2.0f, transform.position.y);
            distanceFromEnemy = new Vector2(transform.position.x + 2.0f, transform.position.y - 5);
        }
        
        //If the linecast is hitting SOMETHING in the layer "ground", result becomes true
        result = Physics2D.Linecast(originEnemy, distanceFromEnemy, 1 << LayerMask.NameToLayer("ground"));
        if (result)
        {
            Debug.DrawLine(originEnemy, distanceFromEnemy, Color.green, 0.5f, false);   //Adds visualization of ray
            
        }
        else
        {
            Debug.DrawLine(originEnemy, distanceFromEnemy, Color.red, 0.5f, false);
            movementLeft = !movementLeft;   //Change direction of movement if there is no more platform to move on
        } 
        
    }

    //More raycasting WOO WOO, this is the enemy's line of sight
    public bool playerDetected()
    {
        Vector2 originPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 eyeSight;
        if (movementLeft)
        {
            eyeSight = new Vector2(transform.position.x - 10, transform.position.y - 1);
        }
        else
        {
            eyeSight = new Vector2(transform.position.x + 10, transform.position.y - 1);
        }
        bool result = Physics2D.Linecast(originPos, eyeSight, 1 << LayerMask.NameToLayer("playerObj"));
        if (result)
        {
            Debug.DrawLine(originPos, eyeSight, Color.green, 0.5f, false);
            PlayerHasBeenDetected = true;
        }
        else
        {
            Debug.DrawLine(originPos, eyeSight, Color.red, 0.5f, false);
            PlayerHasBeenDetected = false;
        }
        return result;
    }
    
	//Changes movement direction based on sound detection
	public void soundDetected(bool left)
	{

		if(left)
		{
			movementLeft = true;
		}
		else
		{
			movementLeft = false;
		}
	}

    //He's dead Jim
    public void killEnemy()
    {
        StartCoroutine("playDeathAnimation");
    }
    
    //Attack the player
    public void shootArrow()
    {
    	GameObject clone = Instantiate(arrow, this.transform.position, transform.rotation) as GameObject;
    	/*if(MovingLeft)
    	{
    		clone.left = true;
    	}
    	else
    	{
    		clone.left = false;
    	}*/
    }


    //Stop enemy from moving so he can shoot an arrow
    IEnumerator disableMovement()
    {
        moveEnemy = false;
		yield return new WaitForSeconds(.5f);
		shootArrow ();
		yield return new WaitForSeconds(1.0f);
        moveEnemy = true;
        allowedToFireAgain = false;
        animator.SetBool("fire", false);
        StopCoroutine("disableMovement");
        StartCoroutine("waitBeforeAttackingAgain");
    }
    //HALT, you can't just attack again! Wait you Foo!
    IEnumerator waitBeforeAttackingAgain()
    {
        yield return new WaitForSeconds(0.5f);
        allowedToFireAgain = true;
        StopCoroutine("waitBeforeAttackingAgain");
    }

    //Make him die like a man
    IEnumerator playDeathAnimation()
    {
        //Deactivate all ability to move because he dead
        animator.SetBool("fire", false);
        animator.SetBool("moveLeft", false);
        animator.SetBool("dead", true);
        moveEnemy = false;

        this.renderer.material.color = new Color(0.25f, 0.25f, 0.25f, .75f);    //Make him dark like a hershey bar
        this.rigidbody2D.isKinematic = true;    //Make it not impacted by gravity

        //DEACTIVATE ALL CHILDREN OBJECTS....Can't just say t.gameObject.SetActive without ifs because it sets the parent inactive too...go figure
        foreach (Transform t in gameObjectChildren)
        {
            if (t.gameObject.name == "SpotlightLeft")
            {
                t.gameObject.SetActive(false);
            }
            else if (t.gameObject.name == "SpotlightRight")
            {
                t.gameObject.SetActive(false);
            }
            else if (t.gameObject.name == "radiusLight")
            {
                t.gameObject.SetActive(false);
            }
        }

        //Move him down a bit because pft let's be honest the sprite sheet designer suuuuuucccks
        if (dead == false)
        {
            this.transform.position += new Vector3(0, -0.05f, 0);
            
        }
        Destroy(this.collider2D);   //Remove his box collider because pft why not
        yield return new WaitForSeconds(0.85f);
        dead = true;    //Now he dead

        StopCoroutine("playDeathAnimation");    //Now he REALLY dead
    }
}
