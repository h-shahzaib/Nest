using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest.Text
{
    internal class TextBuilder : IBuilder
    {
        private readonly List<Token> m_Tokens = [];

        public IOptions Options => InternalOptions;
        public IReadOnlyList<IToken> Tokens => m_Tokens.AsReadOnly();
        public bool IsRootBuilder { get; set; } = true;

        public Options InternalOptions
        {
            get
            {
                if (m_InternalOptions == null)
                    InternalOptions = Text.Options.Default;
                return m_InternalOptions!;
            }
            set => m_InternalOptions = new Options(value);
        }
        private Options? m_InternalOptions;

        public IBuilder L(params string[] lines)
        {
            Token token;

            if (lines.Length == 0)
                token = new LineToken(InternalOptions);
            else if ((lines.Length == 1 && lines.Any(i => i.Contains(Environment.NewLine))) || lines.Length > 1)
                token = new LinesToken(InternalOptions, lines);
            else
                token = new LineToken(InternalOptions, lines[0]);

            m_Tokens.Add(token);

            return this;
        }

        public IBuilder B(string header, Action<IBuilder> builder_act)
        {
            var builder = new TextBuilder();
            builder.InternalOptions = InternalOptions;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            m_Tokens.Add(new BlockToken(InternalOptions, header, builder));

            return this;
        }

        public IBuilder B(Action<IBuilder> builder_act)
        {
            var builder = new TextBuilder();
            builder.InternalOptions = InternalOptions;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            m_Tokens.Add(new BlockToken(InternalOptions, null, builder));

            return this;
        }

        public override string ToString()
        {
            var new_line = Environment.NewLine;

            var string_builder = new StringBuilder();

            for (int i = 0; i < m_Tokens.Count; i++)
            {
                var token = m_Tokens[i];
                var next_token = m_Tokens.ElementAtOrDefault(i + 1);

                var tabs = string.Empty;
                if (!IsRootBuilder && token.Options.NumberOfSpacesInOneTab > 0)
                    tabs = new string(' ', token.Options.NumberOfSpacesInOneTab);

                if (i != 0)
                    string_builder.AppendLine();
                string_builder.Append(tabs + token.ToString().Replace(new_line, $"{new_line}{tabs}"));

                var next_token_not_empty =
                    next_token is LineToken next_line_token
                        && !string.IsNullOrWhiteSpace(next_line_token.Line) ||
                    next_token is LinesToken next_lines_token
                        && next_lines_token.Lines.Length > 0
                        && !string.IsNullOrWhiteSpace(next_lines_token.Lines[0]);

                if (token is BlockToken)
                {
                    if (i != m_Tokens.Count - 1 && (next_token is BlockToken || next_token_not_empty))
                        string_builder.AppendLine();
                }
                else if (token is LineToken line_token)
                {
                    if (i != m_Tokens.Count - 1 && !string.IsNullOrWhiteSpace(line_token.Line) && next_token is not LineToken && (next_token is BlockToken || next_token_not_empty))
                        string_builder.AppendLine();
                }
                else if (token is LinesToken lines_token)
                {
                    if (i != m_Tokens.Count - 1 && lines_token.Lines.Length > 0 && !string.IsNullOrWhiteSpace(lines_token.Lines[lines_token.Lines.Length - 1]) && (next_token is BlockToken || next_token_not_empty))
                        string_builder.AppendLine();
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Unsupported token type '{token.GetType().Name}' encountered."
                    );
                }
            }

            return string_builder.ToString();
        }
    }
}
