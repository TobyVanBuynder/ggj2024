using System.Collections.Generic;
using UnityEngine;

public class TickleUI : MonoBehaviour
{
    private enum TickleButton {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        A,
        B,
        X,
        Y
    }
    private Tickle _currentTickle;
    private List<TickleButton> _sequence;

    public bool IsOpen{get; private set;}

    void Awake()
    {
        IsOpen = false;
    }

    public void Open(int difficulty)
    {
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
    }
}
