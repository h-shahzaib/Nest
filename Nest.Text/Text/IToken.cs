using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    public interface IToken : IBuilder
    {
        public IOptions Options { get; }
    }
}
