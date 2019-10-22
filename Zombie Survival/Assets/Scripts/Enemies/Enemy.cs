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
        Collider[] inRange = Physics.OverlapSphere(gameObject.transform.position, 10f);
        for (int i = 0; i < inRange.Length; i++)
        {
            if (inRange[i].tag == "Player" && (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) > 10 || Mathf.Abs(player.transform.position.z - gameObject.transform.position.x) > 10))
            {
                player = inRange[i].gameObject;
            }
        }
        nav.SetDestination(player.transform.position);
    }

    private void OnDestroy()
    {
        GameManager.enemiesAlive--;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
