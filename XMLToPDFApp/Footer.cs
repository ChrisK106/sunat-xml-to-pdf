using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace XMLToPDFApp
{
    class Footer : PdfPageEventHelper
    {
        private string footerText = string.Empty;

        public Footer(string footerText)
        {
            this.footerText = footerText;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            Font font = new(Font.FontFamily.UNDEFINED, 7.5f);

            PdfPTable tbFooter = new(6);

            tbFooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            tbFooter.DefaultCell.Border = 0;

            PdfPCell _cell = new(new Paragraph(footerText, font));
            _cell.HorizontalAlignment = Element.ALIGN_LEFT;
            _cell.Border = 0;
            _cell.Colspan = 5;
            _cell.PaddingBottom = 10;
            tbFooter.AddCell(_cell);

            _cell = new(new Paragraph("Página " + writer.CurrentPageNumber + "/" + writer.PageNumber, font));
            _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _cell.Border = 0;
            tbFooter.AddCell(_cell);

            string datetimeFileGeneration = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            _cell = new(new Paragraph(datetimeFileGeneration, font));
            _cell.HorizontalAlignment = Element.ALIGN_CENTER;
            _cell.Border = 0;
            _cell.Colspan = 6;
            tbFooter.AddCell(_cell);

            tbFooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) + 35, writer.DirectContent);
        }
    }
}
