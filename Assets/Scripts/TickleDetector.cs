using System;
using UnityEngine;

public class TickleDetector : MonoBehaviour
{
    private Tickle _currentTickle;
    public Action TicklingMinigameEnded;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _currentTickle = other.GetComponent<Tickle>();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _currentTickle = null;
    }

    public bool CanTickle() => _currentTickle != null;
    public void EngageTickle()
    {
        _currentTickle.TicklingMinigameEnded += OnTicklingMinigameEnded;
        _currentTickle.Engage();
    }

    private void OnTicklingMinigameEnded(bool success)
    {
        TicklingMinigameEnded?.Invoke(); // PlayerInput listens to this
        _currentTickle.TicklingMinigameEnded -= OnTicklingMinigameEnded;
    }
}
