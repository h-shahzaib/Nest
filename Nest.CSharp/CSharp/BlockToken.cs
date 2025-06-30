using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.CSharp
{
    public class BlockToken : Token
    {
        internal string? Header { get; }
        internal CSharpBuilder Builder { get; }

        public BlockToken(Options options, string? header, CSharpBuilder builder) : base(options)
        {
            Header = header;
            Builder = builder;
        }

        internal List<BlockToken> Blocks { get; } = [];

        public BlockToken B(string header, Action<CSharpBuilder> builder_act)
        {
            var builder = new CSharpBuilder();
            builder.Options = Options;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            Blocks.Add(new BlockToken(Options, header, builder));
            return this;
        }

        public BlockToken B(Action<CSharpBuilder> builder_act)
        {
            var builder = new CSharpBuilder();
            builder.Options = Options;
            builder.IsRootBuilder = false;
            builder_act.Invoke(builder);

            Blocks.Add(new BlockToken(Options, null, builder));
            return this;
        }

        public override string ToString()
        {
            var output = new StringBuilder();

            for (int i = 0; i < Blocks.Count; i++)
            {
                BlockToken? block = Blocks[i];

                if (!string.IsNullOrWhiteSpace(block.Header))
                    output.AppendLine(ApplyReplacements(block.Header!));

                output.AppendLine("{");
                var content = block.Builder.ToString();
                if (!string.IsNullOrWhiteSpace(content))
                    output.AppendLine(content);

                if (i + 1 == Blocks.Count)
                    output.Append("}");
                else
                    output.AppendLine("}");
            }

            return output.ToString();
        }
    }
}
