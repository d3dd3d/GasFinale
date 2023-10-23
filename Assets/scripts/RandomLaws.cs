using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class RandomLaws
{
    public static double param1;
    public static double param2;
    public static double inChance;
    public static Zakon zk = new Ravn();
    public static bool isDeterm = false;
    public static int timeBtw;
    public static DateTime timeGlobal;
    public static DateTime timeGlobalOrigin;
    public interface Zakon
    {
        public float GenTime();
    }
    public class Ravn: Zakon
    {
        public float GenTime()
        {   
            if (!isDeterm){
                System.Random rn = new System.Random();
                return (float)(param1 + rn.Next((int)param1, (int)param2) * (param2 - param1) / param2);
            }
            else{
                return (float)timeBtw;
            }
        }
    }
    public class Exp: Zakon
    {
        static double[]  stairWidth, stairHeight;
        const double x1 = 7.69711747013104972;
        const double A = 3.9496598225815571993e-3;
        public Exp()
        {
            setupExpTables();
        }
        void setupExpTables()
        {
            stairWidth = new double[257];
            stairHeight = new double[256];
            stairHeight[0] = Math.Pow(Math.E,-x1);
            stairWidth[0] = A / stairHeight[0];
            stairWidth[256] = 0;
            for (int i = 1; i <= 255; ++i)
            {
                stairWidth[i] = -Math.Log(stairHeight[i - 1]);
                stairHeight[i] = stairHeight[i - 1] + A / stairWidth[i];
            }
            
        }

        double ExpZiggurat()
        {
            int iter = 0;
            do
            {
                System.Random rn = new System.Random();
                int stairId = rn.Next(0,255);
                double x = rn.NextDouble() * stairWidth[stairId]; // get horizontal coordinate
                if (x < stairWidth[stairId + 1]) /// if we are under the upper stair - accept
                    return x;
                if (stairId == 0) // if we catch the tail
                    return x1 + ExpZiggurat();
                if ((rn.NextDouble() * (stairHeight[stairId]- stairHeight[stairId-1])+ stairHeight[stairId - 1]) < Math.Pow(Math.E, -x)) // if we are under the curve - accept
                    return x;
                // rejection - go back
            } while (++iter <= 1e9); // one billion should be enough to be sure there is a bug
            return 0;
        }

        public float GenTime()
        {
            return (float)(ExpZiggurat() / param1);
        }
    }
    public class Norm: Zakon
    {
        public Norm()
        {
            setupNormalTables();
        }
        static double[] stairWidth, stairHeight;
        const double x1 = 3.6541528853610088;
        const double A = 4.92867323399e-3; /// area under rectangle

        void setupNormalTables()
        {
            stairWidth = new double[257];
            stairHeight = new double[256];
            // coordinates of the implicit rectangle in base layer
            stairHeight[0] = Math.Pow(Math.E, - .5 * x1 * x1);
            stairWidth[0] = A / stairHeight[0];
            // implicit value for the top layer
            stairWidth[256] = 0;
            for (int i = 1; i <= 255; ++i)
            {
                // such x_i that f(x_i) = y_{i-1}
                stairWidth[i] = Math.Sqrt(-2 * Math.Log(stairHeight[i - 1]));
                stairHeight[i] = stairHeight[i - 1] + A / stairWidth[i];
            }
        }

        double NormalZiggurat()
        {
            int iter = 0;
            do
            {
                System.Random rn = new System.Random();
                int sign = rn.Next(-1, 2);
                while (sign == 0)
                    sign = rn.Next(-1, 2);
                double B = rn.NextDouble() * sign;
                int stairId = rn.Next(0, 255);
                
                
                double x = rn.NextDouble() * stairWidth[stairId]; // get horizontal coordinate
                if (x < stairWidth[stairId + 1])
                    return (B > 0) ? x : -x;
                if (stairId == 0) // handle the base layer
                {
                    double z = -1;
                    double y;
                    if (z > 0) // we don't have to generate another exponential variable as we already have one
                    {
                        x = Math.Exp(x1);
                        z -= 0.5 * x * x;
                    }
                    if (z <= 0) // if previous generation wasn't successful
                    {
                        do
                        {
                            x = Math.Exp(1);
                            y = Math.Exp(x1);
                            z = y - 0.5 * x * x; // we storage this value as after acceptance it becomes exponentially distributed
                        } while (z <= 0);
                    }
                    x += x1;
                    return (B > 0) ? x : -x;
                }
                // handle the wedges of other stairs
                if ((rn.NextDouble() * (stairHeight[stairId] - stairHeight[stairId - 1]) + stairHeight[stairId - 1]) < Math.Exp(-.5 * x * x))
                    return (B > 0) ? x : -x;
            } 
            while (++iter <= 1e9); /// one billion should be enough
            return 0; /// fail due to some error
        }

        public float GenTime()
        {
            return (float)(param1 + NormalZiggurat() * param2);
        }
    }
    // class  Program
    // {
    //     static void Main(string[] args)
    //     {
    //         int[] mas = new int[20];
    //         double koef = 1;
    //         double time = 0;
    //         Params par = new Params();
    //         par.param1 = 5;
    //         par.param2 = 1;
    //         Zakon zk;
    //         zk = new Norm();
    //         Thread thr = new Thread(new ThreadStart(GenMobile));
    //         thr.Start();
    //         Thread.Sleep(10000);
    //         koef = 1;
    //         void GenMobile()
    //         {
    //             while (true)
    //             {
    //                 time = zk.GenTime(par) / koef;
    //                 if (time<0)
    //                     time = zk.GenTime(par) / koef;
    //                 if (time-(int)time>=0.4)
    //                     mas[(int)time+1]++;
    //                 else
    //                     mas[(int)time]++;
    //                 Console.WriteLine(time);
    //                 //Thread.Sleep((int)time*1000);
    //             }
    //         }
    //     }

    // }
}
