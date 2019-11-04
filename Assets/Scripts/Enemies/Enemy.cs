using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class Enemy : MonoBehaviour
{
    //General:
    public float attackDistance = 2f;
    public float attackRange = 4f;
    public int damage = 20;
    public float detectionRadius = 5f;

    //References:
    public Player player;
    public NavMeshAgent nav;

    public void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();

        GetClosestPlayer();
    }

    public void Update()
    {
        AI();

        if (player)
        {
            if (player.playerDead == true)
            {
                player = null;
            }
        }
    }

    public Player GetClosestPlayer()
    {
        Player result = null;
        float minDistance = float.PositiveInfinity;
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (hit.tag == "Player")
            {
                Vector3 playerPosition = hit.transform.position;
                Vector3 enemyPosition = transform.position;
                float distance = Vector3.Distance(playerPosition, enemyPosition);
                if (distance < minDistance)
                {
                    result = hit.GetComponent<Player>();
                    minDistance = distance;
                }
            }
        }
        return result;
    }

    public void AI()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if (distance > attackDistance)
            {
                nav.enabled = true;
                nav.SetDestination(player.transform.position);
            }
            else
            {
                nav.enabled = false;
                StartCoroutine(Attack());
            }
        }
    }

    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.red, 5f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if (hit.collider.tag == "Player")
            {
                Player playerHitRef = hit.collider.gameObject.GetComponent<Player>();
                playerHitRef.TakeDamage(damage);
            }
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        GameManager.enemiesAlive--;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
