namespace big_int;

class Program
{
    static void Main(string[] args)
    {
        int a = int.MaxValue;
        Console.WriteLine(unchecked(a + 1)); // -2147483648
        long b = long.MaxValue;
        Console.WriteLine(unchecked(b + 1)); // -9223372036854775808

        Console.WriteLine(Add("2147483647", "1"));
        Console.WriteLine(Subtract("10000", "1"));        // 9999
         
    }

    static string Validator(string str)
    {
        if (string.IsNullOrEmpty(str))
            return "0";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] < '0' || str[i] > '9')
                return "0";
        }

        int index = 0;
        while (index < str.Length && str[index] == '0')
            index++;
        str = str.Substring(index);
        
        return str;
    }
    
    
    static string Add(string a, string b)
    {
        a = Validator(a);
        b = Validator(b);

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
        a = Validator(a);
        b = Validator(b);

        if (a == b)
            return "0";
        
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
        return (result == "" ? "0" : Validator(result));
    }
}