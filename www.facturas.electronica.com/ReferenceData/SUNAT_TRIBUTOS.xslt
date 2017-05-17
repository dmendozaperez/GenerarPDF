<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl">

  <!-- SUNAT_T_Elementos_Adicionales -->
  <xsl:variable name="datosElementosAdicionales" select="document('SUNAT_T_Elementos_Adicionales.xml')" />
  <xsl:key name="descripcionElementosAdicionales" match="/SUNAT_T_Elementos_Adicionales/elemento" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_ELEMENTO_ADICIONAL">
    <xsl:param name="codigoElementoAdicional" />

    <xsl:for-each select="$datosElementosAdicionales">
      <xsl:value-of select="key('descripcionElementosAdicionales', $codigoElementoAdicional)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Notas -->
  <xsl:variable name="datosNotas" select="document('SUNAT_T_Notas.xml')" />
  <xsl:key name="descripcionNota" match="/SUNAT_T_Notas/nota" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_NOTA">
    <xsl:param name="codigoNota" />

    <xsl:for-each select="$datosNotas">
      <xsl:value-of select="key('descripcionNota', $codigoNota)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Otros_Conceptos_Tributarios -->
  <xsl:variable name="datosConceptosTributarios" select="document('SUNAT_T_Otros_Conceptos_Tributarios.xml')" />
  <xsl:key name="descripcionConceptoTributario" match="/SUNAT_T_Otros_Conceptos_Tributarios/concepto" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_CONCEPTO_TRIBUTARIO">
    <xsl:param name="codigoConceptoTributario" />

    <xsl:for-each select="$datosConceptosTributarios">
      <xsl:value-of select="key('descripcionConceptoTributario', $codigoConceptoTributario)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Precio_Venta -->
  <xsl:variable name="datosPreciosVenta" select="document('SUNAT_T_Precio_Venta.xml')" />
  <xsl:key name="descripcionPrecioVenta" match="/SUNAT_T_Precio_Venta/precio-venta" use="@codigo" />

  <xsl:template name="REF_PRECIOS_VENTA">
    <xsl:param name="codigoPrecioVenta" />

    <xsl:for-each select="$datosPreciosVenta">
      <xsl:value-of select="key('descripcionPrecioVenta', $codigoPrecioVenta)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Afectacion_IGV -->
  <xsl:variable name="datosAfectacionIgv" select="document('SUNAT_T_Afectacion_IGV.xml')" />
  <xsl:key name="descripcionAfectacionIgv" match="/SUNAT_T_Afectacion_IGV/afectacion" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_AFECTACION_IGV">
    <xsl:param name="codigoAfectacionIgv" />

    <xsl:for-each select="$datosAfectacionIgv">
      <xsl:value-of select="key('descripcionAfectacionIgv', $codigoAfectacionIgv)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Sis_Calculo_ISC -->
  <xsl:variable name="datosAfectacionIsc" select="document('SUNAT_T_Sis_Calculo_ISC.xml')" />
  <xsl:key name="descripcionAfectacionIsc" match="/SUNAT_T_Sis_Calculo_ISC/afectacion" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_AFECTACION_ISC">
      <xsl:param name="codigoAfectacionIsc" />

    <xsl:for-each select="$datosAfectacionIsc">
      <xsl:value-of select="key('descripcionAfectacionIsc', $codigoAfectacionIsc)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Tributos -->
  <xsl:variable name="datosTributos" select="document('SUNAT_T_Tributos.xml')" />
  <xsl:key name="descripcionTributo" match="/SUNAT_T_Tributos/tributo" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_TRIBUTO">
    <xsl:param name="codigoTributo" />

    <xsl:for-each select="$datosTributos">
      <xsl:value-of select="key('descripcionTributo', $codigoTributo)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

  <!-- SUNAT_T_Valor_Venta -->
  <xsl:variable name="datosValoresVenta" select="document('SUNAT_T_Valor_Venta.xml')" />
  <xsl:key name="descripcionValorVenta" match="/SUNAT_T_Valor_Venta/valor-venta" use="@codigo" />

  <xsl:template name="REF_DESCRIPCION_VALOR_VENTA">
    <xsl:param name="codigoValorVenta" />

    <xsl:for-each select="$datosValoresVenta">
      <xsl:value-of select="key('descripcionValorVenta', $codigoValorVenta)/@descripcion" />
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>