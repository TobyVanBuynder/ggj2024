using UnityEngine;

public class TickleDetector : MonoBehaviour
{
    private Tickle _currentTickle;
    
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
        // TODO: hook into tickling over event from Tickle.cs
        _currentTickle.Engage();
    }
}
