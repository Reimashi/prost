using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prost.Http
{
    public static class HttpLinqParser
    {

        public const char HT = (char)0x09;
        public const char LF = (char)0x0A;
        public const char CR = (char)0x0D;
        public const char SP = (char)0x20;
        public const char QM = (char)0x22;
        public const char RUB = (char)0x7A;

        public const int OctetLength = 1;
        public const int CharLength = 1;

        public static Boolean IsOctect(this IEnumerable<char> data)
        {
            return data.Count<char>() == OctetLength;
        }
        
        public static Boolean IsChar(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && (uint)data.First<char>() < 0x80;
        }

        public static Boolean IsUpAlpha(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && (uint)data.First<char>() < 0x5B && (uint)data.First<char>() > 0x40;
        }

        public static Boolean IsLoAlpha(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && (uint)data.First<char>() < 0x7B && (uint)data.First<char>() > 0x60;
        }

        public static Boolean IsAlpha(char data)
        {
            return((uint)data < 0x5B && (uint)data > 0x40 || (uint)data < 0x7B && (uint)data > 0x60);
        }

        public static Boolean IsAlpha(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && IsAlpha(data.First<char>());
        }

        public static Boolean IsDigit(char value)
        {
            return value < 0x3A && value > 0x2F;
        }

        public static Boolean IsDigit(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && IsDigit(data.First<char>());
        }

        public static Boolean IsCtl(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && ((uint)data.First<char>() < SP || (uint)data.First<char>() == RUB);
        }

        public static Boolean IsCr(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && (uint)data.First<char>() == CR;
        }

        public static Boolean IsLf(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && (uint)data.First<char>() == LF;
        }

        public static Boolean IsCrLf(this IEnumerable<char> data)
        {
            return data.Count<char>() == (2 * CharLength) && data.First<char>() == CR && data.Skip<char>(1).First<char>() == LF;
        }

        public static Boolean IsSpace(char value)
        {
            return (uint)value == SP;
        }

        public static Boolean IsSpace(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && IsSpace(data.First<char>());
        }

        public static Boolean IsHt(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && (uint)data.First<char>() == HT;
        }

        public static Boolean IsDoubleQuote(this IEnumerable<char> data)
        {
            return data.Count<char>() == CharLength && (uint)data.First<char>() == QM;
        }

        public static Boolean NextIsLws(this IEnumerable<char> data)
        {
            int skip = 0;

            if (data.Count<char>() >= 2 && data.Take<char>(2).IsCrLf()) { skip = 2; }

            if (data.Count<char>() > skip)
            {
                IEnumerable<char> next = data.Skip<char>(skip).Take<char>(1);
                return next.IsSpace() || next.IsHt();
            }
            else return false;
        }

        public static IEnumerable<char> SkipLws(this IEnumerable<char> data)
        {
            int skip = 0;

            if (data.Count<char>() >= 2 && data.Take<char>(2).IsCrLf()) { skip = 2; }

            if (data.Skip<char>(skip).Count<char>() >= 1 && 
                (data.Skip<char>(skip).First<char>() == SP || data.Skip<char>(skip).First<char>() == HT))
            {
                return data.Skip<char>(skip).SkipWhile<char>((dchar) => dchar == SP || dchar == HT);
            }
            else throw new InvalidOperationException("This enumeration don't constains LWS expression.");
        }

        public static IEnumerable<char> TakeText(this IEnumerable<char> data)
        {
            return data.TakeWhile<char>((dchar, index) => dchar < SP || dchar == RUB);
        }

        public static IEnumerable<char> SkipText(this IEnumerable<char> data)
        {
            return data.SkipWhile<char>((dchar, index) => dchar < SP || dchar == RUB);
        }
    }
}
