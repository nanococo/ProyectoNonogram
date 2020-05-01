using System.Collections.Generic;
using UnityEngine;

public class Matrix 
{
    
    private readonly int _height;
    private readonly int _length;

    public List<List<int>> RowClues { get;}
    public List<List<int>> ColumnClues { get;}
    public List<Line> Rows { get; }
    public List<Line> Columns { get; }

    private bool _changeFlag;
    
    public Matrix(List<List<int>> pRowClues, List<List<int>> pColumnClues, int pHeight, int pLength)
    {

        _changeFlag = false;

        RowClues = pRowClues;
        ColumnClues = pColumnClues;
        
        Rows = new List<Line>();
        Columns = new List<Line>();

        _height = pHeight;
        _length = pLength;

        CreateRows();
        CreateColumns();
        
        Test();

        
    }

    private void Test()
    {
        SolveMatrix();

        Debug.Log("ROWS");
        Debug.Log(listToString(Rows));
        Debug.Log("ROWS");
        // Debug.Log("-----------------------------------------------");
        // Debug.Log("COLUMNS");
        // Debug.Log(listToString(Columns));
        // Debug.Log("COLUMNS");
        
        //Le comenté esa vara no se asuste perro
        
        //var backtracking = new Backtracking(this);
        //backtracking.ExecuteBacktracking();
    }
    
    
    private void CreateRows()
    {
        for (int index = 0; index < _length; index++)
        {
            Rows.Add(CreateRow(index));
        }
        
    }

    private Line CreateRow(int index)
    {
        return new Line(RowClues[index], _height, index);
    }

    private void CreateColumns()
    {
        for (int index = 0; index < _height; index++)
        {
            Columns.Add(CreateColumn(index));
        }

    }

    private Line CreateColumn(int index)
    {
        return new Line(ColumnClues[index], _length, index);
    }

    private void SolveMatrix()
    {
        int counter = 0;
        do
        {
            ResetThisChangeFlag();
            Debug.Log("Rows" + counter);
            GoThroughRows();
            Debug.Log(listToString(Rows));
            Debug.Log("Columns" + counter);
            GoThroughColumns();
            Debug.Log(listToString(Rows));
            ResetLinesChangeFlags(Rows);
            ResetLinesChangeFlags(Columns);
            counter++;
        } while (_changeFlag);
        
         
     
    }
    
    private void GoThroughRows()
    {
        foreach (var row in Rows)
        {
            row.AnalyzeLine();
            if (row.WasChanged()) _changeFlag = true;
            UpdateColumns(row);
        }
    }

    private void GoThroughColumns()
    {
        foreach (var column in Columns)
        {
            column.AnalyzeLine();
            if (column.WasChanged()) _changeFlag = true;
            UpdateRows(column);
        }
    }
    

    private void UpdateColumns(Line row)
    {
        foreach (var column in Columns)
        {
            column.Refresh(row);
        }
    }
    private void UpdateRows(Line column)
    {
        foreach (var row in Rows)
        {
            row.Refresh(column);
        }
    }

    private void ResetLinesChangeFlags(List<Line> pLineList)
    {
        foreach (var line in pLineList)
        {
            line.SetChangeFlag(false);
        }
    }

    private void ResetThisChangeFlag()
    {
        this._changeFlag = false;
    }
    

    
    public string PrintMatrix<T>(List<List<T>> matrix) {
        var test = "";
        foreach (var list in matrix)
        {
            test += listToString(list) + "\n";
        }
        
        return test;
    }
    
    public string listToString<T>(List<T> list)
    {
        var result = "";
        foreach (var element in list)
        {
            result += element + "," + "\n";
        }

        result += "";
        return result;
    }
    
    
}
