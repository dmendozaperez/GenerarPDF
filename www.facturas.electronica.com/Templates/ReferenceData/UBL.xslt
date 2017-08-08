<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl">
  
  <!-- UBL_T_Descuentos_Cargos -->
  <xsl:variable name="datosDescuentosCargos" select="document('UBL_T_Descuentos_Cargos.xml')" />
  <xsl:key name="descripcionDescuentoCargo" match="/UBL_T_Descuentos_Cargos/descuento-cargo" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_DESCUENTO_CARGO">
      <xsl:param name="codigoDescuentoCargo" />

    <xsl:for-each select="$datosDescuentosCargos">
      <xsl:value-of select="key('descripcionDescuentoCargo', $codigoDescuentoCargo)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- UBL_T_Categoria_Tributos -->
  <xsl:variable name="datosCategoriasTributos" select="document('UBL_T_Categoria_Tributos.xml')" />
  <xsl:key name="descripcionCategoriaTributo" match="/UBL_T_Categoria_Tributos/categoria" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_CATEGORIA_TRIBUTO">
    <xsl:param name="codigoCategoriaTributo" />

    <xsl:for-each select="$datosCategoriasTributos">
      <xsl:value-of select="key('descripcionCategoriaTributo', $codigoCategoriaTributo)/@descripcion" />
    </xsl:for-each>
  </xsl:template>
  
</xsl:stylesheet>