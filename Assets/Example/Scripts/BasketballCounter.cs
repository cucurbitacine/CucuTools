using UnityEngine;

namespace Example.Scripts
{
    public class BasketballCounter : MonoBehaviour
    {
        public int totalScore;
        public TextMesh TextMesh;
        
        public void AddScore(int score)
        {
            totalScore += score;
            TextMesh.text = totalScore.ToString();
        }
    }
}
