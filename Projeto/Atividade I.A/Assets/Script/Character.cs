using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Velocidade")]
    public float moveSpeed;
    public float rotateSpeed;

    [System.Serializable]
    public class FollowPoint
    {
        public Transform pointPosition;
        public bool stopHere;
        public int nextPointIndex;
    }

    [Header("Pontos para seguir")]
    public FollowPoint[] points;
    public int pointIndex;

    [Header("Rigidbody")]
    public Rigidbody rb;

    [Header("Tempo de espera")]
    public float delayTime;
    float timeSinceWaiting;
    public bool waiting;

    [Header("Movendo")]
    public bool moving;

    [Header("Distância mínima")]
    public float minimumDistance;

    private void Update()
    {
        //Vector3 destination = ((points[pointIndex].pointPosition.position - transform.position) * Time.deltaTime).normalized * moveSpeed;

        Vector3 destination = points[pointIndex].pointPosition.position - transform.position;

        destination = destination.normalized * Time.fixedDeltaTime * moveSpeed;

        destination.y = rb.velocity.y;
        //destination = new Vector3(destination.x, rb.velocity.y, destination.z);

        if (moving)
        {
            rb.velocity = destination;

            Quaternion targetRotation = Quaternion.LookRotation(points[pointIndex].pointPosition.position - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;

            transform.rotation = targetRotation;

            float distance = Vector3.Distance(transform.position, points[pointIndex].pointPosition.position);

            if (distance < minimumDistance)
            {
                StopMoving();
            }
        }

        if (waiting)
        {

            timeSinceWaiting += Time.deltaTime;

            if (timeSinceWaiting >= delayTime)
            {
                TakeDecision();
            }
        }

        //rb.velocity = transform.forward * moveSpeed;

        //transform.LookAt(points[pointIndex].pointPosition.position);

        //Quaternion targetRotation = Quaternion.LookRotation(points[pointIndex].pointPosition.position - transform.position);

        //float rot = Mathf.Min(rotateSpeed * Time.deltaTime, 1);

    }

    public void StopMoving()
    {
        moving = false;

        if (!points[pointIndex].stopHere)
        {
            StartWaiting();
        }
    }

    public void StartWaiting()
    {
        timeSinceWaiting = 0;
        waiting = true;
    }

    public void TakeDecision()
    {
        pointIndex = points[pointIndex].nextPointIndex;
        waiting = false;
        moving = true;
    }
}
