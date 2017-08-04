using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Xml;
using Common.Entities.UBL;
using Bata.FEPE.TemplateEngine.Support.Sunat;
using System.Xml.Xsl;

namespace BataGeneraPDF
{
    /// <summary>
    /// Lógica de interacción para Generar_FE_XmlPdf.xaml
    /// </summary>
    public partial class Generar_FE_XmlPdf : Window
    {
        public Generar_FE_XmlPdf()
        {
            InitializeComponent();
        }
        private string _code_page = "ISO-8859-1";

        private string _empresa = "E";

        private Byte[] get_img(Decimal _id)
        {
            String sqlquery = "USP_GET_BYTES_XMLCDR";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            Byte[] _image = null;
            try
            {
                cn = new SqlConnection(conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _id);
                cmd.Parameters.Add("@IMG", SqlDbType.VarBinary,-1);
                cmd.Parameters["@IMG"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _image =(Byte[]) cmd.Parameters["@IMG"].Value;
            }
            catch(Exception exc)
            {
                _image = null;
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _image;
        }

        private DataTable get_compro_anexoID(Boolean _rango_fecha,DateTime _fechaini,DateTime _fechafin,
                                             String  _tip_doc,String _ser_doc, Decimal _num_ini,Decimal _nu_fin)
        {
            String sqlquery = "[GET_XML_DOC]";
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
                cmd.Parameters.AddWithValue("@ran_fecha", _rango_fecha);
                cmd.Parameters.AddWithValue("@fec_ini", _fechaini);
                cmd.Parameters.AddWithValue("@fec_fin",_fechafin);
                cmd.Parameters.AddWithValue("@tip_doc", _tip_doc);
                cmd.Parameters.AddWithValue("@ser_doc", _ser_doc);
                cmd.Parameters.AddWithValue("@num_ini", _num_ini);
                cmd.Parameters.AddWithValue("@num_fin", _nu_fin);                
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
        
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            string sqlquery = "GET_XML_DOC";
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
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt!=null)
                {
                    if ( dt.Rows.Count > 0)
                    {
                        Byte[] ima =(Byte[]) dt.Rows[0]["documento"];

                        string str = GZipDecompress(ima);

                        string path1 = @"D:\prueba.xml";

                        using (FileStream fileStream = new FileStream(path1, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream, Encoding.GetEncoding("ISO-8859-1")))
                            {
                                //if (isRespuesta)
                                //{
                                    //string str = UtilsStrings.GZipDecompress(resumen.XMLRespuesta);
                                    streamWriter.Write(str);
                                //}
                                //else
                                //{
                                //    string str = UtilsStrings.GZipDecompress(resumen.XMLEnviado);
                                //    streamWriter.Write(str);
                                //}
                            }
                        }

                        //string _ruta = @"D:\archivo.rar";
                        //File.WriteAllBytes(_ruta,ima);
                    }
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private string conexion = ""; 

        //private string conexion = "Server=10.10.10.208;Database=BdTienda;User ID=dmendoza;Password=Bata2013;Trusted_Connection=False;";
        public static string GZipDecompress(byte[] zippedData)
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

        private Boolean _valida()
        {
            Boolean _valor = false;
            try
            {
                if (chkfecha.IsChecked.Value)
                {
                    if (dtpdesde.Text.Length==0)
                    {
                        MessageBox.Show("Selecione una fecha valida", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        dtpdesde.Focus();
                        _valor = true;
                        return _valor;
                    }
                    if (dtphasta.Text.Length == 0)
                    {
                        MessageBox.Show("Selecione una fecha valida", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        dtphasta.Focus();
                        _valor = true;
                        return _valor;
                    }

                    DateTime _fini = Convert.ToDateTime(dtpdesde.Text);
                    DateTime _ffin = Convert.ToDateTime(dtphasta.Text);

                    if (_fini>_ffin)
                    {
                        MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha final", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);                        
                        _valor = true;
                        return _valor;
                    }

                }
                else
                {
                    if (cmbdoc.Text.Length==0)
                    {
                        MessageBox.Show("Seleccione un tipo de documento", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        cmbdoc.Focus();
                        _valor = true;
                        return _valor;
                    }
                    string _ser = txtserie.Text.Trim() ;
                    if (_ser.Length==0)
                    {
                        MessageBox.Show("Ingrese el numero de serie", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtserie.Focus();
                        _valor = true;
                        return _valor;
                    }
                    if (_ser.Length < 4)
                    {
                        MessageBox.Show("La Serie debe de ser de 4 digitos", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtserie.Focus();
                        _valor = true;
                        return _valor;
                    }
                    if (Left(_ser,1)!="F" && Left(_ser, 1) != "B")
                    {
                        MessageBox.Show("La Serie debe de empezar con la letra F ó B", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtserie.Focus();
                        _valor = true;
                        return _valor;
                    }

                    Decimal num_ini,num_fin = 0;

                    Decimal.TryParse(txtdesde.Text.ToString(),out num_ini);
                    Decimal.TryParse(txthasta.Text.ToString(), out num_fin);

                    if (num_ini==0)
                    {
                        MessageBox.Show("Ingrese el numero de documento inicial", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtdesde.Focus();
                        _valor = true;
                        return _valor;
                    }
                    if (num_fin==0)
                    {
                        MessageBox.Show("Ingrese el numero de documento final", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txthasta.Focus();
                        _valor = true;
                        return _valor;
                    }
                    if (num_ini>num_fin)
                    {
                        MessageBox.Show("El numero de documento final debe de se mayor al inicial", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txthasta.Focus();
                        _valor = true;
                        return _valor;
                    }
                   

                }
                if (txtxml.Text.Length == 0)
                {
                    MessageBox.Show("Seleccione la ruta de destino", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    btnxml.Focus();
                    _valor = true;
                    return _valor;
                }
                if (!chkxml.IsChecked.Value && !chkpdf.IsChecked.Value && !chkcdr.IsChecked.Value)
                {
                    MessageBox.Show("Seleccione una opcion XML,PDF ó CDR", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    _valor = true;
                    return _valor;
                }
            }
            catch
            {

            }
            return _valor;
        }

        private void btngenerar_Click(object sender, RoutedEventArgs e)
        {
            if (_valida()) return;
            Mouse.OverrideCursor = Cursors.Wait;
            generar();
            Mouse.OverrideCursor = null;

            //SqlConnection Cn = null;
            //SqlCommand cmd = null;
            //SqlDataAdapter da = null;
            //string sqlquery = "select b.serial,b.serialnum,paydate,b.amount_ret retention ,b.vendnum supplier,name,b.doccode documentcode ,b.docnum documentnumber ,b.sunat_code ,b.invnum_serial ,a.invnum,a.docdate ,amount_inv Invoiceamount, b.status  from [ADONIS_DES]..[ADP6].[doc] a, [ADONIS_DES]..[ADP6].retention_pago_730 b, [ADONIS_DES]..[ADP6].vendmst c  where a.doccode=b.doccode_inv  and a.docnum=b.docnum_inv  and a.coynum=b.coynum  and b.vendnum=c.vendnum  and a.balstat='V'  and b.status<>'D' ";
            //try
            //{


            //}
            //catch
            //{

            //}
        }
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private string _ruta_Xslt_doc(string _archivo, string _emp = "E")
        {
            string _ruta_xslt = "";
            string _ruta_exe_local = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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

        private void enviar_xml(Byte[] _xml,string _ruta_xml)
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

        private void generar()
        {            

            string _ruta = txtxml.Text.ToString();
            string _xml_ruta = _ruta + "\\XML";
            string _pdf_ruta = _ruta + "\\PDF";
            string _cdr_ruta = _ruta + "\\CDR";
            string _ruta_xslt = "";
            try
            {

                if (optencomer.IsChecked.Value)
                {
                    _empresa = "E";
                    conexion = "Server=10.10.10.28;Database=FEPE_SC;User ID=dmendoza;Password=Bata2013;Trusted_Connection=False;";
                }
                else
                {
                    _empresa = "T";
                    conexion = "Server=10.10.10.250;Database=FEPE_SC;User ID=dmendoza;Password=Bata2013;Trusted_Connection=False;";
                }
                
                /*EXPORTAR XML DE LA FE */
                if (!Directory.Exists(_ruta)) Directory.CreateDirectory(_ruta);                
                if (chkxml.IsChecked.Value)                
                    if (!Directory.Exists(_xml_ruta)) Directory.CreateDirectory(_xml_ruta);
                
                if (chkpdf.IsChecked.Value)
                { 
                    if (!Directory.Exists(_pdf_ruta)) Directory.CreateDirectory(_pdf_ruta);
                    if (!Directory.Exists(_xml_ruta)) Directory.CreateDirectory(_xml_ruta);
                }
                if (chkcdr.IsChecked.Value)
                    if (!Directory.Exists(_cdr_ruta)) Directory.CreateDirectory(_cdr_ruta);


                decimal _num_ini = 0;
                decimal _num_fin = 0;

                Decimal.TryParse(txtdesde.Text, out _num_ini);
                Decimal.TryParse(txthasta.Text, out _num_fin);

                string tipo_doc = (cmbdoc.SelectedItem == null) ? "" : ((ComboBoxItem)cmbdoc.SelectedItem).Tag.ToString();

                

                DataTable dt_anexos = get_compro_anexoID(chkfecha.IsChecked.Value, Convert.ToDateTime(dtpdesde.Text), Convert.ToDateTime(dtphasta.Text), tipo_doc, txtserie.Text,_num_ini,_num_fin);

                if (dt_anexos!=null)
                {
                    if (dt_anexos.Rows.Count>0)
                    {
                        /**/
                        if (chkxml.IsChecked.Value || chkpdf.IsChecked.Value || chkcdr.IsChecked.Value)
                        {
                                DataRow[] filas_xml = null;
                               if (chkxml.IsChecked.Value && !chkcdr.IsChecked.Value)
                               {
                                    filas_xml = dt_anexos.Select("tipoAnexo='2'");
                               }
                               if (chkpdf.IsChecked.Value && chkxml.IsChecked.Value && !chkcdr.IsChecked.Value)
                               {
                                    filas_xml = dt_anexos.Select("tipoAnexo='2'");
                               }
                            if (!chkxml.IsChecked.Value && chkcdr.IsChecked.Value)
                               {
                                    filas_xml = dt_anexos.Select("tipoAnexo='3'");
                               }
                               if (chkxml.IsChecked.Value && chkcdr.IsChecked.Value)
                               {
                                    filas_xml = dt_anexos.Select("tipoAnexo='3' or tipoAnexo='2'");
                               }
                               if (chkpdf.IsChecked.Value && chkcdr.IsChecked.Value)
                               {
                                    filas_xml = dt_anexos.Select("tipoAnexo='3' or tipoAnexo='2'");
                               }

                            if (chkpdf.IsChecked.Value && !chkcdr.IsChecked.Value)
                            {
                                filas_xml = dt_anexos.Select("tipoAnexo='3' or tipoAnexo='2'");
                            }

                            if (filas_xml.Length>0)
                                {
                                    prbgenera.Minimum = 0;
                                    prbgenera.Maximum = 100;
                                    prbgenera.Value = 0;
                                    prbgenera.Visibility = Visibility.Visible;
                                    txtpor.Visibility = Visibility.Visible;
                                    txtgenera.Visibility = Visibility.Visible;
                                    double _max = filas_xml.Length;

                                    double value = 0;
                                    double pro = 0;

                                    UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(prbgenera.SetValue);

                                    //txtgenera.Text = "Generando XML";

                                    foreach (DataRow fila in filas_xml)
                                    {
                                        Decimal _id =Convert.ToDecimal(fila["id"]);
                                        string _tipo = fila["tipo"].ToString();
                                        string _name_archivo = fila["RUC"].ToString() + "-" + fila["idtipocomprobante"].ToString() + "-" + fila["serie"].ToString() + "-" + fila["numero"].ToString().PadLeft(8,'0') + ".xml";
                                        string _path_file_xml =((_tipo=="XML")? _xml_ruta:_cdr_ruta) + "/" + _name_archivo;
                                        Byte[] _xml =  get_img(_id);
                                        if (_xml!=null)
                                        enviar_xml(_xml, _path_file_xml);

                                        if (chkpdf.IsChecked.Value)
                                        { 
                                            if (File.Exists(_path_file_xml))
                                            { 
                                                _ruta_xslt = _ruta_Xslt_doc(_name_archivo,(_empresa=="E"?"E":"T"));
                                                if (_tipo=="XML")
                                                { 
                                                    this.GeneratePDF(_path_file_xml, _ruta_xslt, @_pdf_ruta);
                                                }
                                                string _html = _path_file_xml.Replace(".xml",".html");
                                                if (File.Exists(_html))
                                                {
                                                    File.Delete(_html);
                                                }

                                                //if (chkpdf.IsChecked.Value && !chkxml.IsChecked.Value)
                                                //{                                                
                                                //    if (File.Exists(_path_file_xml))
                                                //    {
                                                //         File.Delete(_path_file_xml);
                                                //    }
                                                //}
                                            }
                                        }
                                        value += 1;

                                        pro = Convert.ToInt32((value / _max) * 100);

                                        string _msg = "Generando documentos";

                                        //if (chkxml.IsChecked.Value && chkpdf.IsChecked.Value)
                                        //{
                                        //     _msg = "Generando xml,pdf...";
                                        //}
                                        //if (chkxml.IsChecked.Value && !chkpdf.IsChecked.Value)
                                        //{
                                        //    _msg = "Generando xml...";
                                        //}

                                        //if (!chkxml.IsChecked.Value && chkpdf.IsChecked.Value)
                                        //{
                                        //    _msg = "Generando pdf...";
                                        //}

                                        Dispatcher.Invoke(updatePbDelegate,
                                            System.Windows.Threading.DispatcherPriority.Background,
                                            new object[] { ProgressBar.ValueProperty, pro });

                                        txtgenera.Dispatcher.BeginInvoke(new Action<string>((message) =>
                                        {
                                            txtgenera.Text = _msg;//"Generando xml...";
                                        }), "");



                                    }

                                    //if (!chkxml.IsChecked.Value && chkpdf.IsChecked.Value)
                                    //{
                                    //    if (Directory.Exists(_xml_ruta)) Directory.CreateDirectory(_xml_ruta);
                                    //}

                                    int num2 = (int)MessageBox.Show("Todos los Documentos se generaron correctamente", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Information);
                                    prbgenera.Visibility = Visibility.Hidden;
                                    txtpor.Visibility = Visibility.Hidden;
                                    txtgenera.Visibility = Visibility.Hidden;
                                }                            

                        }
                        /*si es xml y pdf*/
                        if (chkxml.IsChecked.Value && chkpdf.IsChecked.Value && !chkcdr.IsChecked.Value)
                        {
                                

                        }

                    }
                    else
                    {
                        MessageBox.Show("No hay documentos en la base de datos para generar", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("No hay documentos en la base de datos para generar", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                //MessageBox.Show("ok");


                //FileInfo[] files = new DirectoryInfo(@_xml_ruta).GetFiles("*.xml");
                //if (files.Length > 0)
                //{
                //    string _ruta_xslt = "";
                //    prbgenera.Minimum = 0;
                //    prbgenera.Maximum = 100;
                //    prbgenera.Value = 0;
                //    prbgenera.Visibility = Visibility.Visible;
                //    txtpor.Visibility = Visibility.Visible;
                //    double _max = files.Length;

                //    double value = 0;
                //    double pro = 0;
                //    UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(prbgenera.SetValue);
                //    foreach (FileSystemInfo fileSystemInfo in files)
                //    {
                //        _ruta_xslt = _ruta_Xslt_doc(fileSystemInfo.Name);
                //        this.GeneratePDF(fileSystemInfo.FullName, _ruta_xslt, @_pdf_ruta);

                //        value += 1;

                //        pro = Convert.ToInt32((value / _max) * 100);

                //        Dispatcher.Invoke(updatePbDelegate,
                //            System.Windows.Threading.DispatcherPriority.Background,
                //            new object[] { ProgressBar.ValueProperty, pro });

                //    }

                //    int num2 = (int)MessageBox.Show("Todos los PDFs se generaron correctamente", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Information);
                //    prbgenera.Visibility = Visibility.Hidden;
                //    txtpor.Visibility = Visibility.Hidden;
                //}
                //else
                //{
                //    int num3 = (int)MessageBox.Show("No se encontraron XMLs en la carpeta seleccionada.", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Error);
                //int num2 = (int)MessageBox.Show("Ocurrio un error generando los PDFs: " + ex.Message, "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        public string GetCdpType(XmlDocument xmlDocument)
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
        private string TransformXMLToHTML(string inputXml, string xsltString)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(inputXml);
                string cdpType = this.GetCdpType(xmlDocument);
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
                MessageBox.Show(exc.Message);
            }
            return "";
        }
        private void GeneratePDF(string strPathXML, string strPathXSLT, string strPathPDFFolder)
        {
            string path = strPathXML.ToLower().Replace(".xml", ".html");
            string pathPDF = System.IO.Path.Combine(strPathPDFFolder, System.IO.Path.GetFileName(strPathXML.ToLower().Replace(".xml", ".pdf")));
            pathPDF = pathPDF.ToUpper().ToString();
            string xsltString = File.ReadAllText(strPathXSLT);

            string _ruta_exe_local = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            xsltString = xsltString.Replace("../ReferenceData/ISO.xslt", @_ruta_exe_local + "/ReferenceData/ISO.xslt");
            xsltString = xsltString.Replace("../ReferenceData/INEI.xslt", @_ruta_exe_local + "/ReferenceData/INEI.xslt");

            string str = this.TransformXMLToHTML(File.ReadAllText(strPathXML, Encoding.GetEncoding("iso8859-1")), xsltString);
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
        private  string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }
        private new string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chkxml.IsChecked = true;
            chkpdf.IsChecked = true;
            DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpdesde.Text = fecha.ToString();
            dtphasta.Text = DateTime.Today.ToString();
            defecto(0);
        }
        private void defecto(Int32 _estado)
        {
           switch(_estado)
            {
                case 0:
                    dtpdesde.IsEnabled = false;
                    dtphasta.IsEnabled = false;
                    cmbdoc.IsEnabled = true;
                    txtserie.IsEnabled = true;
                    txtdesde.IsEnabled = true;
                    txthasta.IsEnabled = true;
                    break;
                case 1:
                    dtpdesde.IsEnabled = true;
                    dtphasta.IsEnabled = true;
                    cmbdoc.IsEnabled = false;
                    txtserie.IsEnabled = false;
                    txtdesde.IsEnabled = false;
                    txthasta.IsEnabled = false;
                    break;
            }
        }

        private void chkfecha_Click(object sender, RoutedEventArgs e)
        {
            if (chkfecha.IsChecked.Value)
            {
                defecto(1);
            }
            else
            {
                defecto(0);
            }
            
        }

        private void txtdesde_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SoloNumeros(e);
        }
        public void SoloNumeros(TextCompositionEventArgs e)
        {
            //se convierte a Ascci del la tecla presionada
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            //verificamos que se encuentre en ese rango que son entre el 0 y el 9
            if ((ascci >= 48 && ascci <= 57) || (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtdesde_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                txthasta.Focus();
            }
        }

        private void txthasta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
            {
                e.Handled = true;
            }
        }

        private void txtserie_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtdesde.Focus();
            }
        }

        private void btnxml_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of FolderBrowserDialog.
            System.Windows.Forms.FolderBrowserDialog folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            // A new folder button will display in FolderBrowserDialog.
            folderBrowserDlg.ShowNewFolderButton = true;
            folderBrowserDlg.Description = "Por favor seleccione la carpeta a descargar";
            //Show FolderBrowserDialog
            System.Windows.Forms.DialogResult dlgResult = folderBrowserDlg.ShowDialog();
            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                //Show selected folder path in textbox1.
                txtxml.Text = folderBrowserDlg.SelectedPath;
                //Browsing start from root folder.
                //Environment.SpecialFolder rootFolder = folderBrowserDlg.RootFolder;
            }
        }

        private void btnsalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

