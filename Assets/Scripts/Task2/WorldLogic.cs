using System.Collections.Generic;

public class WorldLogic
{
    private Pool _playersPool;
    private Minimap _minimap;
    public int PlayersDrawedOnMinimap { get; private set; }

    public WorldLogic(Pool playersPool, Minimap minimap)
    {
        _playersPool = playersPool;
        _minimap = minimap;
    }

    public void ClearMiniMap()
    {
        _playersPool.ReturnObjects();
        PlayersDrawedOnMinimap = 0;
        _minimap.Clear();
    }

    public void WorldUpdate(Player[] players)
    {
        var lst = FilterPlayers(players);
        foreach (var player in lst)
        {
            DrawPlayerOnMinimap(player);
        }
    }

    private IEnumerable<Player> FilterPlayers(Player[] players)
    {
        //   return NaiveApproachWithLinkedList(players);
        return HeapApproach(players);
    }


    /// <summary>
    /// Forehead decision with linked list for storing first players with maximum hp.  
    /// </summary>
    /// <param name="players">Array of unsorted players</param>
    /// <returns>IEnumerable with players sorted  by maximum hp</returns>
    private IEnumerable<Player> NaiveApproachWithLinkedList(Player[] players)
    {
        void InsertValue(Player newPlayer, LinkedList<Player> l, bool isInsert)
        {
            var prev = l.First;
            while (prev != null)
            {
                if (newPlayer.CompareTo(prev.Value) > 0)
                {
                    l.AddBefore(prev, newPlayer);
                    if (isInsert)
                        l.RemoveLast();
                    return;
                }

                prev = prev.Next;
            }

            l.AddLast(newPlayer);
        }

        var lst = new LinkedList<Player>();
        const int maxCount = Task2.PlayersWithMaximumHpCount;
        Player minPlayer = null;
        for (var i = 0; i < players.Length; i++)
        {
            if (lst.Count >= maxCount && minPlayer?.CompareTo(players[i]) >= 0) continue;

            InsertValue(players[i], lst, lst.Count >= maxCount);
            minPlayer = lst.Last.Value;
        }

        return lst;
    }

    private Heap<Player> _heap = new Heap<Player>(Task2.PlayersWithMaximumHpCount);

    /// <summary>
    /// Advanced decision with storing ordered players in heap. Overhead of place and reordering heap is more less then place data in linked list
    /// </summary>
    /// <param name="players">Array of unsorted players</param>
    /// <returns>IEnumerable with players sorted  by maximum hp</returns>
    private IEnumerable<Player> HeapApproach(Player[] players)
    {
        const int heapSize = Task2.PlayersWithMaximumHpCount;
        _heap.Clear();
        var heap = _heap;
        heap.AddAll(players, 0, heapSize);

        for (var i = heapSize; i < players.Length; i++)
        {
            if (heap.Min.CompareTo(players[i]) >= 0) continue;
            heap.ReplaceMin(players[i]);
        }

        return heap.Elements;
    }


    private void DrawPlayerOnMinimap(Player player)
    {
        var obj = _playersPool.Pop();
        if (obj != null)
            _minimap.Draw(obj, player.Position);


        PlayersDrawedOnMinimap++;
    }
}