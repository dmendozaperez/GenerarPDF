<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl">
  
  <!-- UNECE_T_Unidad_Medida -->
  <xsl:variable name="datosUnidadMedida" select="document('UNECE_T_Unidad_Medida.xml')" />
  <xsl:key name="descripcionUnidadMedida" match="/UNECE_T_Unidad_Medida/unidad" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_UNIDAD_MEDIDA">
    <xsl:param name="codigoUnidadMedida" />

    <xsl:for-each select="$datosUnidadMedida">
      <xsl:value-of select="key('descripcionUnidadMedida', $codigoUnidadMedida)/@descripcion" />
    </xsl:for-each>
  </xsl:template>
  
</xsl:stylesheet>