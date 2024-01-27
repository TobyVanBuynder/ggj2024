using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DragonPart : MonoBehaviour
{
    private float _timeToShake;
    private float _minWait = 6f;
    private float _maxWait = 20f;
    
    private float _cooldown;
    private Vector3 _originalPosition;
    private float _displace = .1f;
    private bool _isShaking;
    private bool _isCountingDown;

    public UnityAction ShakeOff;

    private void Start()
    {
        _originalPosition = transform.position;
        Initialise();
    }

    private void Initialise()
    {
        transform.position = _originalPosition;
        _cooldown = Random.Range(_minWait, _maxWait);
        _isShaking = false;
        _isCountingDown = true;
    }

    private void Update()
    {
        if (_isCountingDown)
        {
            _cooldown -= Time.deltaTime;

            if (_cooldown <= 0f) StartShaking();
        }
        
        if(_isShaking) UpdateShake();
    }

    private void StartShaking()
    {
        _isCountingDown = false;
        _isShaking = true;
        StartCoroutine(StopShaking());
    }

    private IEnumerator StopShaking()
    {
        yield return new WaitForSeconds(1f);
        
        _isShaking = false;
        transform.position += Vector3.up * .5f;
        ShakeOff?.Invoke();
        
        yield return new WaitForSeconds(.5f);

        Initialise();
    }

    private void UpdateShake()
    {
        Vector2 displace = Random.insideUnitCircle * _displace;
        transform.position = _originalPosition + new Vector3(displace.x, displace.y, 0f);
    }
}
