namespace iMax.CommandParser
{
    /// <summary>
    /// Summary description for IEditDistance.
    /// </summary>
    internal interface ISimilarity
    {
        double GetSimilarity(string value, string otherValue);
    }
}
