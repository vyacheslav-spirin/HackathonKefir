using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiedScreenUI : MonoBehaviour
{
    public CanvasGroup group;

    public Text respawnText;

    public AudioSource diedAudio;

    private bool started = false;
    
    void Update()
    {
        if (PlayerController.IsKilled && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            return;
        }
        
        if (!PlayerController.IsKilled || started) return;

        started = true;

        StartCoroutine(ScreenProgram());
    }

    IEnumerator ScreenProgram()
    {
        yield return WaitForRealSeconds(1);
        
        diedAudio.Play();

        while (group.alpha < 1)
        {
            var a = group.alpha;
            a += 0.5f * Time.unscaledDeltaTime;
            if (a > 1) a = 1;
            group.alpha = a;
            yield return new WaitForEndOfFrame();
        }

        respawnText.enabled = true;
    }
    
    public IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}