using System.Text;

namespace Infiniminer.Server;

public class InfiniminerServerConsole
{
    private const ConsoleColor COLOR_NORMAL = ConsoleColor.White;
    private const ConsoleColor COLOR_WARNING = ConsoleColor.Yellow;
    private const ConsoleColor COLOR_ERROR = ConsoleColor.Red;

    private readonly List<string> _history = new List<string>();
    private readonly StringBuilder _currentLine = new StringBuilder();

    public InfiniminerServerConsole()
    {

    }

    public void Clear(bool clearHistory = false)
    {
        Console.Clear();
        _currentLine.Clear();

        if (clearHistory)
        {
            _history.Clear();
        }
    }

    public void Write(string? value)
    {
        SetTextColor(COLOR_NORMAL);
        Console.Write(value);
        _currentLine.Append(value);
    }

    public void WriteWarning(string? value)
    {
        SetTextColor(COLOR_WARNING);
        Console.Write(value);
        _currentLine.Append(value);
    }

    public void WriteError(string? value)
    {
        SetTextColor(COLOR_ERROR);
        Console.Write(value);
        _currentLine.Append(value);
    }

    public void WriteLine(string? value)
    {
        SetTextColor(COLOR_NORMAL);
        Console.WriteLine(value);
        _currentLine.Append(value);
        _history.Add(_currentLine.ToString());
        _currentLine.Clear();
    }

    public void WriteWarningLine(string? value)
    {
        SetTextColor(COLOR_WARNING);
        Console.WriteLine(value);
        _currentLine.Append(value);
        _history.Add(_currentLine.ToString());
        _currentLine.Clear();
    }

    public void WriteErrorLine(string? value)
    {
        SetTextColor(COLOR_ERROR);
        Console.WriteLine(value);
        _currentLine.Append(value);
        _history.Add(_currentLine.ToString());
        _currentLine.Clear();
    }

    public void LogToFile(string path)
    {
        try
        {
            _history.Add(_currentLine.ToString());
            File.AppendAllLines(path, _history);
            Clear(true);
        }
        catch (Exception ex)
        {
            WriteErrorLine(ex.Message);
        }
    }

    private void SetTextColor(ConsoleColor color) => Console.ForegroundColor = color;
}
