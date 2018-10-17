using System;

namespace iMax.CommandParser
{
    /// <summary>
    /// Summary description for IEditDistance.
    /// </summary>
    internal interface ISimilarity
    {
        float GetSimilarity(string string1, string string2);
    }
}
