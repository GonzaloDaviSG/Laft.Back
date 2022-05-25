using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Repository;
using protecta.laft.api.Utils;

namespace protecta.laft.api.Services {
    public class EmailService {
        private readonly EmailSettings _configuration;

        public EmailService () {

        }
        private EmailService (EmailSettings config) {
            _configuration = config;
        }

        //Método para enviar correo electrónico al usuario en sesión LAFT
        public string SenderEmailReportGen (string user, string email, string route, string reportId, string message, string startDate, string enDate, string desReport, string desOperType, string sbsFileType) {
            try {
                if (route != null) {

                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    string bodyResponse = string.Empty;
                    string subject = string.Empty;
                    subject = "Generación de Reporte SBS - Plataforma LAFT";
                    var mm = new MailMessage ();
                    //    MailAddress from = new MailAddress (Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);
                    mm.From = from;
                    mm.To.Add (new MailAddress (email));
                    mm.Subject = subject;
                    bodyResponse = ComposeBodyReportGen (user, email, route, reportId, message, startDate, enDate, desReport, desOperType, sbsFileType, bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;
                    string fileName = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString (bodyResponse, null, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource (fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add (av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add (lr);

                    try {

                        using (SmtpClient smtp = new SmtpClient ("smtp.gmail.com", 587)) {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential (correoOC, constrasenna);
                            smtp.EnableSsl = true;
                            smtp.Send (mm);
                        }

                    } catch (Exception ex) {
                        Utils.ExceptionManager.resolve (ex);
                        return null;
                    }
                }
                return email;
            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public string ComposeBodyReportGen (string user, string email, string route, string reportId, string message, string startDate, string enDate, string desReport, string desOperType, string sbsFileType, string bodyResponse) {
            var reportCountName = "el reporte sbs ";
            var failreports = " no se pudo generar";

            if (route != null) {
                try {
                    if (sbsFileType == "txt") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Se acaba de generar "))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en texto plano "))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport.ToLower () + " con el tipo de operación " + desOperType.ToLower ()))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", "Por favor, puede descargar el reporte desde la plataforma "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", "LAFT"))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", "– opción monitoreo de reportes sbs, consultando el ID: "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", reportId))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Le pedimos que pueda verificarlo en la pantalla de monitoreo de reportes SBS."))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                    }
                    if (sbsFileType == "xls") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Se acaba de generar "))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en formato excel "))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport.ToLower () + " con el tipo de operación " + desOperType.ToLower ()))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", "Por favor, puede descargar el reporte desde la plataforma "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", "LAFT"))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", "– opción monitoreo de reportes sbs, consultando el ID: "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", reportId))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Le pedimos que pueda verificarlo en la pantalla de monitoreo de reportes SBS."))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                    }

                } catch (Exception ex) {
                    Utils.ExceptionManager.resolve (ex);
                    return null;
                }

            } else {
                try {
                    if (sbsFileType == "txt") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Le informamos que " + failreports))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en texto plano"))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport + " con el tipo de operación " + desOperType))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", " Le pedimos porfavor que pueda "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", " contactar a soporte. "))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                    }
                    if (sbsFileType == "xls") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Le informamos que " + failreports))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en excel"))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport + " con el tipo de operación " + desOperType))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", " Le pedimos porfavor que pueda "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", " contactar a soporte. "))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                    }

                } catch (Exception ex) {
                    Utils.ExceptionManager.resolve (ex);
                    return null;
                }

            }
            return bodyResponse;
        }

        public string SenderEmailCompRequest (dynamic param) {
            try {
                string email=param.SEMAIL;
                if (email != null) {

                    string endDate = param.SPERIODO_FECHA;
                    string user=param.NOMBRECOMPLETO; 
                    string cargo=param.SCARGO; 
                    //string email = param.SEMAIL;
                    int nidprofile = param.NIDPROFILE;
                    int nidaccion = param.NIDACCION;
                    string idUsuario = param.NIDUSUARIO;

                    string bodyResponse = string.Empty;
                    string subject = string.Empty;
                    var mm = new MailMessage ();
                    MailAddress from = new MailAddress (Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    //subject = "Solicitud de Complemento - Plataforma LAFT";

                    /*if (user == "Alfredo Chan Way Diaz") {
                        mm.Subject = "Conocimiento del cliente/contratantes - Comercial Rentas - Monitoreo de señales de alerta LAFT";
                    } else if (user == "Diego Rosell Ramírez Gastón") {
                        mm.Subject = "Conocimiento del cliente/contratantes - Comercial Masivos - Monitoreo de señales de alerta LAFT";
                    } else if (user == "Yvan Ruiz Portocarrero") {
                        mm.Subject = "Señales de alerta LAFT - Devolución de primas";
                    }*/

                    //Dictionary<string,dynamic> objRespCorreoCustom = new Dictionary<string,dynamic>();
                    



                    Dictionary<string,dynamic> objParamsUser = new Dictionary<string,dynamic>();
                    objParamsUser["NIDPROFILE"] = nidprofile;
                    objParamsUser["NIDUSUARIO"] = idUsuario;
                    var objRespUser = new SenialRepository().getDataUsuarioByParams(objParamsUser);

                    Console.WriteLine("el objRespUser : "+objRespUser);
                    Console.WriteLine("el LISTA : "+objRespUser["lista"]);

                    foreach (var item in objRespUser["lista"])
                    {
                        Console.WriteLine("el item LISTA : "+item);
                        Console.WriteLine("el item item[NIDPROFILE] : "+item["NIDPROFILE"]);
                        Console.WriteLine("el item item[NIDGRUPOSENAL] : "+item["NIDGRUPOSENAL"]);
                        Dictionary<string,dynamic> objParamsCorreo = new Dictionary<string,dynamic>();
                        objParamsCorreo["NIDPROFILE"] = item["NIDPROFILE"];
                        objParamsCorreo["NIDGRUPOSENAL"] = item["NIDGRUPOSENAL"];
                        objParamsCorreo["NIDACCION"] = nidaccion;
                        Console.WriteLine("el NIDPROFILE : "+objParamsCorreo["NIDPROFILE"]);
                        Console.WriteLine("el NIDGRUPOSENAL : "+objParamsCorreo["NIDGRUPOSENAL"]);
                        Console.WriteLine("el NIDACCION : "+objParamsCorreo["NIDACCION"]);
                        Console.WriteLine("el NOMBRE_USUARIO : "+item["NOMBRE_USUARIO"]);
                        var objRespCorreoCustom = new SenialRepository().getCorreoCustom(objParamsCorreo);
                        Console.WriteLine("el objRespCorreoCustom : "+objRespCorreoCustom);

                        subject = objRespCorreoCustom["SASUNTO_CORREO"];
                        mm.From = from;
                        mm.To.Add (new MailAddress (email));
                        mm.Subject = subject;
                        bodyResponse = ComposeBodyCompRequest2 (endDate, item["NOMBRE_USUARIO"], item["SEMAIL"], cargo, bodyResponse,objRespCorreoCustom["SCUERPO_CORREO"],item["NOMBRE_PERFIL"]);
                        Console.WriteLine ("el bodyResponse : " + bodyResponse);
                        mm.Body = bodyResponse;
                        mm.IsBodyHtml = true;

                        string fileName = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                        AlternateView av = AlternateView.CreateAlternateViewFromString (bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                        LinkedResource lr = new LinkedResource (fileName, MediaTypeNames.Image.Jpeg);
                        mm.AlternateViews.Add (av);
                        lr.ContentId = "Logo";
                        av.LinkedResources.Add (lr);

                        try {

                            using (SmtpClient smtp = new SmtpClient ("smtp.gmail.com", 587)) {
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new NetworkCredential (Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                                smtp.EnableSsl = true;
                                smtp.Send (mm);
                            }

                        } catch (Exception ex) {
                            Console.WriteLine("el erroe en el envio de correo : "+ex);
                            //throw ex;
                            //return "false";
                            //Utils.ExceptionManager.resolve (ex);
                            //return null;
                        }
                    }


                    



                    
                    
                }
                return email;
            } catch (Exception ex) {
                Console.WriteLine ("el exception : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }


        public string SenderEmailCompRequest2(dynamic param)
        {
            try
            {
                string email = param.userEmail;
                if (email != null)
                {
                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    string userFullName = param.userFullName;
                    int validacion = param.valor;
                    string pass = param.pass;
                    string userName = param.userName;

                    string bodyResponse = string.Empty;
                    
                    string subject = string.Empty;
                    var mm = new MailMessage();
                    //MailAddress from = new MailAddress(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);

                    mm.From = from;
                        mm.To.Add(new MailAddress(email));
                        mm.Subject = subject;
                        if (validacion == 1)
                        {
                            mm.Subject = "ACTUALIZACIÓN DE USUARIO - " + userFullName.ToUpper();
                        }
                        else
                        {
                            mm.Subject = "CREACIÓN DE USUARIO - " + userFullName.ToUpper(); ;
                        }

                    bodyResponse = ComposeBodyCompRequest3(userFullName, email, bodyResponse, validacion ,pass, userName);
                        Console.WriteLine("el bodyResponse : " + bodyResponse);
                        mm.Body = bodyResponse;
                        mm.IsBodyHtml = true;

                        string fileName = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                        AlternateView av = AlternateView.CreateAlternateViewFromString(bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                        LinkedResource lr = new LinkedResource(fileName, MediaTypeNames.Image.Jpeg);
                        mm.AlternateViews.Add(av);
                        lr.ContentId = "Logo";
                        av.LinkedResources.Add(lr);

                        try
                        {

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            //smtp.Credentials = new NetworkCredential(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                            smtp.Credentials = new NetworkCredential(correoOC, constrasenna);
                            smtp.EnableSsl = true;
                             smtp.Send(mm);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("el error en el envio de correo : " + ex);
                           
                        }
                    

                }
                return email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el exception : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }

        public string SenderEmailComplemento(dynamic param)
        {
            try
            {
                string email = param.SEMAIL;
                if (email != null)
                {

                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    string userFullName = param.NOMBRECOMPLETO;
                   // int validacion = param.valor;
                    //string pass = param.pass;
                    string userName = param.fullName;
                    string fechaPerido = param.FECHAPERIODO;
                    string nombreAlerta = param.NOMBREALERTA;
                    string cargo = param.SDESCARGO;
                    string bodyResponse = string.Empty;
                    string asunto = param.ASUNTO;
                   
                    string NewAsunto = "";
;

                    if (asunto.IndexOf("[Periodo]") != -1)
                    {
                        NewAsunto = asunto.Replace("[Periodo]", fechaPerido);
                        asunto = NewAsunto;



                    }

                    asunto = asunto.ToUpper();


                    string subject = string.Empty;
                  
                    var mm = new MailMessage();
                    //MailAddress from = new MailAddress(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);

                    mm.From = from;
                    mm.To.Add(new MailAddress(email));
                    mm.Subject = subject;

                    // mm.Subject = "Complemento enviado a " + userFullName;
                    mm.Subject = asunto;




                    bodyResponse = ComposeBodyCompRequestComplemento(userFullName, email, bodyResponse, userName, fechaPerido , nombreAlerta, cargo);
                    Console.WriteLine("el bodyResponse : " + bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;

                    string fileName = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource(fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add(av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add(lr);

                    try
                    {

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            //smtp.Credentials = new NetworkCredential(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                            smtp.Credentials = new NetworkCredential(correoOC, constrasenna);
                            smtp.EnableSsl = true;
                            smtp.Send(mm);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("el error en el envio de correo : " + ex);

                    }


                }
                return email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el exception : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }


        public string ConfirmacionBandeja(dynamic param)
        {
            try
            {
                Console.WriteLine(param);
                string email = "";//param.SEMAIL;
                Console.WriteLine(param.SEMAIL);
                
                int cantidadEmail = param.SEMAIL.Count;
                if (cantidadEmail != 0)
                {

                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    string userFullName = param.NOMBRECOMPLETO;
                    string asunto = param.ASUNTO;
                    //asunto = asunto.ToUpper();
                    string grupo = param.GRUPO;
                    string usuario = param.USUARIO;
                    string mensaje = param.mensaje;
                    string periodo = param.PERIODO;

                    string periodo_asunto = param.PERIODO_ASUNTO;
                    string NewAsunto = "";

                    if (asunto.IndexOf("[Periodo]") != -1)
                    {
                        NewAsunto = asunto.Replace("[Periodo]", periodo_asunto);
                        asunto = NewAsunto;
                        //asunto = NewAsunto.ToUpper();

                    }
                    asunto = asunto.ToUpper();

                    string bodyResponse = string.Empty;

                    string subject = string.Empty;
                    var mm = new MailMessage();
                    //MailAddress from = new MailAddress(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);

                    mm.From = from;
                    //mm.To.Add(new MailAddress(email));
                    mm.To.Add(new MailAddress(correoOC)); // para que se envie el correo a el mismo
                    mm.Subject = subject;
                    for(int i = 0; i < param.SEMAIL.Count; i++)
                    {
                        mm.CC.Add(param.SEMAIL[i].ToString()); // envio de correo con copia
                    }
                    

                    mm.Subject = asunto + " - " + userFullName.ToUpper();



                    bodyResponse = ComposeBodyConfirmacionBandeja(userFullName, bodyResponse, grupo, usuario,mensaje,periodo);
                    Console.WriteLine("el bodyResponse : " + bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;

                    string fileName = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource(fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add(av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add(lr);

                    try
                    {

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            //smtp.Credentials = new NetworkCredential(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                            smtp.Credentials = new NetworkCredential(correoOC, constrasenna);
                            smtp.EnableSsl = true;
                            smtp.Send(mm);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("el error en el envio de correo : " + ex);

                    }


                }
                return email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el exception : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }

        public string RecuperarContrasennaUsuario(dynamic param)
        {
            try
            {
               
                string email = param.SEMAIL;
                int id_usuario = param.ID_USER;
                string usario = param.USUARIO;

                if (email != "")
                {

                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    Dictionary<string, dynamic> objParamsCorreo = new Dictionary<string, dynamic>();
                    objParamsCorreo["NIDACCION"] = 7;
                    Dictionary<string, dynamic> objParamsHash = new Dictionary<string, dynamic>();
                    objParamsHash["ID_USUARIO"] = id_usuario;

                    var objRespCorreo = new SenialRepository().getCorreoCustomAction(objParamsCorreo);
                    var objRespHash = new SenialRepository().getObtenerHash(objParamsHash);
                    
                    
                    string asunto = objRespCorreo["SASUNTO_CORREO"];
                   
                    string mensaje = objRespCorreo["SCUERPO_CORREO"];

                    string periodo_asunto = param.PERIODO_ASUNTO;
                    string NewAsunto = "";

                    if (asunto.IndexOf("[Periodo]") != -1)
                    {
                        NewAsunto = asunto.Replace("[Periodo]", periodo_asunto);
                        asunto = NewAsunto.ToUpper();

                    }
                    asunto = asunto.ToUpper();

                    string hash = objRespHash["SHASH"];
                    string bodyResponse = string.Empty;

                    string subject = string.Empty;
                    var mm = new MailMessage();
                    //MailAddress from = new MailAddress(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);

                    mm.From = from;
                    mm.To.Add(new MailAddress(email));
                    //mm.To.Add(new MailAddress(Config.AppSetting["EmailSettings:Email"])); // para que se envie el correo a el mismo
                    mm.Subject = subject;
                    mm.Subject = asunto;



                    bodyResponse = ComposeBodyRecuperarContrasenna(bodyResponse, mensaje,hash, usario);
                    Console.WriteLine("el bodyResponse : " + bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;

                    string fileName = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource(fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add(av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add(lr);

                    try
                    {

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            //smtp.Credentials = new NetworkCredential(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                            smtp.Credentials = new NetworkCredential(correoOC, constrasenna);
                            smtp.EnableSsl = true;
                            smtp.Send(mm);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("el error en el envio de correo : " + ex);

                    }


                }
                return email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el exception : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }


        public string EnviarCorreoRecordatorio(dynamic param,dynamic objCorreo)
        {
            try
            {

                string email =  param["SEMAIL"];
                string grupo = param["SDESGRUPO_SENAL"];
                string periodo = objCorreo.FechaInicioPeriodo;
                string mensaje = objCorreo.SCUERPO_CORREO;
                string asunto = objCorreo.SASUNTO_CORREO;
               


                string periodo_asunto = "del 01/10/2021 al 31/12/2021";//param.PERIODO_ASUNTO;
                string NewAsunto = "";

                if (asunto.IndexOf("[Periodo]") != -1)
                {
                    NewAsunto = asunto.Replace("[Periodo]", periodo_asunto);
                    asunto = NewAsunto;

                }

                asunto = asunto.ToUpper();
                if (email != "")
                {

                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    string bodyResponse = string.Empty;

                    string subject = string.Empty;
                    var mm = new MailMessage();
                    
                    MailAddress from = new MailAddress(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    //MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);

                    mm.From = from;
                    mm.To.Add(new MailAddress(email));
                    //mm.To.Add(new MailAddress(Config.AppSetting["EmailSettings:Email"])); // para que se envie el correo a el mismo
                    mm.Subject = subject;
                    mm.Subject = asunto;



                    bodyResponse = ComposeBodyCorreoRecordatorio(bodyResponse, grupo, periodo,mensaje);
                    
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;

                    string fileName = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource(fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add(av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add(lr);

                    try
                    {

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                            //smtp.Credentials = new NetworkCredential(correoOC, constrasenna);
                            smtp.EnableSsl = true;
                            smtp.Send(mm);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("el error en el envio de correo : " + ex);

                    }


                }
                return email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el exception : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }

        public string SenderEmailComplementoSinSennal(dynamic param)
        {
            try
            {
                string email = param.SEMAIL;
                if (email != null)
                {

                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    var objPss = new
                    {
                        NID_USUARIO = param.USERID,
                        
                    };

                    var objRespPss = new SenialService().DesenciptarPassUsuario(objPss);
                    string credencial = objRespPss["pss"];

                    string userFullName = param.NOMBRECOMPLETO;
                    string cargo = param.CARGO;
                    string id = param.ID;
                    string fechafin = param.FECHAFIN;
                    //string credencial = "";
                    // int validacion = param.valor;
                    //string pass = param.pass;
                    //string userName = param.fullName;
                    string fechaPerido = param.FECHAPERIODO;
                    
                    string bodyResponse = string.Empty;
                    string asunto = param.ASUNTO;
                   
                    string mensaje = param.MENSAJE;
                    string NewAsunto = "";
                    ;

                    if (asunto.IndexOf("[Periodo]") != -1)
                    {
                        NewAsunto = asunto.Replace("[Periodo]", fechaPerido);
                        asunto = NewAsunto;

                    }
                    asunto = asunto.ToUpper();



                    string subject = string.Empty;

                    var mm = new MailMessage();
                    //MailAddress from = new MailAddress(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);

                    mm.From = from;
                    mm.To.Add(new MailAddress(email));
                    mm.Subject = subject;

                    // mm.Subject = "Complemento enviado a " + userFullName;
                    mm.Subject = asunto;




                    bodyResponse = ComposeBodyComplementoSinsennal(userFullName, bodyResponse, mensaje, fechaPerido ,  cargo,  id,  credencial,  fechafin);
                    Console.WriteLine("el bodyResponse : " + bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;

                    string fileName = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource(fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add(av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add(lr);

                    try
                    {

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            //smtp.Credentials = new NetworkCredential(Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                            smtp.Credentials = new NetworkCredential(correoOC, constrasenna);
                            smtp.EnableSsl = true;
                            smtp.Send(mm);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("el error en el envio de correo : " + ex);

                    }


                }
                return email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el exception : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }




        public string ComposeBodyCompRequest (string endDate, string user, string email, string cargo, string bodyResponse) {

            if (user != null) {

                if (user == "Alfredo Chan Way Diaz") {
                    try {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                            .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Como parte del conocimiento del cliente, se requiere tu confirmación respecto a si la fuerza de ventas de Comercial Rentas (Vida con Renta Temporal/Renta Total/Ahorro Total, según aplique) ha reportado las siguientes situaciones al cierre de "))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", endDate))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));
                    } catch (Exception ex) {
                        throw ex;
                    }
                } else if (user == "Diego Rosell Ramírez Gastón") {
                    try {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                            .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Como parte del conocimiento del cliente (contratantes de productos masivos), se requiere tu confirmación si se han presentado/identificado las siguientes situaciones al cierre de "))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", endDate))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));
                    } catch (Exception ex) {
                        throw ex;
                    }
                } else if (user == "Yvan Ruiz Portocarrero") {
                    try {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                            .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Por favor considerar si en el proceso de Emisión de pólizas (todos los productos de la compañía) si se han presentado/identificado las siguientes situaciones al cierre de "))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", endDate))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));
                    } catch (Exception ex) {
                        throw ex;
                    }
                }
            } else {
                try {
                    string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                    string readText = File.ReadAllText (path);
                    return readText

                        .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                        .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Por favor. Le pedimos que pueda contactar a soporte."))
                        .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", " "))
                        .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                } catch (Exception ex) {
                    Console.WriteLine ("la excepcion 2 : " + ex);
                    Utils.ExceptionManager.resolve (ex);
                    return null;
                }
            }
            return bodyResponse;
        }
        
        public string ComposeBodyCompRequest2 (string endDate, string user, string email, string cargo, string bodyResponse,string cuerpo,string nom_perfil) {

            if (user != null) {

                    try {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                        string readText = File.ReadAllText (path);
                        string textFInal = readText.Replace ("[Mensaje]", string.Format ("<div>{0}</div>", cuerpo));
                        return textFInal

                            .Replace ("[Usuario]", string.Format ("<strong style='font-weight: 100 !important;'>{0}</strong>", user))
                            //.Replace ("[Mensaje]", string.Format ("<div>{0}</div>", cuerpo))
                            .Replace ("[Cargo]", string.Format ("<strong style='font-weight: 100 !important;'>{0}</strong>", cargo))
                            .Replace ("[Perfil]", string.Format ("<strong style='font-weight: 100 !important;'>{0}</strong>", nom_perfil))
                            .Replace ("[FechaFin]", string.Format ("<strong style='font-weight: 100 !important;'>{0}</strong>", endDate))
                            .Replace ("[Link]", string.Format ("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]))
                            .Replace ("[Instruccion]", string.Format ("<strong style='font-weight: 100 !important;'>{0}</strong>", "Por favor ingrese a este URL"));
                          
                    } catch (Exception ex) {
                        throw ex;
                    }
                
            } else {
                try {
                    string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                    string readText = File.ReadAllText (path);
                    return readText

                        .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                        .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Por favor. Le pedimos que pueda contactar a soporte."))
                        .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", " "))
                        .Replace ("[Link]", string.Format ("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                } catch (Exception ex) {
                    Console.WriteLine ("la excepcion 2 : " + ex);
                    Utils.ExceptionManager.resolve (ex);
                    return null;
                }
            }
            return bodyResponse;
        }

        public string ComposeBodyCompRequest3(string userFullName, string email, string bodyResponse, int valicacion , string pass,string userName)
        {

            if (userFullName != null)
            {

                try
                {
                    if (valicacion == 1)
                    {
                        string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyUserUpdate.html");
                        string readText = File.ReadAllText(path);

                        return readText

                            .Replace("[Nombre]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", userFullName))
                            .Replace("[Mensaje]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", "Se realizó la actualización de su contraseña de manera exitosa."))
                            .Replace("[Password]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", pass))
                            .Replace("[Link]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                    }
                    else
                    {
                        string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyUserCreate.html");
                        string readText = File.ReadAllText(path);

                        return readText

                            .Replace("[Nombre]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", userFullName))
                            .Replace("[Mensaje]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", "Se realizó la creación de su usuario de manera exitosa."))
                            .Replace("[Usuario]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", userName))
                            .Replace("[Password]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", pass))
                            .Replace("[Link]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));
                    }




                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                try
                {
                    string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                    string readText = File.ReadAllText(path);
                    return readText

                        .Replace("[Usuario]", string.Format("<strong>{0}</strong>", userName))
                        .Replace("[Mensaje]", string.Format("<strong>{0}</strong>", "Por favor. Le pedimos que pueda contactar a soporte."))
                        .Replace("[FechaFin]", string.Format("<strong>{0}</strong>", " "))
                        .Replace("[Link]", string.Format("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                }
                catch (Exception ex)
                {
                    Console.WriteLine("la excepcion 2 : " + ex);
                    Utils.ExceptionManager.resolve(ex);
                    return null;
                }
            }
            return bodyResponse;
        }


        public string ComposeBodyCompRequestComplemento(string userFullName, string email, string bodyResponse, string fullName ,string fechaPerido, string nombreAlerta, string cargo)
        {

            if (userFullName != null)
            {

                try
                {

                        string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyComplementUsuario.html");
                        string readText = File.ReadAllText(path);

                        return readText

                            .Replace("[Nombre]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", userFullName))
                            .Replace("[Cargo]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", cargo))
                            .Replace("[FechaPeriodo]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", fechaPerido))
                            .Replace("[Sennal]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", nombreAlerta))
                            .Replace("[Link]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                  

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                try
                {
                    string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                    string readText = File.ReadAllText(path);
                    return readText

                        .Replace("[Usuario]", string.Format("<strong>{0}</strong>", fullName))
                        .Replace("[Mensaje]", string.Format("<strong>{0}</strong>", "Por favor. Le pedimos que pueda contactar a soporte."))
                        .Replace("[FechaFin]", string.Format("<strong>{0}</strong>", " "))
                        .Replace("[Link]", string.Format("<strong>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));

                }
                catch (Exception ex)
                {
                    Console.WriteLine("la excepcion 2 : " + ex);
                    Utils.ExceptionManager.resolve(ex);
                    return null;
                }
            }
            return bodyResponse;
        }

        public string ComposeBodyConfirmacionBandeja(string userFullName, string bodyResponse, string grupo, string usuario,string  mensaje, string periodo)
        {

            if (userFullName != null)
            {

                try
                {

                    string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyConfirmacionFormulario.html");
                    string readText = File.ReadAllText(path);
                    string textFinal = readText.Replace("[Mensaje]", string.Format("<div>{0}</div>", mensaje));
                    return textFinal
                        //.Replace("[Mensaje]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", mensaje))
                        .Replace("[Usuario]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", userFullName))
                        .Replace("[Grupo]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", grupo))
                        .Replace("[Periodo]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", periodo));

                    //.Replace("[Link]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));



                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            
            return bodyResponse;
        }

        public string ComposeBodyRecuperarContrasenna( string bodyResponse, string mensaje,string hash,string usario)
        {

             try
                {
                string href = Config.AppSetting["linkCorreosLAFT:url"] + "validador/" + hash;
                    string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRecuperarPass.html");
                    string readText = File.ReadAllText(path);
                    string textFinal = readText.Replace("[Mensaje]", string.Format("<div>{0}</div>", mensaje));
                return textFinal
                    //.Replace("[Mensaje]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", mensaje))
                    .Replace("[Link]", string.Format("<a style='color:blue' target='_blank' href='{1}'>{0}</a>", "Actualización de contraseña ", href))
                    .Replace("[Usuario]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", usario));

                //.Replace("[Link]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));



            }
            catch (Exception ex)
                {
                    throw ex;
                }

            

            return bodyResponse;
        }
        public string EmailSender (string user, string manager, string rol, string email) {
            try {
                if (email != null) {

                    var objRespCorreoCustom = new SenialService().ObtenerCorreoyContrasenaOC();
                    string correoOC = objRespCorreoCustom["correo"];
                    string constrasenna = objRespCorreoCustom["password"];

                    string bodyResponse = string.Empty;
                    string subject = string.Empty;
                    subject = "LLENADO DE FORMULARIO - PLATAFORMA LAFT";
                    subject = subject.ToUpper();
                    var mm = new MailMessage ();
                    //MailAddress from = new MailAddress (Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Asunto"]);
                    MailAddress from = new MailAddress(correoOC, Config.AppSetting["EmailSettings:Asunto"]);
                    mm.From = from;
                    mm.To.Add (new MailAddress (email));
                    mm.Subject = subject;
                    bodyResponse = ComposeBodyReviewForm (user, manager, email, rol, bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;
                    string fileName = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString (bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource (fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add (av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add (lr);

                    try {

                        using (SmtpClient smtp = new SmtpClient ("smtp.gmail.com", 587)) {
                            smtp.UseDefaultCredentials = false;
                            //smtp.Credentials = new NetworkCredential (Config.AppSetting["EmailSettings:Email"], Config.AppSetting["EmailSettings:Password"]);
                            smtp.Credentials = new NetworkCredential(correoOC, constrasenna);
                            smtp.EnableSsl = true;
                            smtp.Send (mm);
                        }

                    } catch (Exception ex) {
                        throw ex;
                    }
                }
                return email;
            } catch (Exception ex) {
                Console.WriteLine (ex.ToString ());
            }
            return null;
        }

        public string ComposeBodyReviewForm (string user, string manager, string email, string cargo, string bodyResponse) {
            if (user != null) {
                try {
                    string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReviewForm.html");
                    string readText = File.ReadAllText (path);
                    return readText

                        .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                        .Replace ("[Manager]", string.Format ("<strong>{0}</strong>", manager))
                        .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Por favor ingrese a esta URL: " + Config.AppSetting["linkCorreosLAFT:url"]));

                } catch (Exception ex) {
                    Utils.ExceptionManager.resolve (ex);
                    return null;
                }

            } else {
                try {
                    string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReviewForm.html");
                    string readText = File.ReadAllText (path);
                    return readText

                        .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                        .Replace ("[Manager]", string.Format ("<strong>{0}</strong>", manager))
                        .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Por favor. Le pedimos que pueda contactar a soporte."));
                } catch (Exception ex) {
                    Utils.ExceptionManager.resolve (ex);
                    return null;
                }
            }
            return bodyResponse;
        }

        public string ComposeBodyCorreoRecordatorio(string bodyResponse, string grupo, string periodo,string mensaje)
        {

            try
            {
                
                string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRecordatorio.html");
                string readText = File.ReadAllText(path);
                string textFinal = readText.Replace("[Mensaje]", string.Format("<div>{0}</div>", mensaje));
                return textFinal
                    //.Replace("[Mensaje]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", mensaje))
                    
                    .Replace("[Periodo]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", periodo))
                    .Replace("[Grupo]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", grupo))
                    .Replace("[Link]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));



            }
            catch (Exception ex)
            {
                throw ex;
            }



            return bodyResponse;
        }

        public string ComposeBodyComplementoSinsennal(string userFullName, string bodyResponse , string mensaje, string periodo, string cargo, string id, string credencial, string fechafin)
        {

            if (userFullName != null)
            {

                try
                {

                    string path = string.Format("{0}{1}", "C:\\TemplatesLAFT\\", "BodyComplementoSinSennal.html");
                    string readText = File.ReadAllText(path);
                    string textFinal = readText.Replace("[Mensaje]", string.Format("<div>{0}</div>", mensaje));
                    return textFinal
                       .Replace("[Usuario]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", userFullName))
                       .Replace("[Cargo]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", cargo))
                       .Replace("[Id]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", id))
                       .Replace("[Credencial]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", credencial))
                       .Replace("[FechaFin]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", fechafin))
                       .Replace("[Link]", string.Format("<strong style='font-weight: 100 !important;'>{0}</strong>", Config.AppSetting["linkCorreosLAFT:url"]));



                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return bodyResponse;
        }
    }
}