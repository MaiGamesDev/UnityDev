using UnityEditor;
using UnityEngine;

public class SensingPlayer : MonoBehaviour
{
    [SerializeField] protected float sensingRange = 3f;
    protected GameObject player;

    protected bool isTrackingPlayer = false;

    protected enum StateType { Left, Idle, Right }
    protected StateType stateType;

    protected void SetStateType(StateType state)
    {
        stateType = state;
    }

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    protected void PlayerSensing()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        isTrackingPlayer = distance <= sensingRange;

        if (isTrackingPlayer)
        {         
            if (player.transform.position.x < transform.position.x)
                SetStateType(StateType.Left);
            else
                SetStateType(StateType.Right);
        }

        else
            isTrackingPlayer = false;
    }
}
