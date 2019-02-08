﻿using System;
using System.IO;
using System.Transactions;

namespace CKAN
{
    public static class Utilities
    {
        /// <summary>
        /// Copies a directory and optionally its subdirectories as a transaction.
        /// </summary>
        /// <param name="sourceDirPath">Source directory path.</param>
        /// <param name="destDirPath">Destination directory path.</param>
        /// <param name="copySubDirs">Copy sub dirs recursively if set to <c>true</c>.</param>
        public static void CopyDirectory(string sourceDirPath, string destDirPath, bool copySubDirs)
        {
            using (TransactionScope transaction = CkanTransaction.CreateTransactionScope())
            {
                _CopyDirectory(sourceDirPath, destDirPath, copySubDirs);
                transaction.Complete();
            }
        }


        private static void _CopyDirectory(string sourceDirPath, string destDirPath, bool copySubDirs)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirPath);

            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundKraken(
                    sourceDirPath,
                    "Source directory does not exist or could not be found.");
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirPath))
            {
                Directory.CreateDirectory(destDirPath);
            }
            else if (Directory.GetDirectories(destDirPath).Length != 0 || Directory.GetFiles(destDirPath).Length != 0)
            {
                throw new PathErrorKraken(destDirPath, "Directory not empty: ");
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = sourceDir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name == "registry.locked")
                {
                    continue;
                }

                string temppath = Path.Combine(destDirPath, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo[] dirs = sourceDir.GetDirectories();

                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirPath, subdir.Name);
                    _CopyDirectory(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
