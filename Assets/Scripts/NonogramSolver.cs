using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NonogramSolver : MonoBehaviour {
    private int _height;
    private int _length;

    private readonly List<List<int>> _rowClues = new List<List<int>>();
    private readonly List<List<int>> _columnClues = new List<List<int>>();
    private bool _skip;
    private int[,] LogicalMatrix { get; set; }

    public Visuals.Board board;

    public Matrix matrix;

    // Start is called before the first frame update
    void Start() {
        
        
        InitializeNonogram();
        InitializeLogicalMatrix();
        
        //Draw the Board
        board.LogicalMatrix = LogicalMatrix;
        board.Draw(_height, _length);
        
        //Adjust Camera
        AdjustCameraSize();
        
        //Execute Bounds
        matrix = new Matrix(_rowClues, _columnClues, _height, _length, board, board.LogicalMatrix);
        matrix.SolveMatrix();
        
        Debug.Log("-----------------------------------------------");
        Debug.Log("Rows");
        Debug.Log(matrix.listToString(matrix.Rows));
        
        //Start Backtracking Thread
        var backtracking = new Backtracking(matrix, LogicalMatrix);
        var thr = new Thread(backtracking.StartBacktracking);
        thr.Start();
        thr.IsBackground = true;
    }

    private void TestLine() {
        // var testClues = new List<int> {3, 4};
        // var line = new Line(testClues, 10, 0, board, LogicalMatrix) {GreaterClueValue = 4};
        var testTuples = new List<(int, int)> {(0,0), (0,1), (0,3), (1,1), (1,3)};
        testTuples.Sort();
        var res = "";
        foreach (var valueTuple in testTuples) {
            res += valueTuple + ",";
        }
        Debug.Log(res);
    }

    private void Update() {
        
        if (MainMenu.showAnimation)
        {
            board.UpdateCells(_height, _length);
        }
       
    }

    private void InitializeLogicalMatrix() {
        LogicalMatrix = new int[_height, _length];
    }

    private void AdjustCameraSize() {
        if (Camera.main != null) {
            var main = Camera.main;
            main.orthographicSize = _height*5 / 2.0f + 10;
            main.transform.position = new Vector3( _length*5/2.0f, -_height*5/2.0f, -10);
        }
    }
    

    private void InitializeNonogram() {
        var lines = System.IO.File.ReadAllLines(MainMenu.puzzlePath);
        var rows = true;
        foreach (var line in lines) {
            if (!_skip) {
                _skip = true;
                var words = line.Split(',');
                _height = int.Parse(words[0].Trim());
                _length = int.Parse(words[1].Trim());
            }
            else {
                if (!line.Contains("FILAS")) {
                    if (line.Contains("COLUMNAS")) {
                        rows = false;
                    }
                    else {  
                        var words = line.Split(',');

                        var listToAdd = new List<int>();
                        
                        foreach (var number in words)
                        {
                            listToAdd.Add(int.Parse(number.Trim()));
                        }
                        
                        if (rows) {
                            _rowClues.Add(listToAdd);
                        }
                        else {
                            _columnClues.Add(listToAdd);
                        }
                    }
                }
            }
        }
    }
}
