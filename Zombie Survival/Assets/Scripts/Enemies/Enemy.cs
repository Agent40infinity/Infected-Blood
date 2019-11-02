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

    //References:
    public GameObject player;
    public NavMeshAgent nav;

    public void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    public void Update()
    {
        AI();
    }

    public void AI()
    { 
        //Collider[] inRange = Physics.OverlapSphere(gameObject.transform.position, 10f);
        //for (int i = 0; i < inRange.Length; i++)
        //{
        //    if (inRange[i].tag == "Player" && (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) > 10 || Mathf.Abs(player.transform.position.z - gameObject.transform.position.x) > 10))
        //    {
        //        player = inRange[i].gameObject;
        //    }
        //}

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
        yield return new WaitForSeconds(0.2f);
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
    }
}
