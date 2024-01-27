using UnityEngine;

public class DragonExpressions : MonoBehaviour
{
    public GameObject[] eyes;
    public GameObject[] pupils;

    private int _currentMoodLevel;

    public void SwitchTo(Dragon.Mood newMood)
    {
        // int numericMood;
        // switch (newMood)
        // {
        //     case Dragon.Mood.Grumpy:
        //         numericMood = 0;
        // }
        //
        // eyes[_currentMoodLevel].SetActive(false);
        // pupils[_currentMoodLevel].SetActive(false);
        //
        // eyes[_currentMoodLevel].SetActive(true);
        // pupils[_currentMoodLevel].SetActive(true);
        //
        // _currentMoodLevel = newMood;
    }
}
