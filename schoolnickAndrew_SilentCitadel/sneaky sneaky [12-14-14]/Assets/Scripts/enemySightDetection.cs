//////////////////////////////////////////////////////////////////////////
// IGNORE
// THIS
// SCRIPT
// IT
// IS
// OLD
// IT IS NOW IMPLEMENTED INTO ARCHER.CS, I KEPT IT FOR REFERENCE SAKE
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class enemySightDetection : MonoBehaviour {


    private Animator animator;
    Vector2 eyeSight;
    Vector2 originPos;
	// Use this for initialization
	void Start () 
    {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (playerDetected())
        {
            animator.SetBool("detected", true);
        }
        else
        {
            animator.SetBool("detected", false);
        }
	}


    private bool playerDetected()
    {
        originPos = new Vector2(transform.position.x, transform.position.y);
        eyeSight = new Vector2(transform.position.x - 30, transform.position.y - 1);
        bool result = Physics2D.Linecast(originPos, eyeSight, 1 << LayerMask.NameToLayer("playerObj"));
        if (result)
        {
            Debug.DrawLine(originPos, eyeSight, Color.green, 0.5f, false);
        }
        else
        {
            Debug.DrawLine(originPos, eyeSight, Color.red, 0.5f, false);
        }
        return result;
    }
}
