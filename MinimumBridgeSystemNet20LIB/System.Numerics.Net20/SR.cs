using System.Collections.Generic;

namespace System.Numerics
{
    internal static class SR
    {
        internal static Dictionary<string, string> exceptions = new Dictionary<string, string>
        {
            ["Overflow_Byte"] = "Value was either too large or too small for an unsigned byte.",
            ["Overflow_Char"] = "Value was either too large or too small for a character.",
            ["Overflow_Currency"] = "Value was either too large or too small for a Currency.",
            ["Overflow_Decimal"] = "Value was either too large or too small for a Decimal.",
            ["Overflow_Int16"] = "Value was either too large or too small for an Int16.",
            ["Overflow_Int32"] = "Value was either too large or too small for an Int32.",
            ["Overflow_Int64"] = "Value was either too large or too small for an Int64.",
            ["Overflow_NegateTwosCompNum"] = "Negating the minimum value of a twos complement number is invalid.",
            ["Overflow_NegativeUnsigned"] = "The string was being parsed as an unsigned number and could not have a negative sign.",
            ["Overflow_SByte"] = "Value was either too large or too small for a signed byte.",
            ["Overflow_Single"] = "Value was either too large or too small for a Single.",
            ["Overflow_Double"] = "Value was either too large or too small for a Double.",
            ["Overflow_TimeSpanTooLong"] = "TimeSpan overflowed because the duration is too long.",
            ["Overflow_TimeSpanElementTooLarge"] = "The TimeSpan could not be parsed because at least one of the numeric components is out of range or contains too many digits.",
            ["Overflow_Duration"] = "The duration cannot be returned for TimeSpan.MinValue because the absolute value of TimeSpan.MinValue exceeds the value of TimeSpan.MaxValue.",
            ["Overflow_UInt16"] = "Value was either too large or too small for a UInt16.",
            ["Overflow_UInt32"] = "Value was either too large or too small for a UInt32.",
            ["Overflow_UInt64"] = "Value was either too large or too small for a UInt64.",
            ["Overflow_NotANumber"] = "The value is not a number.",
            ["Overflow_BigIntInfinity"] = "BigInteger cannot represent infinity.",
            ["Overflow_ParseBigInteger"] = "The value could not be parsed.",

            ["Format_AttributeUsage"] = "Duplicate AttributeUsageAttribute found on attribute type {0}.",
            ["Format_Bad7BitInt32"] = "Too many bytes in what should have been a 7 bit encoded Int32.",
            ["Format_BadBase"] = "Invalid digits for the specified base.",
            ["Format_BadBase64Char"] = "The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.",
            ["Format_BadBase64CharArrayLength"] = "Invalid length for a Base-64 char array or string.",
            ["Format_BadBoolean"] = "String was not recognized as a valid Boolean.",
            ["Format_BadDateTime"] = "String was not recognized as a valid DateTime.",
            ["Format_BadDateTimeCalendar"] = "The DateTime represented by the string is not supported in calendar {0}.",
            ["Format_BadDayOfWeek"] = "String was not recognized as a valid DateTime because the day of week was incorrect.",
            ["Format_DateOutOfRange"] = "The DateTime represented by the string is out of range.",
            ["Format_BadDatePattern"] = "Could not determine the order of year, month, and date from '{0}'.",
            ["Format_BadFormatSpecifier"] = "Format specifier was invalid.",
            ["Format_BadTimeSpan"] = "String was not recognized as a valid TimeSpan.",
            ["Format_BadQuote"] = "Cannot find a matching quote character for the character '{0}'.",
            ["Format_EmptyInputString"] = "Input string was either empty or contained only whitespace.",
            ["Format_ExtraJunkAtEnd"] = "Additional non-parsable characters are at the end of the string.",
            ["Format_GuidBrace"] = "Expected {0xdddddddd, etc}.",
            ["Format_GuidComma"] = "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).",
            ["Format_GuidBraceAfterLastNumber"] = "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).",
            ["Format_GuidDashes"] = "Dashes are in the wrong position for GUID parsing.",
            ["Format_GuidEndBrace"] = "Could not find the ending brace.",
            ["Format_GuidHexPrefix"] = "Expected hex 0x in '{0}'.",
            ["Format_GuidInvLen"] = "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).",
            ["Format_GuidInvalidChar"] = "Guid string should only contain hexadecimal characters.",
            ["Format_GuidUnrecognized"] = "Unrecognized Guid format.",
            ["Format_InvalidEnumFormatSpecification"] = "Format String can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".",
            ["Format_InvalidGuidFormatSpecification"] = "Format String can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".",
            ["Format_InvalidString"] = "Input string was not in a correct format.",
            ["Format_IndexOutOfRange"] = "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.",
            ["Format_UnknowDateTimeWord"] = "The string was not recognized as a valid DateTime. There is an unknown word starting at index {0}.",
            ["Format_NeedSingleChar"] = "String must be exactly one character long.",
            ["Format_NoParsibleDigits"] = "Could not find any recognizable digits.",
            ["Format_RepeatDateTimePattern"] = "DateTime pattern '{0}' appears more than once with different values.",
            ["Format_StringZeroLength"] = "String cannot have zero length.",
            ["Format_TwoTimeZoneSpecifiers"] = "The String being parsed cannot contain two TimeZone specifiers.",
            ["Format_UTCOutOfRange"] = "The UTC representation of the date falls outside the year range 1-9999.",
            ["Format_OffsetOutOfRange"] = "The time zone offset must be within plus or minus 14 hours.",
            ["Format_MissingIncompleteDate"] = "There must be at least a partial date with a year present in the input.",
            ["Format_TooLarge"] = "The value is too large to be represented by this format specifier.",

            ["Argument_MustBeBigInt"] = "The parameter must be a BigInteger.",
            ["Argument_InvalidNumberStyles"] = "An undefined NumberStyles value is being used.",
            ["Argument_InvalidHexStyle"] = "With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber.",
            ["ArgumentOutOfRange_MustBeNonNeg"] = "The number must be greater than or equal to zero.",
            ["Arg_NullArgumentNullRef"] = "The method was called with a null array argument.",
            ["Arg_ArgumentOutOfRangeException"] = "Index was out of bounds: {0}",
            ["Arg_ElementsInSourceIsGreaterThanDestination"] = "Number of elements in source vector is greater than the destination array: {0}",
        };

        public static string GetString(string name)
        {
            if (exceptions.ContainsKey(name))
                return exceptions[name];
            else
                return string.Empty;
        }

        public static string GetString(string name, params object[] args)
        {
            if (exceptions.ContainsKey(name))
                return string.Format(exceptions[name], args);
            else
                return string.Empty;
        }
    }
}