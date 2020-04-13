using System;

namespace CsAvroGen.DomainModel.AvroAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AvroDecimalAttribute : Attribute
    {
        public int Precision { get; }
        public int Scale { get; }

        public AvroDecimalAttribute(int precision, int scale)
        {
            if (precision < 1)
            {
                throw new ArgumentOutOfRangeException("Precision must be greater zero");
            }

            if (scale < 0)
            {
                throw new ArgumentOutOfRangeException("Scale must be greater or equal zero");
            }


            if (scale > precision)
            {
                throw new ArgumentOutOfRangeException("Scale must be less or equal precision");
            }

            Precision = precision;
            Scale = scale;
        }
    }
}
