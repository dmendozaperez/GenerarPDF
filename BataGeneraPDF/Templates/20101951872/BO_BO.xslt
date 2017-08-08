<?xml version="1.0" encoding="utf-8"?>
<!--
Carvajal Tecnologi­a y Servicios S.A. (2015)

Producto: Factura Electronica Peru
-->
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl"
  xmlns:qdt="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2"
  xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
  xmlns:cts="urn:carvajal:names:specification:ubl:peru:schema:xsd:CarvajalAggregateComponents-1"
  xmlns:sac="urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1"
  xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
  xmlns:ds="http://www.w3.org/2000/09/xmldsig#"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:ccts="urn:un:unece:uncefact:documentation:2"
  xmlns:ext="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"
  xmlns:udt="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2"
  xmlns:r="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"
  xmlns:dt="urn:schemas-microsoft-com:datatypes"
  xmlns:iso="urn:iso:paises">

  <xsl:include href="../ReferenceData/ISO.xslt"/>
  <xsl:include href="../ReferenceData/INEI.xslt"/>

  <xsl:output method="html" indent="yes" />

  <!-- PARÁMETROS DE ENTRADA -->
  <xsl:param name="codigoBarras"  />
  <xsl:param name="hash"  />
  <xsl:decimal-format name="euro" decimal-separator="." grouping-separator=","/>

  <!-- HTML STYLES-->
  <xsl:template match="r:Invoice">
    <html>
      <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
        <title>Invoice</title>
        <style type="text/css">

          body { width: 740px; font-family: Arial; font-size:10px; margin-top: 0px; padding: 0; color: #000; }
          table {font-family:inherit; font-size:inherit; width: 100%;border-spacing: 0; cell-padding:0px; cell-spacing:0px; padding:0px;}

          #pagina {position:relative; width: 740px; height: 1080px;margin-left: 20px; top:20px;}
          #MARCA_AGUA{position: absolute; top:0px; left:0; width: 57px; height: 1065px; }
          #ENCADIV {position:relative; top: 0px; left:0px; width: 740px; padding:0}
          #DETAILDIV {position:relative; top: 0px; left:0px; width: 740px; margin:0; padding:0}
          #FINDIV {position: relative; top: 0px; left:90px; width: 650px; margin:0; padding:0}
          #RESDIV {position: relative; top: 0px; left:0px; width: 740px; margin:0; padding:0}
          #itemsDetail {font-size: 8px; text-align: center; width: 100%; border-collapse: collapse}

          .logo {height: 80px;}

          .paginacion{position:absolute;top:-15px;right:0px;text-align:right;font-size:11px;}
          .Separador_Filas{height:10px;}
          .Separador_Columnas{width:10px;}


          .Titulo_Tabla_Totales{font-family:inherit; font-size:inherit; font-weight:bold; width:60%;}
          .Valores_Tabla_Totales{font-family:inherit; font-size:inherit; font-weight:normal; text-align:right; width:60%;}


          .Titulo_Info_RUC{font-family:inherit; font-size:22px; font-weight:bold;text-align:center; vertical-align:middle; }
          .Titulo_Encb{font-family:inherit; font-size:inherit; text-align:center; }
          .Titulo_Encb1{font-family:inherit; font-size:inherit; font-weight:bold;}
          .Titulo_Encb2{font-family:inherit; font-size:inherit; }
          .Titulo_Detalle{font-family:inherit; font-size:inherit; font-weight:bold;text-align:center; }
          .ItemDetalle{vertical-align:top;}


          .BorSolid{border:1px solid #000;}
          .BorDer{border-right:1px solid #000;}
          .BorIzq{border-left:1px solid #000;}
          .BorArr{border-top:1px solid #000;}
          .BorAba{border-bottom:1px solid #000;}

          .col1{word-wrap:break-word;border-color:#000; width:74px}<!--NÚMERO DE GUÍA-->
          .col2{word-wrap:break-word;border-color:#000; width:70px}<!--CÓDIGO DE ARTÍCULO-->
          .col3{word-wrap:break-word;border-color:#000; width:205px}<!--DESCRIPCIÓN-->
          .col4{word-wrap:break-word;border-color:#000; width:50px}<!--PIEZAS-->
          .col5{word-wrap:break-word;border-color:#000; width:45px}<!--CANTIDAD-->
          .col6{word-wrap:break-word;border-color:#000; width:30px}<!--UND-->
          .col7{word-wrap:break-word;border-color:#000; width:71px}<!--PRECIO  UNITARIO-->
          .col8{word-wrap:break-word;border-color:#000; width:70px}<!--VALOR UNITARIO-->
          .col9{word-wrap:break-word;border-color:#000; width:30px}<!--DTO-->
          .col10{word-wrap:break-word;border-color:#000; width:70px}<!--TOTAL-->
          .col11{border-color:#000; width:65px; text-align:left;}
          .col12{border-color:#000; width:65px; text-align:right;}

          .fuente18{font-size:18px}
          .fuente16{font-size:16px}
          .fuente15{font-size:15px}
          .fuente13{font-size:13px}
          .fuente12{font-size:12px}
          .fuente11{font-size:11px}
          .fuente10{font-size:10px}
          .fuente9{font-size:9px}
          .fuente8{font-size:8px}
          .fuente7{font-size:7px}
          .fuente6{font-size:6px}

          .negrilla{font-weight:bold;}

          .color{background-color: #D1D0D0;}

          .bordeSolido{border: 2px solid #000000;}
          .bordeDer{border-right:2px solid #000000;}
          .bordeIzq{border-left:2px solid #000000;}
          .bordeArr{border-top:2px solid #000000;}
          .bordeAba{border-bottom:2px solid #000000;}

        </style>
      </head>
      <body>

        <xsl:variable name="_todo">//TODO: Mapear Campo</xsl:variable>

        <!-- Inicio Variables Documento Factura / Boleta -->
        <xsl:variable name="Campo1_RazonSocialEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName"/>
        </xsl:variable>
        <xsl:variable name="Campo2_DireccionEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:StreetName"/>
        </xsl:variable>
        <xsl:variable name="Campo3_UrbanizacionEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CitySubdivisionName"/>
        </xsl:variable>
        <xsl:variable name="Campo4_DistritoEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:District"/>
        </xsl:variable>
        <xsl:variable name="Campo5_ProvinciaEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CityName"/>
        </xsl:variable>
        <xsl:variable name="Campo6_DepartamentoEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CountrySubentity"/>
        </xsl:variable>
        <xsl:variable name="Campo7_PaisEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode"/>
        </xsl:variable>
        <xsl:variable name="Campo8_RUCEmisor">
          <xsl:value-of select="/*/cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID"/>
        </xsl:variable>
        <xsl:variable name="Campo9_TipoDocumentoEmisor">
          <xsl:variable name="Temp" select="/*/cbc:InvoiceTypeCode" />
          <xsl:choose>
            <xsl:when test="$Temp='01'">FACTURA ELECTRÓNICA</xsl:when>
            <xsl:when test="$Temp='03'">BOLETA DE VENTA ELECTRÓNICA</xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo10_NumeroDocumentoEmisor">
          <xsl:value-of select="/*/cbc:ID"/>
        </xsl:variable>
        <xsl:variable name="Campo11_TipoDocumentoReceptor">
          <xsl:variable name="Temp" select="/*/cac:AccountingCustomerParty/cbc:AdditionalAccountID" />
          <xsl:choose>
            <xsl:when test="$Temp='0'">SIN RUC :</xsl:when>
            <xsl:when test="$Temp='1'">D.N.I. :</xsl:when>
            <xsl:when test="$Temp='4'">C.D.E. :</xsl:when>
            <xsl:when test="$Temp='6'">R.U.C. :</xsl:when>
            <xsl:when test="$Temp='7'">PASS. :</xsl:when>
            <xsl:when test="$Temp='A'">C.D.I. :</xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo12_NumeroDocumentoReceptor">
          <xsl:value-of select="/*/cac:AccountingCustomerParty/cbc:CustomerAssignedAccountID"/>
        </xsl:variable>
        <xsl:variable name="Campo13_RazonSocialReceptor">
          <xsl:value-of select="/*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName"/>
        </xsl:variable>
        <xsl:variable name="Campo14_DireccionReceptor">
          <xsl:value-of select="/*/cac:AccountingCustomerParty/cac:Party/cac:PhysicalLocation/cbc:Description"/>
        </xsl:variable>
        <xsl:variable name="Campo15_FechaEmision">
          <xsl:value-of select="/*/cbc:IssueDate"/>
        </xsl:variable>

        <xsl:variable name="Campo16_TipoMoneda">
          <xsl:variable name="codMoneda" select="/*/cbc:DocumentCurrencyCode"/>
          <xsl:call-template name="REF_NOMBRE_MONEDA">
            <xsl:with-param name="codigoMoneda" select="$codMoneda"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Campo17_Ordencompra">
          <xsl:value-of select="/*/cac:OrderReference / cbc:ID"/>
        </xsl:variable>
        <xsl:variable name="Campo18_CondicionPago">
          <xsl:value-of select="/*/cac:PaymentTerms/cbc:ID"/>
        </xsl:variable>
        <xsl:variable name="Campo22_TipoGuiaRemision">
          <xsl:value-of select="/*/cac:DespatchDocumentReference/cbc:DocumentTypeCode"/>
        </xsl:variable>
        <xsl:variable name="Campo23_NumeroGuiaRemision">
          <xsl:value-of select="/*/cac:DespatchDocumentReference/cbc:ID"/>
        </xsl:variable>
        <xsl:variable name="Campo24_FechaEmisionRemision">
          <xsl:value-of select="/*/cac:DespatchDocumentReference/cbc:IssueDate"/>
        </xsl:variable>
        <xsl:variable name="Campo34_MontoenLetras">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=1000]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo35_LeyendaTranferenciaGratuita">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=1002]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo37_OperacionGratuita">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal[cbc:ID=1004]/cbc:PayableAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo38_OperacionExonerada">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal[cbc:ID=1003]/cbc:PayableAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo39_OperacionInafecta">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal[cbc:ID=1002]/cbc:PayableAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo40_OperacionGravada">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal[cbc:ID=1001]/cbc:PayableAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo41_TotalDescuentos">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal[cbc:ID=2005]/cbc:PayableAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo42_OtrosCargos">
          <xsl:value-of select="/*/cac:LegalMonetaryTotal/cbc:ChargeTotalAmount"/>
        </xsl:variable>
        <xsl:variable name="Campo43_OtrosTributos">
          <xsl:value-of select="/*/cac:TaxTotal/cbc:TaxAmount"/>
        </xsl:variable>
        <xsl:variable name="Campo44_ISC">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme[cbc:ID=2000]/../../../cbc:TaxAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo45_IGV">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme[cbc:ID=1000]/../../../cbc:TaxAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo46_ImporteTotal">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//cac:LegalMonetaryTotal/cbc:PayableAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo47_Percepcion">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal[cbc:ID=2001]/cbc:PayableAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo48_TotalconPercepcion">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal[cbc:ID=2001]/sac:TotalAmount,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo67_NumerolaresolucionemitidaporlaSUNATparaelemisor">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Header/cts:TaxAuthorityResolutionNumber"/>
        </xsl:variable>
        <xsl:variable name="Campo68_TefefonoEmisor">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0002]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo69_SitioWeb">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0003]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo70_N_DOC.INT">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0004]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo71_CODIGOCLI.">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0005]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo72_VENDEDOR">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0006]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo79_IGV">
          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0016]/cbc:Value"/>
        </xsl:variable>
        <xsl:variable name="Campo80_ImporteNCoSF">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0030]/cbc:Value,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Campo83_TOTAL">
          <xsl:variable name="Temp">
            <xsl:value-of select="format-number(//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0033]/cbc:Value,'#,##0.00','euro')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$Temp!='' and $Temp!='NaN'">
              <xsl:value-of select="$Temp"/>
            </xsl:when>
            <xsl:otherwise>0.00</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <!-- Fin Variables Documento Factura / Boleta -->

        <xsl:variable name="ArraySeparator" select="' - '" />
        <xsl:variable name="ListSeparator" select="','" />


        <xsl:variable name="Campo_3_4_5_6_7_Temp">
          <xsl:variable name="Temp" select="concat($Campo3_UrbanizacionEmisor,$ArraySeparator,$Campo4_DistritoEmisor,$ArraySeparator, $Campo5_ProvinciaEmisor,$ArraySeparator, $Campo6_DepartamentoEmisor,$ArraySeparator, $Campo7_PaisEmisor)"></xsl:variable>


          <xsl:call-template name="string-replace-all">
            <xsl:with-param name="replace" select="concat($ArraySeparator,$ArraySeparator)"/>
            <xsl:with-param name="text" select="$Temp"/>
            <xsl:with-param name="by" select="$ArraySeparator"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:variable name="Campo34567_Direccion">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo_3_4_5_6_7_Temp" />
            <xsl:with-param name ="string_to_remove" select="$ArraySeparator" />
          </xsl:call-template>
        </xsl:variable>


        <xsl:variable name="Campo20_DocsModificados">

          <xsl:variable name="Temp">
            <xsl:for-each select="/*/cac:DiscrepancyResponse">
              <xsl:if test="position()>1">
                <xsl:value-of select="$ListSeparator"/>
              </xsl:if>
              <xsl:value-of select="cbc:ReferenceID"/>
            </xsl:for-each>
          </xsl:variable>

          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Temp"  />
            <xsl:with-param name="string_to_remove" select="$ListSeparator"  />
          </xsl:call-template>
        </xsl:variable>



        <!--<xsl:variable name="booEsNota" select="not(//cbc:InvoiceTypeCode)" />-->
        <xsl:variable name="booEsNota" select="not(//cbc:InvoiceTypeCode)" />

        <xsl:variable name="Campo36Temp">
          <xsl:for-each select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID>='0017' and cbc:ID&lt;='0020']">
            <xsl:if test="cbc:Value != ''">
              <xsl:value-of select="cbc:Value" />
              <xsl:value-of select="$ListSeparator" />
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="Campo36_Observaciones">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo36Temp" />
            <xsl:with-param name ="string_to_remove" select="$ListSeparator" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Campo73Temp">
          <xsl:for-each select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID>='0007' and cbc:ID&lt;='0009']">
            <xsl:if test="cbc:Value != ''">
              <xsl:value-of select="cbc:Value" />
              <xsl:value-of select="$ListSeparator" />
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="Campo73_Observaciones_de_cabecera">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo73Temp" />
            <xsl:with-param name ="string_to_remove" select="$ListSeparator" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Campo74Temp">
          <xsl:for-each select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID>='0010' and cbc:ID&lt;='0012']">
            <xsl:if test="cbc:Value != ''">
              <xsl:value-of select="cbc:Value" />
              <xsl:value-of select="$ListSeparator" />
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="Campo74_Observaciones_de_cabecera2">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo74Temp" />
            <xsl:with-param name ="string_to_remove" select="$ListSeparator" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Campo75Temp">
          <xsl:for-each select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID>='0013' and cbc:ID&lt;='0015']">
            <xsl:if test="cbc:Value != ''">
              <xsl:value-of select="cbc:Value" />
              <xsl:value-of select="$ListSeparator" />
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="Campo75_Observaciones_de_pie_de_pagina">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo75Temp" />
            <xsl:with-param name ="string_to_remove" select="$ListSeparator" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Campo76Temp">
          <xsl:for-each select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID>='0021' and cbc:ID&lt;='0023']">
            <xsl:if test="cbc:Value != ''">
              <xsl:value-of select="cbc:Value" />
              <xsl:value-of select="$ListSeparator" />
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="Campo76_Observaciones_de_pie_de_pagina2">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo76Temp" />
            <xsl:with-param name ="string_to_remove" select="$ListSeparator" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Campo77Temp">
          <xsl:for-each select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID>='0024' and cbc:ID&lt;='0026']">
            <xsl:if test="cbc:Value != ''">
              <xsl:value-of select="cbc:Value" />
              <xsl:value-of select="$ListSeparator" />
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="Campo77_Observaciones_de_pie_de_pagina3">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo77Temp" />
            <xsl:with-param name ="string_to_remove" select="$ListSeparator" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Campo78Temp">
          <xsl:for-each select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID>='0027' and cbc:ID&lt;='0029']">
            <xsl:if test="cbc:Value != ''">
              <xsl:value-of select="cbc:Value" />
              <xsl:value-of select="$ListSeparator" />
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="Campo78_Observaciones_de_pie_de_pagina4">
          <xsl:call-template name="trim-cad">
            <xsl:with-param name="text" select="$Campo78Temp" />
            <xsl:with-param name ="string_to_remove" select="$ListSeparator" />
          </xsl:call-template>
        </xsl:variable>


        <xsl:variable name="Campo82_SimboloMoneda">
          <xsl:variable name="Temp" select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID='0032']/cbc:Value" />
          <xsl:choose>
            <xsl:when test="$Temp='PEN'">S/</xsl:when>
            <xsl:when test="$Temp='USD'">$</xsl:when>
          </xsl:choose>
        </xsl:variable>


        <div  id="SuprCont" style="position:absolute;visibility:hidden; top:0px; left:0px;">

          <!-- PAGINACIÓN-->
          <div id="paginador" class="paginacion">
            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
          </div>
          <!--FIN PAGINACIÓN-->

          <div id="MARCA_AGUA" >
            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
          </div>

          <div id="ENCADIV">

            <table width="100%">
              <tr>
                <td>
                  <table>
                    <tr>
                      <!--LOGO-->
                      <td class="bordeSolido" width="170px">
                        <table>
                          <tr>
                            <td align="left" >
                              <Img src="data:image/jpg;base64,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"/>
                            </td>
                          </tr>
                        </table>
                      </td>
                      <!--FIN LOGO-->

                      <td class="Separador_Columnas"></td>

                      <!--INFORMACIÓN DEL EMISOR-->
                      <td style="width:250px;">
                        <table width="100%">
                          <tr>
                            <td>
                            </td>
                          </tr>
                          <tr>
                            <td style="text-align:left" class="fuente18 negrilla">
                              <xsl:value-of select="$Campo1_RazonSocialEmisor" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="text-align:left" class="fuente10">
                              <xsl:value-of select="$Campo2_DireccionEmisor" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="text-align:left" class="fuente10">
                              <xsl:value-of select="$Campo34567_Direccion" />
                              <!--<xsl:value-of select="$Campo3_3_4_5_6_7"/>-->
                            </td>
                          </tr>
                          <tr>
                            <td style="text-align:left" class="fuente10">
                              TELF. : <xsl:value-of select="$Campo68_TefefonoEmisor" />
                            </td>
                          </tr>
                          <tr>
                            <td style="text-align:left" class="fuente10">
                              SITIO WEB : <xsl:value-of select="$Campo69_SitioWeb" />
                            </td>
                          </tr>
                        </table>
                      </td>
                      <!--FIN INFORMACIÓN DEL EMISOR-->

                      <td class="Separador_Columnas"></td>

                      <!--INFORMACÓN DEL VALIDACIÓN GUÍA DE REMISIÓN REMITENTE-->
                      <td style="width:274px; padding:0;" class="bordeSolido">
                        <table width="100%" >
                          <tr  class="Separador_Filas">
                            <td class="Titulo_Info_RUC fuente18 negrilla">
                              R.U.C. N° <xsl:value-of select="$Campo8_RUCEmisor" />
                            </td>
                          </tr>
                          <tr  class="Separador_Filas">
                            <td></td>
                          </tr>
                          <tr>
                            <td class="Titulo_Info_RUC  fuente18 negrilla">
                              <xsl:value-of select="$Campo9_TipoDocumentoEmisor" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr  class="Separador_Filas">
                            <td></td>
                          </tr>
                          <tr>
                            <td class="Titulo_Info_RUC fuente18 negrilla">
                              N° <xsl:value-of select="$Campo10_NumeroDocumentoEmisor" />
                            </td>
                          </tr>
                          <tr  class="Separador_Filas">
                            <td></td>
                          </tr>
                        </table>
                      </td>
                      <!--FIN INFORMACÓN DEL VALIDACIÓN GUÍA DE REMISIÓN REMITENTE-->
                    </tr>
                  </table>
                </td>
              </tr>

              <tr>
                <td class="Separador_Filas">
                </td>
              </tr>

              <!--INFORMACIÓN DEL CUADRO ENCABEZADO 1-->
              <tr>
                <td>
                  <table>
                    <td style="vertical-align:top">
                      <table  width="100%"  style="border:solid 2px" height="100%">
                        <tr>
                          <td width="25%" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            <xsl:value-of select="$Campo11_TipoDocumentoReceptor" /> :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo12_NumeroDocumentoReceptor" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            SEÑORES (ES) :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo13_RazonSocialReceptor" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:center" class="fuente8 negrilla color">
                            DIRECCIÓN :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:center" class="fuente8">
                            <xsl:value-of select="$Campo14_DireccionReceptor" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            CÓDIGO CLI. :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo71_CODIGOCLI." />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            N° DOC INT. :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo70_N_DOC.INT" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                      </table>
                    </td>

                    <td class="Separador_Columnas"></td>

                    <td width="278px">
                      <table style="border:solid 2px; padding:0;">
                        <tr>
                          <td width="100px" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            FECHA DE EMISIÓN :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo15_FechaEmision" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            TIPO DE MONEDA :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo16_TipoMoneda" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            IGV :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo79_IGV" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            ORDEN DE COMPRA :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo17_Ordencompra" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            VENDEDOR :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:value-of select="$Campo72_VENDEDOR" />
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </td>
                        </tr>
                        <tr>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                            CONDICIÓN DE PAGO :
                          </td>
                          <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8">
                            <xsl:if test="$booEsNota=false()">
                              <xsl:value-of select="$Campo18_CondicionPago" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </xsl:if>
                          </td>
                        </tr>
                      </table>
                    </td>

                  </table>
                </td>
              </tr>
              <!--INFORMACIÓN DEL CUADRO ENCABEZADO 1-->

              <tr>
                <td class="Separador_Filas" >
                </td>
              </tr>

              <tr>
                <td>
                  <table  width="100%" style="border:solid 2px">
                    <tr>
                      <td colspan="2" width="100%" height="20px" style="vertical-align:top;text-align: left" class="fuente8 bordeAba">
                        <xsl:value-of select="$Campo73_Observaciones_de_cabecera" />
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </td>
                    </tr>
                    <tr>
                      <td colspan="2" width="100%" height="20px" style="vertical-align:top;text-align: left" class="fuente8">
                        <xsl:value-of select="$Campo74_Observaciones_de_cabecera2" />
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </td>
                    </tr>
                    <!--Inicio Solo Para Nota Credito y Nota Debito-->
                    <xsl:if test="$booEsNota=true()">
                      <tr>
                        <td width="12%" style="vertical-align:top;text-align: left" class="fuente8 negrilla bordeArr BorAba BorDer color" >
                          MOTIVO :
                        </td>
                        <td style="vertical-align:top;text-align: left" class="fuente8 BorAba bordeArr" >
                          (21)<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2" width="100%" style="vertical-align:top;text-align: left" class="fuente8 negrilla bordeAba color" >
                          TIPO DE (19) Y EL NUMERO QUE MODIFICA :
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2" width="100%" height="20px" style="vertical-align:top;text-align: left" class="fuente8" >
                          <xsl:value-of select="$Campo20_DocsModificados" />
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        </td>
                      </tr>
                    </xsl:if>
                    <!--Fin Solo Para Nota Credito y Nota Debito-->
                  </table>
                </td>
              </tr>

              <tr class="Separador_Filas">
                <td >
                </td>
              </tr>

            </table>

          </div>

          <div  id="DETAILDIV">

            <table id="itemsDetail" style="border:solid 2px">
              <tr id="trtitulo">
                <td class="bordeIzq bordeArr bordeDer bordeAba col2 Titulo_Detalle color">
                  ARTÍCULO
                </td>
                <td class="bordeArr bordeAba bordeDer col3 Titulo_Detalle color">
                  DESCRIPCIÓN
                </td>
                <td class="bordeArr bordeAba bordeDer col5 Titulo_Detalle color">
                  CANTIDAD
                </td>
                <td class="bordeArr bordeAba bordeDer col6 Titulo_Detalle color">
                  UNIDAD
                </td>
                <td class="bordeArr bordeAba bordeDer col8 Titulo_Detalle color">
                  VALOR VENTA UNITARIO
                </td>
                <td class="bordeArr bordeAba bordeDer col7 Titulo_Detalle color">
                  PRECIO DE VENTA UNITARIO
                </td>
                <td class="bordeArr bordeAba bordeDer col9 Titulo_Detalle color">
                  DESCUENTO UNITARIO
                </td>
                <td class="bordeArr bordeAba bordeDer col10 Titulo_Detalle color">
                  VALOR VENTA
                </td>
              </tr>

              <xsl:for-each select="/*/cac:InvoiceLine">
                <tr>
                  <td style="text-align:center" class="bordeIzq BorDer BorAba fuente8 col2 ItemDetalle">
                    <xsl:value-of select="cac:Item/cac:SellersItemIdentification/cbc:ID"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </td>
                  <td style="text-align:left" class="BorDer BorAba fuente8 col3 ItemDetalle">
                    <xsl:value-of select="cac:Item/cbc:Description"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </td>
                  <td style="text-align:center" class="BorDer BorAba fuente8 col5 ItemDetalle">
                    <xsl:value-of select="cbc:InvoicedQuantity"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </td>
                  <td style="text-align:center" class="BorDer BorAba fuente8 col6 ItemDetalle">
                    <xsl:value-of select="cbc:InvoicedQuantity/@unitCode"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </td>
                  <td style="text-align:center" class="BorDer BorAba fuente8 col8 ItemDetalle">
                    <xsl:value-of select="cac:Price/cbc:PriceAmount"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </td>
                  <td style="text-align:center" class="BorDer BorAba fuente8 col7 ItemDetalle">
                    <xsl:variable name="Temp" select="count(cac:PricingReference/cac:AlternativeConditionPrice)" ></xsl:variable>
                    <xsl:choose>
                      <xsl:when test="$Temp = 2">
                        <xsl:value-of select="cac:PricingReference/cac:AlternativeConditionPrice[cbc:PriceTypeCode=02]/cbc:PriceAmount"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="cac:PricingReference/cac:AlternativeConditionPrice/cbc:PriceAmount"/>
                      </xsl:otherwise>
                    </xsl:choose>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </td>
                  <td style="text-align:center" class="BorDer BorAba fuente8 col9 ItemDetalle">
                    <xsl:variable name="ID" select="cbc:ID" />
                    <xsl:choose>
                      <xsl:when test="$booEsNota=true()">
                        <xsl:variable name="ChargeIndicator" select="cac:AllowanceCharge/cbc:ChargeIndicator"></xsl:variable>
                        <xsl:if test="$ChargeIndicator='false'">
                          <!--<xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Items/cts:Item/cts:DiscountCharge/cts:Amount"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>-->
                          <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Items/cts:Item[cts:LineNumber=$ID]/cts:DiscountCharge/cts:Amount"/>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        </xsl:if>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="/*/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Items/cts:Item[cts:LineNumber=$ID]/cts:DiscountCharge/cts:Amount"/>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                  <td style="text-align:center" class="BorAba bordeDer fuente8 col10 ItemDetalle">
                    <xsl:value-of select="cbc:LineExtensionAmount"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </td>
                </tr>
              </xsl:for-each>

              <tr id="RellenoDetail">
                <td style="text-align:center" class="bordeIzq BorDer fuente8">
                </td>
                <td style="text-align:left" class="BorDer fuente8">
                </td>
                <td style="text-align:center" class="BorDer fuente8">
                </td>
                <td style="text-align:center" class="BorDer fuente8">
                </td>
                <td style="text-align:center" class="BorDer fuente8">
                </td>
                <td style="text-align:center" class="BorDer fuente8">
                </td>
                <td style="text-align:center" class="BorDer fuente8">
                </td>
                <td style="text-align:center" class="BorDer bordeDer fuente8">
                </td>
              </tr>

              <tr id ="ultimoregistro">
                <td class="bordeSolido BorDer BorAba bordeArr fuente8" colspan="8" style="padding:0">
                  <table  width="100%">
                    <tr>
                      <td width="5%" style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8 negrilla color" rowspan="2">
                        SON :
                      </td>
                      <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 color">
                        <xsl:value-of select="$Campo34_MontoenLetras" />
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </td>
                    </tr>
                    <tr>
                      <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 color">
                        <xsl:value-of select="$Campo35_LeyendaTranferenciaGratuita" />
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </td>
                    </tr>
                    <tr>
                      <td height="40px" colspan="2" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color bordeArr">
                        <xsl:value-of select="$Campo36_Observaciones" />
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>

          </div>

          <!--#REGION QUE SE PINTA SÓLO AL FINAL-->
          <!--<div id="FINDIV">
            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
          </div>-->

          <div id="RESDIV">
            <table>
              <tr>
                <td>

                </td>
              </tr>

              <tr class="Separador_Filas">
                <td colspan="6">
                </td>
              </tr>

              <tr>
                <td>
                  <table align="left" class="bordeSolido" bordercolor="#000000">
                    <tr>
                      <td style="width:67%; height:112px" align="left">
                        <table width="100%" height="112px" >
                          <tr>
                            <td height="39px" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 BorIzq BorDer BorArr BorAba">
                              <xsl:value-of select="$Campo75_Observaciones_de_pie_de_pagina" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td height="39px" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 BorIzq BorDer BorAba ">
                              <xsl:value-of select="$Campo76_Observaciones_de_pie_de_pagina2" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td height="39px" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 BorIzq BorDer BorAba">
                              <xsl:value-of select="$Campo77_Observaciones_de_pie_de_pagina3" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td height="39px" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 BorIzq BorDer BorAba">
                              <xsl:value-of select="$Campo78_Observaciones_de_pie_de_pagina4" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                        </table>
                      </td>
                      <td>
                        <table>
                          <tr>
                            <td width="90px" style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              OP. GRATUITAS
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo37_OperacionGratuita" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              OP. EXONERADAS
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo38_OperacionExonerada" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              OP. INAFECTAS
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo39_OperacionInafecta" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              OP. GRAVADAS
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo40_OperacionGravada" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              TOT. DSCTO.
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo41_TotalDescuentos" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              I.S.C.
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo44_ISC" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              I.G.V.
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                              <xsl:value-of select="$Campo45_IGV" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>
                          <tr>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla BorAba color">
                              IMPORTE TOTAL
                            </td>
                            <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla BorAba color">
                              :
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8 BorAba">
                              <xsl:value-of select="$Campo82_SimboloMoneda" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                            <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8 BorAba">
                              <xsl:value-of select="$Campo46_ImporteTotal" />
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            </td>
                          </tr>

                          <!-- Inicio Solo Para Factura y Boleta-->
                          <xsl:if test="$booEsNota=false()">
                            <tr>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                Importe NC o SF (-)
                              </td>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                :
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo82_SimboloMoneda" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo80_ImporteNCoSF" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                            </tr>
                            <tr>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                TOTAL
                              </td>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                :
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo82_SimboloMoneda" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo83_TOTAL" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                            </tr>
                            <tr>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                PERCEPCION
                              </td>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                :
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo82_SimboloMoneda" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo47_Percepcion" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                            </tr>
                            <tr>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                TOT. CON PERCEP.
                              </td>
                              <td style="word-wrap:break-word;text-align:left;vertical-align:top" class="fuente8 negrilla color">
                                :
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo82_SimboloMoneda" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                              <td style="word-wrap:break-word;text-align:right;vertical-align:top" class="fuente8">
                                <xsl:value-of select="$Campo48_TotalconPercepcion" />
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              </td>
                            </tr>
                          </xsl:if>
                          <!-- Fin Solo Para Factura y Boleta-->
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>

              <tr class="Separador_Filas">
                <td colspan="6">
                </td>
              </tr>

              <tr>
                <td>
                  <table align="left">
                    <tr>
                      <td class="bordeIzq bordeArr bordeDer bordeAba">
                        <table style="text-align:center" class="fuente8">
                          <tr>
                            <td height="15xp" class="fuente8 negrilla">
                              REPRESENTACIÓN IMPRESA DE <xsl:value-of select="$Campo9_TipoDocumentoEmisor" />. ESTA PUEDE
                            </td>
                          </tr>
                          <tr>
                            <td height="15xp" class="fuente8 negrilla">
                              SER CONSULTADA EN WWW.BATA.COM.PE
                            </td>
                          </tr>
                          <tr>
                            <td height="15xp" class="fuente8 negrilla">
                              AUTORIZADO MEDIANTE RESOLUCIÓN N.° <xsl:value-of select="$Campo67_NumerolaresolucionemitidaporlaSUNATparaelemisor" /> /SUNAT
                            </td>
                          </tr>
                          <tr>
                            <td height="10xp"></td>
                          </tr>
                        </table>
                      </td>

                      <td class="Separador_Columnas"></td>

                      <td align="center">
                        <img width="280px" height="50px" src="data:image/png;base64,{$codigoBarras}"/>
                      </td>

                    </tr>
                  </table>
                </td>
              </tr>

            </table>
          </div>

        </div>

        <div id="Contenedor" style="position:absolute;top:0px;left:0px;visibility:visible">

          <script type="text/javascript">
            <!--Versión Lógica 3.55-->

            var Pags = 0;
            var hEncabezado = 0;
            var hPie = 0;

            var hDocumento = 1080; //Recuadro exterior
            var hPagina = 1130; //Altura de la Pagina
            var hSeparacionPaginas=0;
            var hRelleno=0;
            var hTituloColumnas=0;
            var hTitulo1=0;
            var hUltimoRegistro=0;
            var hDisponible=0;
            var hFinDiv=0;

            var TotalItems= document.getElementById("itemsDetail").rows.length -2;  //-2 Corresponde al relleno y al ultimoregistro

            var hFila=0;
            var indexFila=1; //Se supone que la tabla de los detalles solo tiene 1 fila de titulos
            var objTitulo1=null;
            var booErrorFatal=false;

            var booFinPintado=false;
            var booCabeFin;

            <!--Se asignan los tamaños del Encabezado y el Pie de Página como diseño-->

            hSeparacionPaginas=hPagina-hDocumento;

            hEncabezado = document.getElementById("ENCADIV").offsetHeight+5; //El 5 fue por tanteo, se debe verificar otra opción para que el codigo de barras no quede sobre la linea final

            hPie = document.getElementById("RESDIV").offsetHeight;

            objTitulo1=document.getElementById("trtitulo1");
            if(objTitulo1!=null)
            {
            hTitulo1=document.getElementById("trtitulo1").offsetHeight;
            }

            hTituloColumnas = document.getElementById("trtitulo").offsetHeight + hTitulo1;
            hUltimoRegistro= document.getElementById("ultimoregistro").offsetHeight;

            if (document.getElementById("FINDIV")==null)
            {
            booFinPintado=true;
            }else
            {
            hFinDiv=document.getElementById("FINDIV").offsetHeight;
            }


            do
            {
            Pags++;

            <!--Este calculo es necesario porque en algunas plantillas se utiliza el relleno para mostrar otra información-->
            hRelleno=document.getElementById("RellenoDetail").offsetHeight;


            <!--Se Obtiene el valor en Pixeles para la zona de Detalle-->
            hDisponible = hDocumento - (hEncabezado + hPie + hUltimoRegistro + hRelleno + hTituloColumnas);

            <!--OJOOOOOOOOOOOOOOOOOOOOO ELIMINARRRRRRRRR-->
            <!--document.write(hDocumento +"," + hEncabezado +", "+ hDisponible +", "+ hPie);-->
            <!--document.write("Pags : " +Pags);-->

            <!--OJOOOOOOOOOOOOOOOOOOOOO ELIMINARRRRRRRRR-->
            <!--document.write(hDocumento +",Enc: " + hEncabezado +", Detalle: "+ hDisponible +", Pie:"+ hPie  +", uLTrEG:"+ hUltimoRegistro +", hTituloColumnas="+ hTituloColumnas +", Relleno= "+ hRelleno);-->

            <!--OJOOOOOOOOOOOOOOOOOOOOO ELIMINARRRRRRRRR-->
            <!--document.write(Pags);-->
            <xsl:text disable-output-escaping="yes"><![CDATA[if(hDisponible<0)]]></xsl:text>
            {
            document.write("<br/>No se puede generar el reporte porque la suma de tamaños del encabezado y el resumen superan el tamaño vertical de la página");
            document.write("<br/>" + hDocumento +",Enc: " + hEncabezado +", Detalle: "+ hDisponible +", Pie:"+ hPie  +", uLTrEG:"+ hUltimoRegistro +", hTituloColumnas="+ hTituloColumnas +", Relleno= "+ hRelleno);
            booErrorFatal=true;
            break;
            }

            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("<div id='pagina'>");]]></xsl:text>
            var rect = document.getElementById("pagina").getBoundingClientRect();
            hDisponible-=rect.top;

            <!--Crear el objeto de Paginacion-->
            var objPaginador=document.getElementById("paginador");

            if(objPaginador!=null)
            {
            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("<div id='paginador_"+Pags +"' class='paginacion'>");]]></xsl:text>
            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("</div>");]]></xsl:text>
            }


            <!--fin del objeto de Paginacion-->


            <!--inicia container para centrar el contenido-->

            <!--zona constructora de #REGION MARCA_AGUA-->
            var marca=document.getElementById("MARCA_AGUA");

            if(marca!=null)
            {
            document.write(document.getElementById("MARCA_AGUA").outerHTML);
            }
            <!--fin zona constructora de #REGION MARCA_AGUA-->

            <!--zona constructora de #REGION HEADER-->
            document.write(document.getElementById("ENCADIV").outerHTML);
            <!--fin zona constructora de #REGION HEADER-->


            <!--zona constructora de #REGION DETAIL-->
            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("<div id='DETAILDIV'>");]]></xsl:text>


            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("<table id='itemsDetail' >");]]></xsl:text>


            document.write(document.getElementById("trtitulo").outerHTML);
            <xsl:text disable-output-escaping="yes"><![CDATA[if(hTitulo1 > 0)]]></xsl:text>
            {
            document.write(document.getElementById("trtitulo1").outerHTML);
            indexFila++;
            }



            <xsl:text disable-output-escaping="yes"><![CDATA[for( count = indexFila; count < TotalItems; count++)]]></xsl:text>
            {
            hFila = document.getElementById("itemsDetail").rows[count].offsetHeight;

            <xsl:text disable-output-escaping="yes"><![CDATA[if(hFila <hDisponible)]]></xsl:text>
            {

            document.write(document.getElementById("itemsDetail").rows[count].outerHTML);
            indexFila++;

            hDisponible -= hFila;

            }<!--cierra if-->
            else
            {
            break;
            }

            }<!--cierra for-->


            if(booErrorFatal==true)
            {
            break;
            }
            <!--inicia relleno-->



            hRelleno += hDisponible;

            <!--OJOOOOOOOOOOOOOOOOOOOOO ELIMINARRRRRRRRR-->
            <!--document.write("Relleno:" + hRelleno);-->

            booCabeFin=true;

            <xsl:text disable-output-escaping="yes"><![CDATA[if (booFinPintado==false)]]></xsl:text>
            {
            <xsl:text disable-output-escaping="yes"><![CDATA[if(indexFila == TotalItems)]]></xsl:text>
            {
            <xsl:text disable-output-escaping="yes"><![CDATA[if(hRelleno>=hFinDiv)]]></xsl:text> <!--Si Cabe el FindDiv en el relleno-->
            {
            hRelleno -=hFinDiv;
            }else
            {
            booCabeFin=false;
            }
            }
            }


            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("<tr style='height:"+hRelleno+"px'>");]]></xsl:text>
            document.write(document.getElementById("RellenoDetail").innerHTML);
            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("</tr>");]]></xsl:text>

            <!--fin relleno-->
            document.write(document.getElementById("ultimoregistro").outerHTML);
            <xsl:text disable-output-escaping="yes"><![CDATA[document.write('</table>');]]></xsl:text>

            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("</div>");]]></xsl:text>
            <!--fin zona constructora de #REGION DETAIL-->

            <!--Verificar si se puede pintar el FINDIV-->
            <xsl:text disable-output-escaping="yes"><![CDATA[if (booFinPintado==false)]]></xsl:text>
            {
            <xsl:text disable-output-escaping="yes"><![CDATA[if(indexFila == TotalItems)]]></xsl:text>
            {
            <xsl:text disable-output-escaping="yes"><![CDATA[if (booCabeFin)]]></xsl:text>
            {
            booFinPintado=true;


            <!--zona constructora de #REGION FINDIV-->
            document.write(document.getElementById("FINDIV").outerHTML);
            <!--fin zona constructora de #REGION FINDIV-->
            }

            }
            }

            <!--zona constructora de #REGION FOOTER-->
            document.write(document.getElementById("RESDIV").outerHTML);
            <!--fin zona constructora de #REGION FOOTER-->

            <!--cierra pagina-->
            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("</div>");]]></xsl:text>
            <xsl:text disable-output-escaping="yes"><![CDATA[if(indexFila < TotalItems)]]></xsl:text>
            {
            <xsl:text disable-output-escaping="yes"><![CDATA[document.write("<div style='height:"+hSeparacionPaginas+"px'></div>");]]></xsl:text>
            }
            }
            <xsl:text disable-output-escaping="yes"><![CDATA[while ((indexFila < TotalItems)||(booFinPintado==false))]]></xsl:text>

            <xsl:text disable-output-escaping="yes"><![CDATA[for( count = 1; count <= Pags; count++)]]></xsl:text>
            {
            var objPaginador=document.getElementById("paginador_"+ count);

            if(objPaginador!=null)
            {
            objPaginador.innerHTML="Página "+ count +" de " + Pags;
            }else
            {
            break;
            }
            }
          </script>

        </div>

      </body>
    </html>
  </xsl:template>

  <!-- REEMPLAZAR STRING -->
  <xsl:template name="string-replace-all">
    <xsl:param name="text" />
    <xsl:param name="replace" />
    <xsl:param name="by" />
    <xsl:choose>
      <xsl:when test="contains($text, $replace)">
        <xsl:variable name="TempBefore" select="substring-before($text,$replace)"/>
        <xsl:variable name="TempAfter" select="concat($by,substring-after($text,$replace))"/>
        <xsl:value-of select="$TempBefore" />
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text" select="$TempAfter" />
          <xsl:with-param name="replace" select="$replace" />
          <xsl:with-param name="by" select="$by" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- FIN TEMPLATE REEMPLAZAR STRING -->

  <!-- Eliminar la cadena especificada al inicio y el fin de otra -->
  <xsl:template name="trim-cad">
    <xsl:param name="text" />
    <xsl:param name="string_to_remove" />

    <xsl:variable name="Temp1">
      <xsl:choose>
        <xsl:when test="starts-with($text, $string_to_remove) ">
          <xsl:value-of select="substring($text, string-length($string_to_remove)+1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$text" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="LastPos" select="string-length($Temp1)- string-length($string_to_remove)" />

    <xsl:variable name="Temp2">
      <xsl:choose>
        <xsl:when test="substring($Temp1, $LastPos + 1)=$string_to_remove ">
          <xsl:value-of select="substring($Temp1, 1,$LastPos)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$Temp1" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$Temp2"/>
  </xsl:template>
  <!-- FIN TEMPLATE REEMPLAZAR TRIM-CAD -->

  <!-- Template que hace la funcionalidad del SPLIT -->
  <xsl:template name="string-split">
    <!--passed template parameter -->
    <xsl:param name="list"/>
    <xsl:param name="delimiter"/>
    <xsl:choose>
      <xsl:when test="contains($list, $delimiter)">
        <tag>
          <!-- get everything in front of the first delimiter -->
          <xsl:value-of select="substring-before($list,$delimiter)"/>
        </tag>
        <xsl:call-template name="string-split">
          <!-- store anything left in another variable -->
          <xsl:with-param name="list" select="substring-after($list,$delimiter)"/>
          <xsl:with-param name="delimiter" select="$delimiter"/>
        </xsl:call-template>

      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$list = ''">
            <xsl:text/>
          </xsl:when>
          <xsl:otherwise>
            <tag>
              <xsl:value-of select="$list"/>
            </tag>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- FIN TEMPLATE SPLIT -->



</xsl:stylesheet>