using System;
using GameLogic;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private TextAsset prologue, epilogueVictory, epilogueDefeat, credits;
    [SerializeField] private TMP_Text text;

    private Action onClickAction;

    private static Story storyToDisplay;

    public enum Story
    {
        Prologue,
        Victory,
        Defeat,
        Credits
    }

    public static void ShowStory(Story story)
    {
        SceneDirector.LoadScene(SceneDirector.Scene.Story);
        storyToDisplay = story;
    }

    private void DisplayStory(Story story)
    {
        switch (story)
        {
            case Story.Prologue:
                DisplayStory(prologue, () => { SceneDirector.LoadScene(SceneDirector.Scene.Main); });
                break;
            case Story.Victory:
                DisplayStory(epilogueVictory, () => { ShowStory(Story.Credits); });
                break;
            case Story.Defeat:
                DisplayStory(epilogueDefeat, () => { SceneDirector.LoadScene(SceneDirector.Scene.Menu); });
                break;
            case Story.Credits:
                DisplayStory(credits, () => { SceneDirector.LoadScene(SceneDirector.Scene.Menu); });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(story), story, null);
        }
    }

    private void DisplayStory(TextAsset textAsset, Action then)
    {
        ShowTextFromAsset(textAsset);
        SetOnClickAction(then);
    }

    private void ShowTextFromAsset(TextAsset textAsset) => ShowText(textAsset.text);

    private void SetOnClickAction(Action action) => onClickAction = action;

    public void OnClick() => onClickAction?.Invoke();
    private void ShowText(string text) => this.text.text = text;

    private void Start()
    {
        DisplayStory(storyToDisplay);
    }
}