﻿/* ----------------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Infiniminer
{
    /* Loads in a datafile consisting of key/value pairs, in the format of
     * "key = value", which can be read out through the Data dictionary.
     */

    public class DatafileLoader
    {
        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        public Dictionary<string, string> Data
        {
            get { return dataDict; }
        }

        public DatafileLoader(string filename)
        {
            try
            {
                FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);

                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] args = line.Split("=".ToCharArray());
                    if (args.Length == 2 && line[0] != '#')
                    {
                        Data[args[0].Trim()] = args[1].Trim();
                    }
                    line = sr.ReadLine();
                }

                sr.Close();
                file.Close();
            }
            catch (Exception e)
            {

            }
        }
    }
}