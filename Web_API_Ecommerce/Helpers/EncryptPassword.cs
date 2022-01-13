using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.Helpers
{
    public class EncryptPassword
    {
        public static void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; //Aquí envía la llave
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //Envíar el password encriptado.
            }

        }
    }
}
