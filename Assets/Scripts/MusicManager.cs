using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private List<AudioSource> _audioSources;

	// Use this for initialization
	void Start () {
        _audioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
	}

    public void PauseAllAudio()
    {
        _audioSources.RemoveAll(item => item == null);
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        Debug.Log("Array size: " + _audioSources.Count);
    }

    public void ResumeAllAudio()
    {
        _audioSources.RemoveAll(item => item == null);
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.UnPause();
        }
    }
}


