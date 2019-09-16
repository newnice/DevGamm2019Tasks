using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Task2 : MonoBehaviour
{
    public const int PLAYERS_COUNT = 100000;
    public const int PLAYERS_WITH_MAXIMUM_HP_COUNT = 100;

    private WorldLogic worldLogic;
    private Player[] players;
    private Stopwatch perfomanceTimer;

    private void Awake()
    {
        perfomanceTimer = new Stopwatch();
        worldLogic = new WorldLogic();
        players = new Player[PLAYERS_COUNT];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player();
            players[i].ChangeHP();
            players[i].ChangePosition();
        }
    }

    private void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].ChangeHP();
            players[i].ChangePosition();
        }

        worldLogic.ClearMiniMap();

        perfomanceTimer.Start();
        worldLogic.WorldUpdate(players);
        perfomanceTimer.Stop();

        if (worldLogic.PlayersDrawedOnMinimap != PLAYERS_WITH_MAXIMUM_HP_COUNT)
        {
            throw new Exception("Minimap shows wrong amount players");
        }

        Debug.Log($"World update time:{perfomanceTimer.ElapsedMilliseconds}");
        perfomanceTimer.Reset();
    }

    public class Player
    {
        public Vector2 Position => position;
        private Vector2 position;

        public int Hp => hp;
        private int hp;

        public void ChangeHP()
        {
            hp += Random.Range(-100, 100);
        }

        public void ChangePosition()
        {
            position.x += Random.Range(-100f, 100f);
            position.y += Random.Range(-100f, 100f);
        }
    }

    public class WorldLogic
    {
        public int PlayersDrawedOnMinimap => playersDrawedOnMinimap;
        private int playersDrawedOnMinimap;

        public void ClearMiniMap()
        {
            playersDrawedOnMinimap = 0;
        }

        public void WorldUpdate(Player[] players)
        {
            //Write code here
        }

        private void DrawPlayerOnMinimap(Player player)
        {
            playersDrawedOnMinimap++;
        }
    }
}