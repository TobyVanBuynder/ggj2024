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
        Y,
        MAX
    }
    private Tickle _currentTickle;
    private List<TickleButton> _sequence;
    private float _timeToComplete;
    private const float _baseTimeToComplete = 10.0f;
    [SerializeField] private int _numButtonsModifier = 6;

    public bool IsOpen{get; private set;}

    void Awake()
    {
        _sequence = new List<TickleButton>();
        IsOpen = false;
    }

    public void Open(Tickle tickle)
    {
        _currentTickle = tickle;
        SetupSequence(_currentTickle.Difficulty);
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
    }

    private void SetupSequence(int difficulty)
    {
        _sequence.Clear();

        int sequenceSize = difficulty * _numButtonsModifier;
        _sequence.Capacity = sequenceSize;
        for (int b = 0; b < sequenceSize; b++)
        {
            _sequence.Add((TickleButton)Random.Range(0, (int)TickleButton.MAX));
        }
    }
}
