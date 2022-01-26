using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Diagnostics;
using System.Data;

namespace XMLToPDFApp
{
    public partial class AppForm : Form
    {
        Database<Business> dbBusiness = new("data/BusinessDB.json");

        public AppForm()
        {
            InitializeComponent();

            CenterToScreen();
            lblStatus.Text = "Listo";
            Text = "SUNAT XML a PDF v." + Application.ProductVersion;

            loadBusinessList();
        }

        string imgFolder = "data/img";
        string xmlFilePath = string.Empty;

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
                //Get the path of specified file
                xmlFilePath = openFileDialog.FileName;
                txtXMLFilePath.Text = xmlFilePath;
                txtPDFPath.Text = Path.GetDirectoryName(openFileDialog.FileName);
            }

        }

        private void btnGeneratePDF_Click(object sender, EventArgs e)
        {
            try {

            if (txtXMLFilePath.Text.Equals("(Sin Archivo XML Cargado)"))
            {
                MessageBox.Show(this, "Seleccione un archivo XML para continuar", "XML no cargado",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            lblStatus.Text = "Extrayendo datos de archivo XML...";

            XmlDocument doc = new();
            doc.Load(txtXMLFilePath.Text);

            string ublVersion = doc.DocumentElement.GetElementsByTagName("cbc:UBLVersionID")[0].InnerText;
            string customizationId = doc.DocumentElement.GetElementsByTagName("cbc:CustomizationID")[0].InnerText;

            string digestValue = doc.DocumentElement.GetElementsByTagName("ds:DigestValue")[0].InnerText;

            string documentId = doc.DocumentElement.GetElementsByTagName("cbc:ID")[0].InnerText;

            string issueDate = doc.DocumentElement.GetElementsByTagName("cbc:IssueDate")[0].InnerText;

            string dueDate = string.Empty;

            if (doc.DocumentElement.GetElementsByTagName("cbc:DueDate").Count > 0){
                dueDate = doc.DocumentElement.GetElementsByTagName("cbc:DueDate")[0].InnerText;
            }

            string invoiceTypeCode = doc.DocumentElement.GetElementsByTagName("cbc:InvoiceTypeCode")[0].InnerText;

            string htmlSource = string.Empty;
            string documentTitle = string.Empty;
            string customerTypeCode = string.Empty;
            string footerText = string.Empty;

            //FACTURA
            if (invoiceTypeCode == "01")
            {
                htmlSource = Properties.Resources.invoice_template.ToString();
                documentTitle = "FACTURA ELECTRÓNICA";
                customerTypeCode = "06";
                footerText = "Representación impresa de la Factura Electrónica. " +
                "Consulte o descargue su comprobante electrónico en sunat.gob.pe";
            }
            //BOLETA
            else if (invoiceTypeCode == "03")
            {
                htmlSource = Properties.Resources.receipt_template.ToString();
                documentTitle = "BOLETA DE VENTA ELECTRÓNICA";
                customerTypeCode = "01";
                footerText = "Representación impresa de la Boleta de Venta Electrónica. " +
                "Consulte o descargue su comprobante electrónico en sunat.gob.pe";
            }

            XmlNodeList documentNotes = doc.DocumentElement.GetElementsByTagName("cbc:Note");

            string payableAmountText = documentNotes[0].InnerText;
            string documentObservations = string.Empty;

            if (documentNotes.Count > 1)
            {
                documentObservations = documentNotes[1].InnerText;
            }

            string documentCurrencyCode = doc.DocumentElement.GetElementsByTagName("cbc:DocumentCurrencyCode")[0].InnerText;

            //string documentReference = doc.DocumentElement.GetElementsByTagName("cac:AdditionalDocumentReference")[0].InnerText;

            XmlElement supplierParty = (XmlElement) doc.DocumentElement.GetElementsByTagName("cac:AccountingSupplierParty")[0];

            string supplierId = supplierParty.GetElementsByTagName("cbc:ID")[0].InnerText;
            string supplierName = supplierParty.GetElementsByTagName("cbc:Name")[0].InnerText;
            string supplierRegistrationName = supplierParty.GetElementsByTagName("cbc:RegistrationName")[0].InnerText;
            string supplierCityName = supplierParty.GetElementsByTagName("cbc:CityName")[0].InnerText;
            string supplierCountrySubentity = supplierParty.GetElementsByTagName("cbc:CountrySubentity")[0].InnerText;
            string supplierCountrySubentityCode = supplierParty.GetElementsByTagName("cbc:CountrySubentityCode")[0].InnerText;
            string supplierDistrict = supplierParty.GetElementsByTagName("cbc:District")[0].InnerText;
            string supplierLine = supplierParty.GetElementsByTagName("cbc:Line")[0].InnerText;

            XmlElement customerParty = (XmlElement)doc.DocumentElement.GetElementsByTagName("cac:AccountingCustomerParty")[0];

            string customerId = customerParty.GetElementsByTagName("cbc:ID")[0].InnerText;
            //string customerName = customerParty.GetElementsByTagName("cbc:Name")[0].InnerText;
            string customerRegistrationName = customerParty.GetElementsByTagName("cbc:RegistrationName")[0].InnerText;
            string customerCityName = customerParty.GetElementsByTagName("cbc:CityName")[0].InnerText;
            string customerCountrySubentity = customerParty.GetElementsByTagName("cbc:CountrySubentity")[0].InnerText;
            //string customerCountrySubentityCode = customerParty.GetElementsByTagName("cbc:CountrySubentityCode")[0].InnerText;
            string customerDistrict = customerParty.GetElementsByTagName("cbc:District")[0].InnerText;
            string customerLine = customerParty.GetElementsByTagName("cbc:Line")[0].InnerText;

            //string paidAmount = doc.DocumentElement.GetElementsByTagName("cbc:PaidAmount")[0].InnerText;
            string taxAmount = doc.DocumentElement.GetElementsByTagName("cbc:TaxAmount")[0].InnerText;
            string taxableAmount = doc.DocumentElement.GetElementsByTagName("cbc:TaxableAmount")[0].InnerText;
            string lineExtensionAmount = doc.DocumentElement.GetElementsByTagName("cbc:LineExtensionAmount")[0].InnerText;
            string allowanceTotalAmount = doc.DocumentElement.GetElementsByTagName("cbc:AllowanceTotalAmount")[0].InnerText;
            string chargeTotalAmount = doc.DocumentElement.GetElementsByTagName("cbc:ChargeTotalAmount")[0].InnerText;
            string prepaidAmount = doc.DocumentElement.GetElementsByTagName("cbc:PrepaidAmount")[0].InnerText;
            string payableAmount = doc.DocumentElement.GetElementsByTagName("cbc:PayableAmount")[0].InnerText;

            List<InvoiceLine> documentDetail = new();

            foreach (XmlElement invoiceLine in doc.DocumentElement.GetElementsByTagName("cac:InvoiceLine"))
            {
                InvoiceLine line = new();
                line.id = invoiceLine.GetElementsByTagName("cbc:ID")[0].InnerText;
                line.quantity = invoiceLine.GetElementsByTagName("cbc:InvoicedQuantity")[0].InnerText;
                line.itemId = invoiceLine.GetElementsByTagName("cac:Item")[0]["cac:SellersItemIdentification"]["cbc:ID"].InnerText;
                line.description = invoiceLine.GetElementsByTagName("cac:Item")[0]["cbc:Description"].InnerText;
                line.priceAmount = invoiceLine.GetElementsByTagName("cac:Price")[0]["cbc:PriceAmount"].InnerText;
                documentDetail.Add(line);
            }

            string pdfFileName = txtPDFPath.Text + "\\" + documentId + "_" + supplierId + "_" + DateTime.Now.ToString("ddMMyy_HHmmss") + ".pdf";
            
            Business selectedBusiness = dbBusiness.Search(x => x.id == cbxSelectedBusiness.SelectedValue.ToString())[0];

            string headerBusinessID = selectedBusiness.id;
            string headerBusinessName = selectedBusiness.name;
            string headerAddressLine1 = selectedBusiness.addressLine1;
            string headerAddressLine2 = selectedBusiness.addressLine2;
            string headerAddressLine3 = selectedBusiness.addressLine3;

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

            //Código Hash
            htmlSource = htmlSource.Replace("@DIGEST_VALUE", digestValue);

            //Sub Total
            htmlSource = htmlSource.Replace("@DOCUMENT_SUBTOTAL", documentCurrencySymbol + documentSubTotal.ToString("N2"));

            //Anticipos
            htmlSource = htmlSource.Replace("@DOCUMENT_PREPAID_AMOUNT", documentCurrencySymbol + double.Parse(prepaidAmount).ToString("N2"));

            //Descuentos
            htmlSource = htmlSource.Replace("@DOCUMENT_DISCOUNTS", documentCurrencySymbol + "0.00");

            //Valor de Venta
            htmlSource = htmlSource.Replace("@DOCUMENT_LINE_EXTENSION_AMOUNT", documentCurrencySymbol + double.Parse(lineExtensionAmount).ToString("N2"));

            //IGV
            htmlSource = htmlSource.Replace("@DOCUMENT_TAX_AMOUNT", documentCurrencySymbol + double.Parse(taxAmount).ToString("N2"));

            //Otros Cargos
            htmlSource = htmlSource.Replace("@DOCUMENT_OTHERS_CHARGES", documentCurrencySymbol + "0.00");

            //Total a Pagar
            htmlSource = htmlSource.Replace("@DOCUMENT_PAYABLE_AMOUNT", documentCurrencySymbol + double.Parse(payableAmount).ToString("N2"));

            string datetimeFileGeneration = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            htmlSource = htmlSource.Replace("@DATETIME_GENERATION", datetimeFileGeneration);

            lblStatus.Text = "Generando archivo PDF...";


            using (FileStream stream = new(pdfFileName, FileMode.Create)) {
                Document pdfDoc = new(PageSize.A4, 25, 25, 25, 25);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                writer.PageEvent = new Footer(footerText);

                pdfDoc.Open();

                Image headerImage;

                /*
                headerImage = Image.GetInstance(Properties.Resources.logo_PSV, ImageFormat.Jpeg);
                headerImage.ScaleToFit(160, 120);
                //PSV
                headerImage.SetAbsolutePosition(pdfDoc.LeftMargin, pdfDoc.Top - 72);
                //PDI
                headerImage.SetAbsolutePosition(pdfDoc.LeftMargin, pdfDoc.Top - 90);
                */

                if (!selectedBusiness.logoName.Equals(""))
                {
                    string imgPath = Path.Combine(imgFolder, selectedBusiness.logoName);

                    headerImage = Image.GetInstance(imgPath);
                    headerImage.ScaleToFit(160, 120);
                    headerImage.ScaleToFitHeight = true;
                    //headerImage.Alignment = Image.ALIGN_CENTER;
                    headerImage.SetAbsolutePosition(pdfDoc.LeftMargin, pdfDoc.Top - headerImage.ScaledHeight);
                    headerImage.Alignment = Image.UNDERLYING;

                    pdfDoc.Add(headerImage);
                }

                string documentIdSeries = documentId.Substring(0, documentId.Length - documentId.IndexOf("-") + 1);
                string documentIdNumber = documentId.Substring(documentId.IndexOf("-") +1);

                string qrCodeString = headerBusinessID + "|" + invoiceTypeCode + "|" + documentIdSeries + "|" + documentIdNumber + "|" +
                    taxAmount + "|" + payableAmount + "|" + issueDate + "|" + customerTypeCode + "|" + customerId + "|";

                BarcodeQRCode qrCode = new(qrCodeString, 250, 250, null);
                Image qrCodeImage = qrCode.GetImage();
                qrCodeImage.ScaleAbsolute(120, 120);
                //qrCodeImage.Alignment = Image.UNDERLYING;
                qrCodeImage.SetAbsolutePosition((pdfDoc.Right + pdfDoc.RightMargin)/2 - 60, pdfDoc.Bottom + 35);

                pdfDoc.Add(qrCodeImage);

                using (StringReader sr = new(htmlSource))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                }

                pdfDoc.Close();
                stream.Close();
            };

                lblStatus.Text = "Se generó el archivo PDF: " + pdfFileName;

            if (chkOpenGeneratedPDF.Checked)
            {
                try
                {
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo(pdfFileName)
                        {
                            UseShellExecute = true
                        }
                    };

                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error al intentar abrir archivo PDF generado", MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error al generar archivo PDF", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }

        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPDFPath.Text=folderBrowserDialog.SelectedPath;
            }
        }

        private void checkSelectedBusiness()
        {
            txtBusinessID.Text = "20112182021";
            txtBusinessName.Text = "P.S.V. CONSTRUCTORES S.A.";
            txtBusinessAdd1.Text = "AV. DEL PINAR 180 INT. 1104 URB. CHACARILLA DEL ESTANQUE";
            txtBusinessAdd2.Text = "CDRA. 4 DE AV. PRIMAVERA";
            txtBusinessAdd3.Text = "LIMA - SANTIAGO DE SURCO";
            
            txtBusinessID.Text = "20538107566";
            txtBusinessName.Text = "PRUEBAS DINAMICAS E INGENIERIA S.A.C.";
            txtBusinessAdd1.Text = "AV. DEL PINAR 180 INT. 1104 URB. CHACARILLA DEL ESTANQUE";
            txtBusinessAdd2.Text = "CDRA. 4 DE AV. PRIMAVERA";
            txtBusinessAdd3.Text = "LIMA - SANTIAGO DE SURCO";

            /*
            string headerBusinessName = "PSV PILOTES PERÚ S.A.C.";
            string headerBusinessID = "20605411801";
            string headerAddressLine1 = "CALLE LOS ANTARES MZA. A5 LOTE 1 DPTO. A502";
            string headerAddressLine2 = "URB. LA ALBORADA";
            string headerAddressLine3 = "SANTIAGO DE SURCO - LIMA";
            */
        }

        private void loadBusinessList()
        {
            dbBusiness.Load();

            //cbxSelectedBusiness.Items.Clear();
            //cbxBusinessList.Items.Clear();
            cbxBusinessList.DataSource = null;
            cbxSelectedBusiness.DataSource = null;

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
            txtBusinessID.Text = "";
            txtBusinessName.Text = "";
            txtBusinessAdd1.Text = "";
            txtBusinessAdd2.Text = "";
            txtBusinessAdd3.Text = "";
            txtLogoPath.Text = "(Sin Imagen Cargada)";

            cbxBusinessList.Enabled = false;
            btnBusinessNew.Enabled = false;
            btnBusinessEdit.Text = "Guardar";
            btnBusinessDelete.Text = "Cancelar";

            pbxBusinessLogo.Image = null;

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
                            File.Copy(sourceFileName, Path.Combine(imgFolder, imgFileName), true);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Error al guardar imagen cargada");
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
                loadBusinessList();

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
                pbxBusinessLogo.Image = null;
            }
            else
            {
                txtLogoPath.Text = selectedBusiness.logoName;

                try
                {
                    FileStream imageFileStream = new(Path.Combine(imgFolder, selectedBusiness.logoName), FileMode.Open, FileAccess.Read);
                    pbxBusinessLogo.Image = System.Drawing.Image.FromStream(imageFileStream);
                    pbxBusinessLogo.SizeMode = PictureBoxSizeMode.Zoom;
                    imageFileStream.Close();
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
                    loadBusinessList();
                }
            }
            else
            {
                loadBusinessList();
                
                btnBusinessEdit.Text = "Editar";
                btnBusinessDelete.Text = "Eliminar";
                btnBusinessNew.Enabled = true;
                cbxBusinessList.Enabled = true;
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

                FileStream imageFileStream = new(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                pbxBusinessLogo.Image = System.Drawing.Image.FromStream(imageFileStream);
                pbxBusinessLogo.SizeMode = PictureBoxSizeMode.Zoom;
                imageFileStream.Close();
            }
        }
    }
 }
