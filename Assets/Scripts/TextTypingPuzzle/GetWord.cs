using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetWord
{
    public static readonly string[] Words = new string[]
    {
        "algorithm",
        "array",
        "binary",
        "bug",
        "class",
        "code",
        "compiler",
        "constructor",
        "database",
        "debug",
        "dictionary",
        "exception",
        "function",
        "framework",
        "inheritance",
        "interface",
        "library",
        "loop",
        "method",
        "namespace",
        "object",
        "operator",
        "parameter",
        "polymorphism",
        "queue",
        "recursion",
        "stack",
        "string",
        "syntax",
        "variable",
        "borys",
        "unity",
        "script",
        "python",
        "CSharp",
        "solid",
        "singletone",
        "observer",
        "delegate",
        "event",
        "dependency",
        "error",
        "int",
        "enum",
        "static"
    };

public static string GetRandomWord()
    {
        int randomIndex = Random.Range(0, Words.Length);
        return Words[randomIndex];
    }
}
