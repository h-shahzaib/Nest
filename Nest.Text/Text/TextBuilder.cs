using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest.Text
{
    public class TextBuilder : IBuilder
    {
        private readonly List<Token> m_Tokens = [];

        public IOptions Options
        {
            get
            {
                if (m_Options == null)
                    Options = Text.Options.Default;
                return m_Options!;
            }
            set => m_Options = new Options((value as Options)!);
        }
        private Options? m_Options;

        public bool IsRootBuilder { get; set; } = true;

        public IBuilder L(params string[] lines)
        {
            var options = (Options as Options)!;

            Token token;
            if (lines.Length == 0)
                token = new LineToken(options);
            else if ((lines.Length == 1 && lines.Any(i => i.Contains(Environment.NewLine))) || lines.Length > 1)
                token = new LinesToken(options, lines);
            else
                token = new LineToken(options, lines[0]);

            m_Tokens.Add(token);
            return token;
        }

        public IBuilder B(string header, Action<IBuilder> builder_act)
        {
            var options = (Options as Options)!;

            var builder = new TextBuilder();
            builder.Options = Options;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            var token = new BlockToken(options, header, builder);
            m_Tokens.Add(token);
            return token;
        }

        public IBuilder B(Action<IBuilder> builder_act)
        {
            var options = (Options as Options)!;

            var builder = new TextBuilder();
            builder.Options = Options;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            var token = new BlockToken(options, null, builder);
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

                var should_add_line_break = ShouldAddLineBreak(i, token, next_token);
                if (should_add_line_break)
                    string_builder.AppendLine();
            }

            return string_builder.ToString();
        }

        private bool ShouldAddLineBreak(int index, Token current_token, Token next_token)
        {
            if (index == m_Tokens.Count - 1)
                return false;

            if (next_token is BlockToken)
                return true;

            bool current_has_content = TokenHasContent(current_token);
            bool next_has_content = TokenHasContent(next_token);

            return current_has_content && next_has_content && next_token is not LineToken;
        }

        private bool TokenHasContent(Token token)
        {
            return token switch
            {
                BlockToken => true,
                
                LineToken line_token => !string.IsNullOrWhiteSpace(line_token.Line),
                
                LinesToken lines_token => lines_token.Lines.Length > 0 
                    && !string.IsNullOrWhiteSpace(lines_token.Lines[lines_token.Lines.Length - 1]),

                _ => false
            };
        }
    }
}
