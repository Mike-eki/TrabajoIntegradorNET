// BlazorApp/Services/PdfGenerator.cs
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BlazorApp.Services;

public static class PdfGenerator
{
    public static byte[] GenerateAcademicPdf(string studentName, string careerName, List<AcademicStatusItem> subjects)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Estado Académico")
                    .FontSize(24)
                    .Bold()
                    .AlignCenter();

                page.Content()
                    .PaddingVertical(20)
                    .Column(col =>
                    {
                        col.Item().Text($"Estudiante: {studentName}").FontSize(14).Bold();
                        col.Item().Text($"Carrera: {careerName}").FontSize(14).Bold();
                        col.Item().PaddingTop(20);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Materia").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Nota").Bold().AlignCenter();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Estado").Bold().AlignCenter();
                            });

                            foreach (var s in subjects)
                            {
                                var nota = s.FinalGrade.HasValue ? s.FinalGrade.Value.ToString("N1") : "-";
                                var estado = s.FinalGrade.HasValue ? "Finalizada" : "En curso";
                                var color = s.FinalGrade.HasValue && s.FinalGrade >= 4 ? Colors.Green.Darken1 : Colors.Red.Darken1;

                                table.Cell().Padding(5).Text(s.SubjectName);
                                table.Cell().Padding(5).AlignCenter().Text(nota).FontColor(color);
                                table.Cell().Padding(5).AlignCenter().Text(estado);
                            }
                        });

                        col.Item().PaddingTop(40)
                            .AlignCenter()
                            .Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(10)
                            .Italic();
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x => x.CurrentPageNumber());
            });
        }).GeneratePdf();
    }
}

// Clase simple para no depender de DTOs del backend
public class AcademicStatusItem
{
    public string SubjectName { get; set; } = "";
    public decimal? FinalGrade { get; set; }
}