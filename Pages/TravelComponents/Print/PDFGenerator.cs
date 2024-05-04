using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using TravelingTrips.Data;

namespace TravelingTrips.Pages.TravelComponents.Print;

public class PDFGenerator
{
    public void DownloadPDF(IJSRuntime js, string filename="report.pdf")
    {
        js.InvokeVoidAsync("DownloadPdf",
            filename,
            Convert.ToBase64String(PDFReport())
            );
    }

    private byte[] PDFReport()
    {
        var memoryStream = new MemoryStream();
        float margeLeft = 1.5f;
        float margeRight = 1.5f;
        float margeTop = 1.0f;
        float margeBottom = 1.0f;

        Document pdf = new Document(
            PageSize.A4,
            margeLeft.ToDpi(),
            margeRight.ToDpi(),
            margeTop.ToDpi(),
            margeBottom.ToDpi()
        );

        pdf.AddTitle("Blazor PDF");
        pdf.AddAuthor("Zerlog");
        pdf.AddCreationDate();
        pdf.AddKeywords("Blazor");
        pdf.AddSubject("Pdf Generate with iText Sharp");

        PdfWriter writer = PdfWriter.GetInstance(pdf, memoryStream);

        var fontStyle = FontFactory.GetFont("Arial", 16, BaseColor.White);
        var labelHeader = new Chunk("Traveling Trips", fontStyle);

        HeaderFooter header = new HeaderFooter(new Phrase(labelHeader), false)
        {
            BackgroundColor = new BaseColor(50, 20, 120),
            Alignment = Element.ALIGN_CENTER,
            Border = Rectangle.NO_BORDER
        };
        pdf.Header = header;
        
        var labelFooter = new Chunk("Page", fontStyle);
        HeaderFooter footer = new HeaderFooter(new Phrase(labelFooter), true)
        {
            BackgroundColor = new BaseColor(120, 3, 120),
            Alignment = Element.ALIGN_RIGHT,
            Border = Rectangle.NO_BORDER
        };
        pdf.Footer = footer;
        
        pdf.Open();

        var titel = new Paragraph("Your trip", new Font(Font.HELVETICA, 20, Font.BOLD));
        titel.SpacingAfter = 18f;

        pdf.Add(titel);

        Font _fontStyle = FontFactory.GetFont("Tahoma", 12f, Font.NORMAL);

        var _mytext = "Test Test Test Test Test Test Test Test Test Test Test";
        var phrase = new Phrase(_mytext, _fontStyle);
        pdf.Add(phrase);
        
        pdf.Close();

        return memoryStream.ToArray();
    }
} 