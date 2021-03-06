﻿// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Sunat.SunatBarcodeFieldsXPaths
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

namespace Carvajal.FEPE.TemplateEngine.Support.Sunat
{
  public static class SunatBarcodeFieldsXPaths
  {
    public static readonly SunatBarcodeFieldsXPaths.Value InvoiceOrBallot = new SunatBarcodeFieldsXPaths.Value("/r:Invoice/cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID", "/r:Invoice/cbc:InvoiceTypeCode", "/r:Invoice/cbc:ID", "/r:Invoice/cac:TaxTotal/cbc:TaxAmount[../cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID=\"1000\"]", "/r:Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount", "/r:Invoice/cbc:IssueDate", "/r:Invoice/cac:AccountingCustomerParty/cbc:AdditionalAccountID", "/r:Invoice/cac:AccountingCustomerParty/cbc:CustomerAssignedAccountID");
    public static readonly SunatBarcodeFieldsXPaths.Value CreditNote = new SunatBarcodeFieldsXPaths.Value("/r:CreditNote/cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID", "", "/r:CreditNote/cbc:ID", "/r:CreditNote/cac:TaxTotal/cbc:TaxAmount[../cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID=\"1000\"]", "/r:CreditNote/cac:LegalMonetaryTotal/cbc:PayableAmount", "/r:CreditNote/cbc:IssueDate", "/r:CreditNote/cac:AccountingCustomerParty/cbc:AdditionalAccountID", "/r:CreditNote/cac:AccountingCustomerParty/cbc:CustomerAssignedAccountID");
    public static readonly SunatBarcodeFieldsXPaths.Value DebitNote = new SunatBarcodeFieldsXPaths.Value("/r:DebitNote/cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID", "", "/r:DebitNote/cbc:ID", "/r:DebitNote/cac:TaxTotal/cbc:TaxAmount[../cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID=\"1000\"]", "/r:DebitNote/cac:RequestedMonetaryTotal/cbc:PayableAmount", "/r:DebitNote/cbc:IssueDate", "/r:DebitNote/cac:AccountingCustomerParty/cbc:AdditionalAccountID", "/r:DebitNote/cac:AccountingCustomerParty/cbc:CustomerAssignedAccountID");
    public const string DigestValue = "//ds:DigestValue";
    public const string SignatureValue = "//ds:SignatureValue";

    public class Value
    {
      public string CompanyRuc { get; private set; }

      public string PaymentReceiptType { get; private set; }

      public string PaymentReceiptSerialNumber { get; private set; }

      public string Igv { get; private set; }

      public string Total { get; private set; }

      public string IssueDate { get; private set; }

      public string ReceiverDocumentType { get; private set; }

      public string ReceiverDocumentNumber { get; private set; }

      public Value(string companyRuc, string paymentReceiptType, string paymentReceiptSerialNumber, string igv, string total, string issueDate, string receiverDocumentType, string receiverDocumentNumber)
      {
        this.CompanyRuc = companyRuc;
        this.PaymentReceiptType = paymentReceiptType;
        this.PaymentReceiptSerialNumber = paymentReceiptSerialNumber;
        this.Igv = igv;
        this.Total = total;
        this.IssueDate = issueDate;
        this.ReceiverDocumentType = receiverDocumentType;
        this.ReceiverDocumentNumber = receiverDocumentNumber;
      }
    }
  }
}
