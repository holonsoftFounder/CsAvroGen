using System.Collections.Generic;
using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.ShowRunner
{
    internal class IndentProvider
    {
        private readonly int _indentFactor;
        private readonly string _indentBaseStr;
        private int _preGeneratedIndentsCount;
        private int _index = 0;

        private Dictionary<int, string> _preCalcIndent { get; } = new Dictionary<int, string>();

        public IndentProvider(string indentBaseStr, int indentFactor)
        {
            _indentBaseStr = indentBaseStr;
            _indentFactor = indentFactor;
        }

        public void Prepare(int preGeneratedIndents)
        {
            _preGeneratedIndentsCount = preGeneratedIndents;

            for (var i = 1; i <= preGeneratedIndents; i++)
            {
                _preCalcIndent.Add(i, _indentBaseStr.Repeat(i * _indentFactor));
            }
        }


        public void Reset()
        {
            _index = 0;
        }


        public void IncLevel()
        {
            _index++;
        }


        public void DecLevel()
        {
            _index--;

            if (_index < 0) _index = 0;
        }


        public string Get()
        {
            if (_index == 0) return string.Empty;

            if (_index > _preGeneratedIndentsCount)
            {
                return _indentBaseStr.Repeat(_index * _indentFactor);
            }

            return (_preCalcIndent[_index]);
        }
    }
}
