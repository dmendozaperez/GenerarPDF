<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl">

  <!-- INEI_Departamentos: DEPARTAMENTOS -->
  <xsl:variable name="datosDepartamentos" select="document('INEI_Departamentos.xml')" />
  <xsl:key name="nombreDepartamento" match="/INEI_Departamentos/departamento" use="@codigo" />

  <xsl:template name="REF_NOMBRE_DEPARTAMENTO">
    <xsl:param name="codigoDepartamento" />

    <xsl:for-each select="$datosDepartamentos">
      <xsl:value-of select="key('nombreDepartamento', $codigoDepartamento)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- INEI_Distritos: DISTRITOS -->
  <xsl:variable name="datosDistritos" select="document('INEI_Distritos.xml')" />
  <xsl:key name="nombreDistrito" match="/INEI_Distritos/distrito" use="@codigo" />

  <xsl:template name="REF_NOMBRE_DISTRITO">
    <xsl:param name="codigoDistrito" />

    <xsl:for-each select="$datosDistritos">
      <xsl:value-of select="key('nombreDistrito', $codigoDistrito)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- INEI_Privincias: PROVINCIAS -->
  <xsl:variable name="datosPrivincias" select="document('INEI_Provincias.xml')" />
  <xsl:key name="nombrePrivincia" match="/INEI_Privincias/provincia" use="@codigo" />

  <xsl:template name="REF_NOMBRE_PROVINCIA">
    <xsl:param name="codigoPrivincia" />

    <xsl:for-each select="$datosDistritos">
      <xsl:value-of select="key('nombrePrivincia', $codigoPrivincia)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>