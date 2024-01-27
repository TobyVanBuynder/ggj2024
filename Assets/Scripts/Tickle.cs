using UnityEngine;

public class Tickle : MonoBehaviour
{
    private Dragon _dragon;
    private TickleUI _tickleUI;
    public int Difficulty{get; private set; }
    [SerializeField] private float _cooldownTime;
    [SerializeField] private float _currentCooldown;
    [SerializeField] private float _tickInterval = 5.0f;

    void Awake()
    {
        _dragon = FindObjectOfType<Dragon>();
        _tickleUI = FindObjectOfType<TickleUI>();
        _cooldownTime = Random.Range(30, 60);
        Difficulty = 1;
    }

    public void Engage()
    {
        _tickleUI.Open(this);
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
                Difficulty++;
            }
            if (_cooldownTime <= 0) End(false);
        }
    }
}
