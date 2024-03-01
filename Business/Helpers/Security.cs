using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Business
{
    public class Security
    {
        private static RandomNumberGenerator rng = RandomNumberGenerator.Create();

        private static bool IsPrime(BigInteger num, int k)
        {
            if (num <= 1)
                return false;
            if (num <= 3)
                return true;
            if (num % 2 == 0 || num % 3 == 0)
                return false;

            BigInteger d = num - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = RandomBigInteger(2, num - 2);
                BigInteger x = BigInteger.ModPow(a, d, num);
                if (x == 1 || x == num - 1)
                    continue;
                
                for (int j = 1; j < s; j++)
                {
                    x = BigInteger.ModPow(x, 2, num);
                    if (x == 1)
                        return false;
                    if (x == num - 1)
                        break;
                }
                
                if (x != num - 1)
                    return false;
            }

            return true;
        }
        
        private static BigInteger RandomBigInteger(BigInteger min, BigInteger max)
        {
            byte[] bytes = max.ToByteArray();
            BigInteger result;
            do
            {
                rng.GetBytes(bytes);
                result = new BigInteger(bytes);
            } while (result < min || result >= max);
            return result;
        }

        private static BigInteger GeneratePrime(int bits, int k)
        {
            BigInteger num;
            do
            {
                num = RandomBigInteger(BigInteger.Pow(2, bits - 1), BigInteger.Pow(2, bits));
            } while (!IsPrime(num, k));
            return num;
        }

        private static BigInteger GenerateCoprime(BigInteger phi)
        {
            BigInteger e;
            do
            {
                e = RandomBigInteger(2, phi - 1);
            } while (BigInteger.GreatestCommonDivisor(e, phi) != 1);

            return e;
        }

        private static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m;
            BigInteger y = 0;
            BigInteger x = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                BigInteger q = a / m;
                BigInteger t = m;

                m = a % m;
                a = t;
                t = y;

                y = x - q * y;
                x = t;
            }

            if (x < 0)
                x += m0;

            return x;
        }

        public static byte[] returnBytes(string input)
        {
            int numBytes = input.Length / 2;
            byte[] encryptedBytes = new byte[numBytes];
            for (int i = 0; i < numBytes; i++)
            {
                string hex = input.Substring(i * 2, 2);
                encryptedBytes[i] = Convert.ToByte(hex, 16);
            }

            return encryptedBytes;
        }

        public static void GenerateKeys(out BigInteger publicKey, out BigInteger privateKey, out BigInteger n)
        {
            int min_n = 512;
            int max_n = 1024;
            BigInteger p = GeneratePrime(min_n, max_n);
            BigInteger q;
            do
            {
                q = GeneratePrime(min_n, max_n);
            } while (q == p);

            n = BigInteger.Multiply(p, q);
            BigInteger phi = BigInteger.Multiply(p - 1, q - 1);
            publicKey = GenerateCoprime(phi);
            privateKey = ModInverse(publicKey, phi);
        }

        public static byte[] EncryptText(string input, BigInteger publicKey, BigInteger n)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            BigInteger message = new BigInteger(bytes);
            BigInteger encryptedMessage = BigInteger.ModPow(message, publicKey, n);
            return encryptedMessage.ToByteArray();
        }
        
        public static string DecryptText(byte[] encryptedBytes, BigInteger privateKey, BigInteger n)
        {
            BigInteger encryptedMessage = new BigInteger(encryptedBytes);
            BigInteger decryptedMessage = BigInteger.ModPow(encryptedMessage, privateKey, n);
            byte[] decryptedBytes = decryptedMessage.ToByteArray();
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
