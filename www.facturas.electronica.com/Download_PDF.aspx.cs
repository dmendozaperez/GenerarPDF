using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using www.facturas.electronica.com.Bll;
namespace www.facturas.electronica.com
{
    public partial class Download_PDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            string _ruc = this.Request.Params["ruc"].ToString();//"20101951872";
            string _tipo_doc = this.Request.Params["tipo"].ToString();//"03";
            string _serie_doc = this.Request.Params["serie"].ToString();//"B424";
            string _num_doc = this.Request.Params["numero"].ToString();//43554;


            //string _ruc = "20101951872";
            //string _tipo_doc = "03";
            //string _serie_doc = "B030";
            //string _num_doc = "210";

            string _file_pdf = "";
            string _name_pdf = "";
            //string _tipo_doc = "03";
            //string _serie_doc = "B143";
            //decimal _num_doc = 1;
            Boolean _descarga = Basico.download_pdf(this,_ruc,_tipo_doc,_serie_doc,Convert.ToDecimal(_num_doc),ref _file_pdf,ref _name_pdf);

            if (_descarga)
            {
                Response.Clear();
                
                //string pdfPath = _rutapdf;
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(@_file_pdf);

                Response.AddHeader("Content-Disposition", "attachment; filename=" + _name_pdf);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
                Response.End();
                Response.Close();
            }

            
        }
    }
}