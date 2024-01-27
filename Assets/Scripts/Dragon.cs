using UnityEngine;

public class Dragon : MonoBehaviour
{
    enum Mood {
        Grumpy = 0,
        Resting = 3,
        Happy = 4,
        Cute = 5
    }
    private Mood _moodLevel;

    void Awake()
    {
        _moodLevel = Mood.Grumpy;
    }

    public void MakeHappy()
    {
        // TODO: play happi sound
        ChangeMood(+1);
    }

    public void MakeSad()
    {
        // TODO: play angery sound
        ChangeMood(-1);
    }

    private void ChangeMood(int influence)
    {
        _moodLevel += influence;
        
        if (_moodLevel < Mood.Grumpy) _moodLevel = Mood.Grumpy;
        else if (_moodLevel > Mood.Cute) _moodLevel = Mood.Cute;
    }
}
