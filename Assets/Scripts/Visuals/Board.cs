using UnityEngine;

namespace Visuals {
    public class Board : MonoBehaviour {

        public GameObject cellPrefab;

        public Cells[,] BoardCells { get; set; }

        public int[,] LogicalMatrix { get; set; }


        public void Draw(int height, int width) {
            BoardCells = new Cells[height, height];

            var printHelper = "";

            for (var i = 0; i < height; i++) {
                for (var j = 0; j < width; j++) {

                    var newCell = Instantiate(cellPrefab, transform);

                    var newTransform = newCell.GetComponent<Transform>();
                    newTransform.position = new Vector2(j * 5, i * -5);

                    BoardCells[i, j] = newCell.GetComponent<Cells>();
                    printHelper += "X";
                }

                printHelper += "\n";
            }

            Debug.Log(printHelper);
        }

        public void UpdateCells(int height, int width) {
            for (var i = 0; i < height; i++) {
                for (var j = 0; j < width; j++) {
                    if (BoardCells[i, j].currentId != LogicalMatrix[i, j]) {
                        BoardCells[i, j].SetImage(LogicalMatrix[i,j]);    
                    }
                }
            }
        }

        public void PrintMatrix(int[,] matrix) {
            var result = "";
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    result += matrix[i, j];
                }

                result += "\n";
            }
            Debug.Log(result);
        }
    }
}