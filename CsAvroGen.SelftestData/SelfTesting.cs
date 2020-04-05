using System.Collections.Generic;
using System.Drawing;
using CsAvroGen.DomainModel;


namespace holonsoft.CsAvroGen.SelfTestData
{
    public class SelfTesting
    {
        public int? NullableIntField;

        [AvroDefaultValue(5)]
        public int IntField;

        public int[] IntArray;

        public Dictionary<string, int> IntDictionary = new Dictionary<string, int>();
        public Dictionary<string, Point> PointDictionary = new Dictionary<string, Point>();

        public Point PointField;

        public SubclassSelfTesting SubclassSelfTesting;
    }
}
