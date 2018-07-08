using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace prjt.Services.Pdf
{
    public class ListingReportGenerator : IListingReportGenerator
    {
        public void Save(string filePath, Document document)
        {
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);

            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            pdfRenderer.Save(filePath);
        }
    }
}
