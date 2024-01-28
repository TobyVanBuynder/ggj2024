using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DragonPart : MonoBehaviour
{
    private float _timeToShake;
    private float _minWait = 1f;
    private float _maxWait = 3f;
    
    private float _cooldown;
    private Vector3 _originalPosition;
    private float _displace = .1f;
    private bool _isShaking;
    private bool _canShake = true; // Linked to whether the minigame is on or off
    private bool _isCountingDown;
    private DragonShakeSounds _shakeSounds;
    private AudioSource _shakeSource;
    

    private TickleUI _tickleUI;
    private Coroutine _shakeCoroutine;

    public UnityAction ShakeOff;

    private void Awake()
    {
        _originalPosition = transform.position;
        _tickleUI = FindObjectOfType<TickleUI>();
        _tickleUI.UIOpened += MinigameStarted;
        _tickleUI.UIClosed += MinigameStopped;
        _shakeSounds = FindObjectOfType<DragonShakeSounds>();
        _shakeSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        _shakeSource.playOnAwake = false;
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
        if (_canShake && _isCountingDown)
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
        _shakeSource.clip = _shakeSounds.GetRandomShakeSound();
        _shakeSource.pitch = Random.Range(95, 105) / 100f;
        _shakeSource.Play();
        _shakeCoroutine = StartCoroutine(StopShaking());
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

    public void Tickle()
    {
        //Debug.Log($"Engaged {gameObject.name}");
        Initialise();
    }

    public void Release()
    {
        //Debug.Log($"Released {gameObject.name}");
        _isCountingDown = false;
        if(_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
    }

    private void MinigameStarted()
    {
        _canShake = false;
        if(_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
    }

    private void MinigameStopped()
    {
        _canShake = true;
    }
}
