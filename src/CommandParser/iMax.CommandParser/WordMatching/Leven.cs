/*
Matching two strings
Author: Thanh Ngoc Dao - Thanh.dao@gmx.net
Copyright (c) 2005 by Thanh Ngoc Dao.
*/
using System;

namespace iMax.CommandParser
{
    /// <summary>
    /// Summary description for Leven.
    /// </summary>
    internal class Leven : ISimilarity
    {
        public double GetSimilarity(string value, string otherValue)
        {

            float dis = this.ComputeDistance(value, otherValue);
            float maxLen = value.Length;
            if (maxLen < (float)otherValue.Length)
            {
                maxLen = otherValue.Length;
            }
            float minLen = value.Length;
            if (minLen > (float)otherValue.Length)
            {
                minLen = otherValue.Length;
            }

            if (maxLen == 0.0F)
            {
                return 1.0F;
            }
            else
            {
                return maxLen - dis;
            }
        }

        private int Min3(int a, int b, int c)
        {
            return Math.Min(Math.Min(a, b), c);
        }

        private int ComputeDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] distance = new int[n + 1, m + 1]; // matrix
            int cost = 0;

            if (n == 0) return m;
            if (m == 0) return n;
            //init1
            for (int i = 0; i <= n; i++)
            {
                distance[i, 0] = i;
            }
            for (int j = 0; j <= m; j++)
            {
                distance[0, j] = j;
            }

            //find min distance
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
                    distance[i, j] = this.Min3(distance[i - 1, j] + 1, distance[i, j - 1] + 1, distance[i - 1, j - 1] + cost);
                }
            }

            return distance[n, m];
        }
    }
}
