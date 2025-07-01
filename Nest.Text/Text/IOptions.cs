using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nest.Text
{
    public interface IOptions
    {
        public int NumberOfSpacesInOneTab { get; set; }
        public void RegisterCharReplacement(char original_char, char replace_with);
        public void RemoveCharReplacement(char original_char);
        public IReadOnlyDictionary<char, char> GetCharReplacements();
    }
}
