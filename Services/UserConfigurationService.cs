using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Repository;

namespace protecta.laft.api.Services {
    public class UserConfigurationService {

        UserConfigurationRepository userConfigRepository;

        public UserConfigurationService () {
            this.userConfigRepository = new UserConfigurationRepository ();
        }

        public List<UserListResponseDTO> GetAllUsers () {
            try {
                return Utils.Parse.dtos (this.userConfigRepository.GetAllUsers ());

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<UserListResponseDTO> ();
            }
        }

        public List<UserStatusListResponseDTO> GetAllUserStatus () {
            try {
                return Utils.Parse.dtos (this.userConfigRepository.GetAllUserStatus ());

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<UserStatusListResponseDTO> ();
            }
        }

        public UserListResponseDTO GetUserData (int userId) {
            return this.userConfigRepository.GetUserData (userId);
        }

        
        public List<ProfileResponseDTO> GetProfiles () {
            try {
                return this.userConfigRepository.GetProfiles ();

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<ProfileResponseDTO> ();
            }
        }
         public List<CargoResponseDTO> GetLisCargo (int profileId) {
            try {
                return this.userConfigRepository.GetLisCargo (profileId);

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<CargoResponseDTO> ();
            }
        }


        public UpdateUserResponseDTO UpdateUser (int userId, string userName, string userFullName, string pass, int userUpd,
            string endDatepass, int userRolName, string systemId, string userEmail, int cargoId, string modifico, int state) {
            return this.userConfigRepository.UpdateUser (userId, userName, userFullName, pass, userUpd, endDatepass, userRolName, systemId, userEmail, cargoId, modifico, state);
        }

        public CreateUserResponseDTO CreateUser (string userName, string userFullName, string pass, string userReg, int userUpd,
            string startDatepass, string endDatepass, int userRolName, string systemId, string userEmail, int cargoId) {
            return this.userConfigRepository.CreateUser (userName, userFullName, pass, userReg, userUpd, startDatepass, endDatepass, userRolName, systemId, userEmail, cargoId);
               
        }

         public Dictionary<string, dynamic> historyUser(dynamic param)
        {
            Console.WriteLine("el mensaje : "+param);
            return this.userConfigRepository.GetHistorialUser(param);
        }

    }

}