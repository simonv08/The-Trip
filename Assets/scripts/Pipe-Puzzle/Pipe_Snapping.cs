using UnityEngine;

public class Pipe_Snapping : MonoBehaviour
{

    private bool isPuzzleCompleted = false;

    void Start()
    {

    }

    void Update()
    {
        CheckPipeConnections();
    }

    void CheckPipeConnections()
    {
        if (isPuzzleCompleted)
        {
            return;
        }

        // check if all pipes are connected
        // if yes, call completePuzzle()
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pipe"))
        {
            SnapPipe(other.gameObject);
        }
    }

    void SnapPipe(GameObject pipe)
    {
        // snap the pipe to the correct position
        // play snapping sound
    }
}