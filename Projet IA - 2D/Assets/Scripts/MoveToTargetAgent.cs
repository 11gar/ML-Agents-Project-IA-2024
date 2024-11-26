using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Globalization;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using System;

public class MoveToTargetAgent : Agent
{

    [SerializeField] private Transform target;
    private int currentCheckpoint = 1;

    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    private float timePenalty = -0.01f;

    private float timeSpent = 0f;
    private float startTime = 0f;
    private Rigidbody2D rb;
    private RayPerceptionSensorComponent2D raySensor;

    override public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnRaycastHit(RaycastHit2D hit)
    {
        Debug.Log("hit");
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Checkpoint checkpoint))
            {
                if (currentCheckpoint == checkpoint.checkpointIndex)
                {
                    Debug.Log("Checkpoint hit");
                    AddReward(0.5f);
                }
            }
        }
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Target target))
            {
                AddReward(1f);
            }
        }
    }


    public override void OnEpisodeBegin()
    {
        timeSpent = 0f;
        currentCheckpoint = 1;
        transform.localPosition = new Vector3(UnityEngine.Random.Range(-51f, -47f), UnityEngine.Random.Range(9f, 7f));
        target.localPosition = new Vector3(UnityEngine.Random.Range(-44f, -38f), UnityEngine.Random.Range(0f, 9f));
        // transform.localPosition = new Vector3(UnityEngine.Random.Range(-3.5f, -1.5f), UnityEngine.Random.Range(-3.5f, 3.5f));
        // target.localPosition = new Vector3(UnityEngine.Random.Range(1.5f, 3.5f), UnityEngine.Random.Range(-3.5f, 3.5f));
        // transform.localPosition = new Vector3(-49, 8);
        this.startTime = Time.time;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)target.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        // timeSpent += 0.01f;
        // if (timeSpent < 1)
        // {
        //     return;
        // }
        // else
        // {
        //     Debug.Log("go");
        // }

        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        transform.right = new Vector3(moveX, moveY, 0);
        if (moveX == 0 && moveY == 0)
        {
            transform.right = new Vector3(1, 0, 0);
        }

        AddReward(timePenalty);

        float movementSpeed = 1f;
        // float turnSpeed = 180f;

        // transform.rotation *= Quaternion.Euler(0, 0, -turn * turnSpeed * Time.deltaTime);
        // transform.position += transform.right * forward * movementSpeed * Time.deltaTime;


        transform.localPosition += new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void EndEpisodeTriggered()
    {
        EndEpisode();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision);
        if (collision.TryGetComponent(out Target target))
        {
            backgroundSpriteRenderer.color = Color.green;
            AddReward(50f);
            EndEpisodeTriggered();
        }
        else if (collision.TryGetComponent(out Wall wall))
        {
            backgroundSpriteRenderer.color = Color.red;
            timeSpent = Time.time - startTime;
            AddReward(-10f);
            EndEpisodeTriggered();
        }
        else if (collision.TryGetComponent(out Checkpoint checkpoint))
        {

            if (currentCheckpoint == checkpoint.checkpointIndex)
            {
                backgroundSpriteRenderer.color = Color.blue;
                AddReward(10f);
            }
            this.currentCheckpoint = checkpoint.checkpointIndex + 1;
        }
    }
}
