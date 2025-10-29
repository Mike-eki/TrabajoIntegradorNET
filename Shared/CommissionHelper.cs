using System;
using System.Text.RegularExpressions;

namespace Services
{
    public class CommissionHelper
    {
        /// <summary>
        /// Genera el nombre de la comisión a partir del nombre de la materia,
        /// el año del ciclo y el id de la comisión.
        /// </summary>
        /// <param name="materia">Nombre completo de la materia (p. ej. "Análisis Matemático I")</param>
        /// <param name="anio">Año del ciclo (p. ej. 2025)</param>
        /// <param name="idComision">Id de la comisión (p. ej. 4)</param>
        /// <returns>Nombre de la comisión con el formato "INIC-YY-ID"</returns>
        public static string GenerateName(string materia, int anio, int idComision)
        {
            if (string.IsNullOrWhiteSpace(materia))
                throw new ArgumentException("El nombre de la materia no puede estar vacío.", nameof(materia));

            // 1️⃣ Extraer todas las letras mayúsculas del nombre de la materia.
            //    Regex.Matches devuelve una colección de Match; usamos LINQ para concatenar.
            var iniciales = string.Concat(
                Regex.Matches(materia, @"[A-Z]")
                     .Cast<Match>()
                     .Select(m => m.Value));

            // Si la materia no contiene mayúsculas (caso raro), usamos la primera letra de cada palabra.
            if (iniciales.Length == 0)
            {
                iniciales = string.Concat(
                    materia.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(p => char.ToUpper(p[0])));
            }

            // 2️⃣ Obtener los últimos dos dígitos del año.
            string anioDosDigitos = (anio % 100).ToString("D2"); // "25" para 2025, "04" para 2004

            // 3️⃣ Construir el nombre final.
            return $"{iniciales}-{anioDosDigitos}-{idComision}";
        }

        public static string ResolveStatus(int? professorId, string? incomingStatus)
        {
            // Si no hay profesor asignado, siempre es Pendiente
            if (!professorId.HasValue)
                return "Pendiente";

            // Con profesor asignado, validar el estado recibido
            var allowed = new[] { "Activo", "Inactivo" };
            if (incomingStatus != null && allowed.Contains(incomingStatus))
                return incomingStatus;

            // Valor no permitido → lanzar excepción clara
            throw new InvalidOperationException(
                $"El status debe ser 'Activo' o 'Inactivo' cuando se asigna un profesor.");
        }

    }


}
