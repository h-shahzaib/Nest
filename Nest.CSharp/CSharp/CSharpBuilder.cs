using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest.CSharp
{
    public class CSharpBuilder
    {
        private readonly List<Token> m_Tokens = [];

        public Options Options
        {
            get
            {
                if (m_Options == null)
                    Options = Options.Default;
                return m_Options!;
            }
            set => m_Options = new Options(value);
        }
        private Options? m_Options;

        internal bool IsRootBuilder { get; set; } = true;

        public void L(params string[] lines)
        {
            if (lines.Length == 0)
                m_Tokens.Add(new LineToken(Options));
            else if (lines.Length == 1 && lines[0].Contains(Environment.NewLine) || lines.Length > 1)
                m_Tokens.Add(new LinesToken(Options, lines));
            else
                m_Tokens.Add(new LineToken(Options, lines[0]));
        }

        public BlockToken B(string header, Action<CSharpBuilder> builder_act)
        {
            var builder = new CSharpBuilder();
            builder.Options = Options;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            var token = new BlockToken(Options, header, builder);
            token.Blocks.Add(token);
            m_Tokens.Add(token);
            return token;
        }

        public BlockToken B(Action<CSharpBuilder> builder_act)
        {
            var builder = new CSharpBuilder();
            builder.Options = Options;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            var token = new BlockToken(Options, null, builder);
            token.Blocks.Add(token);
            m_Tokens.Add(token);
            return token;
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
