using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueMover : MonoBehaviour
{
    public RectTransform arrowTransform;

    public Image arrowImage;

    public AudioSource music;

    public Text deathCountText;
    public Text gameTimeText;

    public Text[] endTexts;
    
    private bool run = false;

    private static ContinueMover instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) Play();
        
        if (!run) return;

        arrowImage.enabled = true;

        var p = arrowTransform.localPosition;

        p.x = Mathf.MoveTowards(p.x, -800, 5000 * Time.unscaledDeltaTime);
        
        arrowTransform.localPosition = p;

        if (p.x == -800)
        {
            Time.timeScale = 0;

            enabled = false;
            
            End();
        }
    }
    
    private static string ToReadableString(TimeSpan span)
    {
        string formatted = string.Format("{0}{1}{2}{3}",
            span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

        if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

        if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

        return formatted;
    }

    private void End()
    {
        deathCountText.text = "Количество смертей: " + PlayerController.TotalDeathCount;

        var elapsed = DateTime.UtcNow - PlayerController.StartGameTime;
        
        if (elapsed.TotalHours >= 1)
        {
            gameTimeText.text = string.Format("Время игры: {0:D2}:{1:D2}:{2:D2}", (int) elapsed.TotalHours, elapsed.Minutes, elapsed.Seconds);
        }
        else
        {
            gameTimeText.text = string.Format("Время игры: {0:D2}:{1:D2}", elapsed.Minutes, elapsed.Seconds);
        }

        deathCountText.enabled = true;
        gameTimeText.enabled = true;

        foreach (var endText in endTexts)
        {
            endText.enabled = true;
        }
    }

    public static void Play()
    {
        instance.run = true;
        
        instance.music.Play();

        instance.music.time = 39.5f;
    }
}