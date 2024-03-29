using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Units
{
    /// <summary>
    /// A little helper class to convert bytes (e.g. 5GB to 5120MB)
    /// </summary>
    public class ByteUnit
    {
        public double Length { get; set; }

        public Unit Type { get; set; }

        public ByteUnit(double length, Unit type)
        {
            Length = length;
            Type = type;
        }

        /// <summary>
        /// Calculates an appropriate unit and returns the calculated ByteUnit instance
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ByteUnit FindUnit(double value)
        {
            // Get right unit prefix
            int index = 1;
            double nValue = value;

            while (nValue > 1024.0)
            {
                nValue /= 1024.0;
                index++;
            }

            if (index > (int)Unit.TB)
                throw new NotSupportedException("This unit is currently not supported!");

            return new ByteUnit(Math.Round(nValue, 2), (Unit)index);
        }

        #region Operators

        public static bool operator ==(ByteUnit x, ByteUnit y)
        {
            // e.g. 5 MB == 5120 KB would return true, because the units are different, but the value is the same!
            if (x.Type == y.Type && x.Length == y.Length)
                return true;

            if (x.Type == y.Type)
                return false;

            var nY = From(y, x.Type);
            return (x.Length == nY.Length);
        }

        public static bool operator <(ByteUnit x, ByteUnit y)
        {
            // No conversation required if they are the same tyxpe
            if (x.Type == y.Type)
                return x.Length < y.Length;

            // Otherwise convert y to the unit of x
            var nY = From(y, x.Type);
            return x.Length < nY.Length;
        }

        public static bool operator >(ByteUnit x, ByteUnit y)
        {
            if (x == y)
                return false;

            return !(x < y);
        }

        public static bool operator !=(ByteUnit x, ByteUnit y)
        {
            return (x != y);
        }

        public static bool operator <=(ByteUnit x, ByteUnit y)
        {
            return (x < y || x == y);
        }

        public static bool operator >=(ByteUnit x, ByteUnit y)
        {
            return (x > y || x == y);
        }

        public override bool Equals(object obj)
        {
            if (obj is ByteUnit b)
                return (this == b); // this.Length == b.Length && this.Type == b.Type;

            return false;
        }

        public override int GetHashCode()
        {
            return Length.GetHashCode() + Type.GetHashCode();
        }

        #endregion

        #region From Methods

        /// <summary>
        /// Returns a ByteUnit instance in Bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromB(long bytes)
        {
            return new ByteUnit(bytes, Unit.B);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromB(double bytes)
        {
            return new ByteUnit(bytes, Unit.B);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Kilobytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromKB(long bytes)
        {
            return new ByteUnit(bytes, Unit.KB);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Kilobytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromKB(double bytes)
        {
            return new ByteUnit(bytes, Unit.KB);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Megabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromMB(long bytes)
        {
            return new ByteUnit(bytes, Unit.MB);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Megabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromMB(double bytes)
        {
            return new ByteUnit(bytes, Unit.MB);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Gigabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromGB(long bytes)
        {
            return new ByteUnit(bytes, Unit.GB);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Gigabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromGB(double bytes)
        {
            return new ByteUnit(bytes, Unit.GB);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Terabytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ByteUnit FromTB(long bytes)
        {
            return new ByteUnit(bytes, Unit.TB);
        }

        /// <summary>
        /// Returns a ByteUnit instance in Terabytes
        /// </summary>
        /// <param name="bytes"></param>
        public static ByteUnit FromTB(double bytes)
        {
            return new ByteUnit(bytes, Unit.TB);
        }

        /// <summary>
        /// Parses a string to byte unit (e.g. 10 GB/s, 5MB)
        /// </summary>
        /// <param name="value">The value you want to parse</param>
        /// <returns>A ByteUnit instance with the result</returns>
        public static ByteUnit Parse(string value)
        {
            value = value.ToUpper().Replace("/S", string.Empty);

            double? doubleValue = null;

            var match = Regex.Match(value, @"\d+");
            if (match.Success)
            {
                string number = match.Value;
                doubleValue = double.Parse(number, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture);
            }

            if (doubleValue == null)
                throw new ParseException("No valid number found!");

            // Guess the unit (names must be reversed to ensure that B will used as last value to ensure MB etc will not handled as B)
            foreach (var name in Enum.GetNames(typeof(Unit)).Reverse())
            {
                if (value.Contains(name.ToUpper()))
                    return new ByteUnit(doubleValue.Value, (Unit)Enum.Parse(typeof(Unit), name));
            }

            throw new ParseException("Unit couldn't be determined!");
        }

        /// <summary>
        /// Parses a string to byte unit (e.g. 10 GB/s, 5MB)
        /// </summary>
        /// <param name="value">The value you want to parse</param>
        /// <param name="result">The result</param>
        /// <returns>A ByteUnit instance with the result</returns>
        public static bool TryParse(string value, out ByteUnit result)
        {
            result = null;

            try
            {
                result = Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a ByteUnit instance from a System.IO.FileInfo with an appropriate unit
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ByteUnit FromFileInfo(System.IO.FileInfo fileInfo)
        {
            if (fileInfo == null)
                throw new ArgumentNullException("fileInfo");

            return ByteUnit.FindUnit(fileInfo.Length);
        }

        /// <summary>
        /// Converts the given source to a given unit, e.g. 5MB to 5120KB
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetUnit"></param>
        /// <returns></returns>
        public static ByteUnit From(ByteUnit source, Unit targetUnit)
        {
            // Calculate difference
            int difference = (int)source.Type - (int)targetUnit;
            int posDiff = (int)Math.Abs(difference);

            double length = Math.Round(difference < 0 ? source.Length / Math.Pow(1024, posDiff) : source.Length * Math.Pow(1024, posDiff), 2);
            return new ByteUnit(length, targetUnit);
        }

        /// <summary>
        /// Converts this instance to another unit
        /// </summary>
        /// <param name="targetUnit">The target unit to convert to</param>
        /// <returns></returns>
        public ByteUnit To(Unit targetUnit)
        {
            return From(this, targetUnit);
        }

        #endregion

        #region ToString
        public override string ToString()
        {
            return $"{Length:G} {Type}";
        }

        /// <summary>
        /// Returns the unit as a speed
        /// </summary>
        /// <param name="inSeconds"></param>
        /// <returns>e.g. 5 MB/s</returns>
        public string ToString(bool inSeconds)
        {
            return $"{ToString()}/s";
        }
        #endregion
    }

    public class ParseException : Exception
    {
        public ParseException() { }

        public ParseException(string message) : base(message) { }
    }

    public enum Unit
    {
        B = 1,
        KB = 2,
        MB = 3,
        GB = 4,
        TB = 5
    }
}