using System;
using UnityEngine;

public class TickleSpotDetector : MonoBehaviour
{
    private TickleSpot _currentTickleSpot;
    public Action TicklingMinigameEnded;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _currentTickleSpot = other.GetComponent<TickleSpot>();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _currentTickleSpot = null;
    }

    public bool CanTickle() => _currentTickleSpot != null;
    public void EngageTickle()
    {
        _currentTickleSpot.TicklingMinigameEnded += OnTicklingMinigameEnded;
        _currentTickleSpot.Engage();
    }

    private void OnTicklingMinigameEnded(bool success)
    {
        TicklingMinigameEnded?.Invoke(); // PlayerInput listens to this
        _currentTickleSpot.TicklingMinigameEnded -= OnTicklingMinigameEnded;
    }
}
