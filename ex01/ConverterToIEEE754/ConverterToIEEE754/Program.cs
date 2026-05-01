namespace ConverterToIEEE754;
class Converter
{
    public static string FloatToBinary(float num, bool print = false)
    {
        int len = 32;
        int bias = 127;
        int exponentLen = 8;
        int fractionLen = 23;
        
        if (num == 0)
        {
            if (print)
                PrintBinary("0", new string('0', exponentLen), new string('0', fractionLen));
            return new string('0', len);
        }
        
        int fractionShift;
        string sign = num >= 0 ? "0" : "1";
        
        if (num < 0)
            num = -num;
        
        string fraction = CalculateFractionPart(num.ToString("R"), out fractionShift, fractionLen);
        string exponent = DecimalToBinary((ulong)(fractionShift + bias));
        
        if (exponent.Length != exponentLen)
            exponent = exponent.PadLeft(exponentLen, '0');
        
        if (print)
            PrintBinary(sign, exponent, fraction);
        return sign + exponent + fraction;
    }
    
    public static string DoubleToBinary(double num, bool print = false)
    {
        int len = 64;
        int bias = 1023;
        int exponentLen = 11;
        int fractionLen = 52;
        
        if (num == 0)
        {
            if (print)
                PrintBinary("0", new string('0', exponentLen), new string('0', fractionLen));  
            return new string('0', len);
        }
        
        int fractionShift;
        string sign = num >= 0 ? "0" : "1";
        
        if (num < 0)
            num = -num;
        
        string fraction = CalculateFractionPart(num.ToString("R"), out fractionShift, fractionLen);
        string exponent = DecimalToBinary((ulong)(fractionShift + bias));
        
        if (exponent.Length != exponentLen)
            exponent = exponent.PadLeft(exponentLen, '0');
        
        if (print)
            PrintBinary(sign, exponent, fraction);
        return sign + exponent + fraction;
    }
    
    public static float BinaryToFloat(string bits) { return (float)ConvertFromBinary(bits); }
    public static double BinaryToDouble(string bits) { return ConvertFromBinary(bits); }
    
    static double ConvertFromBinary(string bits)
    {
        int len = bits.Length == 64 ? 64 : (bits.Length == 32 ? 32 : 0);
        if (len == 0)
        {
            Console.WriteLine("Wrong length");
            return 0;
        }

        if (!bits.Contains("1"))
            return 0;
        
        int bias        = len == 64 ? 1023 : 127;
        int exponentLen = len == 64 ? 11 : 8;
        int fractionLen = len == 64 ? 52 : 23;

        int sign = bits[0] == '0' ? 1 : -1;
        
        int exponent = 0;

        for (int i = 1; i <= exponentLen; i++)
            exponent = exponent * 2 + (bits[i] - '0');
        exponent -= bias;

        double fraction = 1.0;
        for (int i = exponentLen + 1; i < len; i++)
        {
            if (bits[i] == '1')
                fraction += Math.Pow(2, exponentLen - i);
        }
        return sign * fraction * Math.Pow(2, exponent);
    }
    
    private static void PrintBinary(string sign, string exponent, string fraction) =>
        Console.WriteLine($"{sign}|{exponent}|{fraction}");
    
    private static string DecimalToBinary(ulong dec)
    {
        if (dec == 0)
            return "0";
        
        string tmp;
        string result = "";
        
        while (dec != 0)
        {
            tmp = (dec % 2).ToString();
            result = tmp + result; 
            dec /= 2;
        }
        return result;
    }

    private static int DeterminateFractionShift(ref string result)
    {
        int fractionShift;
        
        string[] bits = result.Split(',');

        if (bits[0].Contains('1'))
        {
            result = bits[0].Substring(bits[0].IndexOf('1') + 1) + bits[1];
            fractionShift = bits[0].Length - 1;
        }
        else
        {
            result = bits[1].Substring(bits[1].IndexOf('1') + 1);
            fractionShift = -bits[1].IndexOf('1') - 1;
        }
        
        return fractionShift;
    }
    
    private static string CalculateFractionPart(string numStr, out int fractionShift, int fractionLen)
    {
        if (!numStr.Contains(','))
            numStr += ",0";
        
        string[] splitNum = numStr.Split(',');
        
        ulong divider = (ulong)Math.Pow(10, splitNum[1].Length);
        ulong num = ulong.Parse(splitNum[0]);
        ulong fraction = ulong.Parse(splitNum[1]);
        
        string result = DecimalToBinary(num) + ",";
 
        for (int i = 0; (i < fractionLen && fraction != 0); ++i)
        {
            fraction *= 2;
            
            if (fraction >= divider)
            {
                result += '1';
                fraction -= divider;
            }
            else
                result += '0';
        }
        fractionShift = DeterminateFractionShift(ref result);

        if (result.Length > fractionLen)
        {
            if (result[fractionLen] == '1')
                result = result.Substring(0, fractionLen - 1) + '1';
            else
                result = result.Substring(0, fractionLen);
        }
        else if (result.Length < fractionLen)
            result = result.PadRight(fractionLen, '0');
        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- Float Tests ---");
        float[] floatCases = { 
            1.0f,
            -1.0f,
            0.15625f,
            3.1415927f,
            0.0f
        };

        foreach (float num in floatCases) {
            Console.WriteLine();
            string bits = Converter.FloatToBinary(num, true);
            float back = Converter.BinaryToFloat(bits);
            Console.WriteLine($"In: {num} -> Out: {back} | {(num == back ? "OK" : "FAIL")}");
        }
        
        Console.WriteLine("\n--- Double Tests ---");
        double[] doubleCases = { 
            1.0, 
            -2.0,
            0.1,
            1234567.890123,
            -0.0
        };

        foreach (double num in doubleCases) {
            Console.WriteLine();
            string bits = Converter.DoubleToBinary(num, true);
            double back = Converter.BinaryToDouble(bits);
            Console.WriteLine($"In: {num} -> Out: {back} | {(num == back ? "OK" : "FAIL")}");
        }
    }
}