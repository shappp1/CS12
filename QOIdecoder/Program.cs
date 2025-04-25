using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOIdecoder
{
    class Program
    {
        static uint swap_endian(uint x)
        {
            return ((x & 0x000000FF) << 24) +
                   ((x & 0x0000FF00) << 8) +
                   ((x & 0x00FF0000) >> 8) +
                   ((x & 0xFF000000) >> 24);
        }

        static void Main(string[] args)
        {
            Console.Write("QOI file path: ");
            string qoi_path = Console.ReadLine();
            if (!File.Exists(qoi_path))
            {
                Console.WriteLine("Invalid path!");
                Environment.Exit(1);
            }

            BinaryReader qoi_file = new BinaryReader(File.OpenRead(qoi_path));

            // Print header info:
            byte[] raw_qoi_header = qoi_file.ReadBytes(14);
            if (Encoding.ASCII.GetString(raw_qoi_header.Take(4).ToArray()) != "qoif")
            {
                Console.WriteLine("Invalid QOI file");
                Environment.Exit(2);
            }

            uint width = swap_endian(BitConverter.ToUInt32(raw_qoi_header, 4));
            uint height = swap_endian(BitConverter.ToUInt32(raw_qoi_header, 8));
            byte channels = raw_qoi_header[13];
            byte colorspace = raw_qoi_header[14];

            Console.WriteLine($"QOI Image:\n{width}\n{height}\n{channels}\n{colorspace}\n");
        }
    }
}
