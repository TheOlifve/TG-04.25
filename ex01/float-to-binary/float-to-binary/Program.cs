namespace float_to_binary;

class Program
{
    static void Main(string[] args)
    {
        float num = 3.14f;
        string bits = ToIEEE754(num, true);
        float back =  FromIEEE754(bits);
        
        Console.WriteLine(back);
    }

    static void PrintIEEE754(string num)
    {
        if (num.Length != 32)
            return;
        num = num.Insert(1, "|");
        num = num.Insert(10, "|");
        Console.WriteLine(num);
    }

    static string DecimalToBinary(int dec)
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

    static int DeterminateFractionShift(ref string result)
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
    
    static string CalculateFractionPart(float num, out int fractionShift)
    {
        string numStr = num.ToString("G");
        if (!numStr.Contains(','))
            numStr += ",0";
        string[] splitNum = numStr.Split(',');
        
        int divider = (int)Math.Pow(10, splitNum[1].Length);
        int fraction = int.Parse(splitNum[1]);
        
        string result = DecimalToBinary((int)num) + ",";
 
        for (int i = 0; (i < 24 && fraction != 0); ++i)
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
        if (result.Length > 23)
            result = result.Substring(0, 23);
        else if (result.Length < 23)
            result = result.PadRight(23, '0');
        return result;
    }

    static string ToIEEE754(float num, bool print = false)
    {
        string result = "00000000000000000000000000000000";
        if (num == 0)
        {
            if (print)
                PrintIEEE754(result);  
            return result;
        }
        int fractionShift;
        
        string sign = num >= 0 ? "0" : "1";
        if (num < 0)
            num = -num;
        string fraction = CalculateFractionPart(num, out fractionShift);
        string exponent = DecimalToBinary(fractionShift + 127);
        
        if (exponent.Length != 8)
            exponent = exponent.PadLeft(8, '0');
        
        result = sign + exponent + fraction;
        if (print)
            PrintIEEE754(result);
        return result;
    }

    static float FromIEEE754(string bits)
    {
        if (bits.Length != 32)
        {
            Console.WriteLine("Wrong length: Must be 32 characters");
            return 0f;
        }

        int sign = bits[0] == '0' ? 1 : -1;
        

        int exponent = 0;

        for (int i = 1; i <= 8; i++)
            exponent = exponent * 2 + (bits[i] - '0');
        exponent -= 127;

        float fraction = 1.0f;
        for (int i = 9; i < 32; i++)
        {
            if (bits[i] == '1')
                fraction += (float)Math.Pow(2, 8 - i);
        }
        return sign * fraction * (float)Math.Pow(2, exponent);
    }
}