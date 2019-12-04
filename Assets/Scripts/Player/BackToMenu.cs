using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class BackToMenu : MonoBehaviour
{
    public UnityEvent onEnter, onExit;

    public void OnTriggerEnter(Collider col)
    {
        onEnter.Invoke();
    }

    public void LoadSpecificScene(int index)
    {
        SceneManager.LoadScene(index); // Loads what ever scene I reference in the inspector
    }
}
