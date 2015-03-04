///////////////////////////////////////////////////////////////////////////////////////////
// For some reason particle systems have a tendency of going behind sprites
// ATTACH THIS SCRIPT TO ALL PARTICLE SYSTEMS to keep them from going behind entities
///////////////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

public class bringParticlesToFront : MonoBehaviour {

    public ParticleSystem caughtParticleSystem;
    GameObject ps;

	// Use this for initialization
	void Start () 
    {
       
        this.particleSystem.renderer.sortingLayerName = "Foreground";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
