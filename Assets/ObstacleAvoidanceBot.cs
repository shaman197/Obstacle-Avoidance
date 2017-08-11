using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleAvoidanceBot : MonoBehaviour
{
    public event UnityAction<GameObject> TriggerSphereLink;

    public Vector3 startPosition;
    public float radius;
    public float speed;
    public float rotationSpeed;
    public Vector3 destination;
    public Vector3 obstacleCastFromOrigin;
    public float maxLinkedTime;
    public float waitTimeForLinkAgain;

    private SphereCollider spherecollider;
    private bool canMove;
    private GameObject otherSphere;
    private bool linkCountdown;
    private bool canLink;


    private void Start()
    {
        startPosition = transform.position;
        spherecollider = GetComponent<SphereCollider>();
        canLink = true;
    }
	
	// Update is called once per frame
	private void FixedUpdate()
    {
        if(canMove)
        {
            CheckObstacle();

            // if there is another sphere in the way go arround it till the time limit or there is another sphere in the way
            if(otherSphere != null)
            {
                transform.RotateAround(otherSphere.transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
            }

            // else just move forward to the desination
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
                transform.LookAt(destination);
            }
        }
	}

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void CheckObstacle()
    {
        // A big sphere is cast to check if there are obstacles in front of him. The cast is bigger than the sphere to check the sides
        Collider[] hits = Physics.OverlapSphere(transform.TransformPoint(obstacleCastFromOrigin) + spherecollider.center, radius);

        if(canLink)
        {
            if (hits.Length >= 1)
            {
                foreach (Collider hit in hits)
                {
                    if (hit.name != "Plane")
                    {
                        otherSphere = hit.gameObject;

                        // The object triggers a event and send it to the manager
                        TriggerSphereLink(otherSphere);

                        if (!linkCountdown)
                            StartCoroutine(RemoveOtherSphereAfterTime());
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spherecollider.center + transform.TransformPoint(obstacleCastFromOrigin), radius);
    }

    private IEnumerator RemoveOtherSphereAfterTime()
    {
        linkCountdown = true;

        yield return new WaitForSecondsRealtime(maxLinkedTime);
        otherSphere = null;

        linkCountdown = false;
    }

    private IEnumerator WaitTimeForNextLink()
    {
        canLink = false;

        yield return new WaitForSecondsRealtime(waitTimeForLinkAgain);

        canLink = true;
    }

    public void RemoveOtherSphere()
    {
        otherSphere = null;
    }

    public void ActivateWaitTimeForNextLink()
    {
        StartCoroutine(WaitTimeForNextLink());
    }

    public bool GetCanLink()
    {
        return canLink;
    }

    public void ResetSphere()
    {
        canMove = false;
        transform.position = startPosition;
    }
}
