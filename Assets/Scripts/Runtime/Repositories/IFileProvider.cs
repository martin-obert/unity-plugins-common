using System;
using System.IO;
using Obert.Common.Runtime.Logging;

namespace Obert.Common.Runtime.Repositories
{
    public interface IFileProvider
    {
        string ReadAllText();
        void WriteAllText(string value);
    }

    public sealed class FileProvider : IFileProvider
    {
        private readonly string _filePath;
        private readonly string _filename;

        public FileProvider(string filename, string filePath)
        {
            _filename = filename ?? throw new ArgumentNullException(nameof(filename));
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        private string FullFilePath => Path.Combine(_filePath, _filename);

        public string ReadAllText()
        {
            if (!File.Exists(FullFilePath))
            {
                throw new FileNotFoundException(FullFilePath);
            }

            return File.ReadAllText(FullFilePath);
        }

        public void WriteAllText(string value)
        {
            if(string.IsNullOrWhiteSpace(value)) return;
            
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }

            Logger.Instance.Log($"Data saved at: {FullFilePath}");
            File.WriteAllText(FullFilePath, value);
        }
    }
}