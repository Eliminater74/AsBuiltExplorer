using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsBuiltExplorer.Utilities
{
    public static class CRC
    {
        // CRC-8 SAE J1850
        // Polynomial: x^8 + x^4 + x^3 + x^2 + 1 (0x1D)
        // Init: 0xFF
        // XorOut: 0xFF
        public static byte CalculateCRC8(byte[] data)
        {
            byte crc = 0xFF;
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (byte)((crc << 1) ^ 0x1D);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }
            return (byte)(crc ^ 0xFF);
        }

        // CRC-16 CCITT-FALSE
        // Polynomial: x^16 + x^12 + x^5 + 1 (0x1021)
        // Init: 0xFFFF
        // XorOut: 0x0000
        public static ushort CalculateCRC16(byte[] data)
        {
            ushort crc = 0xFFFF;
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }
            return crc;
        }

        // Standard Summation (Legacy Ford)
        // Simple 2's complement of sum (roughly), usually handled by legacy algo but good to have here.
        // Actually modAsBuilt.AsBuilt_CalculateChecksum uses a specific method involving Module ID.
        // This helper is for pure data summation if needed.
        public static byte CalculateSumDefault(byte[] data)
        {
            int sum = 0;
            foreach (byte b in data) sum += b;
            return (byte)(sum & 0xFF);
        }
    }
}
