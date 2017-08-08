<?xml version="1.0" encoding="utf-8"?>

<!--
Tecnología y Servicios S.A. (2016)
Factura Electronica Peru
Ing Nelly Jordán Ordóñez
-->

<xsl:stylesheet
  version="1.0"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"  
  xmlns:qdt="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2"
  xmlns:ds="http://www.w3.org/2000/09/xmldsig#"
  xmlns:ext="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"
  xmlns:sac="urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1"
  xmlns:udt="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2"
  xmlns:cts="urn:carvajal:names:specification:ubl:peru:schema:xsd:CarvajalAggregateComponents-1"
  xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
  xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
  xmlns:ccts="urn:un:unece:uncefact:documentation:2" 
  xmlns:r="urn:sunat:names:specification:ubl:peru:schema:xsd:Perception-1"
  xmlns="urn:sunat:names:specification:ubl:peru:schema:xsd:Perception-1" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl"
  xmlns:dt="urn:schemas-microsoft-com:datatypes" 
  xmlns:iso="urn:iso:paises"> 
  
  <xsl:output method="html" indent="yes" />
  
  <!-- PARÁMETROS DE ENTRADA -->
  <xsl:param name="codigoBarras"  />
  <xsl:param name="hash"  />
  <xsl:decimal-format name="euro" decimal-separator="." grouping-separator=","/>
  <xsl:variable name="tamanoPagina">842 595</xsl:variable>

	<!-- HTML STYLES-->
	<xsl:template match="r:Perception">
		<html>
			<head>
				<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
				<title>Perception</title>
				<style type="text/css">

					body { width:4677px; height:3307px; font-family: Arial; font-size:40px; margin-top: 0px; padding: 0; color: #000; }
					table {font-family:inherit; font-size:inherit; width: 100%;border-spacing: 0; cell-padding:0px; cell-spacing:0px; }

					#pagina {position:relative; width:4677px; height: 3200px;margin-left: 112px;margin-right: 10px; top:100px}
					#ENCADIV {position:relative; top: 0px; left:0px; width: 100%; padding:0}
					#DETAILDIV {position:relative; top: 0px; left:0px; width: 100%; margin:0; padding:0}
					#RESDIV {position: relative; top: 0px; left:0px; width: 100%; margin:0; padding:0}
					#itemsDetail {font-size: 8px; text-align: center;border-collapse: collapse}
					#itemsDetail td{padding:0 0 0 0}

					.logo {height: 80px;}
					.paginacion{position:absolute;bottom:15px;right:0px;text-align:right;font-size:30px;}
					.Separador_Filas{height:40px;}
					.Separador_Columnas{width:10px;}
					.Titulo_Tabla_Totales{font-family:inherit; font-size:inherit; font-weight:bold; width:60%;}
					.Valores_Tabla_Totales{font-family:inherit; font-size:inherit; font-weight:normal; text-align:right; width:60%;}
					.Titulo_Info_RUC{font-family:inherit; font-size:80px;font-weight:bold;text-align:center; vertical-align:middle; }
					.Titulo_Detalle{font-family:inherit; font-size:36px; font-weight:bold;text-align:center; }
					.BorSolid{border:4px solid #000;}
					.BorDer{border-right:4px solid #000;}
					.BorIzq{border-left:4px solid #000;}
					.BorArr{border-top:4px solid #000;}
					.BorAba{border-bottom:4px solid #000;}

					.col1{border-color:#000; width:454px;vertical-align:top}
					.col2{border-color:#000; width:339px;vertical-align:top}
					.col3{border-color:#000; width:339px;vertical-align:top}
					.col4{border-color:#000; width:300px;vertical-align:top}
					.col5{border-color:#000; width:358px;vertical-align:top}
					.col6{border-color:#000; width:338px;vertical-align:top}
					.col7{border-color:#000; width:291px;vertical-align:top}
					.col8{border-color:#000; width:362px;vertical-align:top}
					.col9{border-color:#000; width:355px;vertical-align:top}
					.col10{border-color:#000; width:361px;vertical-align:top}
					.col11{border-color:#000; width:478px;vertical-align:top}
					.col12{border-color:#000; width:339px;vertical-align:top}
					.col13{border-color:#000; width:354px;vertical-align:top}

					.gray{background-color:#DCE6F1}
					.fuente18{font-size:18px}
					.fuente16{font-size:16px}
					.fuente15{font-size:15px}
					.fuente13{font-size:13px}
					.fuente12{font-size:12px}
					.fuente11{font-size:11px}
					.fuente10{font-size:10px}
					.fuente9{font-size:9px}
					.fuente8{font-size:40px}
					.fuente7{font-size:7px}
					.fuente6{font-size:6px}
					.negrilla{font-weight:bold;}

				</style>
			</head>
			<body>
				<!-- Inicio Variables Documento Percepción  -->								
				<xsl:variable name="SeparadorEntrada" select="'-'"></xsl:variable>
				<xsl:variable name="SeparadorSalida" select="'-'"></xsl:variable>							
				<xsl:variable name="Campo1_RUC">
					<xsl:value-of select="//cac:AgentParty/cac:PartyIdentification/cbc:ID"/>
				</xsl:variable>
				<xsl:variable name="Campo3_Correlativo">
					<xsl:value-of select="cbc:ID"/>
				</xsl:variable>
				<xsl:variable name="Campo4_AgentPartyRazonSocial">
					<xsl:value-of select="/*/cac:AgentParty/cac:PartyLegalEntity/cbc:RegistrationName"/>
				</xsl:variable>
				<xsl:variable name="Campo5_AgentPartyNombreComercial">
					<xsl:value-of select="//cac:AgentParty/cac:PartyName/cbc:Name"/>
				</xsl:variable>
				<xsl:variable name="Campo6_AgentPartyDireccion">
					<xsl:value-of select="//cac:AgentParty/cac:PostalAddress/cbc:StreetName"/>
				</xsl:variable>
				<xsl:variable name="Campo7_ReceiverPartyRazonSocial">
					<xsl:value-of select="//cac:ReceiverParty/cac:PartyLegalEntity/cbc:RegistrationName"/>
				</xsl:variable>
				<xsl:variable name="Campo8_ReceiverPartyRUC">
					<xsl:value-of select="//cac:ReceiverParty/cac:PartyIdentification/cbc:ID"/>
				</xsl:variable>
				<xsl:variable name="Campo9_ReceiverPartyDirección">
					<xsl:value-of select="//cac:ReceiverParty/cac:PostalAddress/cbc:StreetName"/>
				</xsl:variable>
				<xsl:variable name="Campo10_FechaEmision">						
					<xsl:if test="string-length(//cbc:IssueDate)>0">
						<xsl:variable name="ValorFechaPrimeraPosicion" select="substring-before(cbc:IssueDate, $SeparadorEntrada)"></xsl:variable>
						<xsl:variable name="Transicion" select="substring-after(cbc:IssueDate, $SeparadorEntrada)"></xsl:variable>
						<xsl:variable name="ValorFechaSegundaPosicion" select="substring-before($Transicion,$SeparadorEntrada)"></xsl:variable>
						<xsl:variable name="ValorFechaTerceraPosicion" select="substring-after($Transicion,$SeparadorEntrada)"></xsl:variable>
						<xsl:value-of select="concat($ValorFechaTerceraPosicion,$SeparadorSalida,$ValorFechaSegundaPosicion,$SeparadorSalida,$ValorFechaPrimeraPosicion)"/>
					</xsl:if>					
				</xsl:variable>
				<xsl:variable name="Campo11_SystemCode">
					<xsl:variable name="Temp" select="//sac:SUNATPerceptionSystemCode"/>
					<xsl:choose>
						<xsl:when test="$Temp='01'">PERCEPCIÓN VENTA INTERNA</xsl:when>
						<xsl:when test="$Temp='02'">PERCEPCIÓN A LA ADQUISICIÓN DE COMBUSTIBLE</xsl:when>
						<xsl:when test="$Temp='03'">PERCEPCIÓN REALIZADA AL AGENTE DE PERCEPCIÓN CON TASA ESPECIAL</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="Campo12_TasaPercepcion">
					<xsl:value-of select="//sac:SUNATPerceptionPercent"/>
				</xsl:variable>		
				<xsl:variable name="Campo13_ImpuestoPercibido">
					<xsl:if test="//cbc:TotalInvoiceAmount != ''">
						<xsl:value-of select="format-number(//cbc:TotalInvoiceAmount ,'#,##0.##', 'euro')"/>
					</xsl:if>					
				</xsl:variable>		
				<xsl:variable name="Campo14_ImpuestoCobrado">
					<xsl:if test="//sac:SUNATTotalCashed != ''">
						<xsl:value-of select="format-number(//sac:SUNATTotalCashed ,'#,##0.##', 'euro')"/>
					</xsl:if>
				</xsl:variable>	
				<xsl:variable name="Campo28_MontoLetras">
					<xsl:value-of select="//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty[cbc:ID=0040]/cbc:Value"/>
				</xsl:variable>				
				<!-- Fin Variables Documento Percepción  -->

				<div  id="SuprCont" style="position:absolute;visibility:hidden; top:0px; left:0px;">

					<!-- PAGINACIÓN-->
					<div id="paginador" class="paginacion">
						<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
					</div>
					<!--FIN PAGINACIÓN-->

					<div id="ENCADIV">
						<table width="100%">
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td>
												<!--LOGO-->
												
												<!--FIN LOGO-->
											</td>
											<td style="width:500px">
												<!--COLUMNA VACIA-->
											</td>
											<td style="width:1982px" align="right">
												<table width="100%" height="553px"  class="BorSolid">
													<tr>
														<td class="Separador_Filas">
															<!--FILA VACIA-->
														</td>
													</tr>
													<tr>
														<td class="Titulo_Info_RUC negrilla Separador_Filas">
															RUC: <xsl:value-of select="$Campo1_RUC"/>
														</td>
													</tr>
													<tr>
														<td style="height:50px">
															<!--FILA VACIA-->
														</td>
													</tr>
													<tr>
														<td class="Titulo_Info_RUC  negrilla">
															COMPROBANTE DE PERCEPCIÓN ELECTRÓNICO
														</td>
													</tr>
													<tr>
														<td style="height:50px">
															<!--FILA VACIA-->
														</td>
													</tr>
													<tr>
														<td class="Titulo_Info_RUC  negrilla">
															<xsl:value-of select="$Campo3_Correlativo"/>
														</td>
													</tr>
													<tr>
														<td class="Separador_Filas">
															<!--FILA VACIA-->
														</td>
													</tr>
												</table>
											</td>
											<td style="width:655px;height:100px">
												<!--COLUMNA VACIA-->
											</td>
										</tr>

										<tr >
											<td colspan="4" class="Separador_Filas">
												<!--FILA VACIA-->
											</td>
										</tr>

										<tr>
											<td colspan="4">
												<table width="100%">
													<tr>
														<td style="width:2010px">
															<table width="100%">
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td width="440px">
																		<b>
																			DATOS DEL EMISOR
																		</b>
																	</td>
																	<td>
																		<!--DATOS DEL EMISOR-->
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Nombre o razón social:
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo4_AgentPartyRazonSocial"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Nombre comercial:
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo5_AgentPartyNombreComercial"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Dirección:
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo6_AgentPartyDireccion"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
															</table>
														</td>
														<td style="width:2010px">
															<table width="100%">
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td width="440px">
																		<b>
																			DATOS DEL CLIENTE
																		</b>
																	</td>
																	<td>
																		<!--DATOS DEL EMISOR-->
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Nombre o razón social:
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo7_ReceiverPartyRazonSocial"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			RUC:
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo8_ReceiverPartyRUC"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Dirección:
																		</b>
																	</td>
																	<td>
																	8 <xsl:value-of select="$Campo9_ReceiverPartyDirección"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										
										<tr >
											<td colspan="4" class="Separador_Filas">
												<!--FILA VACIA-->
											</td>
										</tr>

										<tr>
											<td colspan="4">
												<table width="100%">
													<tr>
														<td>
															<table width="100%">
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td width="600px">
																		<b>
																			DATOS DE LA PERCEPCIÓN
																		</b>
																	</td>
																	<td>
																		<!--DATOS DE LA PERCEPCIÓN-->
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			fecha de emisión:
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo10_FechaEmision"/>																																
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td style="colspan=2">
																		<b>																		
																			<xsl:value-of select="$Campo11_SystemCode"/>
																		</b>
																	</td>																	
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Tasa de percepción:
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo12_TasaPercepcion"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Importe total cobrado (S/):
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo14_ImpuestoCobrado"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
																<tr>
																	<td>
																		<b>
																			Importe total percibido (S/):
																		</b>
																	</td>
																	<td>
																		<xsl:value-of select="$Campo13_ImpuestoPercibido"/>
																	</td>
																</tr>
																<tr>
																	<td colspan="2" class="Separador_Filas">
																		<!--FILA VACIA-->
																	</td>
																</tr>
															</table>
														</td>														
													</tr>
												</table>
											</td>
										</tr>
										
										<tr>
											<td style="height:50px">
												<!--FILA VACIA-->
											</td>
										</tr>

										<tr>
											<td colspan="4">
												<table>
													<tr>
														<td class="BorIzq BorArr BorDer fuente8 Titulo_Detalle gray" width="1794px">
															INFORMACIÓN POR CADA COMPROBANTE RELACIONADO
														</td>
														<td class="BorArr BorDer fuente8 Titulo_Detalle gray" width="1353px">
															DATOS DEL COBRO
														</td>
														<td class="BorArr BorDer fuente8 Titulo_Detalle gray" width="838px">
															DATOS PERCEPCIÓN
														</td>
														<td class="BorArr fuente8 Titulo_Detalle gray BorDer" width="687px">
															TIPO DE CAMBIO
														</td>
													</tr>
												</table>
											</td>											
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</div>

					<div  id="DETAILDIV">
						<table id="itemsDetail">
							<tr id="trtitulo" class="BorSolid">
								<td class="BorArr BorDer Titulo_Detalle" width="450px">
									Tipo de<br/>comprobante
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="338px">
									Serie - número
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="338px">
									Fecha de<br/>emisión
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="300px">
									Tipo de<br/>moneda
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="357px">
									Importe total
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="340px">
									Fecha de<br/>cobro
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="290px">
									Nùmero<br/>de cobro
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="365px">
									Importe de cobro<br/>(No incluye<br/>percepción)
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="350px">
									Moneda de<br/>Importe de cobro
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="360px">
									Importe de la<br/>percepción en (S/)
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="478px">
									Importe neto cobrado<br/>deducida la percepción<br/>(S/)
								</td>
								<td class="BorArr BorDer Titulo_Detalle" width="340px">
									Valor aplicado
								</td>
								<td class="BorArr col13 Titulo_Detalle" width="354px">
									Fecha de<br/>cambio
								</td>
							</tr>
									

							<xsl:for-each select="//sac:SUNATPerceptionDocumentReference">							
								<tr>
									<td style="text-align:center" class="BorIzq BorDer fuente8 BorAba col1">									
										<xsl:variable name="Campo15_TipoComprobante">
											<xsl:variable name="Temp" select="cbc:ID/@schemeID"/>
											<xsl:choose>
												<xsl:when test="$Temp='01'">FACTURA</xsl:when>
												<xsl:when test="$Temp='03'">BOLETA DE VENTA</xsl:when>
												<xsl:when test="$Temp='07'">NOTA DE CRÉDITO</xsl:when>
												<xsl:when test="$Temp='08'">NOTA DE DÉBITO</xsl:when>
												<xsl:when test="$Temp='12'">TICKET DE MÁQUINA REGISTRADORA</xsl:when>											
											</xsl:choose>
										</xsl:variable>
										<xsl:value-of select="$Campo15_TipoComprobante"/>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col2">
										<xsl:value-of select="cbc:ID"/>										
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col3">
									 <xsl:if test="string-length(//cbc:IssueDate)>0">
										<xsl:variable name="ValorFechaPrimeraPosicion" select="substring-before(cbc:IssueDate, $SeparadorEntrada)"></xsl:variable>
										<xsl:variable name="Transicion" select="substring-after(cbc:IssueDate, $SeparadorEntrada)"></xsl:variable>
										<xsl:variable name="ValorFechaSegundaPosicion" select="substring-before($Transicion,$SeparadorEntrada)"></xsl:variable>
										<xsl:variable name="ValorFechaTerceraPosicion" select="substring-after($Transicion,$SeparadorEntrada)"></xsl:variable>
										<xsl:value-of select="concat($ValorFechaTerceraPosicion,$SeparadorSalida,$ValorFechaSegundaPosicion,$SeparadorSalida,$ValorFechaPrimeraPosicion)"/>
									  </xsl:if>									
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col4">
										<xsl:value-of select="cbc:TotalInvoiceAmount/@currencyID"/>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col5">
										<xsl:if test="cbc:TotalInvoiceAmount != ''">
											<xsl:value-of select="format-number(cbc:TotalInvoiceAmount ,'#,##0.##', 'euro')"/>
										</xsl:if>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col6">
										<xsl:if test="string-length(cac:Payment/cbc:PaidDate)>0">
											<xsl:variable name="ValorFechaPrimeraPosicion" select="substring-before(cac:Payment/cbc:PaidDate, $SeparadorEntrada)"></xsl:variable>
											<xsl:variable name="Transicion" select="substring-after(cac:Payment/cbc:PaidDate, $SeparadorEntrada)"></xsl:variable>
											<xsl:variable name="ValorFechaSegundaPosicion" select="substring-before($Transicion,$SeparadorEntrada)"></xsl:variable>
											<xsl:variable name="ValorFechaTerceraPosicion" select="substring-after($Transicion,$SeparadorEntrada)"></xsl:variable>
											<xsl:value-of select="concat($ValorFechaTerceraPosicion,$SeparadorSalida,$ValorFechaSegundaPosicion,$SeparadorSalida,$ValorFechaPrimeraPosicion)"/>
										</xsl:if>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col7">
										<xsl:value-of select="cac:Payment/cbc:ID"/>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col8">
										<xsl:if test="cac:Payment/cbc:PaidAmount != ''">
											<xsl:value-of select="format-number(cac:Payment/cbc:PaidAmount ,'#,##0.##', 'euro')"/>
										</xsl:if>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col9">
										<xsl:value-of select="cac:Payment/cbc:PaidAmount/@currencyID"/>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col10">
										<xsl:if test="sac:SUNATPerceptionInformation/sac:SUNATPerceptionAmount != ''">
											<xsl:value-of select="format-number(sac:SUNATPerceptionInformation/sac:SUNATPerceptionAmount ,'#,##0.##', 'euro')"/>
										</xsl:if>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col11">
										<xsl:if test="sac:SUNATPerceptionInformation/sac:SUNATNetTotalCashed != ''">
											<xsl:value-of select="format-number(sac:SUNATPerceptionInformation/sac:SUNATNetTotalCashed ,'#,##0.##', 'euro')"/>
										</xsl:if>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col2">
										<xsl:if test="sac:SUNATPerceptionInformation/cac:ExchangeRate/cbc:CalculationRate != ''">
											<xsl:value-of select="format-number(sac:SUNATPerceptionInformation/cac:ExchangeRate/cbc:CalculationRate ,'#,##0.######', 'euro')"/>
										</xsl:if>
									</td>
									<td style="text-align:center" class="BorDer fuente8 BorAba col13">
										<xsl:if test="string-length(sac:SUNATPerceptionInformation/cac:ExchangeRate/cbc:Date)>0">
										<xsl:variable name="ValorFechaPrimeraPosicion" select="substring-before(sac:SUNATPerceptionInformation/cac:ExchangeRate/cbc:Date, $SeparadorEntrada)"></xsl:variable>
										<xsl:variable name="Transicion" select="substring-after(sac:SUNATPerceptionInformation/cac:ExchangeRate/cbc:Date, $SeparadorEntrada)"></xsl:variable>
										<xsl:variable name="ValorFechaSegundaPosicion" select="substring-before($Transicion,$SeparadorEntrada)"></xsl:variable>
										<xsl:variable name="ValorFechaTerceraPosicion" select="substring-after($Transicion,$SeparadorEntrada)"></xsl:variable>
										<xsl:value-of select="concat($ValorFechaTerceraPosicion,$SeparadorSalida,$ValorFechaSegundaPosicion,$SeparadorSalida,$ValorFechaPrimeraPosicion)"/>
									</xsl:if>										
									</td>
								</tr>
							</xsl:for-each>

							<tr id="RellenoDetail">
								<td colspan="13" style="text-align:left" class="fuente8">
									<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
									<b>Monto en Letras:</b> 
									<xsl:value-of select="$Campo28_MontoLetras"/>
								</td>								
							</tr>

							<tr id ="ultimoregistro">
								<td style="text-align:center">
								</td>
								<td style="text-align:left">
								</td>
								<td style="text-align:center">
								</td>
								<td style="text-align:center">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
								<td style="text-align:right">
								</td>
							</tr>

						</table>
					</div>

					<div id="RESDIV">
						<table >
							<tr>
								<td colspan="6">
									<table>										
										<tr>
											<td class="Titulo_Detalle">
												"Representación Impresa del Comprobante de Percepción Electrónico. Resolución de Superintendencia N° 274-2015/SUNAT"
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

						var hDocumento = 3220; //Recuadro exterior
						var hPagina = 3200; //Altura de la Pagina
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
        <xsl:value-of select="substring-before($text,$replace)" />
        <xsl:value-of select="$by" />
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text"
          select="substring-after($text,$replace)" />
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