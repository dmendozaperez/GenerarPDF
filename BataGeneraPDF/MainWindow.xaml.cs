using Bata.FEPE.TemplateEngine.Support.Sunat;
using Carvajal.FEPE.PDFService.Core.Services;
using Carvajal.FEPE.TemplateEngine.Mapper;
using Carvajal.FEPE.TemplateEngine.Support.Sunat;
using Common.Entities.UBL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Xsl;

namespace BataGeneraPDF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnejcutar_Click(object sender, RoutedEventArgs e)

        {
            //string text1 = @"D:\David\Generador PDF\PDFGenerator\XSLT\FA_FA.xslt"; //this.textBoxTemplatePath.Text;
            //string text2 = @"D:\David\Generador PDF\PDFGenerator\XML";//this.textBoxXMLFolderPath.Text;
            //string text3 = @"D:\David\Generador PDF\PDFGenerator\PDF";//this.textBoxPDFFolderPath.Text;
            //if (string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text1))
            //{
            //    int num1 = (int)MessageBox.Show("Todos los campos deben tener información", "Error");
            //}
            //else
            //{
            //    try
            //    {
            //        FileInfo[] files = new DirectoryInfo(text2).GetFiles("*.xml");
            //        if (files.Length > 0)
            //        {
            //            prbgenera.Value = 0;
            //            prbgenera.Maximum = files.Length;
            //            foreach (FileSystemInfo fileSystemInfo in files)
            //                this.GeneratePDF(fileSystemInfo.FullName, text1, text3);
            //            prbgenera.Value += 1;
            //            int num2 = (int)MessageBox.Show("Todos los PDFs se generaron correctamente", "Información");
            //        }
            //        else
            //        {
            //            int num3 = (int)MessageBox.Show("No se encontraron XMLs en la carpeta seleccionada.", "Información");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        int num2 = (int)MessageBox.Show("Ocurrio un error generando los PDFs: " + ex.Message, "Error");
            //    }
            //}
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
                //XmlDocument xmlDocument = new XmlDocument();
                //xmlDocument.LoadXml(inputXml);
                //string cdpType = this.GetCdpType(xmlDocument);
                //XmlNamespaceManager xmlNamespaceManager = XmlNamespaceManagerFactory.ForPaymentReceiptFile(cdpType, xmlDocument.NameTable);
                //SunatBarcode sunatBarcode = new SunatBarcodeFactory(cdpType, xmlNamespaceManager).Build(xmlDocument);
                //XsltArgumentList arguments = new XsltArgumentList();
                //arguments.AddParam("codigoBarras", string.Empty, (object)sunatBarcode.ToBase64());
                //arguments.AddParam("hash", string.Empty, (object)sunatBarcode.DigestValue);
                //XsltSettings settings = new XsltSettings(true, true);
                //XslCompiledTransform compiledTransform = new XslCompiledTransform();
                //using (XmlReader stylesheet = XmlReader.Create((TextReader)new StringReader(xsltString)))
                //    compiledTransform.Load(stylesheet, settings, (XmlResolver)new XmlUrlResolver());
                //StringWriter stringWriter = new StringWriter();
                //using (XmlReader input = XmlReader.Create((TextReader)new StringReader(inputXml)))
                //    compiledTransform.Transform(input, arguments, (TextWriter)stringWriter);
                //return stringWriter.ToString();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            return "";
        }
        private void GeneratePDF(string strPathXML, string strxml_new, string strPathPDFFolder, string ruc, string tipo, string plantilla)
        {
            string path = strPathXML.ToLower().Replace(".xml", ".html");
            string pathPDF = System.IO.Path.Combine(strPathPDFFolder, System.IO.Path.GetFileName(strPathXML.ToLower().Replace(".xml", ".pdf")));
            pathPDF = pathPDF.ToUpper().ToString();
            //string xsltString = File.ReadAllText(strPathXSLT);

            //string _ruta_exe_local = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //xsltString = xsltString.Replace("../ReferenceData/ISO.xslt", @_ruta_exe_local + "/ReferenceData/ISO.xslt");
            //xsltString = xsltString.Replace("../ReferenceData/INEI.xslt", @_ruta_exe_local + "/ReferenceData/INEI.xslt");


            //string str = this.TransformXMLToHTML(File.ReadAllText(strPathXML, Encoding.GetEncoding("iso8859-1")), xsltString);

            PdfGenerator genera_html = new PdfGenerator();

            string str = genera_html.GeneratePdfFromXmlContent(strxml_new, ruc, tipo, plantilla);



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

        private void btnsalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnxml_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of FolderBrowserDialog.
            System.Windows.Forms.FolderBrowserDialog folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            // A new folder button will display in FolderBrowserDialog.
            folderBrowserDlg.ShowNewFolderButton = true;
            folderBrowserDlg.Description = "Por favor seleccione la carpeta Origen XML";
            //Show FolderBrowserDialog
            System.Windows.Forms.DialogResult dlgResult = folderBrowserDlg.ShowDialog();
            if (dlgResult== System.Windows.Forms.DialogResult.OK)
            {
                //Show selected folder path in textbox1.
                txtxml.Text = folderBrowserDlg.SelectedPath;
                //Browsing start from root folder.
                //Environment.SpecialFolder rootFolder = folderBrowserDlg.RootFolder;
            }
        }

        private void btnpdf_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of FolderBrowserDialog.
            System.Windows.Forms.FolderBrowserDialog folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            // A new folder button will display in FolderBrowserDialog.
            folderBrowserDlg.ShowNewFolderButton = true;
            folderBrowserDlg.Description = "Por favor seleccione la carpeta Destino PDF";
            //Show FolderBrowserDialog
            System.Windows.Forms.DialogResult dlgResult = folderBrowserDlg.ShowDialog();
            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                //Show selected folder path in textbox1.
                txtpdf.Text = folderBrowserDlg.SelectedPath;
                //Browsing start from root folder.
                //Environment.SpecialFolder rootFolder = folderBrowserDlg.RootFolder;
            }
        }

        private void btngenerar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            generar();
            Mouse.OverrideCursor = null;
        }
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void generar()
        {
            //prbgenera.Minimum = 0;
            //prbgenera.Maximum = short.MaxValue;
            //prbgenera.Value = 0;

            //double value = 0;
            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            //UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(prbgenera.SetValue);

            //Tight Loop:  Loop until the ProgressBar.Value reaches the max
            //do
            //{
            //    value += 1;
            //    Dispatcher.Invoke(updatePbDelegate,
            //        System.Windows.Threading.DispatcherPriority.Background,
            //        new object[] { ProgressBar.ValueProperty, value });

            //}
            //while (prbgenera.Value != prbgenera.Maximum);


            //return;
            if (txtxml.Text.Length==0)
            {
                MessageBox.Show("Ingrese La ruta del xml a generar...", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                btnxml.Focus();
                return;
            }
            if (txtpdf.Text.Length == 0)
            {
                MessageBox.Show("Ingrese La ruta del pdf a generar...", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                btnpdf.Focus();
                return;
            }

            string _xml_ruta = txtxml.Text.ToString();
            string _pdf_ruta = txtpdf.Text.ToString();
          
            try
            {
                FileInfo[] files = new DirectoryInfo(@_xml_ruta).GetFiles("*.xml");
                if (files.Length > 0)
                {
                    string _ruta_xslt = "";
                    prbgenera.Minimum = 0;
                    prbgenera.Maximum =100;
                    prbgenera.Value = 0;
                    prbgenera.Visibility = Visibility.Visible;
                    txtpor.Visibility = Visibility.Visible;
                    double _max = files.Length;

                    double value = 0;
                    double pro = 0;
                    UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(prbgenera.SetValue);
                    foreach (FileSystemInfo fileSystemInfo in files)
                    {
                        string _tipo_compro = fileSystemInfo.Name.Substring(12,2);
                        string _ruc= fileSystemInfo.Name.Substring(0, 11);
                        string _plantilla = "";
                        switch (_tipo_compro)
                        {
                            /*FACTURA*/
                            case "01":
                                _plantilla = "FA";
                                break;
                            /*BOLETA*/
                            case "03":
                                _plantilla = "BO";
                                break;
                            /*NOTE DE CREDITO*/
                            case "07":
                                _plantilla = "NC";
                                break;
                            /*NOTA DE DEBITO*/
                            case "08":
                                _plantilla = "ND";
                                break;
                            /*RETENCION*/
                            case "20":
                                _plantilla = "20";
                                break;
                        }

                        string xmlstr = File.ReadAllText(@fileSystemInfo.FullName);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(xmlstr);

                        GenericDocumentMapper formato_new = new GenericDocumentMapper();
                        XmlDocument xmlnew = new XmlDocument();
                        xmlnew = formato_new.Transform(xmldoc);

                        this.GeneratePDF(fileSystemInfo.FullName, xmlnew.InnerXml, @_pdf_ruta, _ruc, _tipo_compro, _plantilla);

                        //_ruta_xslt = _ruta_Xslt_doc(fileSystemInfo.Name);
                        //    this.GeneratePDF(fileSystemInfo.FullName, _ruta_xslt, @_pdf_ruta);

                        value += 1;

                        pro =Convert.ToInt32((value / _max) * 100);

                        Dispatcher.Invoke(updatePbDelegate,
                            System.Windows.Threading.DispatcherPriority.Background,
                            new object[] { ProgressBar.ValueProperty, pro });

                    }
                   
                    int num2 = (int)MessageBox.Show("Todos los PDFs se generaron correctamente", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Information);
                    prbgenera.Visibility = Visibility.Hidden;
                    txtpor.Visibility = Visibility.Hidden;
                }
                else
                {
                    int num3 = (int)MessageBox.Show("No se encontraron XMLs en la carpeta seleccionada.", "Aviso del sistema...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                int num2 = (int)MessageBox.Show("Ocurrio un error generando los PDFs: " + ex.Message, "Aviso del sistema...", MessageBoxButton.OK,MessageBoxImage.Error);
            }
            

        }
        private string _ruta_Xslt_doc(string _archivo,string _emp="E")
        {
            string _ruta_xslt = "";
            string _ruta_exe_local = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
            try
            {
                string _tipo = _archivo.Substring(12, 2);
               
                if (_emp=="E")
                { 
                    switch (_tipo)
                    {
                        case "01":
                            //factura
                           return  _ruta_exe_local + "\\XSLT\\FA_FA.xslt";
                       
                        case "03":
                            //boleta
                            return  _ruta_exe_local + "\\XSLT\\BO_BO.xslt";                                                
                        case "07":
                            //nota de credito
                           return  _ruta_exe_local + "\\XSLT\\NC_NC.xslt";                        
                        case "08":
                            //nota de debito
                            return _ruta_exe_local + "\\XSLT\\ND_ND.xslt";                        
                    }
                }
                if (_emp=="T")
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

        private void ColumnDefinition_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string _ruta_xml_defecto = @"C:\temp\xml";
            string _ruta_pdf_defecto = @"C:\temp\pdf";

            if (!Directory.Exists(_ruta_xml_defecto)) Directory.CreateDirectory(_ruta_xml_defecto);
            if (!Directory.Exists(_ruta_pdf_defecto)) Directory.CreateDirectory(_ruta_pdf_defecto);

            txtpdf.Text = _ruta_pdf_defecto;
            txtxml.Text = _ruta_xml_defecto;    
        }
    }
}
