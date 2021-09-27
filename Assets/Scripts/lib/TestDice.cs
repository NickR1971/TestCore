using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDice : ConsoleService
{
    private CRand rand;

    private void Start()
    {
        Init();
        rand = new CRand(1);
        rand.Randomize();
        AddCommand("dice", Dice,"dice number - roll dice with number faces");
        AddCommand("testdice", Test,"testdice number count - check statistic for dice");
        AddCommand("range", Range,"range min max - get random value in diapasone");
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

    private void Range(string _arg)
    {
        int m1, m2, i;

        if (CUtil.IsDigit(_arg[0]))
        {
            m1 = CUtil.StringToInt(_arg);
            i = 0;
            while (CUtil.IsDigit(_arg[i])) i++;
            while (_arg[i] == ' ') i++;
            if (CUtil.IsDigit(_arg[i]))
            {
                m2 = CUtil.StringToInt(_arg.Substring(i));
                i = rand.Range(m1, m2);
                gameConsole.ShowMessage($"Range({m1},{m2})={i}");
            }
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
