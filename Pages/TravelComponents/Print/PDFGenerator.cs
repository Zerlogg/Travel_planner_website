using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using TravelingTrips.Models;
using TravelingTrips.Data;

namespace TravelingTrips.Pages.TravelComponents.Print;

public class PDFGenerator
{
    public void DownloadPDF(IJSRuntime js, Travel travel, List<Accommodation> accommodations, List<Restaurant> restaurants, List<List<TourismObject>> tourismObjects, string filename = "report.pdf")
    {
        js.InvokeVoidAsync("DownloadPdf",
            filename,
            Convert.ToBase64String(PDFReport(travel, accommodations, restaurants, tourismObjects))
        );
    }

    private byte[] PDFReport(Travel travel, List<Accommodation> accommodations, List<Restaurant> restaurants, List<List<TourismObject>> tourismObjects)
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

        pdf.AddTitle("Traveling Trips PDF");
        pdf.AddAuthor("TravelingTrips");
        pdf.AddCreationDate();
        pdf.AddKeywords("Blazor");
        pdf.AddSubject("Pdf Generate");

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

        var labelFooter = new Chunk("Page ", fontStyle);
        HeaderFooter footer = new HeaderFooter(new Phrase(labelFooter), true)
        {
            BackgroundColor = new BaseColor(120, 3, 120),
            Alignment = Element.ALIGN_RIGHT,
            Border = Rectangle.NO_BORDER
        };
        pdf.Footer = footer;

        pdf.Open();

        var title = new Paragraph($"Trip to {travel.City}", new Font(Font.HELVETICA, 20, Font.BOLD));
        title.SpacingAfter = 18f;
        pdf.Add(title);

        Font boldFont = FontFactory.GetFont("Tahoma", 12f, Font.BOLD);
        Font regularFont = FontFactory.GetFont("Tahoma", 12f, Font.NORMAL);
        
        AddLabeledParagraph(pdf, "Description:", travel.Description, boldFont, regularFont);
        AddLabeledParagraph(pdf, "Budget:", $"{travel.Budget}€", boldFont, regularFont);
        AddLabeledParagraph(pdf, "Trip duration:", $"{travel.StartDate:dd.MM.yyyy} - {travel.EndDate:dd.MM.yyyy}", boldFont, regularFont);
        AddLabeledParagraph(pdf, "Personal preferences:", travel.Preferences, boldFont, regularFont);
        
        AddAccommodationSection(pdf, accommodations, boldFont, regularFont);
        
        AddRestaurantSection(pdf, restaurants, boldFont, regularFont);
        
        AddTourismObjectSection(pdf, tourismObjects, boldFont, regularFont);

        pdf.Close();

        return memoryStream.ToArray();
    }
    
    private void AddLabeledParagraph(Document pdf, string label, string text, Font labelFont, Font textFont)
    {
        var phrase = new Phrase();
        phrase.Add(new Chunk(label, labelFont));
        phrase.Add(new Chunk(" " + text, textFont));
        var paragraph = new Paragraph(phrase)
        {
            SpacingAfter = 8f
        };
        pdf.Add(paragraph);
    }
    
    private void AddAccommodationSection(Document pdf, List<Accommodation> accommodations, Font labelFont, Font textFont)
    {
        if (accommodations != null && accommodations.Any())
        {
            var sectionTitle = new Paragraph("Accommodations", new Font(Font.HELVETICA, 16, Font.BOLD))
            {
                SpacingBefore = 14f,
                SpacingAfter = 10f
            };
            pdf.Add(sectionTitle);

            foreach (var accommodation in accommodations)
            {
                if (accommodation.Image != null && accommodation.Image.Length > 0)
                {
                    var img = Image.GetInstance(accommodation.Image);
                    img.ScaleToFit(150f, 150f);
                    img.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(img);
                }
                
                AddLabeledParagraph(pdf, "Name:", accommodation.Name, labelFont, textFont);
                AddLabeledParagraph(pdf, "Description:", accommodation.Description, labelFont, textFont);
                AddLabeledParagraph(pdf, "Location:", accommodation.Location, labelFont, textFont);
                AddLabeledParagraph(pdf, "Price:", $"{accommodation.Price}€ / per person", labelFont, textFont);
                AddLabeledParagraph(pdf, "Is preferred:", accommodation.IsSelected == true ? "yes" : "no", labelFont, textFont);
                
                pdf.Add(new Paragraph("\n"));
            }
        }
    }
    
    private void AddRestaurantSection(Document pdf, List<Restaurant> restaurants, Font labelFont, Font textFont)
    {
        if (restaurants != null && restaurants.Any())
        {
            var sectionTitle = new Paragraph("Restaurants", new Font(Font.HELVETICA, 16, Font.BOLD))
            {
                SpacingBefore = 0f,
                SpacingAfter = 10f
            };
            pdf.Add(sectionTitle);

            foreach (var restaurant in restaurants)
            {
                if (restaurant.Image != null && restaurant.Image.Length > 0)
                {
                    var img = Image.GetInstance(restaurant.Image);
                    img.ScaleToFit(150f, 150f);
                    img.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(img);
                }
                
                AddLabeledParagraph(pdf, "Name:", restaurant.Name, labelFont, textFont);
                AddLabeledParagraph(pdf, "Description:", restaurant.Description, labelFont, textFont);
                AddLabeledParagraph(pdf, "Location:", restaurant.Location, labelFont, textFont);
                AddLabeledParagraph(pdf, "Working hours:", $"{restaurant.StartHours} - {restaurant.EndHours}", labelFont, textFont);
                AddLabeledParagraph(pdf, "Is preferred:", restaurant.IsSelected == true ? "yes" : "no", labelFont, textFont);
                
                pdf.Add(new Paragraph("\n"));
            }
        }
    }
    
    private void AddTourismObjectSection(Document pdf, List<List<TourismObject>> tourismObjects, Font labelFont, Font textFont)
    {
        int dayIndex = 1;

        foreach (var dayObjects in tourismObjects)
        {
            var sectionTitle = new Paragraph($"Day {dayIndex++} places to visit", new Font(Font.HELVETICA, 16, Font.BOLD))
            {
                SpacingBefore = 14f,
                SpacingAfter = 10f
            };
            pdf.Add(sectionTitle);

            foreach (var tourismObject in dayObjects)
            {
                if (tourismObject.Image != null && tourismObject.Image.Length > 0)
                {
                    var img = Image.GetInstance(tourismObject.Image);
                    img.ScaleToFit(150f, 150f);
                    img.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(img);
                }

                AddLabeledParagraph(pdf, "Name:", tourismObject.Name, labelFont, textFont);
                AddLabeledParagraph(pdf, "Description:", tourismObject.Description, labelFont, textFont);
                AddLabeledParagraph(pdf, "Location:", tourismObject.Location, labelFont, textFont);
                AddLabeledParagraph(pdf, "Price:", $"{tourismObject.Price}€ / per person", labelFont, textFont);
            
                pdf.Add(new Paragraph("\n"));
            }
        }
    }
}