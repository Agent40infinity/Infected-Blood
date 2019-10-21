using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        Vector3.MoveTowards(transform.position, player.transform.position, 50f);
    }

    private void OnDestroy()
    {
        GameManager.enemiesAlive--;
    }
}
