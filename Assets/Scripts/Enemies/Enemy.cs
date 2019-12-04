using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class Enemy : NetworkBehaviour
{
    //General:
    public float attackDistance = 2f;
    public float attackRange = 4f;
    public int damage = 20;
    public float detectionRadius = 5f;
    public float noiseTimer = 0f;
    public int health = 75;

    //References:
    public Player player;
    public NavMeshAgent nav;
    public AudioSource sound;
    public AudioClip[] adClips;

    public void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        GetClosestPlayer();
        sound = GetComponent<AudioSource>();

        noiseTimer = Random.Range(0.5f, 9f);

        if (!isServer)
        {
            nav.Stop();
        }
    }

    public void Update()
    {
        noiseTimer -= Time.deltaTime;
        if (noiseTimer < 0f)
        {
            sound.clip = adClips[Random.Range(0, adClips.Length)];
            sound.Play();
            noiseTimer = Random.Range(0.5f, 5f);
        }

        if (!isServer)
            return;

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

    [Command]
    public void CmdTakeDamage(int damage, Player player)
    {
        player.score += 100;
        player.money += 10;
        health -= damage;
        if (health <= 0)
        {
            CmdDeath();
            player.score += 200;
            player.money += 20;
        }
    }

    [Command]
    public void CmdDeath()
    {
        player.kills++;
        Destroy(this.gameObject);
        NetworkServer.Destroy(this.gameObject);
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
