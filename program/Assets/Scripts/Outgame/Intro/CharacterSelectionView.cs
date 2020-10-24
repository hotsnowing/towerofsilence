using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterSelectionView:MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtJob;

    [SerializeField] private List<CharacterSkillView> skillViewList;
    
    public int Index { get; set; }

    public void Set(int itemIndex,CharacterData characterData ,List<SkillData> skillList)
    {
        Index = itemIndex;
        txtJob.text = characterData.job.ToString();
        
        Debug.Assert(skillViewList.Count == skillList.Count, "[Skill]Invalid List Count");

        for (int i = 0; i<skillList.Count; ++i)
        {
            skillViewList[i].Set(skillList[i].skillType);
        }
    }
}
