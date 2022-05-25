using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Utils {
    public class Parse {

        #region Lista de Formularios

        public static List<FormsResponseDTO> dtos (List<Forms> models) {
            var dtos = new List<FormsResponseDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }
            return dtos;
        }
        public static FormsResponseDTO dto (Forms model) {
            var dto = new FormsResponseDTO ();

            if (model != null) {

                dto.sSkey = (model.sSkey == null) ? string.Empty : model.sSkey;
                dto.sDateResponse = (model.dDateResponse == null) ? string.Empty : model.dDateResponse.Value.ToShortDateString ();
                dto.sPersonInCharge = (model.sPersonCharge == null) ? string.Empty : model.sPersonCharge;
                dto.sPeriod = (model.sPeriod == null) ? string.Empty : model.sPeriod;
                dto.sSignalType = (model.sSignalType == null) ? string.Empty : model.sSignalType;
                dto.sSummary = (model.sSummary == null) ? string.Empty : model.sSummary;
                dto.nStatus = model.nStatus;

            }
            return dto;
        }

        #endregion

        #region Lista del estado de los usuarios

        public static List<UserStatusListResponseDTO> dtos (List<UserStatus> models) {
            var dtos = new List<UserStatusListResponseDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }
            return dtos;
        }

        public static UserStatusListResponseDTO dto (UserStatus model) {
            var dto = new UserStatusListResponseDTO ();

            if (model != null) {

                dto.idState = (model.sUserStatus == null) ? string.Empty : model.sUserStatus;
                dto.stateName = (model.sDescriptionStatus == null) ? string.Empty : model.sDescriptionStatus;
                dto.dCompDate = (model.dCompDate == null) ? string.Empty : model.dCompDate.Value.ToShortDateString ();
                dto.sUserCode = (model.sUserCode == null) ? string.Empty : model.sUserCode;

            }
            return dto;
        }

        #endregion

        #region Menu de configuración de opciones

        public static List<MenuListResponseDTO> dtos (List<Menu> models) {
            var dtos = new List<MenuListResponseDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }
            return dtos;
        }

        public static MenuListResponseDTO dto (Menu model) {
            var dto = new MenuListResponseDTO ();

            if (model != null) {

                dto.sName = (model.sName == null) ? string.Empty : model.sName;
                dto.sDescription = (model.sDescription == null) ? string.Empty : model.sDescription;
                dto.sHtml = (model.sHtml == null) ? string.Empty : model.sHtml;
                dto.nResourceType = model.nResourceType;
                dto.nOrder = model.nOrder;
                dto.sActive = (model.sActive == null) ? string.Empty : model.sActive;
                dto.sRouterLink = (model.sRouterLink == null) ? string.Empty : model.sRouterLink;
                dto.nFatherId = model.nFatherId;
            }

            return dto;
        }

        #endregion

        #region Configuración de usuario

        public static List<UserListResponseDTO> dtos (List<User> models) {
            var dtos = new List<UserListResponseDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }
            return dtos;
        }

        public static UserListResponseDTO dto (User model) {
            var dto = new UserListResponseDTO ();
            Console.WriteLine("el model", model);
            if (model != null) {

                dto.userId = model.nUserId;
                dto.userName = (model.sUser == null) ? string.Empty : model.sUser;
                dto.pass = (model.sPass == null) ? string.Empty : model.sPass;
                dto.userFullName = (model.sFullName == null) ? string.Empty : model.sFullName;
                dto.userState = (model.sState == null) ? string.Empty : model.sState;
                dto.accessAttempts = model.sAccessAttempts;
                dto.startDatepass = (model.dPassStartDate == null) ? string.Empty : model.dPassStartDate.Value.ToShortDateString ();
                dto.endDatepass = (model.dPassEndDate == null) ? string.Empty : model.dPassEndDate.Value.ToShortDateString ();
                dto.userReg = model.nUserReg;
                dto.dateReg = (model.dRegDate == null) ? string.Empty : model.dRegDate.Value.ToShortDateString ();
                dto.userUpd = model.nUserUpd;
                dto.dateUpd = (model.dUpdDate == null) ? string.Empty : model.dUpdDate.Value.ToShortDateString ();
                dto.userRolId = model.nRolId;
                dto.systemId = (model.sSystemId == null) ? string.Empty : model.sSystemId;
                dto.userEmail = (model.sUserEmail == null) ? string.Empty : model.sUserEmail;
                dto.cargo = (model.sCargo == null) ? string.Empty : model.sCargo;
            }

            return dto;
        }

        #endregion

        #region Reportes Sbs


        // public static List<SbsReportGenListResponseDTO> dtos (List<SbsReport> models) {
        //     var dtos = new List<SbsReportGenListResponseDTO> ();

        //     if (models != null) {
        //         foreach (var model in models) {
        //             dtos.Add (dto (model));
        //         }
        //     }
        //     return dtos;
        // }

        // public static SbsReportGenListResponseDTO dto (SbsReport model) {
        //     var dto = new SbsReportGenListResponseDTO ();

        //     if (model != null) {

        //         dto.id = model.sId;
        //         dto.startDate = (model.dStartDate == null) ? string.Empty : model.dStartDate.Value.ToShortDateString ();
        //         dto.endDate = (model.dEndDate == null) ? string.Empty : model.dEndDate.Value.ToShortDateString ();
        //         dto.exchangeRate = model.nExchangeRate;
        //         dto.operType = model.sOperType;
        //         dto.amount = model.dAmount;
        //         dto.userId = (model.nUserId == null) ? string.Empty : model.nUserId;
        //         dto.statusReport = model.nStatusProc;
        //         dto.creationDate = (model.dCompDate == null) ? string.Empty : model.dCompDate.Value.ToShortDateString ();
        //         dto.startProcessDate = (model.dStartDateProc == null) ? string.Empty : model.dStartDateProc.Value.ToShortDateString ();
        //         dto.endProcessDate = (model.dEndDateProc == null) ? string.Empty : model.dEndDateProc.Value.ToShortDateString ();
        //         dto.nameReport = (model.sNameReport == null) ? string.Empty : model.sNameReport;
        //         dto.message = model.sMessage;
        //         dto.fileType = model.sfileType;
        //     }

        //     return dto;
        // }

        #endregion

        #region Transacciones
        public static Carga model (CargaDTO dto) {
            var model = new Carga ();

            if (dto != null) {
                model.nId = dto.id;
                model.dRegistro = Convert.ToDateTime (dto.fechaRegistro);
                model.sUsuario = dto.usuario;
                model.nEstado = (dto.activo ? Estado.Activo : Estado.Inactivo);
            }

            return model;
        }

        public static Registro model (RegistroDTO dto) {
            var model = new Registro ();

            if (dto != null) {
                //DateTime valor = (string.IsNullOrEmpty(dto.fechaRegistro) ? Convert.ToDateTime(dto.fechaRegistro) : default(DateTime));
                model.nId = dto.id;
                model.nSeq = dto.secuencia;
                model.sNumero = dto.numero;
                model.nIdCarga = dto.idCarga;
                model.nIdPersona = dto.persona.id;
                model.sPersona = dto.persona.descripcion;
                model.nIdPais = dto.pais.id;
                model.sPais = dto.pais.descripcion;
                model.nIdSenial = dto.senial.id;
                model.sSenial = dto.senial.descripcion;
                model.nIdDocumento = dto.documento.id;
                model.sDocumento = dto.documento.descripcion;
                model.sNumDoc = dto.numeroDocumento;
                model.sApePat = dto.apellidoPaterno;
                model.sApeMat = dto.apellidoMaterno;
                model.sNombre = dto.nombre;
                model.sObservacion = dto.observacion;
                model.sUsuario = dto.usuario;
                model.dRegistro = (!string.IsNullOrEmpty (dto.fechaRegistro) ? Convert.ToDateTime (dto.fechaRegistro) : default (DateTime));
                model.nEstado = (dto.activo ? Estado.Activo : Estado.Inactivo);
                model.nEditado = (dto.editado ? 1 : 0);
                model.nLiberado = (dto.liberado ? 1 : 0);
                model.dfechaCarga = (!string.IsNullOrEmpty (dto.fechaCarga) ? Convert.ToDateTime (dto.fechaCarga) : default (DateTime));
                model.ScategoriaNombre = dto.categoriaNombre;
                model.dfechaVigencia = (!string.IsNullOrEmpty (dto.fechaVigencia) ? Convert.ToDateTime (dto.fechaVigencia) : default (DateTime));

            }

            return model;
        }

        public static CargaDTO dto (Carga model) {
            var dto = new CargaDTO ();

            if (model != null) {
                dto.id = model.nId;
                dto.fechaRegistro = model.dRegistro.ToString ("dd/MM/yyyy hh:mm:ss");
                dto.usuario = model.sUsuario;
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
                dto.registros = new List<RegistroDTO> ();
            }

            return dto;
        }

        public static RegistroDTO dto (Registro model) {
            var dto = new RegistroDTO ();
            if (model != null) {
                dto.id = model.nId;
                dto.secuencia = model.nSeq;
                dto.numero = model.sNumero;
                dto.idCarga = model.nIdCarga;
                dto.persona = new MaestroDTO () { id = model.nIdPersona, descripcion = model.sPersona };
                dto.pais = new MaestroDTO () { id = model.nIdPais, descripcion = model.sPais };
                dto.documento = new MaestroDTO () { id = model.nIdDocumento, descripcion = model.sDocumento };
                dto.senial = new SenialDTO () { id = model.nIdSenial, descripcion = model.sSenial };
                dto.numeroDocumento = model.sNumDoc;
                dto.apellidoPaterno = model.sApePat;
                dto.apellidoMaterno = model.sApeMat;
                dto.nombre = model.sNombre;
                dto.observacion = model.sObservacion;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = (model.dRegistro == null) ? string.Empty : model.dRegistro.Value.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
                dto.editado = (model.nEditado == 1 ? true : false);
                dto.liberado = (model.nLiberado == 1 ? true : false);
                dto.fechaCarga = (model.dfechaCarga == null) ? string.Empty : model.dfechaCarga.Value.ToShortDateString ();
                dto.categoriaNombre = model.ScategoriaNombre;
                dto.fechaVigencia = (model.dfechaVigencia == null) ? string.Empty : model.dfechaVigencia.Value.ToShortDateString ();
            }
            return dto;
        }

        // public static ListSbsReportResponseDTO dto (SbsReport model) {
        //     var dto = new ListSbsReportResponseDTO();
        //     if (model != null) {
        //         dto.id = model.sId;
        //         dto.startDate = (model.startDate == null) ? string.Empty : model.startDate.Value.ToShortDateString ();
        //         dto.endDate = (model.endDate == null) ? string.Empty : model.endDate.Value.ToShortDateString ();
        //         dto.exchangeRate = model.exchangeRate;
        //         dto.operType = model.operType;
        //         dto.nameReport = model.nameReport;
        //         dto.statusReport = (model.statusProc == 0) ? string.Empty : model.statusProc.ToString ();
        //         dto.message = model.message;                
        //     }
        //     return dto;
        // }

        public static HistoriaRegistroDTO dto (HistoriaRegistro model) {
            var dto = new HistoriaRegistroDTO ();
            if (model != null) {
                dto.id = model.nId;
                dto.secuencia = model.nSeq;
                dto.numero = model.sNumero;
                dto.idCarga = model.nIdCarga;
                dto.idPersona = model.nIdPersona;
                dto.persona = model.sPersona;
                dto.idPais = model.nIdPais;
                dto.pais = model.sPais;
                dto.idSenial = model.nIdSenial;
                dto.senial = model.sSenial;
                dto.idDocumento = model.nIdDocumento;
                dto.documento = model.sDocumento;
                dto.numeroDocumento = model.sNumDoc;
                dto.apellidoPaterno = model.sApePat;
                dto.apellidoMaterno = model.sApeMat;
                dto.nombre = model.sNombre;
                dto.observacion = model.sObservacion;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = model.dRegistro.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
                dto.usuarioDB = model.sUsuarioDB;
                dto.usuarioPC = model.sUsuarioPC;
                dto.ip = model.sIP;
                dto.host = model.sHost;
                dto.fechaRegistroHistoria = model.dRegistroHistoria.ToString ("dd/MM/yyyy HH:mm:ss");
                dto.tipoHisoria = model.sTipoHistoria;
            }
            return dto;
        }

        public static List<Registro> models (List<RegistroDTO> dtos) {
            var models = new List<Registro> ();

            if (dtos != null) {
                foreach (var dto in dtos) {
                    models.Add (model (dto));
                }
            }

            return models;
        }

        public static List<Carga> models (List<CargaDTO> dtos) {
            var models = new List<Carga> ();

            if (dtos != null) {
                foreach (var dto in dtos) {
                    models.Add (model (dto));
                }
            }

            return models;
        }

        public static List<RegistroDTO> dtos (List<Registro> models) {
            var dtos = new List<RegistroDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        public static List<CargaDTO> dtos (List<Carga> models) {
            var dtos = new List<CargaDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        public static List<HistoriaRegistroDTO> dtos (List<HistoriaRegistro> models) {
            var dtos = new List<HistoriaRegistroDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        #endregion

        #region Maestros
        public static MaestroDTO dto (Aplicacion model) {
            var dto = new MaestroDTO ();

            if (model != null) {
                dto.id = model.nId;
                dto.descripcion = model.sDescripcion;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = model.dRegistro.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
            }

            return dto;
        }
        public static Documento models (MaestroDTO dto) {
            var models = new Documento ();

            if (dto != null) {
                models.nId = dto.id;
                models.sDescripcion = dto.descripcion;
                models.sUsuario = dto.usuario;
                models.dRegistro = Convert.ToDateTime (dto.fechaRegistro);
                models.nEstado = (dto.activo ? Estado.Activo : Estado.Inactivo);
            }

            return models;
        }

        public static MaestroDTO dto (Documento model) {
            var dto = new MaestroDTO ();


            if (model != null) {
                dto.id = model.nId;
                dto.descripcion = model.sDescripcion;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = model.dRegistro.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
            }

            return dto;
        }

        public static MaestroDTO dto (Pais model) {
            var dto = new MaestroDTO ();

            if (model != null) {
                dto.id = model.nId;
                dto.descripcion = model.sDescripcion;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = model.dRegistro.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
            }

            return dto;
        }

        public static MaestroDTO dto (Persona model) {
            var dto = new MaestroDTO ();

            if (model != null) {
                dto.id = model.nId;
                dto.descripcion = model.sDescripcion;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = model.dRegistro.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
            }

            return dto;
        }

        public static MaestroDTO dto (Producto model) {
            var dto = new MaestroDTO ();

            if (model != null) {
                dto.id = model.nId;
                dto.descripcion = model.sDescripcion;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = model.dRegistro.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
            }

            return dto;
        }

        public static SenialDTO dto (Senial model) {
            var dto = new SenialDTO ();

            if (model != null) {
                dto.id = model.nId;
                dto.descripcion = model.sDescripcion;
                dto.color = model.sColor;
                dto.usuario = model.sUsuario;
                dto.fechaRegistro = model.dRegistro.ToShortDateString ();
                dto.activo = (model.nEstado == Estado.Activo ? true : false);
                dto.indAlert = (model.nindalert == 1 ? true : false);
                dto.indError = (model.ninderror == 1 ? true : false);
            }

            return dto;
        }
        public static Senial models (SenialDTO model) {
            var dto = new Senial ();

            if (model != null) {
                dto.nId = model.id;
                dto.sDescripcion = model.descripcion;
                dto.sColor = model.color;
                dto.sUsuario = model.usuario;
                dto.dRegistro = Convert.ToDateTime (model.fechaRegistro);
                dto.nEstado = (model.activo) ? Estado.Activo : Estado.Inactivo;
                dto.nindalert = (model.indAlert) ? 1 : 0;
                dto.ninderror = (model.indError) ? 1 : 0;
            }

            return dto;
        }

        public static List<MaestroDTO> dtos (List<Aplicacion> models) {
            var dtos = new List<MaestroDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        public static List<MaestroDTO> dtos (List<Documento> models) {
            var dtos = new List<MaestroDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        public static List<MaestroDTO> dtos (List<Pais> models) {
            var dtos = new List<MaestroDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        public static List<MaestroDTO> dtos (List<Persona> models) {
            var dtos = new List<MaestroDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        public static List<MaestroDTO> dtos (List<Producto> models) {
            var dtos = new List<MaestroDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        public static List<SenialDTO> dtos (List<Senial> models) {
            var dtos = new List<SenialDTO> ();

            if (models != null) {
                foreach (var model in models) {
                    dtos.Add (dto (model));
                }
            }

            return dtos;
        }

        #endregion

        #region Configuraciones

        public static List<ConfigAplicacionDTO> dtos (List<SenialAplicacion> models) {
            List<ConfigAplicacionDTO> dtos = new List<ConfigAplicacionDTO> ();
            foreach (var model in models) {
                dtos.Add (dto (model));
            }
            return dtos;
        }
        public static ConfigAplicacionDTO dto (SenialAplicacion model) {
            ConfigAplicacionDTO dto = new ConfigAplicacionDTO ();
            dto.id = model.nId;
            dto.aplicacion = new MaestroDTO () { id = model.nIdAplicacion };
            dto.productos = new List<ConfigProductoDTO> ();
            dto.usuario = model.sUsuario;
            dto.fechaRegistro = model.dRegistro.ToShortDateString ();
            dto.activo = (model.nEstado == Estado.Activo ? true : false);
            return dto;
        }

        public static List<ConfigProductoDTO> dtos (List<SenialAplicacionProducto> models) {
            List<ConfigProductoDTO> dtos = new List<ConfigProductoDTO> ();
            foreach (var model in models) {
                dtos.Add (dto (model));
            }
            return dtos;
        }

        public static ConfigProductoDTO dto (SenialAplicacionProducto model) {
            ConfigProductoDTO dto = new ConfigProductoDTO ();
            dto.id = model.nId;
            dto.usuario = model.sUsuario;
            dto.fechaRegistro = model.dRegistro.ToShortDateString ();
            dto.activo = (model.nEstado == Estado.Activo ? true : false);
            dto.producto = new MaestroDTO () { id = model.nIdProducto };
            return dto;
        }

        public static List<ConfigAplicacionDTO> dtos (List<RegistroAplicacion> models) {
            List<ConfigAplicacionDTO> dtos = new List<ConfigAplicacionDTO> ();
            foreach (var model in models) {
                dtos.Add (dto (model));
            }
            return dtos;
        }

        public static ConfigAplicacionDTO dto (RegistroAplicacion model) {
            ConfigAplicacionDTO dto = new ConfigAplicacionDTO ();
            dto.id = model.nId;
            dto.aplicacion = new MaestroDTO () { id = model.nIdAplicacion };
            dto.productos = new List<ConfigProductoDTO> ();
            dto.usuario = model.sUsuario;
            dto.fechaRegistro = model.dRegistro.ToShortDateString ();
            dto.activo = (model.nEstado == Estado.Activo ? true : false);
            return dto;
        }

        public static List<ConfigProductoDTO> dtos (List<RegistroAplicacionProducto> models) {
            List<ConfigProductoDTO> dtos = new List<ConfigProductoDTO> ();
            foreach (var model in models) {
                dtos.Add (dto (model));
            }
            return dtos;
        }

        public static ConfigProductoDTO dto (RegistroAplicacionProducto model) {
            ConfigProductoDTO dto = new ConfigProductoDTO ();
            dto.id = model.nId;
            dto.usuario = model.sUsuario;
            dto.fechaRegistro = model.dRegistro.ToShortDateString ();
            dto.activo = (model.nEstado == Estado.Activo ? true : false);
            dto.producto = new MaestroDTO () { id = model.nIdProducto };
            return dto;
        }

        public static List<SenialAplicacion> models (ConfigSenialDTO dto) {
            List<SenialAplicacion> models = new List<SenialAplicacion> ();

            foreach (var item in dto.aplicaciones) {
                SenialAplicacion model = new SenialAplicacion ();
                model.nId = item.id;
                model.nIdSenial = dto.senial.id;
                model.nIdAplicacion = item.aplicacion.id;
                model.sUsuario = item.usuario;
                model.dRegistro = Convert.ToDateTime (item.fechaRegistro);
                model.nEstado = (item.activo ? Estado.Activo : Estado.Inactivo);

                models.Add (model);
            }

            return models;
        }

        public static List<SenialAplicacionProducto> models (ConfigAplicacionDTO dto) {
            List<SenialAplicacionProducto> models = new List<SenialAplicacionProducto> ();

            foreach (var item in dto.productos) {
                SenialAplicacionProducto model = new SenialAplicacionProducto ();
                model.nId = item.id;
                model.nIdSenialApp = dto.id;
                model.nIdProducto = item.producto.id;
                model.sUsuario = item.usuario;
                model.dRegistro = Convert.ToDateTime (item.fechaRegistro);
                model.nEstado = (item.activo ? Estado.Activo : Estado.Inactivo);
                models.Add (model);
            }

            return models;
        }

        /*
                public static List<RegistroAplicacion> models(ConfigRegistroDTO dto)
                {
                    List<RegistroAplicacion> models = new List<RegistroAplicacion>();

                    foreach(var item in dto.aplicaciones)
                    {
                        RegistroAplicacion model = new RegistroAplicacion();
                        model.nId = item.id;
                        model.nIdRegistro = dto.idRegistro;
                        model.nIdAplicacion = item.aplicacion.id;
                        model.sUsuario = item.usuario;
                        model.dRegistro = Convert.ToDateTime(item.fechaRegistro);
                        model.nEstado = (item.activo ? Estado.Activo : Estado.Inactivo);
                        models.Add(model);
                    }

                    return models;
                }*/

        public static RegistroAplicacion model (ConfigAplicacionDTO dto) {
            RegistroAplicacion model = new RegistroAplicacion ();
            model.nId = dto.id;
            model.nIdAplicacion = dto.aplicacion.id;
            model.sUsuario = dto.usuario;
            model.dRegistro = Convert.ToDateTime (dto.fechaRegistro);
            model.nEstado = (dto.activo ? Estado.Activo : Estado.Inactivo);

            return model;
        }

        public static RegistroAplicacionProducto model (ConfigProductoDTO dto) {
            RegistroAplicacionProducto model = new RegistroAplicacionProducto ();
            model.nId = dto.id;
            model.nIdProducto = dto.producto.id;
            model.sUsuario = dto.usuario;
            model.dRegistro = Convert.ToDateTime (dto.fechaRegistro);
            model.nEstado = (dto.activo ? Estado.Activo : Estado.Inactivo);

            return model;
        }

        #endregion

    }
}
