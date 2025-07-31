using System;
using System.IO;

namespace CSharpTools.Files
{
    public static class FileUtility
    {
        /// <summary>
        ///     Input a desired file name.
        ///     Returns the file name or a variation appended with the current DateTime if the file name is already taken by another file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileNameOrVariation(string fileName)
        {
            if (!File.Exists(fileName))
                return fileName;
            
            int lastIndex = fileName.LastIndexOf('.');
            
            string originalFileName = fileName.Substring(0, lastIndex);
            string fileExtension = fileName.Substring(lastIndex + 1);

            string newFileName = $"{originalFileName}_{DateTime.Now}.{fileExtension}";
            return newFileName;
        }
        
        /// <summary>
        ///     Input a desired file name.
        ///     Returns the file name or a variation based on a counter if the file name is already taken by another file.
        ///     Will return: "ExampleFile", "ExampleFile_1", "ExampleFile_2", etc.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="maxIterations">The number of iteration attempts to rename the file.</param>
        /// <returns></returns>
        public static string GetFileNameOrVariation(string fileName, int maxIterations)
        {
            if (!File.Exists(fileName))
                return fileName;

            int lastIndex = fileName.LastIndexOf('.');
            
            string originalFileName = fileName.Substring(0, lastIndex);
            string fileExtension = fileName.Substring(lastIndex + 1);
            
            int counter = 1;

            while (counter < maxIterations)
            {
                string newFileName = $"{originalFileName}_{counter}.{fileExtension}";

                if (!File.Exists(newFileName))
                    return newFileName;

                counter++;
            }

            return "";
        }
    }
}