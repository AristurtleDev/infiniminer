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

public class ConfigurationFileWriter : IDisposable
{
    private readonly FileStream _stream;
    private readonly StreamWriter _writer;
    private bool _isDisposed;

    public ConfigurationFileWriter(string path)
    {
        _stream = File.Create(path);
        _writer = new StreamWriter(_stream);
    }

    ~ConfigurationFileWriter() => Dispose(false);

    public void Write(string key, object value)
    {
        ThrowIfDisposed();
        _writer.WriteLine($"{key} = {value}");
    }

    public void WriteComment(string message)
    {
        ThrowIfDisposed();
        _writer.WriteLine($"# {message}");
    }

    protected void ThrowIfDisposed()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException($"{nameof(ConfigurationFileWriter)} instance was disposed of previously");
        }
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
        _writer.Dispose();

        _isDisposed = true;
    }
}
