﻿using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

    public interface IAccountRepository
{
   
        Task<User> GetUser(string  email);
        bool CheckPassWord(string password, string passwordModel );
   
    }

