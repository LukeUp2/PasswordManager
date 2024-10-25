using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    using PasswordManager.Services;

    public class PasswordStorage
    {
        private readonly EncryptService _encryptService;
        private readonly string _filePath;

        public PasswordStorage(EncryptService encryptService, string filePath = "./passwords.txt")
        {
            _encryptService = encryptService;
            _filePath = filePath;
        }

        public string SavePassword(string name, string password)
        {
            string encryptedPassword = _encryptService.Encrypt(password);
            bool alreadyExists = CheckIfNameAlreadyExists(name);
            if (alreadyExists)
            {
                return "Nome já existente, por favor escolha outro";
            }
            File.AppendAllText(_filePath, $"{name}:{encryptedPassword}\n");
            return "Senha salva com sucesso!";
        }

        public string? GetPassword(string name)
        {
            if (!File.Exists(_filePath))
                return null;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts[0] == name)
                {
                    return _encryptService.Decrypt(parts[1]);
                }
            }
            return null;
        }

        public bool CheckIfNameAlreadyExists(string name)
        {
            if (!File.Exists(_filePath))
                return false;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts[0] == name)
                {
                    return true;
                }
            }

            return false;

        }
    }


}