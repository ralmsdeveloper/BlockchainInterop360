using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDayBlockChain
{
    public class Block
    {
        public Block(string data)
        {
            Nonce = 0;
            Data = Encoding.ASCII.GetBytes(data);
            TimeStamp = DateTime.Now;
            HashPrevious = new byte[] { 0x0 };
        }
        public int Nonce { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] Data { get; set; }
        public byte[] Hash { get; set; }
        public byte[] HashPrevious { get; set; }
    }
}
