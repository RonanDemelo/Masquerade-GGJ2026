using UnityEngine;
using UnityEngine.AI;

public class DebegNavMesh : MonoBehaviour
{
    public bool velocity;
    public bool desiredVelocity;
    public bool path;

    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        if (desiredVelocity)
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }

        if (path)
        {
            Gizmos.color = Color.purple;
            var _agentpath = agent.path;
            Vector3 _prevCorver = transform.position;
            foreach (var _corner in _agentpath.corners)
            {
                Gizmos.DrawLine(_prevCorver, _corner);
                Gizmos.DrawSphere(_corner, 0.1f);
                _prevCorver= _corner;
            }
        }
    }
}
