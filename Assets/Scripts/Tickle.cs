using UnityEngine;

public class Tickle : MonoBehaviour
{
    private Dragon _dragon;
    private TickleUI _tickleUI;
    private int _sequenceDifficulty = 0;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private float _currentCooldown;
    [SerializeField] private float _tickInterval = 5.0f;

    void Awake()
    {
        _dragon = FindObjectOfType<Dragon>();
        _tickleUI = FindObjectOfType<TickleUI>();
        _cooldownTime = Random.Range(30, 60);
    }

    public void Engage()
    {
        _tickleUI.Open(_sequenceDifficulty);
    }

    public void End(bool isSuccess)
    {
        if (isSuccess) _dragon.MakeHappy();
        else _dragon.MakeSad();
        Destroy(gameObject);
    }

    void Update()
    {
        if (!_tickleUI.IsOpen) {
            _cooldownTime -= Time.deltaTime;
            if ((_currentCooldown -= Time.deltaTime) < 0)
            {
                _currentCooldown += _tickInterval;
                _sequenceDifficulty++;
            }
            if (_cooldownTime <= 0) End(false);
        }
    }
}
