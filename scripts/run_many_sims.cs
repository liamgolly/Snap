using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace SnapTraining.scripts;

/// <summary>
/// Runs many 1v1 simulations.
/// This could be abstracted for more complex n player simulations.
/// </summary>
public partial class run_many_sims : Button
{
    private const int SimsPerChance = 5000;
    private const double ChanceInterval = 0.001;
    private const int PlayerCount = 2;
    public override void _Pressed()
    {
        base._Pressed();

        var sims = new Simulator[(int) (1 / ChanceInterval)][];
        var wins = new int[(int) (1 / ChanceInterval)][];
        
        var count = (1 / ChanceInterval * SimsPerChance);
        
        for (var chance = 0; chance < sims.Length; chance++)
        {
            sims[chance] = new Simulator[SimsPerChance];
            wins[chance] = new int[SimsPerChance];
            
            for (var sim = 0; sim < sims[chance].Length; sim++)
            {
                sims[chance][sim] = new Simulator(PlayerCount);
                sims[chance][sim].SetWinChances(new [] {ChanceInterval * chance, 1 - ChanceInterval * chance});
                wins[chance][sim] = -1;
            }
        }

        int c = 0;
        int d = 10;
        var start = DateTime.Now;
        while (true)
        {
            Parallel.ForEach(sims, (chanceArr, _, index) =>
            {
                for (var sim = 0; sim < chanceArr.Length; sim++)
                {
                    if (wins[index][sim] != -1) continue;
                    wins[index][sim] = chanceArr[sim].Step();
                }
            });

            c = (c + 1) % d;
            var remaining = wins.SelectMany(row => row).Count(x => x == -1);
            if (c == 0)
            {
                GD.Print($"Progress: {100 - remaining / count * 100:F}%");
                d += 10;
            }
            if (remaining == 0)
                break;
        }
        GD.Print($"Done!");
        GD.Print($"Time taken: {DateTime.Now - start}");
        GD.Print($"Total Simulations: {count}");
        GD.Print($"Time Per Simulation: {(DateTime.Now - start).TotalMilliseconds / count} milliseconds");
        GD.Print("");
        var ret = new StringBuilder();

        for (var s = 0; s < wins.Length; s++)
        {
            ret.Append($"{s * ChanceInterval}");
            for (var i = 0; i < wins[s].Length; i++)
            {
                ret.Append($",{wins[s][i]}");
            }

            ret.Append('\n');
        }

        DisplayServer.ClipboardSet(ret.ToString());
    }
}