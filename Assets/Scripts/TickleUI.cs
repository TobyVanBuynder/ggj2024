using System.Collections.Generic;
using UnityEngine;

public class TickleUI : MonoBehaviour
{
    private Tickle _currentTickle;
    private const float _baseTimeToComplete = 10.0f;
    private const float _buttonWidthSpacing = 80f;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private List<GameObject> _sequence;
    [SerializeField] private float _timeToComplete = -1f;
    [SerializeField] private bool _isTickling = false;
    [SerializeField] private int _numButtonsModifier = 3;
    [SerializeField] private GameObject[] _buttonPrefabs;

    private int _currentSequenceIndex = 0;

    public bool IsOpen{get; private set;}

    void Awake()
    {
        _sequence = new List<GameObject>();
        IsOpen = false;
    }

    public void Open(Tickle tickle)
    {
        IsOpen = true;
        _currentTickle = tickle;
        _timeToComplete = _baseTimeToComplete - _currentTickle.Difficulty * 0.5f;
        SetupSequence(_currentTickle.Difficulty);
        _currentSequenceIndex = 0;
        _sequence[0].GetComponent<TickleButtonPrompt>().Show();
        _isTickling = true;
    }

    public void Close(bool isSuccess)
    {
        IsOpen = false;
        _currentTickle.End(isSuccess); // TODO: change this latah
    }

    public void OnKeyPressed(TickleButtonType pressedButton)
    {
        TickleButtonPrompt currentButtonPrompt = _sequence[_currentSequenceIndex].GetComponent<TickleButtonPrompt>();
        if (pressedButton == currentButtonPrompt.button)
        {
            currentButtonPrompt.Complete();
            if (_currentSequenceIndex + 1 < _sequence.Count)
            {
                MoveToNextButton();
            }
            else
            {
                // Finish success
                Close(true);
            }
        }
        else
        {
            // Wrong button pressed => failed
            Close(false);
        }
    }

    private void MoveToNextButton()
    {
        _currentSequenceIndex++;
        _sequence[_currentSequenceIndex].GetComponent<TickleButtonPrompt>().Show();
    }

    private void SetupSequence(int difficulty)
    {
        _sequence.Clear();

        int sequenceSize = difficulty * _numButtonsModifier;
        _sequence.Capacity = sequenceSize;
        float halfWidth = (sequenceSize - 1) * _buttonWidthSpacing * 0.5f;
        for (int b = 0; b < sequenceSize; b++)
        {
            int nextButtonToPress = Random.Range(0, /*(int)TickleButtonType.MAX*/2);
            GameObject newButtonPrompt = Instantiate(_buttonPrefabs[nextButtonToPress], _canvas.transform);
            newButtonPrompt.transform.Translate(-halfWidth + b * _buttonWidthSpacing, 50f, 0);
            _sequence.Add(newButtonPrompt);
            
        }
    }

    void Update()
    {
        if (_isTickling)
        {
            if ((_timeToComplete -= Time.deltaTime) <= 0)
            {
                _isTickling = false;
                Close(false);
            }
        }
    }
}
