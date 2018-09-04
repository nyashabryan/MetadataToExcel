using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Exif.Makernotes;

namespace ExtractorOnCommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(@"C:\Users\Nyashadzaishe Bryan\VS Projects\ExtractorOnCommandLine\image3.jpg");
            System.IO.FileInfo[] files = { file };
            List<Image> imagesList = new List<Image>();
            Image currentImage;

            for (var i = 0; i < files.Length; i++)
            {
                Console.WriteLine();
                if (!(files[i].Extension.Equals(".jpg") ||
                    files[i].Extension.Equals(".JPG")))
                {
                    continue;
                }
                Console.WriteLine($"{files[i].FullName}\n");
                currentImage = new Image();
                IReadOnlyList<Directory> directories;
                try
                {
                    directories = ImageMetadataReader.ReadMetadata(files[i].FullName);
                } catch (ImageProcessingException e)
                {
                    Console.WriteLine($"\n {files[i].FullName} could not be read properly.");
                    Console.WriteLine(e.ToString());
                    continue;
                } catch (System.IO.IOException e)
                {
                    Console.WriteLine($"\n {files[i].FullName} could not be read properly.");
                    Console.WriteLine(e.ToString());
                    continue;
                }

                foreach (Directory directory in directories)
                {
                    Console.WriteLine($"{directory.ToString()}\n");
                    
                    foreach(Tag tag in directory.Tags)
                    {
                        Console.WriteLine("{0, 20}: {1, 20}", tag.Name, tag.Description);
                    }
                }
            }

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
    }
}
