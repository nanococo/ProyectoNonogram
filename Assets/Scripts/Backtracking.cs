using UnityEngine;

/// <summary>
/// The Backtracking class is the main handler for all
/// backtracking code solving the nonogram
/// </summary>
public class Backtracking {

    private readonly Matrix _matrix;

    public Backtracking(Matrix matrix) => _matrix = matrix;

    public bool ExecuteBacktracking() {
        for (var i = 0; i < _matrix.Rows.Count; i++) {
            if (!_matrix.Rows[i].IsComplete()) {
                for (var j = 0; j < _matrix.Rows[i].Cells.Count; j++) {
                    if (IsSafe(i, j)) {
                        _matrix.Rows[i].Cells[j].Mark = "1";
                        if (ExecuteBacktracking()) {
                            return true;
                        } else {
                            _matrix.Rows[i].Cells[j].Mark = "0";
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool IsSafe(int rowNumber, int cellNumber) {
        return true;
    }
}