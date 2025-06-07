using System.Security.Cryptography;
using System.Text;

namespace AppClinica.Services
{
    public interface IAesEncryptionService
    {
        string Encriptar(string textoPlano);
        string Desencriptar(string textoCifrado);
    }

    public class AesEncryptionService : IAesEncryptionService
    {
        private readonly string _clave = "AES_SUPER_SECRETA_1234";

        public string Encriptar(string textoPlano)
        {
            if (string.IsNullOrWhiteSpace(textoPlano))
                return string.Empty;
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_clave.PadRight(32).Substring(0, 32));
            aes.IV = Encoding.UTF8.GetBytes(_clave.PadRight(16).Substring(0, 16));

            var encryptor = aes.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(textoPlano);
            var resultado = encryptor.TransformFinalBlock(input, 0, input.Length);

            return Convert.ToBase64String(resultado);
        }

        public string Desencriptar(string textoCifrado)
        {
            if (string.IsNullOrWhiteSpace(textoCifrado))
                return string.Empty;

            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(_clave.PadRight(32).Substring(0, 32));
                aes.IV = Encoding.UTF8.GetBytes(_clave.PadRight(16).Substring(0, 16));

                var decryptor = aes.CreateDecryptor();
                var input = Convert.FromBase64String(textoCifrado);
                var resultado = decryptor.TransformFinalBlock(input, 0, input.Length);

                return Encoding.UTF8.GetString(resultado);
            }
            catch
            {
                return textoCifrado; // Retorna el texto original si no se puede desencriptar
            }
        }

    }
}
