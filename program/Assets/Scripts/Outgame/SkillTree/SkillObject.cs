using UnityEngine;

public class SkillObject : MonoBehaviour
{
    private static SkillObject loadedResource = null;
    public static SkillObject Load(RectTransform parent)
    {
        if (loadedResource == null)
        {
            loadedResource = Resources.Load<SkillObject>("SkillObject");
        }

        return Instantiate(loadedResource, parent);
    }
}
