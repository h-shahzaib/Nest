# Nest — Fluent Structured Text Generation for Code & Scripts

**Nest** is a C# library designed to simplify the generation of structured text — particularly code and scripting content — through a fluent, builder-style API. It is especially well-suited for automation scenarios where manual string formatting, indentation, and structural consistency become repetitive or error-prone.

Nest aims to be the go-to solution for code and script generation, with a focus on being **minimal**, **zero-dependency**, and **highly composable**.

## ✨ Key Features

- 🔧 Fluent API for structured text generation
- 🧱 Core focus on indentation and formatting
- 📦 Modular packages for each target language
- ⚡ Zero dependencies, lightweight and minimal
- 💬 Ideal for code generators, scaffolding tools, and scripting utilities

## 📦 Packages

**Nest** aspires to be a suite of language-specific packages, each containing built-in formatting conventions and behaviors tailored to the respective language.

**Available:**

`Nest.CSharp` – Generate C# code effortlessly

- Nuget: https://www.nuget.org/packages/Nest.CSharp
- GitHub: https://github.com/h-shahzaib/Nest/tree/master/Nest.CSharp

**Coming Soon:**

- `Nest.Yaml` – YAML generation with proper indentation and spacing

## 🧠 Philosophy

At the core of **Nest** lies a simple idea: _structured text should be composable and language-aware, without forcing developers to manage indentation, line breaks, or braces manually._

Nest automatically handles:

- Indentation depth and alignment
- Block formatting (open/close structure)
- Optional line breaks and spacing
- Language-specific structure semantics

## 🧱 Core Constructs

The library revolves around two primary constructs that serve as the foundation for composing structured text:

### `.B(...)` — **Block**

Represents a block of text, typically used for grouping lines with indentation and optional padding (line breaks) before and after.

The behavior & usage of `.B()` is language-specific. In C#, for instance, it automatically wraps the content with braces and applies indentation according to C# conventions.

### `.L(...)` — **Line(s)**

Adds one or more lines of text. Accepts a `params string[]` input.

- **No parameters**: Inserts a blank line
- **One line**: Adds a single line
- **Multiple lines**: Adds all lines, with appropriate line breaks before and/or after

## 💡 Use Case Examples

With just `.L()` and `.B()`, Nest makes it easy to generate structured output in any code or scripting language. You focus on the logic; Nest takes care of the layout.

Stay tuned as support for more languages like YAML, JSON, Bash, and more is on the way.

## 🚧 Under Active Development

Nest is currently in active development. While stable for basic use, the API and behavior may evolve as support for more languages and features is added.

Feedback, contributions & suggestions are welcome!
