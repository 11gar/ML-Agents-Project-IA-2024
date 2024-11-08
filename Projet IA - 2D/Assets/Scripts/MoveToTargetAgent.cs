using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToTargetAgent : Agent
{

    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-3.5f, -1.5f), Random.Range(-3.5f, 3.5f));
        target.localPosition = new Vector3(Random.Range(1.5f, 3.5f), Random.Range(-3.5f, 3.5f));
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

        float movementSpeed = 5f;

        transform.localPosition += new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if(collision.TryGetComponent(out Target target))
        {
            backgroundSpriteRenderer.color = Color.green;
            AddReward(10f);
            EndEpisode();
        }
        else if (collision.TryGetComponent(out Wall wall)){
            backgroundSpriteRenderer.color = Color.red;
            AddReward(-2f);
            EndEpisode();
        }
    }
}
