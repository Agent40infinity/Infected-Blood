using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent nav;

    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        nav = gameObject.GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        nav.SetDestination(player.transform.position);
    }

    private void OnDestroy()
    {
        GameManager.enemiesAlive--;
    }
}
