using System.Collections.Generic;
using System;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository.Interfaces {
    public interface IMenuConfigRepository {
       
      List<MenuListResponseDTO> GetOptionList (MenuListParametersDTO param);
       List<MenuListResponseDTO> GetSubOptionList (SubmenuListParametersDTO param);
    }
}