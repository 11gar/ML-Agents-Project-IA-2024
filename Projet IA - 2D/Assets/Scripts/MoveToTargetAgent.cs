using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Globalization;
using Unity.VisualScripting;

public class MoveToTargetAgent : Agent
{

    [SerializeField] private Transform target;
    private int currentCheckpoint = 1;

    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    private float timePenalty = -0.01f;

    private float timeSpent = 0f;
    private float startTime = 0f;
    private Rigidbody2D rb;


    override public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnEpisodeBegin()
    {
        currentCheckpoint = 1;
        //transform.localPosition = new Vector3(Random.Range(-3.5f, -1.5f), Random.Range(-3.5f, 3.5f));
        //target.localPosition = new Vector3(Random.Range(1.5f, 3.5f), Random.Range(-3.5f, 3.5f));
        transform.localPosition = new Vector3(-49, 8);
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

        float movementSpeed = 5f;

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
