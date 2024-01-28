using System;
using UnityEngine;

public class DragonExpressions : MonoBehaviour
{
    public GameObject[] spittingFire;
    public GameObject[] grumpy;
    public GameObject[] resting;
    public GameObject[] happy;
    public GameObject[] happy2;
    public GameObject[] treasure;

    private Dragon.Mood _currentMood;

    public void SwitchFaceTo(Dragon.Mood newMood)
    {
        if (_currentMood == Dragon.Mood.SpittingFire)
            foreach (GameObject go in spittingFire) go.SetActive(false);
        else if (_currentMood == Dragon.Mood.Grumpy)
            foreach (GameObject go in grumpy) go.SetActive(false);
        else if (_currentMood == Dragon.Mood.Resting)
            foreach (GameObject go in resting) go.SetActive(false);
        else if (_currentMood == Dragon.Mood.Happy)
            foreach (GameObject go in happy) go.SetActive(false);
        else if (_currentMood == Dragon.Mood.Cute)
            foreach (GameObject go in happy2) go.SetActive(false);
        else if (_currentMood == Dragon.Mood.Treasure)
            foreach (GameObject go in treasure) go.SetActive(false);

        if (newMood == Dragon.Mood.SpittingFire)
            foreach (GameObject go in spittingFire) go.SetActive(true);
        else if (newMood == Dragon.Mood.Grumpy)
            foreach (GameObject go in grumpy) go.SetActive(true);
        else if (newMood == Dragon.Mood.Resting)
            foreach (GameObject go in resting) go.SetActive(true);
        else if (newMood == Dragon.Mood.Happy)
            foreach (GameObject go in happy) go.SetActive(true);
        else if (newMood == Dragon.Mood.Cute)
            foreach (GameObject go in happy2) go.SetActive(true);
        else if (newMood == Dragon.Mood.Treasure)
            foreach (GameObject go in treasure) go.SetActive(true);
        
        _currentMood = newMood;
    }
}
