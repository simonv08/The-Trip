using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Manager_Pipe_Puzzle : MonoBehaviour
{
    [Header("Puzzle layout")]
    [SerializeField] private Pipe_Snapping[] pieces;
    [SerializeField] private Transform sourceConnection;
    [SerializeField] private Transform destinationConnection;
    [SerializeField] private float connectionDistance = 0.08f;
    [SerializeField, Range(0f, 45f)] private float connectionAngle = 20f;

    [Header("Completion")]
    [SerializeField] private GameObject loosePiecesRoot;
    [SerializeField] private GameObject completedModel;
    [SerializeField] private UnityEvent onPuzzleCompleted;

    private bool isPuzzleCompleted;

    public bool IsPuzzleCompleted => isPuzzleCompleted;

    private void Awake()
    {
        if (pieces == null || pieces.Length == 0)
        {
            pieces = GetComponentsInChildren<Pipe_Snapping>(true);
        }

        if (completedModel != null)
        {
            completedModel.SetActive(false);
        }
    }

    private void Start()
    {
        CheckSolution();
    }

    public void NotifyPieceSnapped(Pipe_Snapping piece)
    {
        CheckSolution();
    }

    public bool CheckSolution()
    {
        if (isPuzzleCompleted || pieces == null || pieces.Length == 0 ||
            sourceConnection == null || destinationConnection == null)
        {
            return isPuzzleCompleted;
        }

        foreach (Pipe_Snapping piece in pieces)
        {
            if (piece == null || !piece.IsSnapped)
            {
                return false;
            }
        }

        if (!EveryConnectionIsSealed() || !HasPathFromSourceToDestination())
        {
            return false;
        }

        CompletePuzzle();
        return true;
    }

    private bool EveryConnectionIsSealed()
    {
        var usedConnections = new HashSet<Transform>();

        foreach (Pipe_Snapping piece in pieces)
        {
            foreach (Transform point in piece.ConnectionPoints)
            {
                Transform connected = FindConnectedPoint(point);
                if (point == null || connected == null || !usedConnections.Add(connected))
                {
                    return false;
                }
            }
        }

        Transform source = FindConnectedPoint(sourceConnection);
        Transform destination = FindConnectedPoint(destinationConnection);
        return source != null && destination != null && source != destination;
    }

    private bool HasPathFromSourceToDestination()
    {
        Pipe_Snapping sourcePiece = GetOwner(FindConnectedPoint(sourceConnection));
        Pipe_Snapping destinationPiece = GetOwner(FindConnectedPoint(destinationConnection));
        if (sourcePiece == null || destinationPiece == null)
        {
            return false;
        }

        var visited = new HashSet<Pipe_Snapping>();
        var pending = new Queue<Pipe_Snapping>();
        pending.Enqueue(sourcePiece);

        while (pending.Count > 0)
        {
            Pipe_Snapping current = pending.Dequeue();
            if (!visited.Add(current))
            {
                continue;
            }

            if (current == destinationPiece)
            {
                return true;
            }

            foreach (Transform point in current.ConnectionPoints)
            {
                Pipe_Snapping next = GetOwner(FindConnectedPoint(point));
                if (next != null && !visited.Contains(next))
                {
                    pending.Enqueue(next);
                }
            }
        }

        return false;
    }

    private Transform FindConnectedPoint(Transform point)
    {
        if (point == null)
        {
            return null;
        }

        Transform connected = null;
        float closestDistance = connectionDistance;

        foreach (Pipe_Snapping piece in pieces)
        {
            if (piece == null)
            {
                continue;
            }

            foreach (Transform candidate in piece.ConnectionPoints)
            {
                if (candidate == null || candidate == point)
                {
                    continue;
                }

                float distance = Vector3.Distance(point.position, candidate.position);
                float angle = Vector3.Angle(point.forward, -candidate.forward);
                if (distance <= closestDistance && angle <= connectionAngle)
                {
                    connected = candidate;
                    closestDistance = distance;
                }
            }
        }

        return connected;
    }

    private Pipe_Snapping GetOwner(Transform point)
    {
        return point == null ? null : point.GetComponentInParent<Pipe_Snapping>();
    }

    private void CompletePuzzle()
    {
        isPuzzleCompleted = true;

        if (loosePiecesRoot != null)
        {
            loosePiecesRoot.SetActive(false);
        }

        if (completedModel != null)
        {
            completedModel.SetActive(true);
        }

        onPuzzleCompleted?.Invoke();
    }
}
