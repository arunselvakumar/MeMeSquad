﻿namespace Jericho.Services
{
    using System;
    using System.Threading.Tasks;

    using Jericho.Models.v1.Entities;
    using Jericho.Services.Interfaces;

    using Microsoft.Azure.Documents.Client;

    public class UserService : IUserService
    {
        #region Constructor

        public UserService()
        {
            this.InitializeDbConnection();
        }

        #endregion

        #region Public Methods

        public async Task CreateUserAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public bool LoginUserAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public bool IsUserNameExists(string username)
        {
            throw new NotImplementedException();
        }

        public bool IsEmailAddressExists(string email)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private bool IsUserAuthorized(UserEntity user, UserEntity document)
        {
            var isUserNameAndPasswordMatches = user.UserName.Equals(document.UserName) && user.Password.Equals(document.Password);
            var isEmailAddressAndPasswordMathes = user.UserName.Equals(document.EMail) && user.Password.Equals(document.Password);

            return isUserNameAndPasswordMatches || isEmailAddressAndPasswordMathes;
        }

        private void InitializeDbConnection()
        {
            // TechEd Europe https://www.youtube.com/watch?v=-5HKtWhqWC8
            // Enabled TCP & Direct Connection for increased throughput.
            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };

            // this.documentClient = new DocumentClient(new Uri(this.documentDbConfig.ConnectionString), this.documentDbConfig.PrimaryKey, connectionPolicy);
        }

        #endregion
    }
}