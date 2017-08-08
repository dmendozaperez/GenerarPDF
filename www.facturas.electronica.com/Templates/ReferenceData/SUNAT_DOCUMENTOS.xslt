<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl">

  <!-- SUNAT_T_Documentos -->
  <xsl:variable name="datosDocumentos" select="document('SUNAT_T_Documentos.xml')" />
  <xsl:key name="nombreDocumento" match="/SUNAT_T_Documentos/documento" use="@codigo" />

  <xsl:template name="REF_NOMBRE_DOCUMENTO">
    <xsl:param name="codigoDocumento" />

    <xsl:for-each select="$datosDocumentos">
      <xsl:value-of select="key('nombreDocumento', $codigoDocumento)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Documentos_Identidad -->
  <xsl:variable name="datosDocumentosIdentidad" select="document('SUNAT_T_Documentos_Identidad.xml')" />
  <xsl:key name="nombreDocumentoIdentidad" match="/SUNAT_T_Documentos_Identidad/documento" use="@codigo" />

  <xsl:template name="REF_NOMBRE_DOCUMENTO_IDENTIDAD">
    <xsl:param name="codigoDocumentoIdentidad" />

    <xsl:for-each select="$datosDocumentosIdentidad">
      <xsl:value-of select="key('nombreDocumentoIdentidad', $codigoDocumentoIdentidad)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Documentos_Relacionados -->
  <xsl:variable name="datosDocumentosRelacionados" select="document('SUNAT_T_Documentos_Relacionados.xml')" />
  <xsl:key name="nombreDocumentoRelacionado" match="/SUNAT_T_Documentos_Relacionados/documento" use="@codigo" />

  <xsl:template name="REF_NOMBRE_DOCUMENTO_RELACIONADO">
    <xsl:param name="codigoDocumentoRelacionado" />

    <xsl:for-each select="$datosDocumentosRelacionados">
      <xsl:value-of select="key('nombreDocumentoRelacionado', $codigoDocumentoRelacionado)/@descripcion" />
    </xsl:for-each>
  </xsl:template>
  
</xsl:stylesheet>