using System.Collections.Generic;
using UnityEngine;

public class Matrix 
{
    
    private readonly int _height;
    private readonly int _length;
    private List<List<int>> _matrixRepresentation;
    private readonly List<List<int>> _rowClues;
    private readonly List<List<int>> _columnClues;

    private readonly List<Line> _rows;
    private readonly List<Line> _columns;

    private bool _changeFlag;
    

    public Matrix(List<List<int>> pMatrixRepresentation, List<List<int>> pRowClues, List<List<int>> pColumnClues, int pHeight, int pLength)
    {

        _changeFlag = false;
        
        _matrixRepresentation = pMatrixRepresentation;
        _rowClues = pRowClues;
        _columnClues = pColumnClues;
        
        _rows = new List<Line>();
        _columns = new List<Line>();

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
        Debug.Log(listToString(_rows));
        Debug.Log("ROWS");
        Debug.Log("-----------------------------------------------");
        Debug.Log("COLUMNS");
        Debug.Log(listToString(_columns));
        Debug.Log("COLUMNS");
        
        
    }
    
    
    private void CreateRows()
    {
        for (int index = 0; index < _length; index++)
        {
            _rows.Add(CreateRow(index));
        }
        
    }

    private Line CreateRow(int index)
    {
        return new Line(_rowClues[index], _height, index);
    }

    private void CreateColumns()
    {
        for (int index = 0; index < _height; index++)
        {
            _columns.Add(CreateColumn(index));
        }

    }

    private Line CreateColumn(int index)
    {
        return new Line(_columnClues[index], _length, index);
    }

    private void SolveMatrix()
    {
        int counter = 0;
        do
        {
            ResetThisChangeFlag();
            Debug.Log("Rows" + counter);
            GoThroughRows();
            Debug.Log(listToString(_rows));
            Debug.Log("Columns" + counter);
            GoThroughColumns();
            Debug.Log(listToString(_rows));
            ResetLinesChangeFlags(_rows);
            ResetLinesChangeFlags(_columns);
            counter++;
        } while (_changeFlag);
        
         
     
    }
    
    private void GoThroughRows()
    {
        foreach (var row in _rows)
        {
            row.AnalyzeLine();
            if (row.WasChanged()) _changeFlag = true;
            UpdateColumns(row);
        }
    }

    private void GoThroughColumns()
    {
        foreach (var column in _columns)
        {
            column.AnalyzeLine();
            if (column.WasChanged()) _changeFlag = true;
            UpdateRows(column);
        }
    }
    

    private void UpdateColumns(Line row)
    {
        foreach (var column in _columns)
        {
            column.Refresh(row);
        }
    }
    private void UpdateRows(Line column)
    {
        foreach (var row in _rows)
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
