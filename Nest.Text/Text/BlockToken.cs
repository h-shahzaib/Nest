using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    internal class BlockToken : Token
    {
        internal string? Header { get; }
        internal TextBuilder Builder { get; }

        public BlockToken(Options options, string? header, TextBuilder builder) : base(options)
        {
            Header = header;
            Builder = builder;
        }

        public override string Compose()
        {
            var output = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Header))
            {
                output.AppendLine(ApplyReplacements(Header!));
                output.Append(Builder.ToString());
            }
            else
            {
                output.AppendLine(Builder.ToString());
            }

            return output.ToString();
        }
    }
}
