using UnityEngine;
using UnityEngine.UI;

public class SkillText : MonoBehaviour
{
    public Text text;
    
    private PlayerController _player;
    
    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (_player.IsInvisible)
        {
            text.enabled = true;

            text.text = "Невидимость: " + _player.InvisibleTime.ToString("0.0");
        }
        else
        {
            text.enabled = false;
        }
    }
}