using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

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
    private const float _baseTimeToComplete = 10.0f;
    [SerializeField] private List<TickleButton> _sequence;
    [SerializeField] private float _timeToComplete;
    [SerializeField] private bool _isTickling = false;
    [SerializeField] private int _numButtonsModifier = 6;

    public bool IsOpen{get; private set;}

    void Awake()
    {
        _sequence = new List<TickleButton>();
        IsOpen = false;
    }

    public void Open(Tickle tickle)
    {
        IsOpen = true;
        _currentTickle = tickle;
        _timeToComplete = _baseTimeToComplete - _currentTickle.Difficulty * 0.5f;
        SetupSequence(_currentTickle.Difficulty);
        _isTickling = true;
    }

    public void Close(bool isSuccess)
    {
        IsOpen = false;
        _currentTickle.End(isSuccess); // TODO: change this latah
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

    void Update()
    {
        if (_isTickling)
        {
            _timeToComplete -= Time.deltaTime;
            if (_timeToComplete <= 0) {
                _isTickling = false;
                Close(false);
            }
        }
    }
}
