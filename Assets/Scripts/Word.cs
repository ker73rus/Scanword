using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Word
{
    public string Text = "Error";
    public int id = 0;
    public (int, int) position;
    public int startX = 0;
    public int startY = 0;
    public int endX = 0;
    public int endY = 0;
    public string alphabet = "ÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏĞÑÒÓÔÕÖ×ØÙÚÛÜİŞß";
    public string solution;
    public List<(int, int)> map;
    public Word() { 
        
    }

    public Word(int id, string text, (int, int) position)
    {
        this.id = id;
        this.Text = text;
        this.position = position;   
    }

    public Word(string text, int id, int startX, int startY, int endX, int endY)
    {
        Text = text;
        this.id = id;
        this.startX = startX;
        this.startY = startY;
        this.endX = endX;
        this.endY = endY;
        int deltaX = endX - startX;
        int deltaY = endY - startY;
        map = new List<(int, int)>();
        for (int i = 0; i < Math.Max(deltaX, deltaY); i++)
        {
            map.Add((deltaX == 0 ? startX : startX + i, deltaY == 0 ? startY : startY + i));
        }
    }
    public Word(string text, int id, int startX, int startY, int endX, int endY, (int,int) position)
    {
        Text = text;
        this.id = id;
        this.startX = startX;
        this.startY = startY;
        this.endX = endX;
        this.endY = endY;
        this.position = position;
    }

    public void AddSolution(string solution)
    {
        this.solution = solution ;
        char[] chars = solution.Distinct().ToArray();
        solution = chars.ArrayToString();
        List<string> letters = new();
        foreach (var item in solution)
        {
            alphabet = alphabet.Replace(item.ToString(), "");
            letters.Add(item.ToString());
        }
        while (alphabet.Length > 16 - solution.Length)
        {
            int i = UnityEngine.Random.Range(0, alphabet.Length);
            alphabet = alphabet.Replace(alphabet[i].ToString(), "");
            //íåïğàâèëüíî ñîçäà¸òñÿ
        }
        alphabet += solution;
        letters =  (from a in alphabet.ToList() select a.ToString()).ToList();
        letters.Sort();
        alphabet = "";
        foreach (var item in letters)
        {
            alphabet += item;
        }
    }

    public void AddCells(int startX, int startY, int endX, int endY)
    {
        this.startX = startX;
        this.endX = endX;
        this.startY = startY;
        this.endY = endY;
        int deltaX = endX - startX;
        int deltaY = endY - startY;
        map = new List<(int, int)>();
        for (int i = 0; i <= Math.Max(deltaX, deltaY); i++)
        {
            map.Add((deltaX == 0 ? startX : startX + i, deltaY == 0 ? startY : startY + i));
        }

    }
}
