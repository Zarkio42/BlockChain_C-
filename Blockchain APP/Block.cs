using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain_APP
{
    public interface IBlock
    {
        public byte[] Data { get; }
        public byte[] Hash { get; set; }
        public int Nonce { get; set; }
        public byte[] PrevHash { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    public class Block : IBlock
    {
        public Block(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Nonce = 0;
            PrevHash = new byte[] { 0x00 };
            TimeStamp = DateTime.Now;
        }
        public byte[] Data { get; }
        public byte[] Hash { get; set; }
        public int Nonce { get; set; }
        public byte[] PrevHash { get; set; }
        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return $"{BitConverter.ToString(Hash).Replace("-", "")} : \n {BitConverter.ToString(PrevHash).Replace("-", "")} \n {Nonce} {TimeStamp}";
        }
    }
    public static class BlockExtension
    {
        public static byte[] GenerateHash(this IBlock block)
        {
            using (SHA256 sha = SHA256.Create())
            using (MemoryStream st = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(st))
            {
                bw.Write(block.Data);
                bw.Write(block.Nonce);
                bw.Write(block.PrevHash);
                bw.Write(Encoding.UTF8.GetBytes(block.TimeStamp.ToString()));

                return sha.ComputeHash(st.ToArray());
            }
        }

        public static byte[] MineHash(this IBlock block, byte[] difficulty)
        {
            if (difficulty == null) throw new ArgumentNullException(nameof(difficulty));
            byte[] hash = new byte[0];

            while (!hash.Take(2).SequenceEqual(difficulty))
            {
                block.Nonce++;
                hash = block.GenerateHash();
            }
            return hash;
        }

        public static bool IsValid(this IBlock block)
        {
            var bk = block.GenerateHash();
            return block.Hash.SequenceEqual(bk);
        }

        public static bool IsPrevBlock(this IBlock block, IBlock prevBlock)
        {
            if (prevBlock == null) throw new ArgumentNullException(nameof(prevBlock));
            return prevBlock.IsValid() && block.PrevHash.SequenceEqual(prevBlock.Hash);
        }

        public static bool IsValid(this IEnumerable<IBlock> items)
        {
            var enums = items.ToList();
            return enums.Zip(enums.Skip(1), Tuple.Create).All(block => block.Item2.IsValid() &&
            block.Item2.IsPrevBlock(block.Item2));
        }
    }

    public class BlockChain : IEnumerable<IBlock>
    {
        private List<IBlock> _items = new List<IBlock>();

        public byte[] Diffiulty { get;}

        public BlockChain(byte[] diffiulty, IBlock genesis)
        {
            Diffiulty = diffiulty;
            genesis.Hash = genesis.MineHash(diffiulty);
            Items.Add(genesis);
        }

        public void Add(IBlock item)
        {
            if(_items.LastOrDefault() != null)
            {
                item.PrevHash = _items.LastOrDefault().Hash;
            }
            item.Hash = item.MineHash(Diffiulty);
            Items.Add(item);

        }

        public List<IBlock> Items
        {
            get => _items;
            set => _items = value;
        }

        public int Count => _items.Count;

        public IBlock this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        public IEnumerator<IBlock> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
