using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Task2 : MonoBehaviour
{
    [SerializeField] private Pool playersPool = null;
    [SerializeField] private Minimap minimap = null;
    public const int PlayersCount = 100000;
    public const int PlayersWithMaximumHpCount = 100;

    private WorldLogic _worldLogic;
    private Player[] _players;
    private Stopwatch _performanceTimer;

    private void Awake()
    {
        _performanceTimer = new Stopwatch();
        _worldLogic = new WorldLogic(playersPool, minimap);
        _players = new Player[PlayersCount];
        for (var i = 0; i < _players.Length; i++)
        {
            _players[i] = new Player();
            _players[i].ChangeHp();
            _players[i].ChangePosition();
        }
    }

    private void Update()
    {
        for (var i = 0; i < _players.Length; i++)
        {
            _players[i].ChangeHp();
            _players[i].ChangePosition();
        }


        DrawWorld();
    }

    private void DrawWorld()
    {
        _worldLogic.ClearMiniMap();
        _performanceTimer.Start();
        _worldLogic.WorldUpdate(_players);
        _performanceTimer.Stop();

        if (_worldLogic.PlayersDrawedOnMinimap != PlayersWithMaximumHpCount)
        {
            throw new Exception("Minimap shows wrong amount players");
        }

        Debug.Log($"World update time:{_performanceTimer.ElapsedMilliseconds}");
        _performanceTimer.Reset();
    }
}