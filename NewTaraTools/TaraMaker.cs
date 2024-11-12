using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TaraMaker
{
    public class FileEntry
    {
        public string RelativeName { get; }
        public FileInfo File { get; }

        public FileEntry(string relativeName, FileInfo file)
        {
            RelativeName = relativeName;
            File = file;
        }
    }

    public void CreateTaraFile(string sourceDirPath, string outputFilePath)
    {
        DirectoryInfo sourceDir = new DirectoryInfo(sourceDirPath);
        List<FileEntry> fileEntries = new List<FileEntry>();
        CollectFiles(sourceDir, null, fileEntries);

        using (BinaryWriter writer = new BinaryWriter(new FileStream(outputFilePath, FileMode.Create)))
        {
            // Write the number of files in BigEndian
            Span<byte> buffer = stackalloc byte[4];
            BinaryPrimitives.WriteInt32BigEndian(buffer, fileEntries.Count);
            writer.Write(buffer);

            // Write headers
            foreach (var entry in fileEntries)
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(entry.RelativeName);

                // Write two additional placeholder bytes (could be specific metadata or markers)
                writer.Write((byte)0);
                writer.Write((byte)0);

                // Write file name and null-terminate it
                writer.Write(nameBytes);
                writer.Write((byte)0); // Null terminator

                // Write file size as 3 bytes in BigEndian with a leading zero byte
                Span<byte> sizeBuffer = stackalloc byte[4];
                BinaryPrimitives.WriteInt32BigEndian(sizeBuffer, (int)entry.File.Length);
                writer.Write(sizeBuffer.Slice(1, 3)); // Only write the last 3 bytes

                Console.WriteLine($"Writing header: {entry.RelativeName}, {entry.File.Length}");
            }

            // Write file contents
            foreach (var entry in fileEntries)
            {
                byte[] bytes = File.ReadAllBytes(entry.File.FullName);
                writer.Write(bytes);
                Console.WriteLine($"File {entry.RelativeName} data has been written");
            }
        }
    }

    private void CollectFiles(DirectoryInfo dir, string baseName, List<FileEntry> collector)
    {
        foreach (var file in dir.GetFiles())
        {
            string relativeName = baseName == null ? file.Name : $"{baseName}/{file.Name}";
            collector.Add(new FileEntry(relativeName, file));
        }

        foreach (var subDir in dir.GetDirectories())
        {
            string newBaseName = baseName == null ? subDir.Name : $"{baseName}/{subDir.Name}";
            CollectFiles(subDir, newBaseName, collector);
        }
    }
}
