using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private CharacterSelectionView characterSelectionView;
    private List<CharacterSelectionView> characterList = new List<CharacterSelectionView>();
    
    public void OnClickStartButton()
    {
        StartCoroutine(CoRoutineAtIntro());
    }

    private IEnumerator CoRoutineAtIntro()
    {
        yield return StartCoroutine(CoSelectCharacter());
        yield return StartCoroutine(CoMoveCharacterToCurrentStage());

        MoveToMapScene();
    }

    private IEnumerator CoSelectCharacter()
    {
        ShowCharacters();
        while (GameDataManager.Instance.SelectedCharacter)
        {
            yield return null;
        }
    }

    private void ShowCharacters()
    {
        var scriptable = StartingCharacter.Load();
        
        characterSelectionView.gameObject.SetActive(true);
        var parent = characterSelectionView.transform.parent;
        const int ITEM_COUNT = 3;

        characterList.Add(characterSelectionView);
        for (int i = 0; i < ITEM_COUNT-1; ++i)
        {
            var instantiated = Instantiate(characterSelectionView, parent);
            characterList.Add(instantiated);
        }

        for (int i = 0; i < ITEM_COUNT; ++i)
        {
            var data = scriptable.characterList[i];
            
            var characterHandle = characterList[i];
            characterHandle.Set(data.characterJob, data.skillTypeList);
        }
    }

    private IEnumerator CoMoveCharacterToCurrentStage()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void MoveToMapScene()
    {
        SceneManager.LoadScene("Map");
    }
}
