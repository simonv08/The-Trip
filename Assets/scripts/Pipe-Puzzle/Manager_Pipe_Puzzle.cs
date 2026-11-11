using UnityEngine;

public class Manager_Pipe_Puzzle
{

    private bool isPuzzleCompleted = false;

    void Start()
    {

    }

    void Update()
    {
        IsCompleted();
    }

    void IsCompleted()
    {
        if (isPuzzleCompleted)
        {
            return;
        }

        // check if all pipes are connected
        // if yes, call completePuzzle()
    }

    void completePuzzle()
    {
        isPuzzleCompleted = true;
        Debug.Log("Puzzle Completed!");
        // change loose pipes to 1 set model
        // play animation
    }
}
