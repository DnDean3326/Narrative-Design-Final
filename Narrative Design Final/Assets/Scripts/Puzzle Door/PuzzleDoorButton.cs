using System;
using UnityEngine;

public class PuzzleDoorButton : MonoBehaviour
{
    private PuzzleDoor puzzleDoor;

    public void Initialize(PuzzleDoor parent)
    {
        puzzleDoor = parent;
    }

    public void PuzzleDoorButtonPressed()
    {
        puzzleDoor.AttemptDoorOpen();
    }
}
