using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Repository;

namespace protecta.laft.api.Services {
    public class MenuConfigService {

        MenuConfigRepository menuConfigRepository;

        public MenuConfigService () {
            this.menuConfigRepository = new MenuConfigRepository ();
        }

        public List<MenuListResponseDTO> GetOptionList (MenuListParametersDTO param) {
            try {
                  return this.menuConfigRepository.GetOptionList (param);

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<MenuListResponseDTO> ();
            }
        }

            public List<MenuListResponseDTO> GetSubOptionList (SubmenuListParametersDTO param) {
            try {
                  return this.menuConfigRepository.GetSubOptionList (param);

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<MenuListResponseDTO> ();
            }
        }

    }

}