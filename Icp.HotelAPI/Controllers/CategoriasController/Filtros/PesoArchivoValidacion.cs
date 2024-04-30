using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.CategoriasController.Filtros
{
    public class PesoArchivoValidacion: ValidationAttribute
    {
        private readonly int pesoMaximoEnMegabytes;

        public PesoArchivoValidacion(int PesoMaximoEnMegabytes )
        {
            pesoMaximoEnMegabytes = PesoMaximoEnMegabytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null )
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > pesoMaximoEnMegabytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMaximoEnMegabytes}mb");
            }

            return ValidationResult.Success;
        }
    }
}
