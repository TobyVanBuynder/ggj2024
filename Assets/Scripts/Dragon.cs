using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public float intervalBetweenTickleSpots = 10f;
    
    private Tickle[] _tickleSpots;
    private Coroutine tickleSpotDisplayRoutine;
    private int _tickleSpotToActivate;
    
    private enum Mood
    {
        Grumpy = 0,
        Resting = 3,
        Happy = 4,
        Cute = 5
    }
    private Mood _moodLevel;

    void Awake()
    {
        _moodLevel = Mood.Grumpy;
    }

    private void Start()
    {
        _tickleSpots = FindObjectsOfType<Tickle>();
        foreach (Tickle t in _tickleSpots)
        {
            t.gameObject.SetActive(false);
            t.TicklingMinigameEnded += OnMinigameEnded;
        }

        tickleSpotDisplayRoutine = StartCoroutine(DisplayNewTickleSpot());
    }

    private IEnumerator DisplayNewTickleSpot()
    {
        _tickleSpots[_tickleSpotToActivate].Appear();
        _tickleSpotToActivate = (_tickleSpotToActivate + 1) % _tickleSpots.Length;
        
        yield return new WaitForSeconds(intervalBetweenTickleSpots);
    }

    private void OnMinigameEnded(bool success)
    {
        ChangeMood(success ? +1 : -1);
    }

    private void ChangeMood(int influence)
    {
        _moodLevel += influence;
        
        if (_moodLevel < Mood.Grumpy) _moodLevel = Mood.Grumpy;
        else if (_moodLevel > Mood.Cute) _moodLevel = Mood.Cute;
    }

    public void EndGame()
    {
        StopCoroutine(tickleSpotDisplayRoutine);
    }

    private void OnDestroy()
    {
        foreach (Tickle t in _tickleSpots)
        {
            t.TicklingMinigameEnded -= OnMinigameEnded;
        }
    }
}
