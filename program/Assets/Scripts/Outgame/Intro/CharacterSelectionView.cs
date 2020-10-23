using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterSelectionView:MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtJob;

    [SerializeField] private List<CharacterSkillView> skillViewList;

    public void Set(EnumCharacterJob job,List<EnumSkillType> skillList)
    {
        txtJob.text = job.ToString();
        
        Debug.Assert(skillViewList.Count == skillList.Count, "[Skill]Invalid List Count");

        for (int i = 0; i<skillList.Count; ++i)
        {
            skillViewList[i].Set(skillList[i]);
        }
    }
}
