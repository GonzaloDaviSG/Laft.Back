using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.Models;
using protecta.laft.api.DTO;
using protecta.laft.api.Repository;
using System.Text;

namespace protecta.laft.api.Services
{
    public class LoginService
    {

        LoginRepository repository;
        public LoginService()
        {
            this.repository = new LoginRepository();
        }

        public userResponseDTO ValExistUser(string username, string password)
        {
            return this.repository.ValExistUser(username.ToUpper(), password);
        }


        public string GenerarCodigo()
        {
            int longitud = 7;
            const string alfabeto = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder token = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int indice = rnd.Next(alfabeto.Length);
                token.Append(alfabeto[indice]);
            }
            return token.ToString();
        }

    }
}