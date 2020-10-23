using UnityEngine;
using TMPro;

public class CharacterSkillView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtSkillName;

    public void Set(EnumSkillType skill)
    {
        txtSkillName.text = skill.ToString();
    }
}
