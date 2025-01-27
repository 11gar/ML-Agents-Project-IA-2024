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

    public float movementSpeed = 0.8f;


    private float[] startLocation = { 0, 0 };
    private float[] targetLocation = { 0, 0 };
    private float startDistance = 0;
    public Rigidbody2D rb;

    private int collidedWalls = 0;
    public override void OnEpisodeBegin()
    {
        timeSpent = 0f;
        currentCheckpoint = 1;
        transform.localPosition = new Vector3(UnityEngine.Random.Range(-51f, -47f), UnityEngine.Random.Range(9f, 7f));
        this.startLocation = new float[] { transform.localPosition.x, transform.localPosition.y };
        target.localPosition = new Vector3(UnityEngine.Random.Range(-44f, -38f), UnityEngine.Random.Range(0f, 8f));
        this.targetLocation = new float[] { target.localPosition.x, target.localPosition.y };

        this.startDistance = Vector2.Distance(new Vector2(this.startLocation[0], this.startLocation[1]), new Vector2(this.targetLocation[0], this.targetLocation[1]));
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

        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        transform.right = new Vector3(moveX, moveY, 0);
        if (moveX == 0 && moveY == 0)
        {
            transform.right = new Vector3(1, 0, 0);
        }

        AddReward(timePenalty);


        this.timeSpent = Time.time - startTime;
        rb.MovePosition(transform.position + (new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed));
        if (timeSpent > 10f)
        {
            // backgroundSpriteRenderer.color = Color.red;
            // EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void EndEpisodeTriggered()
    {
        float endDistance = Vector2.Distance(new Vector2(transform.localPosition.x, transform.localPosition.y), new Vector2(target.localPosition.x, target.localPosition.y));
        float distanceReward = startDistance - endDistance;
        AddReward(distanceReward);
        EndEpisode();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision);
        if (collision.TryGetComponent(out Target target))
        {
            // backgroundSpriteRenderer.color = Color.green;
            AddReward(10f);
            EndEpisodeTriggered();
        }
        else if (collision.TryGetComponent(out Wall wall))
        {
            // backgroundSpriteRenderer.color = Color.red;
            timeSpent = Time.time - startTime;
            collidedWalls += 1;
            AddReward(-5f);
            EndEpisodeTriggered();
        }
        else if (collision.TryGetComponent(out Checkpoint checkpoint))
        {

            if (currentCheckpoint == checkpoint.checkpointIndex)
            {
                // backgroundSpriteRenderer.color = Color.blue;
                AddReward(2f);
            }
            this.currentCheckpoint = checkpoint.checkpointIndex + 1;
        }
    }
}
