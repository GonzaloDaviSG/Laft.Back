using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Repository;

namespace protecta.laft.api.Services {
    public class FormsService {

        FormsRepository formsRepository;

        public FormsService () {
            this.formsRepository = new FormsRepository ();        }

        public List<FormsResponseDTO> GetFormsList () {
            try {
                return Utils.Parse.dtos (this.formsRepository.GetFormsList ());

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<FormsResponseDTO> ();
            }
        }

    }

}