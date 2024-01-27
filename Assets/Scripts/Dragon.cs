using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public float spotLifetime = 20f;
    public float intervalBetweenTickleSpots = 10f;
    
    private Tickle[] _tickleSpots;
    private Coroutine tickleSpotDisplayRoutine;
    private int _tickleSpotToActivate;
    
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
    }

    private void ChangeMood(int influenceChange)
    {
        _moodLevel += influenceChange;
        
        switch (_moodLevel)
        {
            case < Mood.Grumpy:
                _moodLevel = Mood.Grumpy;
                break;
            case > Mood.Cute:
                _moodLevel = Mood.Cute;
                break;
        }
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
