using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InteropDayBlockChain
{
    public class Program
    {
        static void Main(string[] args)
        {
            var listBlock = new List<Block>();

            // Esse bloco não é obrigatório
            var block = new Block("Genesis");
            block.Hash = block.MineHash();
            listBlock.Add(block);

            // Inserir 10 transações
            for (int i = 0; i < 10; i++)
            {
                block = new Block($"Bloco Teste {i}");
                block.HashPrevious = listBlock.LastOrDefault().Hash;
                block.Hash = block.MineHash();

                listBlock.Add(block);
            }
            
            // Exibir blocos
            for (int i = 1; i < 10; i++)
            {
                // Validar Bloco
                var isValid = listBlock[i].ValidarHash() && listBlock[i].ValidarHashAnterior(listBlock[i - 1]);

                Console.WriteLine($"Data.........: {listBlock[i].TimeStamp}");
                Console.WriteLine($"Informação...: {Encoding.ASCII.GetString(listBlock[i].Data)}");
                Console.WriteLine($"Hash Anterior: {BitConverter.ToString(listBlock[i].HashPrevious).ToLower().Replace("-","")}");
                Console.WriteLine($"Hash Bloco...: {BitConverter.ToString(listBlock[i].Hash).ToLower().Replace("-", "")}");
                Console.WriteLine($"Bloco Válido.: {isValid}");
                Console.WriteLine($"-----------------------------------------------------------");
            }

            Console.ReadKey();
        }
    }

    public static class Extensions
    {
        public static byte[] ComputeHash(this Block block)
        {
            using(var sha256 = SHA256.Create())
            {
                using(var stream = new MemoryStream())
                {
                    using(var bin = new BinaryWriter(stream))
                    {
                        bin.Write(block.Data);
                        bin.Write(block.HashPrevious);
                        bin.Write(block.Nonce);
                        bin.Write(BitConverter.GetBytes(block.TimeStamp.ToBinary()));

                        return sha256.ComputeHash(stream.ToArray());
                    }
                }
            }
        }

        public static byte[] MineHash(this Block block)
        {
            var hash = new byte[0]; 

            // Gerar hash com um grau de dificuldade
            while (!hash.Take(2).SequenceEqual(new byte[] { 0x0, 0x0 }) 
                &&  block.Nonce <= int.MaxValue)
            {
                block.Nonce++;
                hash = block.ComputeHash();
            }

            return hash;
        }

        public static bool ValidarHash(this Block block)
        {
            var bloco = block.ComputeHash();
            return block.Hash.SequenceEqual(bloco);
        }

        public static bool ValidarHashAnterior(this Block block, Block blocoAnterior)
        {
            var anterior = blocoAnterior.ComputeHash();
            return blocoAnterior.ValidarHash() && block.HashPrevious.SequenceEqual(anterior);
        }

    }
}
