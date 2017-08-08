<?xml version="1.0" encoding="utf-8"?>
<!--
Carvajal Tecnología y Servicios S.A. (2014)

Producto:             Factura Electrónica Perú v1.3
Código de Plantilla:  FPEX01
Tipo de Comprobante:  Factura
Cliente:              yyyy
Empresa:              xxxx
Diseñador:            Jhon Doe
Última Actualización: 26/09/2014
Descripción:          zzzzz
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
  xmlns:r="urn:oasis:names:specification:ubl:schema:xsd:DebitNote-2">

  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" encoding="utf-8" />
  
  <!-- PARÁMETROS DE ENTRADA -->
  <xsl:param name="codigoBarras"  />
  <xsl:param name="hash"  />

  <!-- ESPECIFICACIÓN PLANTILLA -->
  <xsl:variable name="anchoLinea">45</xsl:variable>
  <xsl:variable name="tamanoPagina">A4</xsl:variable>
  <xsl:variable name="tipoAjuste">W</xsl:variable>

  <!-- VARIABLES DE DISEÑO DE LA PLANTILLA -->
  <xsl:variable name="lineasPorPagina" select="5" />
  <xsl:variable name="cantidadItemsDetalle" select="count(/r:DebitNote/cac:DebitNoteLine)" />
  <xsl:variable name="totalPaginas" select="ceiling($cantidadItemsDetalle div $lineasPorPagina)" />
  <xsl:variable name="soloUnItemDetallePorPagina" select="$lineasPorPagina = 1" />
  
  <!-- DATOS DE REFERENCIA -->
  <xsl:include href="../ReferenceData/ISO.xslt"/>
  
  <!-- HTML -->
  <xsl:template match="r:DebitNote">
    <html>
      <head>
        <title>Nota Débito</title>
        <meta http-equiv="content-type" content="text/html; charset=utf-8"/>
        <style type="text/css">
          .pagina {
          background-color: white;
          width: 100%;
          height: 800px;
          margin: 5px 0px;
          border-style: solid;
          border-width: 1px;
          border-color: black;
          }
          .encabezado {
          background-color: lightblue;
          width: 100%;
          margin: 5px 0px;
          border-style: solid;
          border-width: 1px;
          border-color: black;
          }
          .pie {
          background-color: lightgreen;
          width: 100%;
          margin: 5px 0px 150px 0px;
          border-style: solid;
          border-width: 1px;
          border-color: black;
          }
          .item {
          background-color: yellow;
          width: 100%;
          margin: 0px 0px;
          border-style: solid;
          border-width: 1px;
          border-color: black;
          }
          .codigoBarras {
          height: 41px;
          width: 413px
          }
        </style>
      </head>
      <body>
        <div>
          <!-- ESTRUCTURA DE PÁGINAS -->
          <xsl:call-template name="ESTRUCTURA_PAGINAS" />
        </div>
      </body>
    </html>
  </xsl:template>

  <!-- ESTRUCTURA DE PÁGINAS -->
  <xsl:template name="ESTRUCTURA_PAGINAS">    
    <xsl:for-each select="cac:DebitNoteLine">
      <!-- ENCABEZADO DE PÁGINA -->
      <xsl:call-template name="ENCABEZADO_DE_PAGINA" />

      <!-- ITEM DETALLE -->
      <xsl:call-template name="ITEMS_DETALLE" />

      <!-- PIE DE PÁGINA -->
      <xsl:call-template name="PIE_DE_PAGINA" />
    </xsl:for-each>
  </xsl:template>

  <!-- ENCABEZADO DE PÁGINA -->
  <xsl:template name="ENCABEZADO_DE_PAGINA">
    <xsl:variable name="esPrimerItemDetalleDeLaPagina" select="position() mod $lineasPorPagina = 1" />

    <xsl:choose>
      <xsl:when test="$soloUnItemDetallePorPagina">
        <xsl:call-template name="SECCION_ENCABEZADO_DE_PAGINA" />
      </xsl:when>
      <xsl:when test="$esPrimerItemDetalleDeLaPagina">
        <xsl:call-template name="SECCION_ENCABEZADO_DE_PAGINA" />
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- SECCIÓN ENCABEZADO DE PÁGINA -->
  <xsl:template name="SECCION_ENCABEZADO_DE_PAGINA">
    <div class="encabezado">
      <xsl:call-template name="SECCION_DATOS_COMPROBANTE" />
    </div>
  </xsl:template>

  <!-- ITEMS DE DETALLE -->
  <xsl:template name="ITEMS_DETALLE" match="cac:DebitNoteLine">
    <div class="item">
      ID: <xsl:value-of select="cbc:ID"/>
      <br/>
      DebitedQuantity: <xsl:value-of select="cbc:DebitedQuantity"/>
      <br/>
      PriceAmount: <xsl:value-of select="cac:Price/cbc:PriceAmount"/>
    </div>
  </xsl:template>

  <!-- PIE DE PÁGINA -->
  <xsl:template name="PIE_DE_PAGINA">
    <xsl:variable name="esUltimoItemDetalleDeLaPagina" select="position() mod $lineasPorPagina = 0" />
    <xsl:variable name="esUltimoItemDetalle" select="position() = $cantidadItemsDetalle" />

    <xsl:choose>
      <xsl:when test="$esUltimoItemDetalleDeLaPagina">
        <xsl:call-template name="SECCION_PIE_DE_PAGINA" />
      </xsl:when>
      <xsl:when test="$esUltimoItemDetalle">
        <xsl:call-template name="SECCION_PIE_DE_PAGINA" />
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- SECCIÓN PIE DE PÁGINA -->
  <xsl:template name="SECCION_PIE_DE_PAGINA">
    <div class="pie">
      <xsl:call-template name="SECCION_REMISIONES" />
      <br/>
      <xsl:call-template name="SECCION_RESUMEN" />
      <br/>
      <xsl:call-template name="CODIGO_BARRAS" />
      <br/>
      <xsl:call-template name="PAGINA_ACTUAL_TOTAL_PAGINAS" />
    </div>
  </xsl:template>

  <!-- SECCIÓN DATOS DE COMPROBANTE -->
  <xsl:template name="SECCION_DATOS_COMPROBANTE">
      <table>
        <tr>
          <td>
            Serie-Numero: <xsl:value-of select="../cbc:ID"/>
          </td>
          <td>
            <xsl:variable name="codigoPais" select="../cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode" />
            
            País del Emisor: <xsl:value-of select="$codigoPais"/> - 
            <xsl:call-template name="REF_NOMBRE_PAIS">
              <xsl:with-param name="codigoPais" select="$codigoPais"/>
            </xsl:call-template>
          </td>
        </tr>
      </table>
  </xsl:template>

  <!-- SECCIÓN REMISIONES -->
  <xsl:template name="SECCION_REMISIONES">
    <xsl:variable name="esUltimoItemDetalle" select="position() = $cantidadItemsDetalle" />

    <xsl:choose>
      <xsl:when test="$esUltimoItemDetalle">
        <xsl:for-each select="../cac:DespatchDocumentReference">
          <div class="item">
            <xsl:call-template name="ITEMS_REMISIONES" />
          </div>
        </xsl:for-each>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- ITEMS REMISIONES -->
  <xsl:template name="ITEMS_REMISIONES">
    ID: <xsl:value-of select="cbc:ID"/>
    <br/>
    Tipo: <xsl:value-of select="cbc:DocumentTypeCode"/>
  </xsl:template>

  <!-- SECCIÓN TOTALES -->
  <xsl:template name="SECCION_RESUMEN">
    <table>
      <tr>
        <td>
          Total Impuestos: <xsl:value-of select="cac:TaxTotal/cbc:TaxAmount"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <!-- CODIGO DE BARRAS -->
  <xsl:template name="CODIGO_BARRAS">
    <img src="data:image/png;base64,{$codigoBarras}" style="codigoBarras" />
  </xsl:template>

  <!-- PÁGINA ACTUAL -->
  <xsl:template name="PAGINA_ACTUAL">
    <xsl:variable name="paginaActual" select="ceiling(position() div $lineasPorPagina)" />
    <xsl:value-of select="$paginaActual"/>
  </xsl:template>

  <!-- PÁGINA ACTUAL Y TOTAL DE PÁGINAS -->
  <xsl:template name="PAGINA_ACTUAL_TOTAL_PAGINAS">
    <xsl:variable name="paginaActual" select="ceiling(position() div $lineasPorPagina)" />
    <xsl:value-of select="$paginaActual"/> - <xsl:value-of select="$totalPaginas"/>
  </xsl:template>

</xsl:stylesheet>