using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{
    public Image image;
    
    private PlayerController _player;
    
    void Awake()
    {
        _player = Object.FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (_player.CooldownTotal < 0.01f)
        {
            image.fillAmount = 0;
        }
        else
        {
            var progress = _player.CooldownLeft / _player.CooldownTotal;

            image.fillAmount = progress;
        }
    }
}