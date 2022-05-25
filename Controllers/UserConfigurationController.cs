using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Services;
using protecta.laft.api.Services.Interfaces;
using protecta.laft.api.Utils;

namespace protecta.laft.api.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class UserConfigurationController : ControllerBase {
        private SenialService service;
        private EmailService emailService;
        private UserConfigurationService userConfigService;

        public UserConfigurationController () {
            this.service = new SenialService();
            this.emailService = new EmailService();
            this.userConfigService = new UserConfigurationService ();

        }

        //Controlador para obtener la lista de usuarios activos
        [HttpGet ("userList")]
        public ActionResult<List<UserListResponseDTO>> GetAllUsers () {
            return this.userConfigService.GetAllUsers ();
        }

        //Controlador para obtener los estados de un usuario
        [HttpGet ("userStatusList")]
        public ActionResult<List<UserStatusListResponseDTO>> GetAllUserStatus () {
            return this.userConfigService.GetAllUserStatus ();
        }

        //Controlador para actualizar usuario
        [HttpPost ("userData")]
        public ActionResult<UserListResponseDTO> GetUserData (UserDataParametersDTO inputParams) {
            if (ModelState.IsValid) {
                var userData = this.userConfigService.GetUserData (inputParams.userId);
                return userData;
            }
            return BadRequest (ModelState);
        }

        [HttpGet ("getProfile")]
        public ActionResult GetProfiles () {
            return Ok (this.userConfigService.GetProfiles ());
        }
        
        [HttpPost ("getLisCargo")]
        public ActionResult<CargoResponseDTO> GetLisCargo (CargoParametersDTO inputParams) {
            return Ok (this.userConfigService.GetLisCargo (inputParams.profileId));
        }

        //Controlador para actualizar usuario
        [HttpPost ("updateUser")]
        public ActionResult<UpdateUserResponseDTO> UpdateUser (UpdateUserParametersDTO inputParams) {
            if (ModelState.IsValid) {

                var userUpdated = this.userConfigService.UpdateUser (inputParams.userId, inputParams.userName, inputParams.userFullName,
                    inputParams.pass, inputParams.userUpd, inputParams.endDatepass, inputParams.userRolId, inputParams.systemId, inputParams.userEmail, inputParams.cargoId, inputParams.modifico , inputParams.state);
                if(inputParams.valor == 1)
                {
                    //Servicio para enviar el correo
                    var emailSended = this.emailService.SenderEmailCompRequest2(inputParams);
                }
                

                return userUpdated;

                
            }
            return BadRequest (ModelState);
        }

        //Controlador para crear usuario
        [HttpPost ("createUser")]
        public ActionResult<CreateUserResponseDTO> CreateUser (CreateUserParametersDTO inputParams) {
            if (ModelState.IsValid) {

                var userUpdated = this.userConfigService.CreateUser (inputParams.userName, inputParams.userFullName,inputParams.pass, inputParams.userReg, inputParams.userUpd, inputParams.startDatepass, inputParams.endDatepass, inputParams.userRolId, inputParams.systemId, inputParams.userEmail, inputParams.cargoId);

                //Servicio para enviar el correo
                var emailSended = this.emailService.SenderEmailCompRequest2(inputParams);
                return userUpdated;
            }
            return BadRequest (ModelState);
        }
            //Controlador para EL HISTORIAL usuario
        [HttpPost("HistorialUser")]
         public IActionResult historyUser(dynamic inputParams)
         {
             return Ok (this.userConfigService.historyUser(inputParams));
             /*if (ModelState.IsValid)
             {

                 var userHData = this.userConfigService.historyUser(inputParams.pUser);
                 return userHData;
             }
             return BadRequest(ModelState);*/
         }
    }
}