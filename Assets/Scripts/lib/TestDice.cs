using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDice : MonoBehaviour
{
    private IGameConsole gameConsole;
    private CRand rand;

    private void Start()
    {
        gameConsole = AllServices.Container.Get<IGameConsole>();
        rand = new CRand(1);
        rand.Randomize();
        gameConsole.AddCommand(new CGameConsoleCommand("dice", Dice,"dice number - roll dice with number faces"));
        gameConsole.AddCommand(new CGameConsoleCommand("testdice", Test,"testdice number count - roll dice number and check statistic"));
    }
    private void OnDestroy()
    {
        gameConsole.RemoveCommand("dice");
        gameConsole.RemoveCommand("testdice");
    }
    private void Dice(string _arg)
    {
        uint d, v;

        if (CUtil.IsDigit(_arg[0]))
        {
            d = (uint)CUtil.StringToInt(_arg);
            v = rand.Dice(d);
            gameConsole.ShowMessage($"d{d}={v}");
        }
    }

    private void Test(string _arg)
    {
        uint d, v;
        int n, i;
        if (CUtil.IsDigit(_arg[0]))
        {
            d = (uint)CUtil.StringToInt(_arg);
            i = 0;
            while (CUtil.IsDigit(_arg[i])) i++;
            while (_arg[i] == ' ') i++;
            if (CUtil.IsDigit(_arg[i]))
            {
                string str = _arg.Substring(i);
                int[] test = new int[d];
                n = CUtil.StringToInt(str);
                for (i = 0; i < d; i++) test[i] = 0;
                for (i = 0; i < n; i++)
                {
                    v = rand.Dice(d);
                    test[v - 1]++;
                }
                gameConsole.ShowMessage($"dice {d} test");
                for (i = 0; i < d; i++)
                {
                    float k = 100.0f * (((float)test[i]) / ((float)n));
                    gameConsole.ShowMessage($"value {i + 1}={test[i]} ({k}%)");
                }
            }
        }
    }
}
