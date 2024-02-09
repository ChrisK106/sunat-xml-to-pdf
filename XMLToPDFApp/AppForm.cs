using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace XMLToPDFApp
{
    public partial class AppForm : Form
    {
        // Initialize the database object by setting its filename
        readonly Database<Business> dbBusiness = new("BusinessDB.json");
        private readonly XmlDocument xmlDocument = new();

        public AppForm()
        {
            InitializeComponent();
            CenterToScreen();

            Text = $"SUNAT XML a PDF v.{Application.ProductVersion}";
            lblStatus.Text = "Listo";

            LoadBusinessList();
        }

        private void btnLoadXML_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Archivos XML (*.xml)|*.xml",
                Title = "Seleccione un archivo XML",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtXMLFilePath.Text = openFileDialog.FileName;
                txtPDFPath.Text = Path.GetDirectoryName(openFileDialog.FileName);
            }

        }

        private void btnGeneratePDF_Click(object sender, EventArgs e)
        {
            if (txtXMLFilePath.Text.Equals("(Sin Archivo XML Cargado)"))
            {
                MessageBox.Show(this, "Seleccione un archivo XML para continuar.",
                    "XML no cargado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            if (cbxSelectedBusiness.Enabled == false)
            {
                MessageBox.Show(this, "Primero debe registrar una empresa.\nLos datos de esta se utilizarán en la cabecera del PDF.",
                    "No existe una empresa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            try
            {
                // Collecting required XML data
                lblStatus.Text = "Extrayendo datos de archivo XML...";

                xmlDocument.Load(txtXMLFilePath.Text);

                string ublVersion = GetFirstValueFromXmlRootElement("cbc:UBLVersionID");
                string customizationId = GetFirstValueFromXmlRootElement("cbc:CustomizationID");

                string digestValue = GetFirstValueFromXmlRootElement("ds:DigestValue");
                string documentId = GetFirstValueFromXmlRootElement("cbc:ID");
                string issueDate = GetFirstValueFromXmlRootElement("cbc:IssueDate");
                string dueDate = GetFirstValueFromXmlRootElement("cbc:DueDate");

                string paymentTermsTotalAmount = string.Empty;
                List<PaymentTerm> documentCreditInfo = new();

                if (dueDate.Equals(string.Empty))
                {
                    dueDate = GetFirstValueFromXmlRootElement("cbc:PaymentDueDate");

                    if (!dueDate.Equals(string.Empty))
                    {
                        // FACTURA CON INFORMACIÓN DE CRÉDITO
                        int paymentTermId = 0;

                        foreach (XmlElement paymentTerm in xmlDocument.DocumentElement.GetElementsByTagName("cac:PaymentTerms"))
                        {
                            if (paymentTermId == 0)
                            {
                                paymentTermsTotalAmount = paymentTerm.GetElementsByTagName("cbc:Amount")[0].InnerText;
                                paymentTermId++;
                            }
                            else
                            {
                                PaymentTerm pt = new();
                                pt.id = paymentTermId.ToString();
                                pt.dueDate = paymentTerm.GetElementsByTagName("cbc:PaymentDueDate")[0].InnerText;
                                pt.amount = paymentTerm.GetElementsByTagName("cbc:Amount")[0].InnerText;
                                documentCreditInfo.Add(pt);
                                paymentTermId++;
                            }
                        }
                    }
                }

                string htmlSource = string.Empty;
                string documentTitle = string.Empty;
                string customerTypeCode = string.Empty;
                string footerText = string.Empty;

                string invoiceTypeCode = GetFirstValueFromXmlRootElement("cbc:InvoiceTypeCode");

                // FACTURA
                if (invoiceTypeCode == "01")
                {
                    if (paymentTermsTotalAmount.Equals(string.Empty))
                    {
                        htmlSource = Properties.Resources.invoice_template.ToString();
                    }
                    else
                    {
                        htmlSource = Properties.Resources.invoice_template_with_credit_info.ToString();
                    }

                    documentTitle = "FACTURA ELECTRÓNICA";
                    customerTypeCode = "06";
                    footerText = "Representación impresa de la Factura Electrónica. " +
                    "Consulte o descargue su comprobante electrónico en sunat.gob.pe";
                }
                // BOLETA
                else if (invoiceTypeCode == "03")
                {
                    htmlSource = Properties.Resources.receipt_template.ToString();
                    documentTitle = "BOLETA DE VENTA ELECTRÓNICA";
                    customerTypeCode = "01";
                    footerText = "Representación impresa de la Boleta de Venta Electrónica. " +
                    "Consulte o descargue su comprobante electrónico en sunat.gob.pe";
                }

                XmlNodeList documentNotes = xmlDocument.DocumentElement.GetElementsByTagName("cbc:Note");

                string payableAmountText = documentNotes[0].InnerText;
                string documentObservations = string.Empty;

                if (documentNotes.Count > 1)
                {
                    documentObservations = documentNotes[1].InnerText;
                }

                string documentCurrencyCode = GetFirstValueFromXmlRootElement("cbc:DocumentCurrencyCode");

                //string documentReference = doc.DocumentElement.GetElementsByTagName("cac:AdditionalDocumentReference")[0].InnerText;

                string supplierId = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:ID");
                string supplierName = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:Name");
                string supplierRegistrationName = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:RegistrationName");
                string supplierCityName = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:CityName");
                string supplierCountrySubentity = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:CountrySubentity");
                string supplierCountrySubentityCode = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:CountrySubentityCode");
                string supplierDistrict = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:District");
                string supplierLine = GetFirstValueFromGivenXmlElement("cac:AccountingSupplierParty", "cbc:Line");

                string customerId = GetFirstValueFromGivenXmlElement("cac:AccountingCustomerParty", "cbc:ID");
                string customerRegistrationName = GetFirstValueFromGivenXmlElement("cac:AccountingCustomerParty", "cbc:RegistrationName");
                string customerCityName = GetFirstValueFromGivenXmlElement("cac:AccountingCustomerParty", "cbc:CityName");
                string customerCountrySubentity = GetFirstValueFromGivenXmlElement("cac:AccountingCustomerParty", "cbc:CountrySubentity");
                string customerDistrict = GetFirstValueFromGivenXmlElement("cac:AccountingCustomerParty", "cbc:District");
                string customerLine = GetFirstValueFromGivenXmlElement("cac:AccountingCustomerParty", "cbc:Line");

                string taxAmount = GetFirstValueFromXmlRootElement("cbc:TaxAmount");
                string taxableAmount = GetFirstValueFromXmlRootElement("cbc:TaxableAmount");
                string lineExtensionAmount = GetFirstValueFromXmlRootElement("cbc:LineExtensionAmount");
                string allowanceTotalAmount = GetFirstValueFromXmlRootElement("cbc:AllowanceTotalAmount");
                string chargeTotalAmount = GetFirstValueFromXmlRootElement("cbc:ChargeTotalAmount");
                string prepaidAmount = GetFirstValueFromXmlRootElement("cbc:PrepaidAmount");
                string payableAmount = GetFirstValueFromXmlRootElement("cbc:PayableAmount");

                List<InvoiceLine> documentDetail = new();

                foreach (XmlElement invoiceLine in xmlDocument.DocumentElement.GetElementsByTagName("cac:InvoiceLine"))
                {
                    InvoiceLine line = new();
                    line.id = invoiceLine.GetElementsByTagName("cbc:ID")[0].InnerText;
                    line.quantity = invoiceLine.GetElementsByTagName("cbc:InvoicedQuantity")[0].InnerText;
                    line.itemId = invoiceLine.GetElementsByTagName("cac:Item")[0]["cac:SellersItemIdentification"]["cbc:ID"].InnerText;
                    line.description = invoiceLine.GetElementsByTagName("cac:Item")[0]["cbc:Description"].InnerText;
                    line.priceAmount = invoiceLine.GetElementsByTagName("cac:Price")[0]["cbc:PriceAmount"].InnerText;
                    documentDetail.Add(line);
                }

                Business selectedBusiness = dbBusiness.Search(x => x.id == cbxSelectedBusiness.SelectedValue.ToString())[0];

                string headerBusinessID = selectedBusiness.id;
                string headerBusinessName = selectedBusiness.name;
                string headerAddressLine1 = selectedBusiness.addressLine1;
                string headerAddressLine2 = selectedBusiness.addressLine2;
                string headerAddressLine3 = selectedBusiness.addressLine3;

                // Parsing collected XML data to document
                lblStatus.Text = "Parseando datos XML obtenidos al formato del documento...";

                htmlSource = htmlSource.Replace("@H_BUSSINESS_ID", headerBusinessID);
                htmlSource = htmlSource.Replace("@H_BUSSINESS_NAME", headerBusinessName);
                htmlSource = htmlSource.Replace("@H_ADDRESS_1", headerAddressLine1);
                htmlSource = htmlSource.Replace("@H_ADDRESS_2", headerAddressLine2);
                htmlSource = htmlSource.Replace("@H_ADDRESS_3", headerAddressLine3);

                htmlSource = htmlSource.Replace("@H_DOCUMENT_TITLE", documentTitle);

                htmlSource = htmlSource.Replace("@DOCUMENT_ID", documentId);

                htmlSource = htmlSource.Replace("@CUSTOMER_ID", customerId);
                htmlSource = htmlSource.Replace("@CUSTOMER_NAME", customerRegistrationName);
                htmlSource = htmlSource.Replace("@CUSTOMER_ADDRESS_LINE", customerLine);

                htmlSource = htmlSource.Replace("@ISSUE_DATE", DateTime.Parse(issueDate).ToString("dd/MM/yyyy"));

                if (dueDate != string.Empty)
                {
                    htmlSource = htmlSource.Replace("@DUE_DATE", DateTime.Parse(dueDate).ToString("dd/MM/yyyy"));
                }
                else
                {
                    htmlSource = htmlSource.Replace("@DUE_DATE", "-");
                }

                string documentCurrencyName = string.Empty;
                string documentCurrencySymbol = string.Empty;

                if (documentCurrencyCode == "PEN")
                {
                    documentCurrencyName = "SOLES";
                    documentCurrencySymbol = "S/. ";
                }
                else if (documentCurrencyCode == "USD")
                {
                    documentCurrencyName = "DÓLARES AMERICANOS";
                    documentCurrencySymbol = "US$ ";
                }
                else if (documentCurrencyCode == "EUR")
                {
                    documentCurrencyName = "EUROS";
                    documentCurrencySymbol = "€ ";
                }

                htmlSource = htmlSource.Replace("@CURRENCY_NAME", documentCurrencyName);

                string strDocumentDetail = string.Empty;
                double documentSubTotal = 0;

                foreach (InvoiceLine line in documentDetail)
                {
                    strDocumentDetail += "<tr>";
                    strDocumentDetail += "<td>" + line.quantity + "</td>";
                    strDocumentDetail += "<td>" + line.itemId + "</td>";
                    strDocumentDetail += "<td>" + line.description + "</td>";
                    strDocumentDetail += "<td align='right'>" + double.Parse(line.priceAmount).ToString("N2") + "</td>";
                    strDocumentDetail += "</tr>";

                    if (double.Parse(line.priceAmount) > 0)
                    {
                        documentSubTotal += double.Parse(line.priceAmount);
                    }
                }

                htmlSource = htmlSource.Replace("@DOCUMENT_DETAIL", strDocumentDetail);

                //TOTAL A PAGAR (TEXTO)
                htmlSource = htmlSource.Replace("@TEXT_PAYABLE_AMOUNT", payableAmountText);

                //Observaciones
                if (!documentObservations.Equals(string.Empty))
                {
                    htmlSource = htmlSource.Replace("@DOCUMENT_OBSERVATIONS", "OBSERVACIONES: <br></br>" + documentObservations);
                }
                else
                {
                    htmlSource = htmlSource.Replace("@DOCUMENT_OBSERVATIONS", "");
                }

                // Código Hash
                htmlSource = htmlSource.Replace("@DIGEST_VALUE", digestValue);

                // Sub Total
                htmlSource = htmlSource.Replace("@DOCUMENT_SUBTOTAL", documentCurrencySymbol + documentSubTotal.ToString("N2"));

                // Anticipos
                htmlSource = htmlSource.Replace("@DOCUMENT_PREPAID_AMOUNT", documentCurrencySymbol + double.Parse(prepaidAmount).ToString("N2"));

                // Descuentos
                htmlSource = htmlSource.Replace("@DOCUMENT_DISCOUNTS", documentCurrencySymbol + "0.00");

                // Valor de Venta
                htmlSource = htmlSource.Replace("@DOCUMENT_LINE_EXTENSION_AMOUNT", documentCurrencySymbol + double.Parse(lineExtensionAmount).ToString("N2"));

                // IGV
                htmlSource = htmlSource.Replace("@DOCUMENT_TAX_AMOUNT", documentCurrencySymbol + double.Parse(taxAmount).ToString("N2"));

                // Otros Cargos
                htmlSource = htmlSource.Replace("@DOCUMENT_OTHERS_CHARGES", documentCurrencySymbol + "0.00");

                // Total a Pagar
                htmlSource = htmlSource.Replace("@DOCUMENT_PAYABLE_AMOUNT", documentCurrencySymbol + double.Parse(payableAmount).ToString("N2"));

                // INFORMACIÓN DE CRÉDITO
                if (!paymentTermsTotalAmount.Equals(string.Empty))
                {
                    htmlSource = htmlSource.Replace("@DOCUMENT_CREDIT_TOTAL", documentCurrencySymbol + double.Parse(paymentTermsTotalAmount).ToString("N2"));
                    htmlSource = htmlSource.Replace("@DOCUMENT_CREDIT_QUOTAS", documentCreditInfo.Count.ToString());

                    string strDocumentCreditInfo = string.Empty;

                    foreach (PaymentTerm pt in documentCreditInfo)
                    {
                        strDocumentCreditInfo += "<tr>";
                        strDocumentCreditInfo += "<td align='center'>" + pt.id + "</td>";
                        strDocumentCreditInfo += "<td align='center'>" + DateTime.Parse(pt.dueDate).ToString("dd/MM/yyyy") + "</td>";
                        strDocumentCreditInfo += "<td align='right'>" + double.Parse(pt.amount).ToString("N2") + "</td>";
                        strDocumentCreditInfo += "</tr>";
                    }

                    htmlSource = htmlSource.Replace("@DOCUMENT_CREDIT_INFO", strDocumentCreditInfo);
                }

                string datetimeFileGeneration = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                htmlSource = htmlSource.Replace("@DATETIME_GENERATION", datetimeFileGeneration);

                // Creating PDF 
                lblStatus.Text = "Generando PDF...";

                string pdfFilePath = Path.Combine(txtPDFPath.Text, documentId + "_" + supplierId + "_" + DateTime.Now.ToString("ddMMyy_HHmmss") + ".pdf");

                using (FileStream stream = new(pdfFilePath, FileMode.Create))
                {
                    Document pdfDoc = new(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    writer.PageEvent = new Footer(footerText);

                    pdfDoc.Open();

                    Image headerImage;

                    if (!selectedBusiness.logoName.Equals(""))
                    {
                        string imgPath = Path.Combine(dbBusiness.imgDirectory, selectedBusiness.logoName);

                        headerImage = Image.GetInstance(imgPath);
                        headerImage.ScalePercent(24f);
                        headerImage.ScaleToFit(160f, 95f);

                        float xAxisAdjustment = (160f - headerImage.ScaledWidth) / 2f;
                        float yAxisAdjustment = (95f - headerImage.ScaledHeight) / 2f;

                        headerImage.SetAbsolutePosition(pdfDoc.LeftMargin + xAxisAdjustment, pdfDoc.Top - headerImage.ScaledHeight - 2f - yAxisAdjustment);

                        pdfDoc.Add(headerImage);
                    }

                    string documentIdSeries = documentId.Substring(0, documentId.Length - documentId.IndexOf('-') + 1);
                    string documentIdNumber = documentId.Substring(documentId.IndexOf('-') + 1);

                    string qrCodeString = headerBusinessID + "|" + invoiceTypeCode + "|" + documentIdSeries + "|" + documentIdNumber + "|" +
                        taxAmount + "|" + payableAmount + "|" + issueDate + "|" + customerTypeCode + "|" + customerId + "|";

                    BarcodeQRCode qrCode = new(qrCodeString, 250, 250, null);
                    Image qrCodeImage = qrCode.GetImage();
                    qrCodeImage.ScaleAbsolute(120, 120);
                    //qrCodeImage.Alignment = Image.UNDERLYING;
                    qrCodeImage.SetAbsolutePosition((pdfDoc.Right + pdfDoc.RightMargin) / 2 - 60, pdfDoc.Bottom + 35);

                    pdfDoc.Add(qrCodeImage);

                    using (StringReader sr = new(htmlSource))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                };

                lblStatus.Text = "PDF generado en... " + pdfFilePath;

                if (chkOpenGeneratedPDF.Checked)
                {
                    try
                    {
                        var p = new Process
                        {
                            StartInfo = new ProcessStartInfo(pdfFilePath)
                            {
                                UseShellExecute = true
                            }
                        };

                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Error al intentar abrir PDF generado", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error al generar PDF", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }

        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPDFPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void LoadBusinessList()
        {
            dbBusiness.Load();

            cbxBusinessList.DataSource = null;
            cbxSelectedBusiness.DataSource = null;

            if (dbBusiness.dataCollection.Count == 0)
            {
                cbxBusinessList.Enabled = false;
                cbxSelectedBusiness.Enabled = false;
                btnBusinessEdit.Enabled = false;
                btnBusinessDelete.Enabled = false;
                gbxBusinessData.Enabled = false;
                return;
            }
            else
            {
                cbxBusinessList.Enabled = true;
                cbxSelectedBusiness.Enabled = true;
                btnBusinessEdit.Enabled = true;
                btnBusinessDelete.Enabled = true;
                gbxBusinessData.Enabled = true;
            }

            DataTable dt = new();
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            foreach (Business business in dbBusiness.dataCollection)
            {
                dt.Rows.Add(business.id, business.name + " - " + business.id);
            }

            cbxBusinessList.DisplayMember = "name";
            cbxBusinessList.ValueMember = "id";
            cbxBusinessList.DataSource = dt;

            cbxSelectedBusiness.DisplayMember = "name";
            cbxSelectedBusiness.ValueMember = "id";
            cbxSelectedBusiness.DataSource = dt;
        }

        private void btnBusinessNew_Click(object sender, EventArgs e)
        {
            // Clear Business data fields
            ResetBusinessDataFields();

            cbxBusinessList.Enabled = false;
            gbxBusinessData.Enabled = true;

            btnBusinessNew.Enabled = false;
            btnBusinessEdit.Enabled = true;
            btnBusinessDelete.Enabled = true;
            btnBusinessEdit.Text = "Guardar";
            btnBusinessDelete.Text = "Cancelar";

            txtBusinessID.ReadOnly = false;
            txtBusinessName.ReadOnly = false;
            txtBusinessAdd1.ReadOnly = false;
            txtBusinessAdd2.ReadOnly = false;
            txtBusinessAdd3.ReadOnly = false;
            btnSelectLogo.Enabled = true;
        }

        private void btnBusinessEdit_Click(object sender, EventArgs e)
        {
            if (txtBusinessID.Text.Trim().Equals("") || txtBusinessName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Debe ingresar datos RUC y Razón Social de empresa");
                return;
            }

            if (btnBusinessEdit.Text == "Guardar")
            {
                string logoName = string.Empty;

                if (!txtLogoPath.Text.Trim().Equals("(Sin Imagen Cargada)"))
                {
                    string imgFileExtension = Path.GetExtension(txtLogoPath.Text.Trim());
                    string imgFileName = txtBusinessID.Text.Trim() + imgFileExtension;

                    // Will overwrite if the destination file already exists.
                    string sourceFileName = txtLogoPath.Text.Trim();

                    if (!sourceFileName.Equals("(Sin Imagen Cargada)"))
                    {
                        try
                        {
                            File.Copy(sourceFileName, Path.Combine(dbBusiness.imgDirectory, imgFileName), true);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Error al guardar imagen de logo cargada");
                        }
                    }

                    logoName = imgFileName;
                }

                Business business = new(txtBusinessID.Text.Trim(), txtBusinessName.Text.Trim(),
                    txtBusinessAdd1.Text.Trim(), txtBusinessAdd2.Text.Trim(), txtBusinessAdd3.Text.Trim(), logoName);

                if (txtBusinessID.ReadOnly == true)
                {
                    dbBusiness.Update(x => x.id == business.id, business);
                }
                else
                {
                    dbBusiness.Insert(business);
                }

                dbBusiness.Save();
                LoadBusinessList();

                cbxBusinessList.Enabled = true;
                btnBusinessNew.Enabled = true;
                btnBusinessEdit.Text = "Editar";
                btnBusinessDelete.Text = "Eliminar";
            }
            else
            {
                txtBusinessID.ReadOnly = true;
                txtBusinessName.ReadOnly = false;
                txtBusinessAdd1.ReadOnly = false;
                txtBusinessAdd2.ReadOnly = false;
                txtBusinessAdd3.ReadOnly = false;
                btnSelectLogo.Enabled = true;

                cbxBusinessList.Enabled = false;
                btnBusinessNew.Enabled = false;
                btnBusinessEdit.Text = "Guardar";
                btnBusinessDelete.Text = "Cancelar";
            }
        }

        private void cbxBusinessList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxBusinessList.SelectedIndex == -1) return;

            Business selectedBusiness = dbBusiness.Search(x => x.id == cbxBusinessList.SelectedValue.ToString())[0];

            txtBusinessID.Text = selectedBusiness.id;
            txtBusinessName.Text = selectedBusiness.name;
            txtBusinessAdd1.Text = selectedBusiness.addressLine1;
            txtBusinessAdd2.Text = selectedBusiness.addressLine2;
            txtBusinessAdd3.Text = selectedBusiness.addressLine3;

            if (selectedBusiness.logoName.Equals(""))
            {
                txtLogoPath.Text = "(Sin Imagen Cargada)";

                if (pbxBusinessLogo.Image != null) pbxBusinessLogo.Image.Dispose();
                pbxBusinessLogo.Image = null;
            }
            else
            {
                txtLogoPath.Text = selectedBusiness.logoName;

                try
                {
                    SetPictureBoxBusinessLogo(Path.Combine(dbBusiness.imgDirectory, selectedBusiness.logoName));
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al cargar logo registrado para la empresa.");
                }
            }

            txtBusinessID.ReadOnly = true;
            txtBusinessName.ReadOnly = true;
            txtBusinessAdd1.ReadOnly = true;
            txtBusinessAdd2.ReadOnly = true;
            txtBusinessAdd3.ReadOnly = true;
            btnSelectLogo.Enabled = false;
        }

        private void btnBusinessDelete_Click(object sender, EventArgs e)
        {
            if (btnBusinessDelete.Text == "Eliminar")
            {
                if (MessageBox.Show(this, "¿Desea eliminar la empresa " + cbxBusinessList.Text + "?", "Eliminar empresa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    dbBusiness.Delete(x => x.id == cbxBusinessList.SelectedValue.ToString());
                    dbBusiness.Save();

                    ResetBusinessDataFields();
                    LoadBusinessList();
                }
            }
            else
            {
                LoadBusinessList();

                btnBusinessEdit.Text = "Editar";
                btnBusinessDelete.Text = "Eliminar";
                btnBusinessNew.Enabled = true;
            }
        }

        private void btnSelectLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Imágenes (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *png",
                Title = "Seleccione una imagen para logo de empresa",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtLogoPath.Text = openFileDialog.FileName;
                SetPictureBoxBusinessLogo(openFileDialog.FileName);
            }
        }

        private void ResetBusinessDataFields()
        {
            txtBusinessID.Text = "";
            txtBusinessName.Text = "";
            txtBusinessAdd1.Text = "";
            txtBusinessAdd2.Text = "";
            txtBusinessAdd3.Text = "";
            txtLogoPath.Text = "(Sin Imagen Cargada)";

            if (pbxBusinessLogo.Image != null) pbxBusinessLogo.Image.Dispose();
            pbxBusinessLogo.Image = null;
        }

        private void SetPictureBoxBusinessLogo(string imagePath)
        {
            FileStream imageFileStream = new(imagePath, FileMode.Open, FileAccess.Read);

            if (pbxBusinessLogo.Image != null) pbxBusinessLogo.Image.Dispose();

            pbxBusinessLogo.Image = System.Drawing.Image.FromStream(imageFileStream);
            pbxBusinessLogo.SizeMode = PictureBoxSizeMode.Zoom;
            imageFileStream.Close();
        }

        private string GetFirstValueFromXmlRootElement(string tagName)
        {
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.GetElementsByTagName(tagName);

            if (xmlNodeList.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return xmlNodeList[0].InnerText;
            }
        }

        private string GetFirstValueFromGivenXmlElement(string xmlElementName, string tagName)
        {
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.GetElementsByTagName(xmlElementName);

            if (xmlNodeList.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                XmlElement xmlElement = (XmlElement)xmlNodeList[0];

                xmlNodeList = xmlElement.GetElementsByTagName(tagName);

                if (xmlNodeList.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return xmlNodeList[0].InnerText;
                }
            }
        }

    }
}
