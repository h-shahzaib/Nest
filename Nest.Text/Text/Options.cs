using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nest.Text
{
    internal class Options : IOptions
    {
        private readonly Dictionary<char, char> m_CharReplacements = new() 
        {
            { '`', '"' } 
        };

        public Options()
        {
        }

        public Options(Options options)
        {
            m_CharReplacements = new Dictionary<char, char>(options.m_CharReplacements);
            NumberOfSpacesInOneTab = options.NumberOfSpacesInOneTab;
        }

        public int NumberOfSpacesInOneTab { get; set; } = 4;

        public void RegisterCharReplacement(char original_char, char replace_with)
        {
            if (original_char == ' ')
                throw new NotSupportedException("Cannot add replacement for `space` character.");

            m_CharReplacements[original_char] = replace_with;
        }

        public void RemoveCharReplacement(char original_char)
        {
            m_CharReplacements.Remove(original_char);
        }

        public IReadOnlyDictionary<char, char> GetCharReplacements()
        {
            return new ReadOnlyDictionary<char, char>(m_CharReplacements);
        }

        public static Options Default => new();
    }
}
