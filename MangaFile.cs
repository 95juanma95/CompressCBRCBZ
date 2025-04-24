using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressCBRCBZ
{
    internal class MangaFile
    {
        public string? inputPathCompleto;
        public string? inputPath;
        public string? inputTempPath;
        public string? filename;
        public string? fileExtension;
        public string? outputFileExtension;
        public string? outputPathCompleto;
        public string? outputPath;
        public string? outputTempPath;
        public string? TempPath;
        public int calidad = 100;
        public int? resolucionPPI;

        public bool CargadoCorrectamente()
        {
            // Si uno de estos es verdadero, retorna falso, asi se si se cargo o no correctamente.
            return !(String.IsNullOrEmpty(this.inputPath) ||
                    String.IsNullOrEmpty(this.inputPathCompleto) ||
                    String.IsNullOrEmpty(this.inputTempPath) ||
                    String.IsNullOrEmpty(this.filename) ||
                    String.IsNullOrEmpty(this.fileExtension) ||
                    String.IsNullOrEmpty(this.outputFileExtension) ||
                    String.IsNullOrEmpty(this.outputPathCompleto) ||
                    String.IsNullOrEmpty(this.outputTempPath) ||
                    String.IsNullOrEmpty(this.outputPath));
             
        }

        public void CargarInput(string pathCompleto)
        {
            this.inputPathCompleto = pathCompleto;
            this.inputPath = Path.GetDirectoryName(pathCompleto);
            this.TempPath = this.inputPath + "\\_Temp";
            this.inputTempPath = this.TempPath + "\\Input";
            this.filename = Path.GetFileNameWithoutExtension(pathCompleto);
            this.fileExtension = Path.GetExtension(pathCompleto);
        }

        public void CargarOutput(string? pathCompleto)
        {
            this.outputFileExtension = ".cbz"; // En teoría siempre es cbz porque es lo que lee Kindle.

            if (pathCompleto == null)
            {
                this.outputPath = this.inputPath;
                this.outputTempPath = this.outputPath + "\\_Temp\\Output";
                // El Path completo es el input pero con "OUTPUT" + filename + ".cbz" dentro, para no sobreescribir el input.
                this.outputPathCompleto = this.outputPath + "\\OUTPUT_" + this.filename + this.outputFileExtension;
            }
            else
            {
                this.outputPathCompleto = pathCompleto;
                this.outputPath = Path.GetDirectoryName(pathCompleto);
                this.outputTempPath = this.outputPath + "\\_Temp\\Output";
            }

        }

        public void CargarCalidad(int? calidad)
        {
            if (calidad == null)
            {
                this.calidad = 100;
            }
            else
            {
                this.calidad = (int)calidad;
            }
        }

        // Esta funcion tecnicamente se puede remover ya que es un setter, pero se deja en caso de que se quiera setear un PPI por defecto.
        public void CargarResolucionPPI(int? resolucionPPI)
        {
            this.resolucionPPI = resolucionPPI;
        }

    }
}
