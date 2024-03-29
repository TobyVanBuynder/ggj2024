using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TickleUI : MonoBehaviour {
	private TickleSpot _currentTickleSpot;
	private const float _baseTimeToComplete = 10.0f;
	private const float _buttonWidthSpacing = 80f;
	[SerializeField] private RectTransform _panel;
	[SerializeField] private List<GameObject> _sequence;
	[SerializeField] private float _timeLeft = -1f;
	[SerializeField] private float _fullCompleteTime = -1f;
	[SerializeField] private bool _isTickling = false;
	[SerializeField] private GameObject[] _buttonPrefabs;
	[SerializeField] private GameObject _minigameTimeBar;
	[SerializeField] private Slider _minigameTimeSlider;

	public Action UIOpened;
	public Action UIClosed;

	private int _numButtonsModifier = 2;
	private int _currentSequenceIndex = 0;

	public bool IsOpen { get; private set; }

	void Awake() {
		_sequence = new List<GameObject>();
		IsOpen = false;
		_panel.gameObject.SetActive(false);
		_minigameTimeBar.SetActive(false);
	}

	public void Open(TickleSpot tickleSpot) {
		IsOpen = true;
		_panel.gameObject.SetActive(true);
		_currentTickleSpot = tickleSpot;
		_fullCompleteTime = _timeLeft = _baseTimeToComplete - _currentTickleSpot.Difficulty * 0.5f;
		SetupSequence(_currentTickleSpot.Difficulty);
		_currentSequenceIndex = 0;
		_sequence[0].GetComponent<TickleButtonPrompt>().Show();
		_isTickling = true;
		_minigameTimeBar.SetActive(true);
		UIOpened?.Invoke();
	}

	void UpdateTimerUI() {
		var t = _timeLeft / _fullCompleteTime;
		_minigameTimeSlider.value = t;
	}

	public void Close(bool isSuccess) {
		IsOpen = false;
		for (int i = 0; i < _sequence.Count; i++) {
			Destroy(_sequence[i]);
		}
		_panel.gameObject.SetActive(false);
		_minigameTimeBar.SetActive(false);
		_currentTickleSpot.End(isSuccess);
		UIClosed?.Invoke();
	}

	public void OnKeyPressed(TickleButtonType pressedButton) {
		TickleButtonPrompt currentButtonPrompt = _sequence[_currentSequenceIndex].GetComponent<TickleButtonPrompt>();
		if (pressedButton == currentButtonPrompt.button) {
			currentButtonPrompt.Complete();
			if (_currentSequenceIndex + 1 < _sequence.Count) {
				MoveToNextButton();
			}
			else {
				// Finish success
				Close(true);
			}
		}
		else {
			// Wrong button pressed => failed
			Close(false);
		}
	}

	private void MoveToNextButton() {
		_currentSequenceIndex++;
		_sequence[_currentSequenceIndex].GetComponent<TickleButtonPrompt>().Show();
	}

	private void SetupSequence(int difficulty) {
		_sequence.Clear();

		int sequenceSize = difficulty * _numButtonsModifier;
		_sequence.Capacity = sequenceSize;
		for (int b = 0; b < sequenceSize; b++) {
			int nextButtonToPress = Random.Range(0, (int) TickleButtonType.MAX);
			GameObject newButtonPrompt = Instantiate(_buttonPrefabs[nextButtonToPress], _panel);
			_sequence.Add(newButtonPrompt);
		}
	}

	void Update() {
		if (_isTickling) {
			UpdateTimerUI();

			if ((_timeLeft -= Time.deltaTime) <= 0) {
				_isTickling = false;
				Close(false);
			}
		}
	}
}
