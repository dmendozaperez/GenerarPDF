<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl">

  <!-- ISO3166: PAISES -->
  <xsl:variable name="datosPaises" select="document('ISO3166.xml')" />
  <xsl:key name="nombrePais" match="/ISO3166/pais" use="@codigo" />

  <xsl:template name="REF_NOMBRE_PAIS">
    <xsl:param name="codigoPais" />

    <xsl:for-each select="$datosPaises">
      <xsl:value-of select="key('nombrePais', $codigoPais)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- ISO4217: MONEDAS -->
  <xsl:variable name="datosMonedas" select="document('ISO4217.xml')" />
  <xsl:key name="nombreMoneda" match="/ISO4217/moneda" use="@codigo" />

  <xsl:template name="REF_NOMBRE_MONEDA">
    <xsl:param name="codigoMoneda" />

    <xsl:for-each select="$datosMonedas">
      <xsl:value-of select="key('nombreMoneda', $codigoMoneda)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>