using System;
using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public float spotLifetime = 20f;
    public float intervalBetweenTickleSpots = 10f;
    
    private TickleSpot[] _tickleSpots;
    private Coroutine tickleSpotDisplayRoutine;
    private int _tickleSpotToActivate;
    private GameObject _treasureObject;

    public Action OnEndGame;
    
    public enum Mood
    {
        SpittingFire = -1,
        Grumpy = 0,
        Resting = 3,
        Happy = 4,
        Cute = 5
    }
    private Mood _moodLevel;

    private void Start()
    {
        _moodLevel = Mood.Grumpy;
        _tickleSpots = FindObjectsOfType<TickleSpot>();
        foreach (TickleSpot t in _tickleSpots)
        {
            t.gameObject.SetActive(false);
            t.TicklingMinigameEnded += OnMinigameEnded;
        }

        tickleSpotDisplayRoutine = StartCoroutine(DisplayNewTickleSpot());
    }

    private IEnumerator DisplayNewTickleSpot()
    {
        while (true)
        {
            _tickleSpots[_tickleSpotToActivate].Appear(spotLifetime);
            _tickleSpotToActivate = (_tickleSpotToActivate + 1) % _tickleSpots.Length;
        
            yield return new WaitForSeconds(intervalBetweenTickleSpots);
        }
    }

    private void OnMinigameEnded(bool success)
    {
        ChangeMood(success ? +1 : -1);
        if (_moodLevel == Mood.Cute) EndGame();
    }

    private void ChangeMood(int influenceChange)
    {
        _moodLevel += influenceChange;

        if (_moodLevel < Mood.Grumpy) _moodLevel = Mood.Grumpy;
        if (_moodLevel > Mood.Cute) _moodLevel = Mood.Cute;
    }

    public void EndGame()
    {
        StopCoroutine(tickleSpotDisplayRoutine);
        OnEndGame?.Invoke();
        ShowTreasure();
    }

    private void ShowTreasure()
    {
        // TODO: Open Dragon's mouth
        _treasureObject.SetActive(true);
    }

    private void OnDestroy()
    {
        foreach (TickleSpot t in _tickleSpots)
        {
            t.TicklingMinigameEnded -= OnMinigameEnded;
        }
    }
}
