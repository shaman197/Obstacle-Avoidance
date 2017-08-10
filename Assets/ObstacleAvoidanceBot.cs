using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBot : MonoBehaviour
{
    public Vector3 startPosition;
    public float radius;
    public float speed;
    public Vector3 destination;
    public Vector3 nextPosition;

    private SphereCollider spherecollider;
    private bool canMove;
    public GameObject otherSphere;

	private void Start()
    {
        startPosition = transform.position;
        spherecollider = GetComponent<SphereCollider>();
        radius = spherecollider.radius;
    }
	
	// Update is called once per frame
	private void FixedUpdate()
    {
        if(canMove)
        {
            CheckObatacle();

            if(otherSphere != null)
            {
                transform.RotateAround(otherSphere.transform.position, Vector3.forward, Time.deltaTime * speed);
            }

            //else
            //{
            //    transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
            //    transform.LookAt(destination);
            //}
        }
	}

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spherecollider.center + transform.TransformPoint(nextPosition), radius);
    }

    public void CheckObatacle()
    {
        Collider[] hits = Physics.OverlapSphere(transform.TransformPoint(nextPosition) + spherecollider.center, radius);

        //Debug.Log(transform.name + " " + hits.Length);

        if (hits.Length >= 1)
        {
            foreach (Collider hit in hits)
            {
                if (hit.name != "Plane")
                {
                    otherSphere = hit.gameObject;
                }
            }
        }

    }
}
