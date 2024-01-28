using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonShakeSounds : MonoBehaviour
{
    public AudioClip[] _shakeSounds;

    public AudioClip GetRandomShakeSound()
    {
        return _shakeSounds[Random.Range(0, _shakeSounds.Length)];
    }
}
