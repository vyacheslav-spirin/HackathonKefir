using UnityEngine;
using UnityEngine.UI;

public class SkillIconView : MonoBehaviour
{
    public Image icon;

    public int charId;

    private PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        icon.enabled = _player.GetCharId() == charId;
    }
}