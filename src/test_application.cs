using System;
using BBDiese;


public class TestApplication
{
    static void Main(string[] args)
    {
        string input = Console.In.ReadToEnd();
        string output = BBCode.ToHtml(input);
        Console.WriteLine(output);
    }
}