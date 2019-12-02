using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Varibles 
    public AudioSource adSource; // Declaring the audio source is public
    public AudioClip[] adClips; // An Array of music clips
    public static MusicManager instance = null;
    #endregion

    #region Start Coroutine
    public void Awake()
    {
        StartCoroutine(PlayAudioSequentially());

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // TO ensure that the music does not stop between scenes

        DontDestroyOnLoad(gameObject);
        // And also ensures that only one instance of the AudioManager can occur
    }
    #endregion
    #region 
    IEnumerator PlayAudioSequentially()
    {
        yield return null;

        int i = 0;
        while(true)
        {
            adSource.clip = adClips[i];

            adSource.Play(); // Play the clips

            while (adSource.isPlaying)
            {
                yield return null;
            }

            i = (i + 1) % adClips.Length; // Modulo which is the % operator, divides i+1 with the adclips.length and finds the remainder
        } // Go back and select the next clip in the array

        // StartCoroutine(PlayAudioSequentially()); // If you leave the for loop, you can restart this function like this
    }
    #endregion



}