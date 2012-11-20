/*  
 Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor.IO
{
    using System;
    using System.Globalization;
    using System.IO;
    using Trinet.Core.IO.Ntfs;

    /// <summary>
    /// Provides extension methods to the <see cref="System.IO.DirectoryInfo"/> class.
    /// </summary>
    internal static class DirectoryInfoExtensions
    {
        private const string ZoneIdentifierStreamName = "Zone.Identifier";

        /// <summary>
        /// Unblocks the directory.
        /// </summary>
        /// <param name="directory">Extension instance.</param>
        /// <param name="recursive">True to recursively unblock all files and folders within the directory.</param>
        static public void Unblock(this DirectoryInfo directory, bool recursive)
        {
            if (directory.Exists == false)
                throw new DirectoryNotFoundException(String.Format(CultureInfo.InvariantCulture, "The specified directory '{0}' could not be found.", directory.FullName));

            if (directory.AlternateDataStreamExists(DirectoryInfoExtensions.ZoneIdentifierStreamName))
                directory.DeleteAlternateDataStream(DirectoryInfoExtensions.ZoneIdentifierStreamName);

            foreach (FileInfo file in directory.GetFiles())
                file.Unblock();

            if (recursive)
            {
                foreach(DirectoryInfo subDirectory in directory.GetDirectories())
                    subDirectory.Unblock(true);
            }
        }
    }
}