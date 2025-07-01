using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    public interface IBuilder
    {
        public IOptions Options { get; }
        public IReadOnlyList<IToken> Tokens { get; }
        public IBuilder L(params string[] lines);
        public IBuilder B(string header, Action<IBuilder> builder_act);
        public IBuilder B(Action<IBuilder> builder_act);
        public string ToString();
    }
}
