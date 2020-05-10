using UnityEngine;

namespace Visuals {
    //Default Cell Image is the Empty Cell
    public class Cells : MonoBehaviour {
        public Sprite emptyCell;
        public Sprite markedCell;
        public Sprite discardedCell;

        private SpriteRenderer _spriteRenderer;
        
        public int currentId = 1;

        public void Start() {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }


        public void SetImage(int image) {
            switch (image) {
                case 1:
                    //Empty Cell
                    currentId = 1;
                    _spriteRenderer.sprite = emptyCell;
                    break;
                case 2:
                    //Marked Cell
                    currentId = 2;
                    _spriteRenderer.sprite = markedCell;
                    break;
                case 3:
                    //Discarded Cell
                    currentId = 3;
                    _spriteRenderer.sprite = discardedCell;
                    break;
                    
            }
        }
    }
}