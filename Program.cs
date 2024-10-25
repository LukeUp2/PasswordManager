using System;
using PasswordManager.Services;

class Program
{
    static void Main()
    {
        Console.Write("Defina uma senha mestre para criptografia: ");
        var masterPassword = Console.ReadLine() ?? throw new Exception("Senha mestre nao fornecida");
        var cryptoService = new EncryptService(masterPassword);
        var passwordStorage = new PasswordStorage(cryptoService);

        while (true)
        {
            Console.WriteLine("\nBem vindo ao seu Gerenciador de Senhas!\n");
            Console.WriteLine("1. Salvar uma nova senha");
            Console.WriteLine("2. Recuperar uma senha");
            Console.WriteLine("3. Sair");
            Console.Write("Escolha uma opção: ");
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Nome do serviço (ex: Instagram): ");
                    string? serviceName = Console.ReadLine();

                    Console.Write("Senha para salvar: ");
                    string? servicePassword = Console.ReadLine();

                    if (servicePassword == null || serviceName == null)
                    {
                        break;
                    }
                    var result = passwordStorage.SavePassword(serviceName, servicePassword);
                    Console.WriteLine($"{result}");
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Write("Nome do serviço para recuperar a senha: ");
                    serviceName = Console.ReadLine();
                    if (serviceName == null)
                    {
                        break;
                    }
                    string? retrievedPassword = passwordStorage.GetPassword(serviceName);


                    if (retrievedPassword != null)
                        Console.WriteLine($"Senha recuperada para {serviceName}: {retrievedPassword}");
                    else
                        Console.WriteLine("Serviço não encontrado.");
                    Console.ReadKey();
                    break;

                case "3":
                    Console.WriteLine("Saindo do gerenciador de senhas...");
                    return;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }
}