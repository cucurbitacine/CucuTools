using CucuTools;
using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife
{
    public class CellDivider : CucuBehaviour
    {
        public int score;
        public int scoreMax;
        
        [Space]
        public CellController cell;
        public CellProvider provider;
        
        public void Eat(CellController food, int amount)
        {
            amount = Mathf.Min(amount, food.health);
            food.health -= amount;
            
            score += amount;
            
            Divide();
        }
        
        public void Divide()
        {
            if (score >= scoreMax && provider && provider.TrySpawn(cell.cellType, out var copy))
            {
                score -= scoreMax;
                
                copy.transform.SetParent(cell.transform.parent);
                copy.transform.SetPositionAndRotation(cell.position, cell.rotation);
                
                copy.fixedArea = cell.fixedArea;
                copy.stateMachine.Restart();
            }
        }
    }
}