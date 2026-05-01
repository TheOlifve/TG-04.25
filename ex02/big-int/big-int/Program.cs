namespace big_int;

class Program
{
    static void Main(string[] args)
    {
        int a = int.MaxValue;
        Console.WriteLine(unchecked(a + 1)); // -2147483648
        long b = long.MaxValue;
        Console.WriteLine(unchecked(b + 1)); // -9223372036854775808

        
        int passed = 0, failed = 0;

        void Check(string label, string result, string expected)
        {
            bool ok = result == expected;
            string status = ok ? "✓" : "✗";
            if (ok) passed++; else failed++;
            Console.ForegroundColor = ok ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"  {status} {label}");
            if (!ok)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"      Ожидалось : {expected}");
                Console.WriteLine($"      Получилось: {result}");
            }
            Console.ResetColor();
        }

        // ── Add ────────────────────────────────────────────────
        Console.WriteLine("\n=== Add ===");
        Check("Add(\"0\", \"0\")",                           Add("0", "0"),                           "0");
        Check("Add(\"1\", \"1\")",                           Add("1", "1"),                           "2");
        Check("Add(\"1000000000\", \"1000000000\")",         Add("1000000000", "1000000000"),         "2000000000");
        Check("Add(\"-1\", \"-1\")",                         Add("-1", "-1"),                         "-2");
        Check("Add(\"-9999999999\", \"-1\")",                Add("-9999999999", "-1"),                "-10000000000");
        Check("Add(\"5\", \"-3\")",                          Add("5", "-3"),                          "2");
        Check("Add(\"-5\", \"3\")",                          Add("-5", "3"),                          "-2");
        Check("Add(\"5\", \"-5\")",                          Add("5", "-5"),                          "0");
        Check("Add(\"2147483647\", \"-1\")",                 Add("2147483647", "-1"),                 "2147483646");
        Check("Add(\"-9999999999\", \"9999999999\")",        Add("-9999999999", "9999999999"),        "0");
        Check("Add(\"2147483647\", \"1\")",                  Add("2147483647", "1"),                  "2147483648");
        Check("Add(\"-2147483648\", \"-1\")",                Add("-2147483648", "-1"),                "-2147483649");
        Check("Add(\"9223372036854775807\", \"1\")",         Add("9223372036854775807", "1"),         "9223372036854775808");

        // ── Subtract ───────────────────────────────────────────
        Console.WriteLine("\n=== Subtract ===");
        Check("Subtract(\"0\", \"0\")",                      Subtract("0", "0"),                      "0");
        Check("Subtract(\"1\", \"1\")",                      Subtract("1", "1"),                      "0");
        Check("Subtract(\"5\", \"3\")",                      Subtract("5", "3"),                      "2");
        Check("Subtract(\"10000\", \"1\")",                  Subtract("10000", "1"),                  "9999");
        Check("Subtract(\"9999999999\", \"10000000000\")",   Subtract("9999999999", "10000000000"),   "-1");
        Check("Subtract(\"3\", \"5\")",                      Subtract("3", "5"),                      "-2");
        Check("Subtract(\"-3\", \"-5\")",                    Subtract("-3", "-5"),                    "2");
        Check("Subtract(\"-5\", \"3\")",                     Subtract("-5", "3"),                     "-8");
        Check("Subtract(\"-1000000\", \"-999999\")",         Subtract("-1000000", "-999999"),         "-1");
        Check("Subtract(\"5\", \"-3\")",                     Subtract("5", "-3"),                     "8");
        Check("Subtract(\"2147483647\", \"-1\")",            Subtract("2147483647", "-1"),            "2147483648");
        Check("Subtract(\"-2147483648\", \"1\")",            Subtract("-2147483648", "1"),            "-2147483649");

        // ── Multiply ───────────────────────────────────────────
        Console.WriteLine("\n=== Multiply ===");
        Check("Multiply(\"0\", \"999\")",                    Multiply("0", "999"),                    "0");
        Check("Multiply(\"1\", \"999\")",                    Multiply("1", "999"),                    "999");
        Check("Multiply(\"-1\", \"999\")",                   Multiply("-1", "999"),                   "-999");
        Check("Multiply(\"3\", \"4\")",                      Multiply("3", "4"),                      "12");
        Check("Multiply(\"710\", \"3000\")",                 Multiply("710", "3000"),                 "2130000");
        Check("Multiply(\"-3\", \"-4\")",                    Multiply("-3", "-4"),                    "12");
        Check("Multiply(\"-710\", \"-3000\")",               Multiply("-710", "-3000"),               "2130000");
        Check("Multiply(\"-3\", \"4\")",                     Multiply("-3", "4"),                     "-12");
        Check("Multiply(\"3\", \"-4\")",                     Multiply("3", "-4"),                     "-12");
        Check("Multiply(\"-710\", \"3000\")",                Multiply("-710", "3000"),                "-2130000");
        Check("Multiply(\"1000000\", \"-1000000\")",         Multiply("1000000", "-1000000"),         "-1000000000000");
        Check("Multiply(\"2147483647\", \"2\")",             Multiply("2147483647", "2"),             "4294967294");
        Check("Multiply(\"-2147483648\", \"2\")",            Multiply("-2147483648", "2"),            "-4294967296");
        
        Console.WriteLine();
        Console.ForegroundColor = failed == 0 ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"Итог: {passed} passed, {failed} failed из {passed + failed}");
        Console.ResetColor();    
    }

    static string Validator(string str, out bool signed)
    {
        signed = false;
        if (string.IsNullOrEmpty(str)) return "0";
        
        char sign = '0';
        
        if (str.Length != 0 && (str[0] == '-' || str[0] == '+'))
        {
            sign = str[0];
            str = str.Substring(1, str.Length - 1);
        }
        
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] < '0' || str[i] > '9')  return "0";
        }

        int index = 0;
        
        while (index < str.Length && str[index] == '0')
            index++;
        
        str = str.Substring(index);
        
        if (str.Length == 0) str = "0";

        signed = false;
        if (sign == '-')
            signed = true;
        
        return str;
    }

    static int Compare(string a, string b)
    {
        if (a.Length != b.Length)
            return a.Length > b.Length ? 1 : -1;
        else
        {
            int substract;
            for (int i = 0; i < a.Length; i++)
            {
                substract = a[i] - b[i];
                if (substract != 0) return substract > 0 ? 1 : -1;
            }
        }
        return 0;
    }
    static string Add(string a, string b)
    {
        bool aSigned;
        bool bSigned;
        a = Validator(a, out aSigned);
        b = Validator(b, out bSigned);
        
        int comparation = Compare(a, b);
        
        if (aSigned && bSigned)
            return "-" + (Add(a, b));
        else if (aSigned)
        {
            if (comparation == 1)
                return "-" + (Subtract(a, b));
            else if (comparation == -1)
                return (Subtract(a, b));
            else
                return (Subtract(b, a));
        }
        else if (bSigned)
        {
            if (comparation == 1)
                return (Subtract(a, b));
            else if (comparation == -1)
                return "-" + (Subtract(b, a));
            else
                return "0";
        }
            

        int carry = 0;
        int i = a.Length - 1;
        int j = b.Length - 1;
        
        string result = "";

        while (i >= 0 || j >= 0 || carry != 0)
        {
            int digitA = i >= 0 ? (a[i] - '0') : 0;
            int digitB = j >= 0 ? (b[j] - '0') : 0;
            
            int sum =  digitA + digitB + carry;
            carry = sum / 10;
            
            result = (char)('0' + sum % 10) + result;
            i--;
            j--;
        }
        
        return result == "" ? "0" : result;
    }
    
    static string Subtract(string a, string b)
    {
        bool aSigned;
        bool bSigned;
        a = Validator(a, out aSigned);
        b = Validator(b, out bSigned);
        
        int comparation = Compare(a, b);

        if (aSigned && bSigned)
        {
            if (comparation == 1)
                return "-" + (Subtract(a, b));
            else if (comparation == -1)
                return (Subtract(b, a));
            else
                return "0";
        }
        else if (aSigned)
            return "-" + (Add(a, b));
        else if (bSigned)
            return (Add(a, b));
        
        if (comparation == -1)
            return "-" + (Subtract(b, a));
        
        int carry = 0;
        int i = a.Length - 1;
        int j = b.Length - 1;
        
        string result = "";

        while (i >= 0)
        {
            int digitA = i >= 0 ? (a[i] - '0' - carry) : 0;
            int digitB = j >= 0 ? (b[j] - '0') : 0;

            carry = 0;
            if (digitA < digitB)
            {
                carry = 1;
                digitA += 10;
            }
            int sum =  digitA - digitB;
            
            result = (char)('0' + sum % 10) + result;
            i--;
            j--;
        }

        bool signForValidator;
        return (result == "" ? "0" : Validator(result, out signForValidator));
    }
    
    static string Multiply(string a, string b)
    {
        bool aSigned;
        bool bSigned;
        a = Validator(a, out aSigned);
        b = Validator(b, out bSigned);

        if (a == "0" || b == "0")
            return "0";
        int carry = 0;
        int i = a.Length - 1;
        int j = b.Length - 1;
        int shift = 0;
        
        string   result = "";
        string[] numbers = new string[b.Length];

        while (j >= 0)
        {
            int digitB = j >= 0 ? (b[j] - '0') : 0;

            for (int x = i; x >= 0 || carry != 0; x--)
            {
                int digitA = x >= 0 ? (a[x] - '0') : 0;
                int sum = digitA * digitB + carry;
                
                carry = sum / 10;
                numbers[shift] = (char)('0' + sum % 10) + numbers[shift];
            }
            numbers[shift] = numbers[shift].PadRight(numbers[shift].Length + shift, '0');
            result = Add(numbers[shift], result);
            shift++;
            j--;
        }
        if (aSigned && bSigned)
            return result;
        else if (aSigned || bSigned)
            return "-" + result;
        return result;
    }
}