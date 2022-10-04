using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void OnStartPlay()
    {
        StoryManager.ShowStory(StoryManager.Story.Prologue);
    }

    public void OnShowCredits()
    {
        StoryManager.ShowStory(StoryManager.Story.Credits);
    }
}