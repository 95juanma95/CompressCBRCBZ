using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using SharpCompress;
using SharpCompress.Archives.Rar;
using SharpCompress.Readers;

namespace CompressCBRCBZ;

internal class Program
{
    static void Main(string[] args)
    {
        /* Ejemplo argumentos
        
        "-input:C:\Comics\Prueba.cbr" "-output:C:\Comics\Prueba.cbz" "-calidad:25" "-PPI:48"

        */


        // En caso de que no haya enviado los argumentos
        if (args == null || args.Length == 0)
        {
            Console.WriteLine("No se han enviado los argumentos. Usar el argumento \"-help\" para mas información");
            Console.WriteLine("Ejemplo: \"-help\"");
            Console.ReadLine();
            return;
        }

        MangaFile mangaFile = new MangaFile();

        foreach (string arg in args)
        {

            // Si esta pidiendo ayuda:
            if (arg == "-help" || arg == "-h" || arg == "\"-help\"" || arg == "help")
            {
                Console.WriteLine("Los argumentos deben ser los siguientes, separados por un espacio en blanco: ");
                Console.WriteLine(" \"-input\": Archivo de entrada en formato cbz o cbr.");
                Console.WriteLine(" \tEjemplo: \"-input:C:\\Comics\\Prueba.cbr\"");
                Console.WriteLine(" \"-output\": Opcional. Archivo de salida en formato cbz.");
                Console.WriteLine(" \tEjemplo: \"-output:C:\\Comics\\Prueba.cbz\"");
                Console.WriteLine(" \"-calidad\": Opcional. Calidad de la transformación, escala de 0 a 100, donde 0 es calidad minima y 100 es mantener toda la calidad.");
                Console.WriteLine(" \tEjemplo: \"-calidad:25\"");
                Console.WriteLine(" \"-PPI\": Opcional. Cantidad de Pixel Per Inch de la imagen a transformar.");
                Console.WriteLine(" \tEjemplo: \"-PPI:48\"");
                Console.WriteLine(" Ejemplo de uso: \"-input:C:\\Comics\\Prueba.cbr\" \"-output:C:\\Comics\\Prueba.cbz\" \"-calidad:25\" \"-PPI:48\"");
                Console.ReadLine();
                return;
            }

            string value = string.Empty;

            if (arg.Contains(":"))
            {
                value = arg.Substring(arg.IndexOf(":") + 1);
            }

            switch (arg.Substring(0, arg.IndexOf(":")))
            {
                case "-input":
                    mangaFile.CargarInput(value);
                    break;
                case "-output":
                    mangaFile.CargarOutput(value);
                    break;
                case "-calidad":
                    mangaFile.CargarCalidad(Convert.ToInt32(value));
                    break;
                case "-PPI":
                    mangaFile.CargarResolucionPPI(Convert.ToInt32(value));
                    break;
                default:
                    Console.WriteLine("Se han cargado incorrectamente los argumentos. Usar \"-help\" para obtener mas información");
                    Console.ReadLine();
                    return;
            }

        }

        if (!args.Any(arg => arg.Contains("-output")))
        {
            mangaFile.CargarOutput(null);
        }

        if (!args.Any(arg => arg.Contains("-calidad")))
        {
            mangaFile.CargarCalidad(null);
        }

        if (!args.Any(arg => arg.Contains("-PPI")))
        {
            mangaFile.CargarResolucionPPI(null);
        }

        if (!mangaFile.CargadoCorrectamente())
        {
            Console.WriteLine("Se han cargado incorrectamente los argumentos. Usar \"-help\" para obtener mas información");
            Console.ReadLine();
            return;
        }

        // Crear las carpetas temporales
        Tooling.CrearCarpeta(mangaFile.inputTempPath);
        Tooling.CrearCarpeta(mangaFile.outputTempPath);

        // Extraes las imagenes a la carpeta de uso temporal
        Tooling.Extraer(mangaFile);

        // Comprime las imagenes dentro de Input, las coloca en Output
        Tooling.ComprimirImagenes(mangaFile);

        // Crea el cbz final
        Tooling.Comprimir(mangaFile);

        // Borrar las carpetas temporales
        Tooling.BorrarCarpeta(mangaFile.TempPath);

    }

}