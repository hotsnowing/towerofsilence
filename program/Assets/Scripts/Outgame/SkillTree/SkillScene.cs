using System.Collections.Generic;
using UnityEngine;

public class SkillScene : MonoBehaviour
{
    private Dictionary<int, SkillObject> loadedSkillmap = new Dictionary<int, SkillObject>();
    private OptSkillTreeTable _skillTreeTable;

    private void Start()
    {
        SetupSkillTree();
    }

    private void SetupSkillTree()
    {
        
    }
}
