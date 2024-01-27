using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DragonPart : MonoBehaviour
{
    private float _timeToShake = 1f;
    
    private float _cooldown;
    private Vector3 _originalPosition;
    private float _displace = .1f;
    private bool _isShaking;

    public UnityAction ShakeOff;

    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        _originalPosition = transform.position;
        _cooldown = _timeToShake;
        _isShaking = false;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        if (_cooldown <= 0f) StartShaking();
        
        if(_isShaking) UpdateShake();
    }

    private void StartShaking()
    {
        _isShaking = true;
        StartCoroutine(StopShaking());
    }

    private IEnumerator StopShaking()
    {
        yield return new WaitForSeconds(1f);

        ShakeOff?.Invoke();
        
        Initialise();
    }

    private void UpdateShake()
    {
        Vector2 displace = Random.insideUnitCircle * _displace;
        transform.position = _originalPosition + new Vector3(displace.x, displace.y, 0f);
    }
}
