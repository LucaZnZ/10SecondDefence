using System.Collections.Generic;
using GameLogic;
using OneLiners;
using UnityEngine;
using UnityEngine.Events;
using ZnZUtil;

public class GameEndController : MonoBehaviour
{
    [SerializeField] private List<OneLinerEvent> victory, defeat;
    [SerializeField] private float SceneTransitionDelay = 1f;

    public UnityEvent OnVictory, OnDefeat;

    public void CheckForGameOver(OneLinerEvent oneLinerEvent, string name)
    {
        if (victory.Contains(oneLinerEvent))
            OnVictory?.Invoke();
        else if (defeat.Contains(oneLinerEvent))
            OnDefeat?.Invoke();
    }

    public void CheckForOutOfTime(int time)
    {
        if (time <= 0)
            OnDefeat?.Invoke();
    }

    public void PlayAudio(string track) => AudioManager.PlayAudioTrack(track);

    private void ShowStoryAfterDelay(StoryManager.Story story)
    {
        var cd = new Countdown(SceneTransitionDelay, 1f);
        StartCoroutine(cd.PerformAfterRun(
            () => StoryManager.ShowStory(story)));
    }

    private void Start()
    {
        OnVictory.AddListener(() => ShowStoryAfterDelay(StoryManager.Story.Victory));
        OnDefeat.AddListener(() => ShowStoryAfterDelay(StoryManager.Story.Defeat));
        OnVictory.AddListener(() => PlayAudio("Victory"));
        OnDefeat.AddListener(() => PlayAudio("Defeat"));
    }
}