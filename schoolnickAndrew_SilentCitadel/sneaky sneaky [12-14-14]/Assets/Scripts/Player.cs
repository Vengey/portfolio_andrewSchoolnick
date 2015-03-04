using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float speed;
    public float jumpHeight = 500.0f;
    public bool Grounded = false;
    public bool Walking = false;
    public int Hitpoints = 3;

    public bool arrowLeft = true;

    private bool readyToDie = false;
    private bool die = false;

    private Animator animator;

    private BoxCollider2D boxCollideOriginal;   //This is the boxCollider of the player


    public GameObject tombstone;

    private bool facingRight = true;

    GameManager gameManager;

    Archer archerScript;    //This is the script we're going to use for referencing the archer to take his HP away


    private bool playerMovement = true;
    private bool allowPlayerMovement = true;

    //Check if player is well currently crouched
    private bool isCurrentlyCrouched = false;

    //These are for more linecasting fun later//
    Vector2 groundPos1;
    Vector2 originPos1;
    Vector2 groundPos2;
    Vector2 originPos2;
    ////////////////////////////////////////////

    private GameObject objectToAttack;

    private bool notKillingPlayer = true;

    private bool playerClimbing = false;


	void Start () 
    {
        //Get some components Bruh
        animator = this.GetComponent<Animator>();
        boxCollideOriginal = this.GetComponent<BoxCollider2D>();
        Hitpoints = 3;
        GameObject mainGO = GameObject.Find("Manager");
        gameManager = mainGO.GetComponent<GameManager>();
	}
	
	void Update () 
    {
    	if(Input.GetKey(KeyCode.LeftShift))
    	{
    		speed = 10.0f;
    		Walking = true;
    	}
    	else
    	{
    		speed = 25.0f;
    		Walking = false;
    	}
        //transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);        
        if (Input.anyKey && playerMovement && notKillingPlayer)     //If player holds any key, and CAN move
        {

            if (Input.GetKey(KeyCode.D) && allowPlayerMovement)     //Move right if ALLOWED to move(This is for attacks)
            {
                
                facingRight = true;                         //Player is facing right
               
                //"ground" is just the layer I assigned all platforms, walls etc., I made it for anything the player collides with
                //This if statement makes a raycast and if it collides with a wall stop the player from moving
                //This is what keeps the player from jittering into the wall, IMPORTANT
                if (inFrontOfPlayer("ground") == false)   
                {
                    this.transform.Translate(Vector3.right * speed * Time.deltaTime);
                }

                animator.SetBool("facingRight", true);
                animator.SetBool("walking", true);
                animator.SetBool("attacking", false);
                
            }
            //Same idea as D key
            if (Input.GetKey(KeyCode.A) && allowPlayerMovement)
            {
                
                facingRight = false;
                if (inFrontOfPlayer("ground") == false)
                {
                    this.transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                animator.SetBool("facingRight", false);
                animator.SetBool("walking", true);
                animator.SetBool("attacking", false);

            }



            if (Input.GetKeyDown(KeyCode.W))
            {
                //Add force to the players rigidbody allowing it to move upwards if the above if statement is true

                if (isGrounded() && inFrontOfPlayer("ladder") == false)
                {
                    animator.SetBool("crouching", false);
                    if (inFrontOfPlayer("ground") && inFrontOfPlayer("ladder") == false)
                    {
                        StartCoroutine("disableMovement");
                        //Depending on which direction the player was facing when making jump will impact which direction he jumps towards

                        if (facingRight)    //Jump TOWARDS the left
                        {
                            facingRight = false;
                            animator.SetBool("facingRight", false);
                            rigidbody2D.AddForce(Vector2.up * jumpHeight * 2.75f);
                            rigidbody2D.AddForce(Vector2.right * jumpHeight * 1.75f * -1);

                        }
                        else                //Jump TOWARDS the right
                        {
                            facingRight = true;
                            animator.SetBool("facingRight", true);
                            rigidbody2D.AddForce(Vector2.up * jumpHeight * 2.75f);
                            rigidbody2D.AddForce(Vector2.right * jumpHeight * 1.75f);
                        }
                    }
                    else    //This is just for regular old jumping, not wall jumping
                    {
                        rigidbody2D.AddForce(Vector2.up * jumpHeight * 3.75f);
                    }
                }
            }
            if (Input.GetKey(KeyCode.W))
            {

                if (inFrontOfPlayer("ladder") && playerClimbing == false)
                {
                    this.rigidbody2D.isKinematic = true;
                    Debug.Log("Attach to Ladder");
                    animator.SetBool("climbingLadder", true);
                    animator.SetBool("movingOnLadder", false);

                    

                    playerClimbing = true;
                }
                else if (inFrontOfPlayer("ladder") && playerClimbing)
                {
                    Debug.Log("Climbing Ladder");
                    animator.SetBool("movingOnLadder", true);
                    animator.SetBool("climbingLadder", false);

                    
					if(Walking)
					{
                   		this.transform.position += new Vector3(0, 0.2f, 0);
                    }
                    else
                    {
						this.transform.position += new Vector3(0, 0.5f, 0);
                    }

                }
                
            }


            //Crouching time WOOT 1337ZORZ ROMHACKS
            if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("crouching", true);
                boxCollideOriginal.size = new Vector2(0.55f, 0.33f);
                boxCollideOriginal.center = new Vector2(0, -0.16f);
                isCurrentlyCrouched = true; //MAKE SURE PLAYER CANT JUST STAND UP MID CROUCHING
            }


            if (inFrontOfPlayer("ladder"))
            {
                Debug.Log("Ladder area Collided With");
                animator.SetBool("inFrontOfLadder", true);

            }
            
            
        }
        //IDLE MOTION OH JOY, NO BUTTONS PRESSED BALLS TO THE WALLS FUN!
        else
        {
            if (inFrontOfPlayer("ladder") == false)
            {
                this.rigidbody2D.isKinematic = false;
                playerClimbing = false;
                animator.SetBool("inFrontOfLadder", false);
                animator.SetBool("movingOnLadder", false);
                animator.SetBool("climbingLadder", false);
            }
            else if (inFrontOfPlayer("ladder") == true && playerClimbing == true)
            {
                //playerClimbing = false;
                animator.SetBool("climbingLadder", true);
                animator.SetBool("movingOnLadder", false);
            }

            animator.SetBool("walking", false);
            animator.SetBool("crouching", false);
            boxCollideOriginal.size = new Vector2(0.26f, 0.67f);
            boxCollideOriginal.center = new Vector2(0, 0);
            if (facingRight)
            {
                animator.SetBool("facingRight", true);
                
            }
            else
            {
                animator.SetBool("facingRight", false);
            }
        }

        //Had to put Space in a separate category because we only want it to happen ever space press not every frame
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded())
        {
            
            animator.SetBool("attacking", true);
            StartCoroutine("disableMovement");

            objectToAttack = collidedGameObject();  //Oh joy this linecasting code.....this was painful

            //No objects within reach to attack
            if (objectToAttack == null)
            {
                Debug.Log("Nothing to attack");
            }

            //Told you we'd use that archer script initialization from up above, here it is, in all its glory
            else if (objectToAttack.gameObject.name == "archerEnemy")
            {
                archerScript = objectToAttack.GetComponent<Archer>();   //GET THAT COMPONENT SON

                //Now to make the player do CRITICAL DAMAGE if he is a ghost, or cant be seen that is
                if (archerScript.PlayerHasBeenDetected == false)
                {
                    archerScript.Health -= 100;
                }

                //Well crap...you done got spotted
                else
                {
                    archerScript.Health -= 50;
                }
                
                Debug.Log(archerScript.Health);
            }

        }
        
       

        //Are you on the ground based on the linecasting? Better be
        if (isGrounded() == false)
        {
            animator.SetBool("jumping", true);
            animator.SetBool("attacking", false);
            animator.SetBool("crouching", false);
        }
        //If not you are jumping
        else
        {
            animator.SetBool("jumping", false);
            
        }


        //THIS IS TO CHECK TO MAKE SURE THE PLAYER CAN'T UNCROUCH MIDCROUCH
        if (isCeilingAbovePlayer() && isGrounded() && isCurrentlyCrouched)
        {
            animator.SetBool("jumping", false);
            animator.SetBool("attacking", false);
            animator.SetBool("crouching", true);
            boxCollideOriginal.size = new Vector2(0.55f, 0.33f);
            boxCollideOriginal.center = new Vector2(0, -0.16f);
        }
        //Once he comes out of a crawlspace let him breathe and stand
        else if (isCeilingAbovePlayer() == false && isGrounded() && isCurrentlyCrouched)
        {
            isCurrentlyCrouched = false;
        }




       


       
	}



    //USE RAYCASTING TO CHECK IF PLAYER IS ON GROUND//
    public bool isGrounded()
    {
        originPos1 = new Vector2(transform.position.x + 2.5f, transform.position.y);
        groundPos1 = new Vector2(transform.position.x + 2.5f, transform.position.y - 5);
        originPos2 = new Vector2(transform.position.x - 2.5f, transform.position.y);
        groundPos2 = new Vector2(transform.position.x - 2.5f, transform.position.y - 5);
        bool result1 = Physics2D.Linecast(originPos1, groundPos1, 1 << LayerMask.NameToLayer("ground"));
        bool result2 = Physics2D.Linecast(originPos2, groundPos2, 1 << LayerMask.NameToLayer("ground"));
        bool grounded = false;
        if (result1 || result2)
        {
            Debug.DrawLine(originPos1, groundPos1, Color.green, 0.5f, false);
            Debug.DrawLine(originPos2, groundPos2, Color.green, 0.5f, false);
            grounded = true;
        }
        else
        {
            Debug.DrawLine(originPos1, groundPos1, Color.red, 0.5f, false);
            Debug.DrawLine(originPos2, groundPos2, Color.red, 0.5f, false);
            grounded = false;
        }
        return grounded;
    }


    //Use more linecasting but in opposite direction of isGrounded
    public bool isCeilingAbovePlayer()
    {
        originPos1 = new Vector2(transform.position.x + 2.5f, transform.position.y);
        groundPos1 = new Vector2(transform.position.x + 2.5f, transform.position.y + 1.5f);
        originPos2 = new Vector2(transform.position.x - 2.5f, transform.position.y);
        groundPos2 = new Vector2(transform.position.x - 2.5f, transform.position.y + 1.5f);
        bool result1 = Physics2D.Linecast(originPos1, groundPos1, 1 << LayerMask.NameToLayer("ground"));
        bool result2 = Physics2D.Linecast(originPos2, groundPos2, 1 << LayerMask.NameToLayer("ground"));
        bool stayCrouched;
        if (result1 || result2)
        {
            Debug.DrawLine(originPos1, groundPos1, Color.green, 0.5f, false);
            Debug.DrawLine(originPos2, groundPos2, Color.green, 0.5f, false);
            stayCrouched = true;
        }
        else
        {
            Debug.DrawLine(originPos1, groundPos1, Color.red, 0.5f, false);
            Debug.DrawLine(originPos2, groundPos2, Color.red, 0.5f, false);
            stayCrouched = false;
        }
        return stayCrouched;
    }


    //Check if a wall is in front of the player to smooth out movement
    private bool inFrontOfPlayer(string layer)
    {
        Vector2 playerOrigin = new Vector2(transform.position.x, transform.position.y);
        Vector2 distanceFromPlayer;
        bool result;
        
        if (facingRight)
        {
            distanceFromPlayer = new Vector2(transform.position.x + 1.5f, transform.position.y);
        }
        else
        {
            distanceFromPlayer = new Vector2(transform.position.x - 1.5f, transform.position.y);
        }

        result = Physics2D.Linecast(playerOrigin, distanceFromPlayer, 1 << LayerMask.NameToLayer(layer));

        if (result)
        {
            Debug.DrawLine(playerOrigin, distanceFromPlayer, Color.green, 0.5f, false);
        }
        else
        {
            Debug.DrawLine(playerOrigin, distanceFromPlayer, Color.red, 0.5f, false);
        }   


        return result;
    }


    //NOW FOR THE SUPER DUPER FUN ATTACKING COLLISION CHECK OH BOY!
    private GameObject collidedGameObject()
    {
        Vector2 playerOrigin = new Vector2(transform.position.x, transform.position.y);
        Vector2 distanceFromPlayer;
        RaycastHit2D hitRay;
        bool result;
        GameObject objectIntersected;   //This will be the object we return
        if (facingRight)
        {
            distanceFromPlayer = new Vector2(transform.position.x + 2.0f, transform.position.y);
        }
        else
        {
            distanceFromPlayer = new Vector2(transform.position.x - 2.0f, transform.position.y);
        }
        //Create a boolean linecast AND a collision detection raycast, one will return a boolean, one will return the object it hits
        //result returns a boolean
        //hitRay returns the object the linecast hit
        result= Physics2D.Linecast(playerOrigin, distanceFromPlayer, 1 << LayerMask.NameToLayer("enemy"));
        hitRay = Physics2D.Linecast(playerOrigin, distanceFromPlayer, 1 << LayerMask.NameToLayer("enemy"));
        
        if (result) //If there was a collision
        {
            //Get the gameObject that was hit by the linecast
            objectIntersected = hitRay.collider.gameObject;
        }
        else  //If not then just return null so it doesn't break our code
        {
            objectIntersected = null;
        }
        return objectIntersected;       //Now return it and work with it

    }


    void OnDestroy()        //Im keeping this here in case you need it, but read up on what it does, because it does some weird stuff if not careful
    {
        
    }


   
    public void killPlayer()
    {

        notKillingPlayer = false;
        Hitpoints = 3;
        
        
        
        GameObject coffin = Instantiate(tombstone, this.transform.position, this.transform.rotation) as GameObject;
        StartCoroutine(waitUntilDead());
    }
    IEnumerator waitUntilDead()
    {
        this.transform.DetachChildren();
        Destroy(this.gameObject);
        yield return new WaitForSeconds(1);
        die = true;
    }

    


    //Disable player movement
    IEnumerator disableMovement()
    {
        allowPlayerMovement = false;
        yield return new WaitForSeconds(0.5f);
        allowPlayerMovement = true;
        animator.SetBool("attacking", false);
    }

    
    

    
}
