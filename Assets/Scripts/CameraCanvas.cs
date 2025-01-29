using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraCanvas : MonoBehaviour
{
    [SerializeField]
    Button[] buttons;
    [SerializeField]
    TextMeshProUGUI quest;
    [SerializeField]
    TextMeshProUGUI answer;
    string rightLetters = "";
    [SerializeField]
    Main main;
    List<Cell> cellList;
    Word curword;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void NextWord()
    {
        main.ToNext(curword.id);
    }
    public void PreviousWord() 
    {
        main.ToPrev(curword.id);
    }


    public void FillInputPanel(Word word, List<Cell> cells)
    {
        curword = word;
        cellList = cells;
        answer.text = "";
        rightLetters = "";
        quest.text = word.Text;
        if(word.alphabet.Length != 16)
        {
            print("pupupu");
        }
        foreach (var cell in cells) {
            answer.text = cell.status == CellStatus.Complited ? answer.text + cell.text : answer.text + "_";
            rightLetters += cell.solution;
        }
        for(int i = 0; i < 16; i++)
        {
            GameObject button = buttons[i].gameObject;
            if (!button.activeSelf) button.SetActive(true);
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = word.alphabet[i].ToString();
            if (rightLetters.Contains(word.alphabet[i]))
            {
                if ((from c in cellList where c.status == CellStatus.Complited select c.text).Contains(word.alphabet[i].ToString())) {
                    if (rightLetters.Count(_ => _ == word.alphabet[i]) == 1)
                    button.SetActive(false);
                }
                string output = word.alphabet[i].ToString().Clone() as string;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => { RightButton(output); ButtonOff(button); });
            }
            else
            {
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(WrongButton);
            }
        }

    }

    public void WrongButton()
    {

    }

    public void ButtonOff(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }


    public void RightButton(string text)
    {
        if(rightLetters.Count((_) => _.ToString() == text) > 1)
        {
            char[] chars = answer.text.ToCharArray();
            List<int> ind = rightLetters.AllIndexesOf(text).ToList();
            foreach(int i in ind)
            {
                chars[i] = text.ToCharArray()[0];
                cellList[i].text = text;
                cellList[i].status = CellStatus.Complited;
                main.CellChanged(cellList[i]);
            }
            answer.text = chars.ArrayToString();
        }
        else
        {
            char[] chars = answer.text.ToCharArray();
            chars[rightLetters.IndexOf(text)] = text.ToCharArray()[0];
            cellList[rightLetters.IndexOf(text)].text = text;
            cellList[rightLetters.IndexOf(text)].status = CellStatus.Complited;
            main.CellChanged(cellList[rightLetters.IndexOf(text)]);
            answer.text = chars.ArrayToString();
        }

        if (!answer.text.Contains("_")) {
            main.WordComlited(quest.text);
            NextWord();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
