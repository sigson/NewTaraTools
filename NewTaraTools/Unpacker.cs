using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Unpacker
{
    public class ListEntry
    {
        public string FileName { get; set; }
        public int FileSize { get; set; }

        public ListEntry(string fileName, int fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }

    public static void Unpack(string filePath, string outputDir)
    {
        if(!Directory.Exists(outputDir)) { Directory.CreateDirectory(outputDir); }
        outputDir = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(filePath));
        if (!Directory.Exists(Path.Combine(outputDir))) { Directory.CreateDirectory(outputDir); }
        //Directory.CreateDirectory(outputDir)
        List<ListEntry> files = new List<ListEntry>();

        using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
        {
            Span<byte> buffer = stackalloc byte[4];
            Span<byte> buffer3 = stackalloc byte[3];

            // Reading the number of files in BigEndian
            reader.Read(buffer);
            int numFiles = BinaryPrimitives.ReadInt32BigEndian(buffer);
            
            // Reading headers
            for (int i = 0; i < numFiles; i++)
            {
                // Read a null-terminated UTF-8 encoded string for the file name
                reader.ReadByte(); reader.ReadByte();
                string fileName = ReadNullTerminatedString(reader);
                //reader.ReadByte(); //reader.ReadByte();
                // Read file size in BigEndian
                reader.Read(buffer3);
                List<byte> byteList = new List<byte>();
                byteList.Add(0);
                byteList.Add(buffer3[0]);
                byteList.Add(buffer3[1]);
                byteList.Add(buffer3[2]);
                int fileSize = BinaryPrimitives.ReadInt32BigEndian(byteList.ToArray());

                files.Add(new ListEntry(fileName, fileSize));
            }

            Console.WriteLine($"Unpacking {numFiles} files to {Path.GetFullPath(outputDir)}");

            // Reading and writing file contents
            foreach (var entry in files)
            {
                Console.WriteLine($"{entry.FileName} [{entry.FileSize}]");
                byte[] fileBytes = reader.ReadBytes(entry.FileSize);

                string outputFilePath = Path.Combine(outputDir, entry.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath) ?? string.Empty);

                using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                {
                    outputFileStream.Write(fileBytes);
                }
            }
        }
    }

    // Метод для чтения строки до символа `\0`
    private static string ReadNullTerminatedString(BinaryReader reader)
    {
        List<byte> byteList = new List<byte>();

        while (true)
        {
            byte b = reader.ReadByte();
            if (b == 0) break; // Если достигли `\0`, прекращаем чтение
            byteList.Add(b);
        }

        return Encoding.UTF8.GetString(byteList.ToArray());
    }
}
