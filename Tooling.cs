using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using SharpCompress;
using SharpCompress.Archives.Rar;
using SharpCompress.Readers;

namespace CompressCBRCBZ
{
    internal static class Tooling
    {
        public static void CrearCarpeta(string Filepath)
        {
            if (Directory.Exists(Filepath))
            {
                Console.WriteLine("La carpeta ya existe.");
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(Filepath);
            }
        }

        public static void BorrarCarpeta(string Filepath)
        {
            if (!Directory.Exists(Filepath))
            {
                Console.WriteLine("La carpeta no existe.");
            }
            else
            {
                Directory.Delete(Filepath, true);
            }
        }

        public static void MoverArchivos(string CarpetaOrigen, string CarpetaDestino)
        {
            List<string> Archivos = Directory.GetFiles(CarpetaOrigen).ToList();
            foreach (string Archivo in Archivos)
            {
                FileInfo fileInfo = new FileInfo(Archivo);
                if (new FileInfo(CarpetaDestino + "\\" + fileInfo.Name).Exists == false)
                {
                    fileInfo.MoveTo(CarpetaDestino + "\\" + fileInfo.Name);
                }
            }
        }

        public static void Extraer(MangaFile mangaFile)
        {
            if (mangaFile.fileExtension == ".cbr")
            {
                ExtraerCBR(mangaFile);
            }
            else
            {
                ExtraerCBZ(mangaFile);
            }

            return;
        }

        public static void ExtraerCBR(MangaFile mangaFile)
        {

            var archive = RarArchive.Open(mangaFile.inputPathCompleto);

            // La carpeta que compone el cbr
            string PathCarpetaInterna = archive.Entries.First<RarArchiveEntry>().Key;
            PathCarpetaInterna = mangaFile.inputTempPath + "\\" + PathCarpetaInterna.Substring(0, PathCarpetaInterna.IndexOf("\\"));

            var reader = archive.ExtractAllEntries();
            reader.WriteAllToDirectory(mangaFile.inputTempPath, new SharpCompress.Common.ExtractionOptions()
            {
                ExtractFullPath = true,
                Overwrite = true
            });
            reader.Dispose();
            archive.Dispose();

            MoverArchivos(PathCarpetaInterna, mangaFile.inputTempPath);

            BorrarCarpeta(PathCarpetaInterna);

            return;
        }

        public static void ExtraerCBZ(MangaFile mangaFile)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(mangaFile.inputPathCompleto, mangaFile.inputTempPath);

            return;
        }

        public static void Comprimir(MangaFile mangaFile)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(mangaFile.outputTempPath, mangaFile.outputPathCompleto);

            return;
        }

        public static string DevolverPath(string Path)
        {
            string PathReversed = new string(Path.ToCharArray().Reverse().ToArray());

            PathReversed = PathReversed.Substring(PathReversed.IndexOf("\\") + 1);

            return new string(PathReversed.ToCharArray().Reverse().ToArray());
        }

        public static string DevolverFileName(string Path)
        {
            return new string(Path.ToCharArray().Reverse().ToArray());
        }

        public static void ComprimirImagen(string InputFilepath, string OutputFilepath, int calidad, int? resolucionPPI)
        {
            Bitmap bmp1 = new Bitmap(InputFilepath);

            // En caso de que la calidad sea 100 y la resolucion PPI sea null (implica simplemente transformacion) solo mover la imagen.
            if (calidad == 100 && resolucionPPI == null)
            {
                bmp1.Save(OutputFilepath);
            }
            else
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                Encoder QualityEncoder = Encoder.Quality;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, calidad);

                myEncoderParameters.Param[0] = myEncoderParameter;

                if (resolucionPPI != null)
                {
                    bmp1.SetResolution((int)resolucionPPI, (int)resolucionPPI);
                }

                bmp1.Save(OutputFilepath, jpgEncoder, myEncoderParameters);
            }

            bmp1.Dispose();

            return;
        }

        public static void ComprimirImagenes(MangaFile mangaFile)
        {
            List<string> ListInputFilename = Directory.GetFiles(mangaFile.inputTempPath).ToList<string>();

            foreach (string InputFilename in ListInputFilename)
            {
                ComprimirImagen(InputFilename, mangaFile.outputTempPath + "\\" + Path.GetFileName(InputFilename), mangaFile.calidad, mangaFile.resolucionPPI);
            }

            return;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
