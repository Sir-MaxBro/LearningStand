using iMax.CommandParser;

namespace Stand.Domain.Abstract
{
    public interface ICompiler
    {
        ValidResult IsValid(string command);

        string Resource { get; set; }
    }
}
