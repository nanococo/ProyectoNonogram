using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NonogramSolver : MonoBehaviour {
    private int Height { get; set;  }
    private int Length { get; set;  }

    private readonly List<List<int>> _rowClues = new List<List<int>>();
    private readonly List<List<int>> _columnClues = new List<List<int>>();
    private bool _skip;
    private int[,] LogicalMatrix { get; set; }

    public Visuals.Board board;

    public Matrix matrix;
    private Thread _thr;

    public GameObject timeTextObject;
    public bool isFinished;
    private Backtracking _backtracking;

    // Start is called before the first frame update
    void Start() {

        InitializeNonogram();
        InitializeLogicalMatrix();
        
        //Draw the Board
        board.LogicalMatrix = LogicalMatrix;
        board.Draw(Height, Length);
        
        //Adjust Camera
        AdjustCameraSize();
        
        //Execute Bounds
        matrix = new Matrix(_rowClues, _columnClues, Height, Length, board, board.LogicalMatrix);
        matrix.SolveMatrix();
        
        Debug.Log("-----------------------------------------------");
        Debug.Log("Rows");
        Debug.Log(matrix.listToString(matrix.Rows));
        
        //Start Backtracking Thread
        _backtracking = new Backtracking(matrix, LogicalMatrix);
        _thr = new Thread(_backtracking.StartBacktracking);
        _thr.Start();
        _thr.IsBackground = true;
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
            board.UpdateCells(Height, Length);
        }
        isFinished = _backtracking.IsFinishedResult;
    }

    private void OnDestroy() {
        if (_thr.IsAlive) {
            _thr.Abort();
        }
    }

    private void InitializeLogicalMatrix() {
        LogicalMatrix = new int[Height, Length];
    }

    private void AdjustCameraSize() {
        if (Camera.main != null) {
            var main = Camera.main;
            main.orthographicSize = Height*5 / 2.0f + 10;
            main.transform.position = new Vector3( Length*5/2.0f, -Height*5/2.0f, -10);
        }
    }
    

    private void InitializeNonogram() {
        var lines = System.IO.File.ReadAllLines(MainMenu.puzzlePath);
        var rows = true;
        foreach (var line in lines) {
            if (!_skip) {
                _skip = true;
                var words = line.Split(',');
                Height = int.Parse(words[0].Trim());
                Length = int.Parse(words[1].Trim());
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
