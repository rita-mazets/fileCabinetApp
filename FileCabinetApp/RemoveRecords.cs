using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class RemoveRecords
    {
        public int Count { get; set; }

        private readonly List<int> ids = new ();

        private string fileName = "removeId.db";



        public RemoveRecords()
        {
            this.ReadFile();
        }


        private void WriteId(int value)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(this.fileName, FileMode.Append)))
            {
                writer.Write(value);
            }
        }

        private void ReadFile()
        {
            using (BinaryReader reader = new BinaryReader(File.Open(this.fileName, FileMode.OpenOrCreate)))
            {
                while (reader.PeekChar() > -1)
                {
                    int value = reader.ReadInt32();
                    this.ids.Add(value);
                }
            }

            if (this.ids.Count == 0)
            {
                this.WriteId(0);
                this.Count = 0;
            }
            else
            {
                this.Count = this.ids[0];
                this.ids.RemoveAt(0);
            }
        }

        public void Add(int id)
        {
            this.Count++;

            using (BinaryWriter writer = new BinaryWriter(File.Open(this.fileName, FileMode.Open)))
            {
                writer.Write(this.Count);
            }

            this.WriteId(id);
        }

        public bool Contains(int id)
        {
            if (this.ids.Contains(id))
            {
                return true;
            }

            return false;

        }
    }
}
