using Nest.Text.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nest.Text
{
    public class TextBuilder
    {
        private readonly TextBuilderContext m_Context;

        public TextBuilder() => m_Context = new();
        public TextBuilder(TextBuilderOptions options) => m_Context = new(options);
        internal TextBuilder(TextBuilderContext context) => m_Context = context;

        internal Token? ParentToken { get; private set; } = null;
        internal bool IsRootBuilder { get; private set; } = true;

        public TextBuilderOptions Options => m_Context.Options;

        public TextBuilder L(string line = "")
        {
            Token token;

            if (line.Contains(Environment.NewLine))
                token = new LinesToken(m_Context.Options, line);
            else
                token = new LineToken(m_Context.Options, line);

            m_Context.Tokens.Add(token);

            return GetChainBuilder(token, m_Context);
        }

        public TextBuilder L(params string[] lines)
        {
            var token = new LinesToken(m_Context.Options, string.Join(Environment.NewLine, lines));

            m_Context.Tokens.Add(token);

            return GetChainBuilder(token, m_Context);
        }

        public TextBuilder B(Action<TextBuilder> builder_act)
        {
            var builder = new TextBuilder(m_Context.Options);
            builder_act.Invoke(builder);
            builder.IsRootBuilder = false;

            if (builder.m_Context.Tokens.Count > 0)
            {
                var token = new BlockToken(m_Context.Options, builder);
                m_Context.Tokens.Add(token);
                return GetChainBuilder(token, m_Context);
            }

            return this;
        }

        private TextBuilder GetChainBuilder(Token token, TextBuilderContext context)
        {
            if (ParentToken != null)
            {
                token.ParentToken = ParentToken;
                ParentToken = token;
                return this;
            }
            else
            {
                var chain_builder = new TextBuilder(context);
                chain_builder.ParentToken = token;
                chain_builder.IsRootBuilder = false;
                return chain_builder;
            }
        }

        public override string ToString()
        {
            var string_builder = new StringBuilder();
            BuildText(string_builder, this, 0);
            return string_builder.ToString();
        }

        private static void BuildText(StringBuilder @string, TextBuilder builder, int spaces_count)
        {
            var new_line = Environment.NewLine;

            for (int i = 0; i < builder.m_Context.Tokens.Count; i++)
            {
                var token = builder.m_Context.Tokens[i];

                var spaces = new string(' ', spaces_count);
                if (!builder.IsRootBuilder && token.Options.IndentSize > 0)
                    spaces = new string(' ', spaces_count + token.Options.IndentSize);

                if (i != 0)
                    @string.AppendLine();

                if (token is LineToken line_token)
                {
                    @string.Append(spaces + ApplyReplacements(token.Options, line_token.Line));
                }
                else if (token is LinesToken lines_token)
                {
                    @string.Append(string.Join(new_line, lines_token.Lines.Split([Environment.NewLine], StringSplitOptions.None).Select(i => spaces + ApplyReplacements(token.Options, i))));
                }
                else if (token is BlockToken block_token)
                {
                    if (block_token.Builder.Options.BlockStyle == BlockStyle.Braces)
                        @string.AppendLine(spaces + '{');

                    BuildText(@string, block_token.Builder, spaces.Length);

                    if (block_token.Builder.Options.BlockStyle == BlockStyle.Braces)
                        @string.Append(new_line + spaces + '}');
                }

                if (ShouldAddLineBreak(builder.m_Context.Tokens, i, token))
                    @string.AppendLine();
            }
        }

        private static bool ShouldAddLineBreak(List<Token> tokens, int index, Token first_token)
        {
            if (TokenIsEmpty(first_token))
                return false;

            var second_token = index + 1 < tokens.Count ? tokens[index + 1] : null;
            if (second_token is null)
                return false;

            if (TokenIsEmpty(second_token))
                return false;

            if ((first_token is LinesToken || first_token is BlockToken) && first_token.ParentToken is null)
                return true;

            if ((second_token is LinesToken || second_token is BlockToken) && second_token.ParentToken is null)
                return true;

            var third_token = index + 2 < tokens.Count ? tokens[index + 2] : null;
            if (second_token.ParentToken is null && third_token is not null && third_token.ParentToken is not null && ReferenceEquals(third_token.ParentToken, second_token))
                return true;

            if (first_token.ParentToken is not null && second_token.ParentToken is null)
                return true;

            return false;
        }

        private static bool TokenIsEmpty(Token token)
        {
            return token switch
            {
                LineToken line_token => string.IsNullOrWhiteSpace(line_token.Line),
                LinesToken lines_token => string.IsNullOrWhiteSpace(lines_token.Lines),
                _ => false
            };
        }

        private static string ApplyReplacements(TextBuilderOptions options, string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var output = str;
            foreach (var replacement in options.GetCharReplacements())
                output = output.Replace(replacement.Key, replacement.Value);

            return output;
        }
    }
}
