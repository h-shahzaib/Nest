using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nest.Text
{
    internal abstract class Token : IBuilder
    {
        public IOptions Options { get; }

        public Token(Options options)
        {
            Options = new Options(options);
            Tokens.Add(this);
        }

        public string ApplyReplacements(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var output = str;

            foreach (var replacement in Options.GetCharReplacements())
            {
                output = output.Replace(replacement.Key, replacement.Value);
            }

            return output;
        }

        public List<Token> Tokens { get; } = [];

        public IBuilder L(params string[] lines)
        {
            throw new NotImplementedException();
        }

        public IBuilder B(string header, Action<IBuilder> builder_act)
        {
            throw new NotImplementedException();
        }

        public IBuilder B(Action<IBuilder> builder_act)
        {
            throw new NotImplementedException();
        }

        public sealed override string ToString()
        {
            return string.Empty;
        }

        public abstract string Compose();
    }
}
