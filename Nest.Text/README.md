# **Nest.Text**

**Nest.Text** is a zero-dependency, fluent text generation library that helps you build structured content — from **C#**, **Python**, and **YAML** to **HTML**, **XML**, and more. It lets you describe what to generate — Nest takes care of how it's formatted.

---

## 📦 Installation

```bash
dotnet add package Nest.Text
```

---

## 🚀 What Is It?

Nest.Text provides a builder-style API to generate code, markup, or any structured text using:

* `.L(...)` – Write one or more lines
* `.B(...)` – Begin a block with nested content

You **chain** these calls to preserve layout — Nest adds or avoids line breaks based on chaining and structure awareness.

---

## 🔁 Line Break Behavior

| Usage                          | Result                                                     |
| ------------------------------ | ---------------------------------------------------------- |
| `_.L("one");`<br>`_.L("two");` | ❌ No Line break between
| `_.L("one").L("two");`         | ❌ No line break between                                    |
|                                | ✅ Line Break before or after chain
| `_.L("line1", "line2");`       | ✅ Line break before or after block, **not between lines** |
| `_.L(new[] { "x", "y" });`     | ✅ Same as above — before & after only                      |

> 🔥 **No line breaks between chained calls**, but Nest adds breaks around separate statements and multi-line entries automatically.

---

## ⚙️ Options

```csharp
_.Options.BlockStyle = BlockStyle.Braces; // or IndentOnly
_.Options.IndentSize = 4;                 // spaces per indent level
```

---

## 🔄 Smart Quote Replacement

You can use backticks (\`) instead of escaped quotes in your strings:

```csharp
_.L("Console.WriteLine(`Hello World!`);"); 
// Outputs: Console.WriteLine("Hello World!");
```

To customize or disable:

```csharp
_.Options.RegisterCharReplacement('`', '"');
_.Options.RemoveCharReplacement('`');
```

---

## 🧪 C# Example (Braces Block Style with Chaining)

```csharp
var _ = new TextBuilder();
_.Options.BlockStyle = BlockStyle.Braces;
_.Options.IndentSize = 4;

_.L("using System.Text;");

_.L("namespace MyProgram").B(_ =>
{
    _.L("public class MyProgram").B(_ =>
    {
        _.L("public static void Main(string[] args)").B(_ =>
        {
            _.L("if (count > 6)").B(_ =>
            {
                _.L("Console.WriteLine(`Hello World!`);");
                _.L("Console.WriteLine(`Hello Again!`);");
            })
            .L("else").B(_ =>
            {
                _.L("Console.WriteLine(`Hello World!`);");
            });
        });
    });
});

Console.WriteLine(_.ToString());
```

---

## 🐍 Python Example (IndentOnly)

```csharp
var _ = new TextBuilder();
_.Options.BlockStyle = BlockStyle.IndentOnly;

_.L("def greet():").B(_ =>
{
    _.L("print(`Hello World!`)");
    _.L("print(`Hello Again!`)");
});

Console.WriteLine(_.ToString());
```

---

## 🌐 HTML Example

```csharp
var _ = new TextBuilder();
_.Options.BlockStyle = BlockStyle.IndentOnly;
_.Options.IndentSize = 2;

_.L("<div>").B(_ =>
{
    _.L("<span>Hello World!</span>");
    _.L("<span>Hello Again!</span>");
}
).L("</div>");

Console.WriteLine(_.ToString());
```

---

## 📄 XML Example

```csharp
var _ = new TextBuilder();
_.Options.BlockStyle = BlockStyle.IndentOnly;

_.L("<config>").B(_ =>
{
    _.L("<entry key=`theme`>dark</entry>");
    _.L("<entry key=`lang`>en</entry>");
}
).L("</config>");

Console.WriteLine(_.ToString());
```

---

## 📘 YAML Example

```csharp
var _ = new TextBuilder();
_.Options.BlockStyle = BlockStyle.IndentOnly;

_.L("Library:").B(_ =>
{
    _.L("name: `Nest`");
    _.L("use: `Structured Text Generation`");

    _.L("features:").B(_ =>
    {
        _.L("- Automated Indentation");
        _.L("- Easy To Use");
        _.L("- Zero Dependency");
    });
});

Console.WriteLine(_.ToString());
```

---

## 📚 Summary

* Fluent and **chainable** API
* Smart formatting — line breaks where needed, not where not
* Custom indentation and block styles
* Backtick-friendly string writing
* Debuggable at every step
* No dependencies, works anywhere .NET runs

---

## 🔗 Links

* 📦 NuGet: [Nest.Text on NuGet](https://www.nuget.org/packages/Nest.Text/)
* 💻 GitHub: [github.com/h-shahzaib/Nest](https://github.com/h-shahzaib/Nest)

