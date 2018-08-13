using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PianoController : MonoBehaviour {

	private AudioSource source;
	public CustomerAI pianist;
	private bool isPlaying = true;
	public AudioClip faceplantSfx;
	public AudioClip[] songs;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void LateUpdate () {
		if(!source.isPlaying){
			source.clip = songs[(int) Random.Range(0, songs.Length)];
			source.Play();
		}
		if(isPlaying && pianist.state != CustomerAI.State.piano){
			source.clip = faceplantSfx;
			source.Play();
			isPlaying = false;
		}
		if(!isPlaying && pianist.state == CustomerAI.State.piano){
			isPlaying = true;
		}
	}

	void OnMouseDown(){
		AudioSource.PlayClipAtPoint(faceplantSfx, transform.position);
	}

	void OnCollisionEnter(Collision collision){
		AudioSource.PlayClipAtPoint(faceplantSfx, transform.position, collision.impulse.magnitude);
	}
}
