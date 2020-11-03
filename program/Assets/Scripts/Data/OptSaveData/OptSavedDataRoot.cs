using System.Collections.Generic;

[System.Serializable]
public class OptSavedDataRoot
{
    public OptSavedUser userData;
    public List<OptSavedCharacter> characterList = new List<OptSavedCharacter>();
}
