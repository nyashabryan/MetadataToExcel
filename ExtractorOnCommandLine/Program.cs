using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.Exif.Makernotes;

namespace ExtractorOnCommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Users\Nyashadzaishe Bryan\VS Projects\ExtractorOnCommandLine");

            List<Image> images = LoadImages(directoryInfo);
            
            Console.WriteLine("");
            Console.WriteLine("Press Any Key to Exit");
            Console.ReadKey();
        }

        public static void PrintImagesResults(List<Image> listOfImages)
        {
            foreach(Image image in listOfImages)
            {
                
            }
        }

        public static FileInfo[] getImageFiles(DirectoryInfo source)
        {
            return source.GetFiles("*.jpg").Concat((source.GetFiles("*.jpeg"))).ToArray();
           
        }
        
        public static List<Image> LoadImages(DirectoryInfo source)
        {
            List<Image> imagesList = new List<Image>();
            FileInfo[] files = getImageFiles(source);
            
            for (var i = 0; i < files.Length; i++)
            {
                IReadOnlyList<MetadataExtractor.Directory> directories;
                try
                {
                    directories = ImageMetadataReader.ReadMetadata(files[i].FullName);
                }
                catch (ImageProcessingException e)
                {
                    Console.WriteLine($"\n {files[i].FullName} could not be read properly.");
                    Console.WriteLine(e.ToString());
                    continue;
                }
                catch (IOException e)
                {
                    Console.WriteLine($"\n {files[i].FullName} could not be read properly.");
                    Console.WriteLine(e.ToString());
                    continue;
                }

                string fileName;
                int fileSize;

                var fileDirectory = directories.OfType<FileMetadataDirectory>().FirstOrDefault();
                if (fileDirectory != null)
                {
                    if (fileDirectory.HasTagName(FileMetadataDirectory.TagFileName))
                        fileName = fileDirectory.GetString(FileMetadataDirectory.TagFileName);
                    if (fileDirectory.HasTagName(FileMetadataDirectory.TagFileSize))
                        fileSize = fileDirectory.GetInt32(FileMetadataDirectory.TagFileSize);
                }

                string deviceName;
                string deviceSoftware;
                string deviceModel;
                string imageHeight;
                string imageWidth;
                string dateTime;
                var ifd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
                if (ifd0Directory != null)
                {
                    if (ifd0Directory.HasTagName(ExifIfd0Directory.TagDateTime))
                        dateTime = ifd0Directory.GetString(ExifIfd0Directory.TagDateTime);
                    if (ifd0Directory.HasTagName(ExifIfd0Directory.TagMake))
                        deviceName = ifd0Directory.GetString(ExifIfd0Directory.TagMake);
                    if (ifd0Directory.HasTagName(ExifIfd0Directory.TagSoftware))
                        deviceSoftware = ifd0Directory.GetString(ExifIfd0Directory.TagSoftware);
                    if (ifd0Directory.HasTagName(ExifIfd0Directory.TagModel))
                        deviceModel = ifd0Directory.GetString(ExifIfd0Directory.TagModel);
                    if (ifd0Directory.HasTagName(ExifIfd0Directory.TagImageHeight))
                        imageHeight = ifd0Directory.GetString(ExifIfd0Directory.TagImageHeight);
                    if (ifd0Directory.HasTagName(ExifIfd0Directory.TagImageWidth))
                        imageWidth = ifd0Directory.GetString(ExifIfd0Directory.TagImageWidth);
                }

                var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                string imageUniqueID;
                if (subIfdDirectory != null && subIfdDirectory.HasTagName(ExifSubIfdDirectory.TagImageUniqueId))
                    imageUniqueID = subIfdDirectory.GetString(ExifSubIfdDirectory.TagImageUniqueId);

                var gpsDirectory = directories.OfType<GpsDirectory>().FirstOrDefault();
                string latitude;
                string longitude;
                string latitudeRef;
                string longitudeRef;
                
                if (gpsDirectory != null)
                {
                    if (gpsDirectory.HasTagName(GpsDirectory.TagLatitude))
                        latitude = gpsDirectory.GetString(GpsDirectory.TagLatitude);
                    if (gpsDirectory.HasTagName(GpsDirectory.TagLongitude))
                        longitude = gpsDirectory.GetString(GpsDirectory.TagLongitude);
                    if (gpsDirectory.HasTagName(GpsDirectory.TagLongitudeRef))
                        longitudeRef = gpsDirectory.GetString(GpsDirectory.TagLongitudeRef);
                    if (gpsDirectory.HasTagName(GpsDirectory.TagLatitudeRef))
                        latitudeRef = gpsDirectory.GetString(GpsDirectory.TagLatitudeRef);
                }


            }

            return imagesList;
        }
    }
}
