using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class Matrix 
{
    
    private readonly int _height;
    private readonly int _length;

    public List<List<int>> RowClues { get;}
    public List<List<int>> ColumnClues { get;}
    public List<Line> Rows { get; }
    public List<Line> Columns { get; }

    private bool _changeFlag;
    private int[,] _LogicalMatrix;

    private Visuals.Board _board;
    
    
    
    public Matrix(List<List<int>> pRowClues, List<List<int>> pColumnClues, int pHeight, int pLength, Visuals.Board board, int[,] logicalMatrix) {
        
        _board = board;

        _changeFlag = false;

        RowClues = pRowClues;
        ColumnClues = pColumnClues;
        
        Rows = new List<Line>();
        Columns = new List<Line>();

        _height = pHeight;
        _length = pLength;

        _LogicalMatrix = logicalMatrix;
        
        CreateRows();
        CreateColumns();
    }

    private void CreateRows()
    {
        for (var rowNumber = 0; rowNumber < _height; rowNumber++)
        {
            Rows.Add(CreateRow(rowNumber));
        }
        
    }

    private Line CreateRow(int rowNumber)
    {
        return new Line(RowClues[rowNumber], _length, rowNumber, _board, _LogicalMatrix);
    }

    private void CreateColumns() {
        for (var columnNumber = 0; columnNumber < _length; columnNumber++)
        {
            Columns.Add(CreateColumn(columnNumber));
        }

    }

    private Line CreateColumn(int index)
    {
        return new Line(ColumnClues[index], _height, index, _board, _LogicalMatrix);
    }

    public void SolveMatrix()
    {
        var counter = 0;
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
            row.AnalyzeLine(true);
            if (row.WasChanged()) _changeFlag = true;
            UpdateColumns(row);
        }
    }

    private void GoThroughColumns()
    {
        foreach (var column in Columns)
        {
            column.AnalyzeLine(false);
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
        foreach (var row in Rows) {
            row.Refresh(column);
        }
    }

    private static void ResetLinesChangeFlags(List<Line> pLineList)
    {
        foreach (var line in pLineList)
        {
            line.SetChangeFlag(false);
        }
    }

    private void ResetThisChangeFlag() {
        _changeFlag = false;
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


    public void RemoveDemonMarks() {
        foreach (var row in Rows) {
            foreach (var rowCell in row.Cells) {
                rowCell.DemonMark = false;
            }
        }
        foreach (var column in Columns) {
            foreach (var rowCell in column.Cells) {
                rowCell.DemonMark = false;
            }
        }
    }

    public void ExecuteBackTracking() {
        
    }
}
