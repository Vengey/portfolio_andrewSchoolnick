using UnityEngine;
using System.Collections;

public class tombStoneScript : MonoBehaviour {


    private bool animate = true;
    private bool changeDirection = false;
	// Use this for initialization
	void Start () 
    {
        StartCoroutine("animationTime");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (animate)
        {
            if (changeDirection)
            {
                this.transform.Translate(0, -1, 0);
            }
            else
            {
                this.transform.Translate(0, 1, 0);
            }
        }
	}
    IEnumerator animationTime()
    {
        yield return new WaitForSeconds(0.5f);
        changeDirection = true;
        yield return new WaitForSeconds(0.5f);
        animate = false;
        Application.LoadLevel(Application.loadedLevel);
        StopCoroutine("disableMovement");

    }
}
