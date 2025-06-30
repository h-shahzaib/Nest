# Nest.CSharp

**Nest.CSharp** is a fluent, lightweight, zero-dependency source code generator preconfigured for C#. It helps you generate structured C# source code through a clean, builder-style API — so you can focus on _what_ to generate, not _how_ to format it.

---

## 📦 Installation

Install via the .NET CLI:

```
dotnet add package Nest.CSharp
```

---

## 🚀 Overview

Built on **.NET Standard 2.0**, Nest.CSharp works across **all .NET versions** with **no external dependencies**.
It handles indentation, structure, formatting, and character escaping — all out of the box.

---

## 🧪 Example

Let's imagine we want to generate the following C# code:

```csharp
using System.Text;

namespace MyProgram
{
    public class MyProgram
    {
        public static void Main(string[] args)
        {
            if (count > 6)
            {
                Console.WriteLine("Hello World!");
                Console.WriteLine("Hello World!");
            }
            else
            {
                Console.WriteLine("Hello World!");
            }
        }
    }
}
```

Here's how we'll do it:

```csharp
using Nest.CSharp;

var _ = new CSharpBuilder();

_.L("using System.Text;");

_.B("namespace MyProgram", _ =>
{
    _.B("public class MyProgram", _ =>
    {
        _.B("public static void Main(string[] args)", _ =>
        {
            _.B("if (count > 6)", _ =>
            {
                _.L("Console.WriteLine(`Hello World!`);");
                _.L("Console.WriteLine(`Hello World!`);");
            })
            .B("else", _ =>
            {
                _.L("Console.WriteLine(`Hello World!`);");
            });
        });
    });
});

Console.WriteLine(_.ToString());
```

---

## ✨ Key Features

### ✅ Fluent API — Code That Writes Code

Write source generation logic that looks nearly identical to the output. No manual formatting, indentation, or brace management required.

### 🧠 Token-Aware Formatting

The builder system knows its context — it automatically places line breaks and indentation where needed according to widely accepted formatting rules in the C# community. Which makes your generated code clean and readable.

### 🔄 Character Replacement System

Write code with backticks instead of escaped double quotes:

```csharp
_.L("Console.WriteLine(`Hello World!`);"); // <-- Notice Backtick (`)
```

It is preconfigured to replace Backtick (\`) with Double Quotes ("). You can customize & remove character replacements as below:

```csharp
_.Options.RegisterCharReplacement('`', '"');
_.Options.RemoveCharReplacement('`');
```

---

## 🤔 Why Nest?

- ✅ Fluent, readable, and chainable API
- ✅ No need to manage indentation or formatting manually
- ✅ Output closely mirrors your generation code
- ✅ No dependencies
- ✅ Works with any .NET project

---

## 📚 Getting Started

1. Install the package
2. Use `CSharpBuilder` to define your structure
3. Call `.ToString()` to get the generated output

---

## 🔗 Links

- 📦 NuGet: [https://www.nuget.org/packages/Nest.CSharp/](https://www.nuget.org/packages/Nest.CSharp/)
- 💻 GitHub: [https://github.com/h-shahzaib/Nest](https://github.com/h-shahzaib/Nest)
