using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TickleSpot : MonoBehaviour
{
    private TickleUI _tickleUI;
    public int Difficulty{get; private set; }
    [SerializeField] private float _cooldownTime;
    [SerializeField] private float _currentCooldown;
    [SerializeField] private float _tickInterval = 10.0f;

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
        _cooldownTime = Random.Range(30, 60);
        _currentCooldown = _tickInterval;
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
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!_tickleUI.IsOpen)
        {
            _cooldownTime -= Time.deltaTime;
            _currentCooldown -= Time.deltaTime;
            if (_cooldownTime <= 0)
            {
                Disappeared?.Invoke();
                gameObject.SetActive(false);
            }
            else if (_currentCooldown <= 0)
            {
                _currentCooldown += _tickInterval;
                Difficulty++;
            }
        }
    }
}
