using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tickle : MonoBehaviour
{
    private TickleUI _tickleUI;
    public int Difficulty{get; private set; }
    [SerializeField] private float _cooldownTime;
    [SerializeField] private float _currentCooldown;
    [SerializeField] private float _tickInterval = 5.0f;

    private float _lifetime;

    public Action<bool> TicklingMinigameEnded;
    public Action Disappeared;

    void Awake()
    {
        _tickleUI = FindObjectOfType<TickleUI>();
    }

    // Invoked by the Dragon
    public void Appear()
    {
        _lifetime = 20f;
        _cooldownTime = Random.Range(30, 60);
        Difficulty = 1;
        gameObject.SetActive(true);
    }

    public void Engage()
    {
        _tickleUI.Open(this);
    }

    public void End(bool isSuccess)
    {
        TicklingMinigameEnded?.Invoke(isSuccess); // Notifies the TickleDetector, and Dragon
        Destroy(gameObject);
    }

    void Update()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
        {
            // Close the spot
            Disappeared?.Invoke();
            gameObject.SetActive(false);
        }
        
        if (!_tickleUI.IsOpen)
        {
            _cooldownTime -= Time.deltaTime;
            if ((_currentCooldown -= Time.deltaTime) < 0)
            {
                _currentCooldown += _tickInterval;
                Difficulty++;
            }
            if (_cooldownTime <= 0) End(false);
        }
    }
}
