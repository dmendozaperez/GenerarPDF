using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Common.Entities.UBL;
using Bata.FEPE.TemplateEngine.Support.Sunat;
using System.Xml.Xsl;

namespace www.facturas.electronica.com.Bll
{
    
    public class Basico
    {
        private static string conexion = "Server=10.10.10.28;Database=FEPE_SC;User ID=dmendoza;Password=Bata2013;Trusted_Connection=False;";

        private static DataTable dt_getxmldoc_empresa(string _ruc,string _tipo_doc,string _ser_doc,Decimal _num_doc)
        {
            string sqlquery = "USP_GET_DOC_XML_EMPRESA";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(conexion);
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ruc", _ruc);
                cmd.Parameters.AddWithValue("@tip_doc", _tipo_doc);
                cmd.Parameters.AddWithValue("@ser_doc", _ser_doc);
                cmd.Parameters.AddWithValue("@num_doc", _num_doc);                
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }

        public static Boolean download_pdf(Page pag,string _ruc, string _tipo_doc, string _ser_doc, Decimal _num_doc,ref string _ruta_pdf,ref string _name_pdf)
        {
            Boolean _valida = false;
            DataTable dt = null;
            string _ruta_xslt = "";
            try
            {
                dt = dt_getxmldoc_empresa(_ruc, _tipo_doc, _ser_doc, _num_doc);
                if (dt!=null)
                {
                    if (dt.Rows.Count>0)
                    {
                        DateTime fecha_compro = Convert.ToDateTime(dt.Rows[0]["fechacomprobante"]);
                        string _anio = fecha_compro.Year.ToString();
                        string _mes = fecha_compro.Month.ToString().PadLeft(2, '0');

                        string _carpeta_default = "Descargar";
                        string _carpeta_pdf = _anio + _mes;
                        string _archivo_pdf = _ruc + "_" + _tipo_doc + "_" + _num_doc.ToString() + ".pdf";
                        string _archivo_xml = _ruc + "_" + _tipo_doc + "_" + _num_doc.ToString() + ".xml";
                        string _archivo_html = _ruc + "_" + _tipo_doc + "_" + _num_doc.ToString() + ".html";

                        _name_pdf = _archivo_pdf;

                        /*verificar si existe la carpeta*/
                        //string path = Path.Combine(Server.MapPath("~/imagenes/anuncios"), id_anun);
                        //string vrutaserver_folder=pag.Server.MapPath("../../" + _carpeta_default + "/" + _carpeta_pdf);

                        string vrutaserver_folder = pag.Server.MapPath("~/" + _carpeta_default + "/" + _carpeta_pdf);
                        string vrutaserver_site = pag.Server.MapPath("");

                        /*si la carpeta no existe entonces vamos crearlo*/
                        if (!Directory.Exists(@vrutaserver_folder))
                        {
                            Directory.CreateDirectory(@vrutaserver_folder);
                        }
                        /*ahora buscamos el archivo si existe*/
                        string _ruta_archivo = vrutaserver_folder + "\\" + _archivo_pdf;

                        if (File.Exists(_ruta_archivo))
                        {
                            /*si el archivo existe entonces quiere decir que ya fue generado y solo extraemos de la carpeta para descargar*/
                            _valida = true;
                            _ruta_pdf = _ruta_archivo;
                        }
                        else
                        {
                            string _path_file_xml = vrutaserver_folder + "\\" + _archivo_xml;
                            string _path_file_html = vrutaserver_folder + "\\" + _archivo_html;
                            Byte[] _xml =(Byte[]) dt.Rows[0]["documento"];
                            enviar_xml(_xml, _path_file_xml);

                            /*si existe el xml entonces se va generar el pdf*/
                            if (File.Exists(@_path_file_xml))
                            {
                                _ruta_xslt = _ruta_Xslt_doc(vrutaserver_site, _archivo_xml);
                                GeneratePDF(vrutaserver_site, _path_file_xml, _ruta_xslt, @vrutaserver_folder);
                                _valida = true;
                                _ruta_pdf = _ruta_archivo;
                                /*SI EL PDF SE GENERO CORRECTAMENTE ENTONCES BORRAMOS EL XML Y SU HTML*/
                                if (File.Exists(@_path_file_xml)) File.Delete(@_path_file_xml);
                                if (File.Exists(@_path_file_html)) File.Delete(@_path_file_html);

                            }

                        }

                    }
                }
            }
            catch(Exception exc)
            {
                dt = null;
            }
            return _valida;
        }
        private static string GetCdpType(XmlDocument xmlDocument)
        {
            new XmlNamespaceManager(xmlDocument.NameTable).AddNamespace(xmlDocument.DocumentElement.Prefix, xmlDocument.DocumentElement.NamespaceURI);
            if (xmlDocument.DocumentElement.LocalName.ToLower().Equals("invoice"))
            {
                XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("cbc:InvoiceTypeCode");
                if (elementsByTagName.Count > 0)
                    return elementsByTagName[0].InnerText ?? "";
            }
            else
            {
                if (xmlDocument.DocumentElement.LocalName.ToLower().Equals("creditnote"))
                    return 7.ToString();
                if (xmlDocument.DocumentElement.LocalName.ToLower().Equals("debitnote"))
                    return 8.ToString();
            }
            return "";
        }
        private static string TransformXMLToHTML(string inputXml, string xsltString)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(inputXml);
                string cdpType = GetCdpType(xmlDocument);
                XmlNamespaceManager xmlNamespaceManager = XmlNamespaceManagerFactory.ForPaymentReceiptFile(cdpType, xmlDocument.NameTable);
                SunatBarcode sunatBarcode = new SunatBarcodeFactory(cdpType, xmlNamespaceManager).Build(xmlDocument);
                XsltArgumentList arguments = new XsltArgumentList();
                arguments.AddParam("codigoBarras", string.Empty, (object)sunatBarcode.ToBase64());
                arguments.AddParam("hash", string.Empty, (object)sunatBarcode.DigestValue);
                XsltSettings settings = new XsltSettings(true, true);
                XslCompiledTransform compiledTransform = new XslCompiledTransform();
                using (XmlReader stylesheet = XmlReader.Create((TextReader)new StringReader(xsltString)))
                    compiledTransform.Load(stylesheet, settings, (XmlResolver)new XmlUrlResolver());
                StringWriter stringWriter = new StringWriter();
                using (XmlReader input = XmlReader.Create((TextReader)new StringReader(inputXml)))
                    compiledTransform.Transform(input, arguments, (TextWriter)stringWriter);
                return stringWriter.ToString();
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
            }
            return "";
        }
        private static void GeneratePDF(string _path_server,string strPathXML, string strPathXSLT, string strPathPDFFolder)
        {
            string path = strPathXML.ToLower().Replace(".xml", ".html");
            string pathPDF = System.IO.Path.Combine(strPathPDFFolder, System.IO.Path.GetFileName(strPathXML.ToLower().Replace(".xml", ".pdf")));
            pathPDF = pathPDF.ToUpper().ToString();
            string xsltString = File.ReadAllText(strPathXSLT);

            string _ruta_exe_local = _path_server;//System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            xsltString = xsltString.Replace("../ReferenceData/ISO.xslt", @_ruta_exe_local + "/ReferenceData/ISO.xslt");
            xsltString = xsltString.Replace("../ReferenceData/INEI.xslt", @_ruta_exe_local + "/ReferenceData/INEI.xslt");

            string str = TransformXMLToHTML(File.ReadAllText(strPathXML, Encoding.GetEncoding("iso8859-1")), xsltString);
            File.WriteAllText(path, str);
            //this.HtmlToPDF(str, pathPDF);
            //GetPDF(str);
            var htmlContent = str;

            //var htmlContent = String.Format(str,
            //DateTime.Now);

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            htmlToPdf.PageHeight = 242;
            htmlToPdf.PageWidth = 170;
            var margins = new NReco.PdfGenerator.PageMargins();
            margins.Bottom = 2;
            margins.Top = 1;
            margins.Left = 2;
            margins.Right = 5;
            htmlToPdf.Margins = margins;
            htmlToPdf.Orientation = NReco.PdfGenerator.PageOrientation.Portrait;
            htmlToPdf.Zoom = 1F;
            htmlToPdf.Size = NReco.PdfGenerator.PageSize.A4;

            //htmlToPdf.Orientation = NReco.PdfGenerator.PageOrientation.Portrait;
            //htmlToPdf.Margins = new NReco.PdfGenerator.PageMargins { Top = 25, Bottom = 25, Left = 25, Right = 25 };
            //htmlToPdf.Zoom = 2.88f;
            //htmlToPdf.CustomWkHtmlArgs = "--encoding UTF-8";
            var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);

            string _archivo_pdf = strPathPDFFolder + "\\" + System.IO.Path.GetFileName(strPathXML.ToLower().Replace(".xml", ".pdf"));

            _archivo_pdf = _archivo_pdf.ToUpper().ToString();

            System.IO.File.WriteAllBytes(@_archivo_pdf, pdfBytes);

            //File.Delete(path);
            //Console.Write("Listo");
        }
        private static string _ruta_Xslt_doc(string _ruta_server,string _archivo, string _emp = "E")
        {
            string _ruta_xslt = "";
            string _ruta_exe_local = _ruta_server;// System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                string _tipo = _archivo.Substring(12, 2);

                if (_emp == "E")
                {
                    switch (_tipo)
                    {
                        case "01":
                            //factura
                            return _ruta_exe_local + "\\XSLT\\FA_FA.xslt";

                        case "03":
                            //boleta
                            return _ruta_exe_local + "\\XSLT\\BO_BO.xslt";
                        case "07":
                            //nota de credito
                            return _ruta_exe_local + "\\XSLT\\NC_NC.xslt";
                        case "08":
                            //nota de debito
                            return _ruta_exe_local + "\\XSLT\\ND_ND.xslt";
                    }
                }
                if (_emp == "T")
                {
                    switch (_tipo)
                    {
                        case "01":
                            //factura
                            return _ruta_exe_local + "\\XSLT\\FA.xslt";

                        case "03":
                            //boleta
                            return _ruta_exe_local + "\\XSLT\\BO.xslt";
                        case "07":
                            //nota de credito
                            return _ruta_exe_local + "\\XSLT\\NC.xslt";
                        case "08":
                            //nota de debito
                            return _ruta_exe_local + "\\XSLT\\ND.xslt";
                    }
                }
            }
            catch
            {
                throw;
            }
            return _ruta_xslt;
        }
        private static string _code_page = "ISO-8859-1";
        private static void enviar_xml(Byte[] _xml, string _ruta_xml)
        {
            try
            {
                string str = GZipDecompress(_xml);

                if (str.Length == 0) return;

                string path1 = _ruta_xml;// @"D:\prueba.xml";

                using (FileStream fileStream = new FileStream(path1, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream, Encoding.GetEncoding(_code_page)))
                    {

                        streamWriter.Write(str);

                    }
                }
            }
            catch
            {

            }
        }
        private static string GZipDecompress(byte[] zippedData)
        {
            string str1 = string.Empty;
            try
            {
                MemoryStream memoryStream = new MemoryStream(zippedData);
                GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Decompress);
                StreamReader streamReader = new StreamReader((Stream)gzipStream, Encoding.GetEncoding("ISO-8859-1"));
                str1 = streamReader.ReadToEnd();
                streamReader.Close();
                gzipStream.Close();
                memoryStream.Close();
            }
            catch
            {
                str1 = "";
            }
            return str1;
        }
    }
}