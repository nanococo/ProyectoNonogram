using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class Timer : MonoBehaviour {
        
        
        private TextMeshProUGUI timeText;
        public NonogramSolver solver;
        
        void Start() {
            timeText = GetComponent<TextMeshProUGUI>();
        }

        void Update() {
            if (!solver.isFinished) {
                timeText.text = ((Time.timeSinceLevelLoad)).ToString("0.00");    
            }
        }
    }
}