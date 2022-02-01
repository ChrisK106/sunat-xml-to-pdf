
namespace XMLToPDFApp
{
    partial class AppForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtXMLFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadXML = new System.Windows.Forms.Button();
            this.btnGeneratePDF = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPDFPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.chkOpenGeneratedPDF = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gbxBusinessData = new System.Windows.Forms.GroupBox();
            this.pbxBusinessLogo = new System.Windows.Forms.PictureBox();
            this.btnSelectLogo = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLogoPath = new System.Windows.Forms.TextBox();
            this.txtBusinessAdd3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBusinessAdd2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBusinessAdd1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBusinessName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBusinessID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbxSelectedBusiness = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnBusinessDelete = new System.Windows.Forms.Button();
            this.btnBusinessEdit = new System.Windows.Forms.Button();
            this.btnBusinessNew = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxBusinessList = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbxBusinessData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBusinessLogo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtXMLFilePath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnLoadXML);
            this.groupBox1.Location = new System.Drawing.Point(17, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Archivo XML";
            // 
            // txtXMLFilePath
            // 
            this.txtXMLFilePath.Location = new System.Drawing.Point(46, 34);
            this.txtXMLFilePath.Name = "txtXMLFilePath";
            this.txtXMLFilePath.ReadOnly = true;
            this.txtXMLFilePath.Size = new System.Drawing.Size(628, 23);
            this.txtXMLFilePath.TabIndex = 2;
            this.txtXMLFilePath.Text = "(Sin Archivo XML Cargado)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ruta:";
            // 
            // btnLoadXML
            // 
            this.btnLoadXML.Location = new System.Drawing.Point(680, 34);
            this.btnLoadXML.Name = "btnLoadXML";
            this.btnLoadXML.Size = new System.Drawing.Size(79, 23);
            this.btnLoadXML.TabIndex = 0;
            this.btnLoadXML.Text = "Cargar XML";
            this.btnLoadXML.UseVisualStyleBackColor = true;
            this.btnLoadXML.Click += new System.EventHandler(this.btnLoadXML_Click);
            // 
            // btnGeneratePDF
            // 
            this.btnGeneratePDF.Location = new System.Drawing.Point(697, 275);
            this.btnGeneratePDF.Name = "btnGeneratePDF";
            this.btnGeneratePDF.Size = new System.Drawing.Size(96, 44);
            this.btnGeneratePDF.TabIndex = 1;
            this.btnGeneratePDF.Text = "Generar PDF";
            this.btnGeneratePDF.UseVisualStyleBackColor = true;
            this.btnGeneratePDF.Click += new System.EventHandler(this.btnGeneratePDF_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPDFPath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnSelectFolder);
            this.groupBox2.Location = new System.Drawing.Point(17, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 84);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Archivo PDF";
            // 
            // txtPDFPath
            // 
            this.txtPDFPath.Location = new System.Drawing.Point(80, 34);
            this.txtPDFPath.Name = "txtPDFPath";
            this.txtPDFPath.ReadOnly = true;
            this.txtPDFPath.Size = new System.Drawing.Size(594, 23);
            this.txtPDFPath.TabIndex = 2;
            this.txtPDFPath.Text = "(Sin Archivo XML Cargado)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Guardar en:";
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(680, 34);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(79, 23);
            this.btnSelectFolder.TabIndex = 0;
            this.btnSelectFolder.Text = "Examinar...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // chkOpenGeneratedPDF
            // 
            this.chkOpenGeneratedPDF.AutoSize = true;
            this.chkOpenGeneratedPDF.Checked = true;
            this.chkOpenGeneratedPDF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenGeneratedPDF.Location = new System.Drawing.Point(17, 286);
            this.chkOpenGeneratedPDF.Name = "chkOpenGeneratedPDF";
            this.chkOpenGeneratedPDF.Size = new System.Drawing.Size(221, 19);
            this.chkOpenGeneratedPDF.TabIndex = 4;
            this.chkOpenGeneratedPDF.Text = "Abrir archivo PDF luego de generarse";
            this.chkOpenGeneratedPDF.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(16, 386);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 15);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "lblStatus";
            // 
            // gbxBusinessData
            // 
            this.gbxBusinessData.Controls.Add(this.pbxBusinessLogo);
            this.gbxBusinessData.Controls.Add(this.btnSelectLogo);
            this.gbxBusinessData.Controls.Add(this.label9);
            this.gbxBusinessData.Controls.Add(this.txtLogoPath);
            this.gbxBusinessData.Controls.Add(this.txtBusinessAdd3);
            this.gbxBusinessData.Controls.Add(this.label7);
            this.gbxBusinessData.Controls.Add(this.txtBusinessAdd2);
            this.gbxBusinessData.Controls.Add(this.label6);
            this.gbxBusinessData.Controls.Add(this.txtBusinessAdd1);
            this.gbxBusinessData.Controls.Add(this.label5);
            this.gbxBusinessData.Controls.Add(this.txtBusinessName);
            this.gbxBusinessData.Controls.Add(this.label4);
            this.gbxBusinessData.Controls.Add(this.txtBusinessID);
            this.gbxBusinessData.Controls.Add(this.label3);
            this.gbxBusinessData.Location = new System.Drawing.Point(15, 53);
            this.gbxBusinessData.Name = "gbxBusinessData";
            this.gbxBusinessData.Size = new System.Drawing.Size(778, 264);
            this.gbxBusinessData.TabIndex = 2;
            this.gbxBusinessData.TabStop = false;
            this.gbxBusinessData.Text = "Datos de Empresa";
            // 
            // pbxBusinessLogo
            // 
            this.pbxBusinessLogo.Location = new System.Drawing.Point(561, 30);
            this.pbxBusinessLogo.Name = "pbxBusinessLogo";
            this.pbxBusinessLogo.Size = new System.Drawing.Size(199, 172);
            this.pbxBusinessLogo.TabIndex = 13;
            this.pbxBusinessLogo.TabStop = false;
            // 
            // btnSelectLogo
            // 
            this.btnSelectLogo.Location = new System.Drawing.Point(675, 217);
            this.btnSelectLogo.Name = "btnSelectLogo";
            this.btnSelectLogo.Size = new System.Drawing.Size(85, 23);
            this.btnSelectLogo.TabIndex = 12;
            this.btnSelectLogo.Text = "Examinar...";
            this.btnSelectLogo.UseVisualStyleBackColor = true;
            this.btnSelectLogo.Click += new System.EventHandler(this.btnSelectLogo_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(53, 220);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 15);
            this.label9.TabIndex = 11;
            this.label9.Text = "Logo :";
            // 
            // txtLogoPath
            // 
            this.txtLogoPath.Location = new System.Drawing.Point(99, 217);
            this.txtLogoPath.Name = "txtLogoPath";
            this.txtLogoPath.ReadOnly = true;
            this.txtLogoPath.Size = new System.Drawing.Size(562, 23);
            this.txtLogoPath.TabIndex = 10;
            this.txtLogoPath.Text = "(Imagen No Cargada)";
            // 
            // txtBusinessAdd3
            // 
            this.txtBusinessAdd3.Location = new System.Drawing.Point(99, 179);
            this.txtBusinessAdd3.Name = "txtBusinessAdd3";
            this.txtBusinessAdd3.Size = new System.Drawing.Size(443, 23);
            this.txtBusinessAdd3.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 15);
            this.label7.TabIndex = 8;
            this.label7.Text = "Dirección 3 :";
            // 
            // txtBusinessAdd2
            // 
            this.txtBusinessAdd2.Location = new System.Drawing.Point(99, 141);
            this.txtBusinessAdd2.Name = "txtBusinessAdd2";
            this.txtBusinessAdd2.Size = new System.Drawing.Size(443, 23);
            this.txtBusinessAdd2.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 15);
            this.label6.TabIndex = 6;
            this.label6.Text = "Dirección 2 :";
            // 
            // txtBusinessAdd1
            // 
            this.txtBusinessAdd1.Location = new System.Drawing.Point(99, 104);
            this.txtBusinessAdd1.Name = "txtBusinessAdd1";
            this.txtBusinessAdd1.Size = new System.Drawing.Size(443, 23);
            this.txtBusinessAdd1.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Dirección 1 :";
            // 
            // txtBusinessName
            // 
            this.txtBusinessName.Location = new System.Drawing.Point(99, 66);
            this.txtBusinessName.Name = "txtBusinessName";
            this.txtBusinessName.Size = new System.Drawing.Size(443, 23);
            this.txtBusinessName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "RUC :";
            // 
            // txtBusinessID
            // 
            this.txtBusinessID.Location = new System.Drawing.Point(99, 30);
            this.txtBusinessID.Name = "txtBusinessID";
            this.txtBusinessID.Size = new System.Drawing.Size(443, 23);
            this.txtBusinessID.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Razón Social :";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(819, 362);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbxSelectedBusiness);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.btnGeneratePDF);
            this.tabPage1.Controls.Add(this.chkOpenGeneratedPDF);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(811, 334);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "XML/PDF";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbxSelectedBusiness
            // 
            this.cbxSelectedBusiness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSelectedBusiness.FormattingEnabled = true;
            this.cbxSelectedBusiness.Location = new System.Drawing.Point(149, 227);
            this.cbxSelectedBusiness.Name = "cbxSelectedBusiness";
            this.cbxSelectedBusiness.Size = new System.Drawing.Size(644, 23);
            this.cbxSelectedBusiness.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 230);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 15);
            this.label10.TabIndex = 5;
            this.label10.Text = "Empresa seleccionada:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnBusinessDelete);
            this.tabPage2.Controls.Add(this.btnBusinessEdit);
            this.tabPage2.Controls.Add(this.btnBusinessNew);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.cbxBusinessList);
            this.tabPage2.Controls.Add(this.gbxBusinessData);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(811, 334);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Empresas";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnBusinessDelete
            // 
            this.btnBusinessDelete.Location = new System.Drawing.Point(721, 14);
            this.btnBusinessDelete.Name = "btnBusinessDelete";
            this.btnBusinessDelete.Size = new System.Drawing.Size(72, 23);
            this.btnBusinessDelete.TabIndex = 7;
            this.btnBusinessDelete.Text = "Eliminar";
            this.btnBusinessDelete.UseVisualStyleBackColor = true;
            this.btnBusinessDelete.Click += new System.EventHandler(this.btnBusinessDelete_Click);
            // 
            // btnBusinessEdit
            // 
            this.btnBusinessEdit.Location = new System.Drawing.Point(644, 14);
            this.btnBusinessEdit.Name = "btnBusinessEdit";
            this.btnBusinessEdit.Size = new System.Drawing.Size(71, 23);
            this.btnBusinessEdit.TabIndex = 6;
            this.btnBusinessEdit.Text = "Editar";
            this.btnBusinessEdit.UseVisualStyleBackColor = true;
            this.btnBusinessEdit.Click += new System.EventHandler(this.btnBusinessEdit_Click);
            // 
            // btnBusinessNew
            // 
            this.btnBusinessNew.Location = new System.Drawing.Point(576, 14);
            this.btnBusinessNew.Name = "btnBusinessNew";
            this.btnBusinessNew.Size = new System.Drawing.Size(62, 23);
            this.btnBusinessNew.TabIndex = 5;
            this.btnBusinessNew.Text = "Nuevo";
            this.btnBusinessNew.UseVisualStyleBackColor = true;
            this.btnBusinessNew.Click += new System.EventHandler(this.btnBusinessNew_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 15);
            this.label8.TabIndex = 4;
            this.label8.Text = "Empresa :";
            // 
            // cbxBusinessList
            // 
            this.cbxBusinessList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBusinessList.FormattingEnabled = true;
            this.cbxBusinessList.Location = new System.Drawing.Point(115, 15);
            this.cbxBusinessList.Name = "cbxBusinessList";
            this.cbxBusinessList.Size = new System.Drawing.Size(442, 23);
            this.cbxBusinessList.TabIndex = 3;
            this.cbxBusinessList.SelectedIndexChanged += new System.EventHandler(this.cbxBusinessList_SelectedIndexChanged);
            // 
            // AppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 415);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AppForm";
            this.Text = "SUNAT XML a PDF";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbxBusinessData.ResumeLayout(false);
            this.gbxBusinessData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBusinessLogo)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoadXML;
        private System.Windows.Forms.TextBox txtXMLFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGeneratePDF;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPDFPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.CheckBox chkOpenGeneratedPDF;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox gbxBusinessData;
        private System.Windows.Forms.TextBox txtBusinessID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBusinessAdd3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBusinessAdd2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBusinessAdd1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBusinessName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxBusinessList;
        private System.Windows.Forms.Button btnSelectLogo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLogoPath;
        private System.Windows.Forms.PictureBox pbxBusinessLogo;
        private System.Windows.Forms.Button btnBusinessDelete;
        private System.Windows.Forms.Button btnBusinessEdit;
        private System.Windows.Forms.Button btnBusinessNew;
        private System.Windows.Forms.ComboBox cbxSelectedBusiness;
        private System.Windows.Forms.Label label10;
    }
}

