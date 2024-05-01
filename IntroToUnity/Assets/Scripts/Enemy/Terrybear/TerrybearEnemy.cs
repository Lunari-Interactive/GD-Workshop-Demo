using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrybearEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float defaultSpeed = 45f;
    float normalSpeed;
    public float runSpeed = 85f;
    public float detectionDistance;
    public GameObject eyes;

    [Header("Area of Patrol")]
    public GameObject patrolPoint1;
    public GameObject patrolPoint2;

    bool isAggressive = false;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform currentPoint;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        currentPoint = patrolPoint1.transform;
        anim.SetBool("isAgressive", false);
        normalSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.x > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, Vector2.right, detectionDistance);
            if (hit && hit.collider.tag == "Player")
            {
                anim.SetBool("isAgressive", true);
                defaultSpeed = runSpeed;
            }
            else
            {
                anim.SetBool("isAgressive", false);
                defaultSpeed = normalSpeed;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, -Vector2.right, detectionDistance);
            if (hit && hit.collider.tag == "Player")
            {
                anim.SetBool("isAgressive", true);
                defaultSpeed = runSpeed;
            }
            else
            {
                anim.SetBool("isAgressive", false);
                defaultSpeed = normalSpeed;
            }
        }

        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == patrolPoint1.transform)
        {
            rb.velocity = new Vector2(defaultSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-defaultSpeed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) <= 0.5f && currentPoint == patrolPoint1.transform)
        {
            currentPoint = patrolPoint2.transform;
            Flip();
        }
        if (Vector2.Distance(transform.position, currentPoint.position) <= 0.5f && currentPoint == patrolPoint2.transform)
        {
            currentPoint = patrolPoint1.transform;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPoint1.transform.position, 0.5f);
        Gizmos.DrawWireSphere(patrolPoint2.transform.position, 0.5f);
        Vector3 pos = new Vector3(eyes.transform.position.x + detectionDistance, eyes.transform.position.y, eyes.transform.position.z);
        Gizmos.DrawLine(eyes.transform.position, pos);
    }
}
