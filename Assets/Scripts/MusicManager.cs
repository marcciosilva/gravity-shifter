using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		
	}
	
	void Awake () {
        GameObject go = GameObject.Find("Music");

	}
}
