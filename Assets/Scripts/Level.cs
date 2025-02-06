using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int width { get; set; }
    public int height { get; set; }
    public Cell[,] cells { get; set; }
    public List<Word> words { get; set; }
    public string name;
 
    public Level()
    {
        width = 0;
        height = 0;
        words = new List<Word>();
    }
    public Level(int width, int height)
    {
        this.width = width;
        this.height = height;
        words = new List<Word>();
        cells = new Cell[width, height];
    }
    public Level(int width, int height, Cell[,] cells)
    {
        this.width = width;
        this.height = height;
        this.cells = cells;
    }
}
