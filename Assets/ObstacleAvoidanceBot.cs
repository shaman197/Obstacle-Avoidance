using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBot : MonoBehaviour
{
    public Vector3 startPosition;
    public float radius;
    public float speed;
    public Vector3 destination;

    private SphereCollider spherecollider;
    private bool canMove;

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
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
        }
	}

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
