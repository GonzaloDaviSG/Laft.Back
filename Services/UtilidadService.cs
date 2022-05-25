using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using protecta.laft.api.Models;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace protecta.laft.api.Services
{
    public class UtilidadService
    {
        public UtilidadService()
        {

        }

        public void getsDocuments()
        {
            // Escribimos el encabezamiento en el documento
            string datastr = "{ \"mensaje\":\"Se generó el proceso correctamente\",\"sMessage\":null,\"code\":0,\"nCode\":0,\"items\":[{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":2,\"sdestipolista\":\"LISTAS PEP\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":5,\"sdestipolista\":\"LISTAS ESPECIALES\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":3,\"sdesproveedor\":\"REGISTRO NEGATIVO\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":1,\"sdestipolista\":\"LISTAS INTERNACIONALES\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":3,\"sdestipolista\":\"LISTAS FAMILIAR PEP\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":2,\"sdestipolista\":\"LISTAS PEP\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":5,\"sdestipolista\":\"LISTAS ESPECIALES\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":3,\"sdesproveedor\":\"REGISTRO NEGATIVO\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":1,\"sdestipolista\":\"LISTAS INTERNACIONALES\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"\",\"nidtipolista\":3,\"sdestipolista\":\"LISTAS FAMILIAR PEP\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"POZO GOMERO JOSE RENATO\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"46610806\",\"sporceN_COINCIDENCIA\":\"100\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"DNI\",\"stipO_PERSONA\":\"PERSONA NATURAL\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"NOMBRE\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"CON COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":2,\"sdestipolista\":\"LISTAS PEP\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":5,\"sdestipolista\":\"LISTAS ESPECIALES\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":3,\"sdesproveedor\":\"REGISTRO NEGATIVO\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":1,\"sdestipolista\":\"LISTAS INTERNACIONALES\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":3,\"sdestipolista\":\"LISTAS FAMILIAR PEP\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":2,\"sdestipolista\":\"LISTAS PEP\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":5,\"sdestipolista\":\"LISTAS ESPECIALES\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":3,\"sdesproveedor\":\"REGISTRO NEGATIVO\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":1,\"sdestipolista\":\"LISTAS INTERNACIONALES\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"-\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"-\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"-\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"-\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"\",\"nidtipolista\":3,\"sdestipolista\":\"LISTAS FAMILIAR PEP\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":\"POZO GOMERO JOSE RENATO\",\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":\"46610806\",\"sporceN_COINCIDENCIA\":\"79\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"DNI\",\"stipO_PERSONA\":\"PERSONA NATURAL\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":1,\"sdesproveedor\":\"IDECON\",\"stipocoincidencia\":\"NOMBRE\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"CON COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":null,\"nidtipolista\":2,\"sdestipolista\":\"LISTAS PEP\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":null,\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":null,\"sporceN_COINCIDENCIA\":null,\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":null,\"stipO_PERSONA\":null,\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":4,\"sdesproveedor\":\"WORDLCHECK\",\"stipocoincidencia\":null,\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":null,\"nidtipolista\":1,\"sdestipolista\":\"LISTAS INTERNACIONALES\",\"snombrE_BUSQUEDA\":\"POZO GOMERO JOSE RENATO\",\"snombrE_COMPLETO\":null,\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":null,\"sporceN_COINCIDENCIA\":null,\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":null,\"stipO_PERSONA\":null,\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":4,\"sdesproveedor\":\"WORDLCHECK\",\"stipocoincidencia\":null,\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":null,\"nidtipolista\":2,\"sdestipolista\":\"LISTAS PEP\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":null,\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":null,\"sporceN_COINCIDENCIA\":null,\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":null,\"stipO_PERSONA\":null,\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":4,\"sdesproveedor\":\"WORDLCHECK\",\"stipocoincidencia\":null,\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":null,\"nidtipolista\":2,\"sdestipolista\":\"LISTAS PEP\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":null,\"snombrE_TERMINO\":null,\"snuM_DOCUMENTO\":null,\"sporceN_COINCIDENCIA\":null,\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":null,\"stipO_PERSONA\":null,\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":4,\"sdesproveedor\":\"WORDLCHECK\",\"stipocoincidencia\":null,\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"SIN COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":1,\"sdestipolista\":\"LISTAS INTERNACIONALES\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"MIFARMA\",\"snombrE_TERMINO\":\"MIFARMA\",\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"92\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"EMPRESA  (PERSONA JURÍDICA)\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":4,\"sdesproveedor\":\"WORDLCHECK\",\"stipocoincidencia\":\"NOMBRE\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"CON COINCIDENCIA\"},{ \"dfechA_BUSQUEDA\":\"28/04/2022 1:07:50 PM\",\"scargo\":\"-\",\"nidtipolista\":1,\"sdestipolista\":\"LISTAS INTERNACIONALES\",\"snombrE_BUSQUEDA\":\"MI FARMA S.A.C\",\"snombrE_COMPLETO\":\"MI FARMA SAC\",\"snombrE_TERMINO\":\"MI FARMA SAC\",\"snuM_DOCUMENTO\":\"-\",\"sporceN_COINCIDENCIA\":\"90\",\"sporceN_SCORE\":null,\"stipO_DOCUMENTO\":\"-\",\"stipO_PERSONA\":\"EMPRESA  (PERSONA JURÍDICA)\",\"susuariO_BUSQUEDA\":\"Graciela Sifuentes Alvarez\",\"nidproveedor\":4,\"sdesproveedor\":\"WORDLCHECK\",\"stipocoincidencia\":\"NOMBRE\",\"snumdoC_BUSQUEDA\":null,\"scoincidencia\":\"CON COINCIDENCIA\"}],\"mensajeError\":null}";
            ResponseCoincidenciaDemanda item = JsonConvert.DeserializeObject<ResponseCoincidenciaDemanda>(datastr);
            try
            {
                Document doc = new Document(PageSize.LETTER);
                // Indicamos donde vamos a guardar el documento
                using (var document = new Document())
                {
                    using (var reader = new PdfReader(@"C:\Users\MATERIAGRIS\Downloads\pdfBase\newtemplateWc - copia.pdf"))
                    {
                        FileStream os = new FileStream(@"C:\Users\MATERIAGRIS\Downloads\pdfBase\newtemplateWc - copia - copia.pdf", FileMode.Open);
                        //using (var stamper = new PdfStamper.createXmlSignature(reader, os);)
                        //{
                        //    PdfDictionary page = reader.GetPageN(1);

                        //    PdfStamper stp = new PdfStamper(reader, os);
                        //    stp.sigXmlApp = new XmlSignatureAppearance(stp.stamper);
                        //    //stp.sigApp.setSigout(bout);
                        //    //stp.sigApp.setOriginalout(os);
                        //    stp.sigXmlApp.SetStamper(stp);




                        //PdfDictionary resources = page.GetAsDict(PdfName.RESOURCES);
                        //PdfDictionary xobjects = resources.GetAsDict(PdfName.XOBJECT);
                        //Dictionary<PdfName, PdfObject>.KeyCollection.Enumerator enumerator = xobjects.Keys.GetEnumerator();
                        //enumerator.MoveNext();
                        //XmlSignatureAppearance appearance = stamper.XmlSignatureAppearance;
                        //appearance.SetXmlLocator(new XfaXmlLocator(stamper));
                        //XmlDocument xml = appearance.GetXmlLocator().GetDocument();
                        ////xml.Load("");
                        //document.Open();

                        //foreach (var file in strFiles)
                        //{

                        //PdfDictionary d = reader.GetPageN(1);
                        //stamper.(reader);
                        //}
                        //}

                        document.Close();
                    }
                }

                //PdfStamper
                //PdfWriter writer = PdfWriter.GetInstance(doc,
                //                            new FileStream(@"C:\Users\MATERIAGRIS\Downloads\-_IDECON_LISTAS PEP.pdf", FileMode.Append));

                //// Le colocamos el título y el autor
                //// **Nota: Esto no será visible en el documento
                //doc.AddTitle("Mi primer PDF");
                //doc.AddCreator("Roberto Torres");
                //// Abrimos el archivo
                //doc.Open();
                //// Creamos el tipo de Font que vamos utilizar
                //iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                //iTextSharp.text.Font _title = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
                //iTextSharp.text.Image _header = iTextSharp.text.Image.GetInstance("./img/protecta.png");
                //_header.BorderWidth = 0;
                //_header.Alignment = Element.ALIGN_RIGHT;
                //float percentage = 0.0f;
                //percentage = 150 / _header.Width;
                //_header.ScalePercent(percentage * 100);
                //doc.Add(_header);
                //Paragraph titulo = new Paragraph("Resultado de la búsqueda", _title);
                //doc.Add(new Paragraph("Resultado de la búsqueda", _title));
                //doc.Add(Chunk.NEWLINE);
                //PdfPTable table = new PdfPTable(1);
                //float[] widths = { 450f };
                //table.SetTotalWidth(widths);
                //PdfPCell clApellido = new PdfPCell(new Phrase("Apellido", _standardFont));
                //clApellido.BorderWidth = 0;
                //clApellido.BorderWidthBottom = 0.75f;
                //table.AddCell(clApellido);
                //for (int i = 0; i < 50; i++)
                //{
                //    PdfPCell a = new PdfPCell(new Phrase("Gon" + i, _standardFont));
                //    table.AddCell(a);
                //}
                //doc.Add(table);


                //iTextSharp.text.Image _footer = iTextSharp.text.Image.GetInstance("./img/Refinitiv.png");
                //_footer.BorderWidth = 0;
                //float percentage2 = 0.0f;
                //percentage2 = 150 / _footer.Width;
                //_footer.ScalePercent(percentage2 * 100);
                //_footer.SetAbsolutePosition(430, 25);
                //// Insertamos la imagen en el documento
                //doc.Add(_footer);
                //// Escribimos el encabezamiento en el documento
                ////

                //// Creamos una tabla que contendrá el nombre, apellido y país
                //// de nuestros visitante.
                ////PdfPTable tblPrueba = createTable();

                //// Finalmente, añadimos la tabla al documento PDF y cerramos el documento
                ////doc.Add(tblPrueba);

                //doc.Close();
                //writer.Close();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public PdfPTable createTable()
        {
            PdfPTable tblPrueba = new PdfPTable(3);
            // tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            //PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            // clNombre.BorderWidth = 0;
            // clNombre.BorderWidthBottom = 0.75f;

            // PdfPCell clApellido = new PdfPCell(new Phrase("Apellido", _standardFont));
            // clApellido.BorderWidth = 0;
            // clApellido.BorderWidthBottom = 0.75f;

            // PdfPCell clPais = new PdfPCell(new Phrase("País", _standardFont));
            // clPais.BorderWidth = 0;
            // clPais.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            // tblPrueba.AddCell(clNombre);
            // tblPrueba.AddCell(clApellido);
            // tblPrueba.AddCell(clPais);
            // Llenamos la tabla con información
            //clNombre = new PdfPCell(new Phrase("Roberto", _standardFont));
            // clNombre.BorderWidth = 0;

            // clApellido = new PdfPCell(new Phrase("Torres", _standardFont));
            // clApellido.BorderWidth = 0;

            // clPais = new PdfPCell(new Phrase("Puerto Rico", _standardFont));
            // clPais.BorderWidth = 0;
            // tblPrueba.AddCell(clNombre);
            // tblPrueba.AddCell(clApellido);
            // tblPrueba.AddCell(clPais);
            return tblPrueba;
        }
        public string gettestque()
        {
            System.Collections.Generic.Queue<int> q = new System.Collections.Generic.Queue<int>();
            int que = 0;
            int i = 10;
            q.Enqueue(0);
            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);
            q.Enqueue(4);
            q.Enqueue(5);
            q.Enqueue(6);
            q.Enqueue(7);
            q.Enqueue(8);
            q.Enqueue(9);
            while (q.Count > 0)
            {
                int num = q.Peek() % 2;
                if (num == 0)
                {
                    q.Dequeue();
                }
                else {

                    q.Dequeue();
                    q.Enqueue(i);
                    i++;
                }
            }
            return "";
        }
    }

}
