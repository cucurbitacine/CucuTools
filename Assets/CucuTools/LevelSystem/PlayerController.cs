using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        public const int PlayerDefault = 0;
        
        [SerializeField] private Dictionary<int, Transform> players = new Dictionary<int, Transform>();

        public event Action<int, Transform> PlayerChanged;
        public event Action<int, int> PlayerCountChanged;
        
        public int GetPlayersCount() => players.Count;
        
        public bool ContainsPlayer(int playerId)
        {
            return players.ContainsKey(playerId);
        }
        
        public Transform GetPlayer(int playerId = PlayerDefault)
        {
            return players[playerId];
        }
        
        public void ChangedPlayer(int playerId, Transform player)
        {
            var prevPlayersCount = GetPlayersCount();
            
            players[playerId] = player;
            PlayerChanged?.Invoke(playerId, player);

            var nowPlayersCount = GetPlayersCount();
            if (prevPlayersCount != nowPlayersCount)
            {
                PlayerCountChanged?.Invoke(prevPlayersCount, nowPlayersCount);
            }
        }
        
        public virtual IEnumerator EnablePlayer(ContextContainer levelContext)
        {
            if (!ContainsPlayer(PlayerDefault))
            {
                ChangedPlayer(PlayerDefault, transform);
            }
            
            yield break;
        }

        public virtual void DisablePlayer()
        {
        }
    }
}