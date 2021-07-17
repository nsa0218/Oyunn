using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    public Button previousButton;
    [SerializeField]
    public Button nextButton;
    [SerializeField]
    public GameObject findOpponentPanel=null;
    [SerializeField]
    public GameObject charSelectionPanel =null;
    private Button charSelect;

    public int currentChar = 0;

    public void Awake()
    {
        SelectChar(0);
    }

    private void SelectChar(int _index)
    {
        previousButton.interactable = (_index != 0);
        nextButton.interactable = (_index != transform.childCount - 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeChar(int _change)
    {
        currentChar += _change;
        SelectChar(currentChar);
    }
    public void Select()
    {
        Launcher2.instance.selectedCharacter = currentChar;
        charSelectionPanel.SetActive(false);
        findOpponentPanel.SetActive(true);
        

    }
}


