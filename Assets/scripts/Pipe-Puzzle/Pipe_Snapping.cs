using UnityEngine;

public class Pipe_Snapping : MonoBehaviour
{
    [SerializeField] private Transform snapTarget;
    [SerializeField] private float snapDistance = 0.15f;
    [SerializeField, Range(0f, 45f)] private float snapAngle = 15f;
    [SerializeField] private Transform[] connectionPoints;
    [SerializeField] private string socketTag = "PipeSocket";

    private Rigidbody body;
    private Manager_Pipe_Puzzle puzzleManager;

    public bool IsSnapped { get; private set; }
    public Transform[] ConnectionPoints => connectionPoints ?? System.Array.Empty<Transform>();

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        puzzleManager = GetComponentInParent<Manager_Pipe_Puzzle>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsSnapped || snapTarget == null || other.gameObject.tag != socketTag)
        {
            return;
        }

        Transform socket = other.transform;
        if (socket != snapTarget && !socket.IsChildOf(snapTarget) && !snapTarget.IsChildOf(socket))
        {
            return;
        }

        if (Vector3.Distance(transform.position, snapTarget.position) > snapDistance ||
            Quaternion.Angle(transform.rotation, snapTarget.rotation) > snapAngle)
        {
            return;
        }

        SnapToTarget();
    }

    public void SnapToTarget()
    {
        if (IsSnapped || snapTarget == null)
        {
            return;
        }

        transform.SetPositionAndRotation(snapTarget.position, snapTarget.rotation);
        IsSnapped = true;

        if (body != null)
        {
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.isKinematic = true;
        }

        puzzleManager?.NotifyPieceSnapped(this);
    }

    public void ResetSnap()
    {
        IsSnapped = false;
        if (body != null)
        {
            body.isKinematic = false;
        }
    }
}
