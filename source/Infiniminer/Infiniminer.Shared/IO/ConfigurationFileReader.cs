/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2009 Zach Barth
Copyright (c) 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

namespace Infiniminer.IO;

public class ConfigurationFileReader : IDisposable
{
    private readonly Stream _stream;
    private readonly StreamReader _reader;
    private bool _isDisposed;

    public ConfigurationFileReader(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Unable to find configuration file at the path given: '{path}'");
        }

        _stream = File.OpenRead(path);
        _reader = new StreamReader(_stream);
    }

    public ConfigurationFileReader(Stream stream)
    {
        _stream = stream;
        _reader = new StreamReader(stream);
    }

    ~ConfigurationFileReader() => Dispose(false);


    public ConfigurationItem? ReadLine()
    {
        if(_isDisposed)
        {
            throw new ObjectDisposedException($"{nameof(ConfigurationFileReader)} instance was previously disposed");
        }

        string? line = _reader?.ReadLine();
        if (line is null) { return null; }

        string key = string.Empty;
        string value = string.Empty;

        if (line.Length > 0 && line[0] != '#')
        {
            string[] kv = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

            if (kv.Length == 2)
            {
                key = kv[0].Trim();
                value = kv[1].Trim();
            }
        }

        return new ConfigurationItem(key, value);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool isDisposing)
    {
        if (_isDisposed) { return; }

        _stream.Dispose();
        _reader.Dispose();

        _isDisposed = true;
    }
}