using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MyFileManager
{
    public static class FileInfoExtension
    {
        public static string CalcMD5(this FileInfo file)
        {
            try
            {
                using (FileStream fileStream = file.Open(FileMode.Open))
                {
                    MD5 myMD5 = MD5.Create();
                    var hash = myMD5.ComputeHash(fileStream);
                    string result = string.Empty;
                    foreach (var b in hash)
                    {
                        result += String.Format("{0:x2}", b);
                    }
                    return result;
                }
            }
            catch (FileNotFoundException inner)
            {
                throw new MyFileNotFoundException(inner.Message, inner);
            }
            catch (DirectoryNotFoundException inner)
            {
                throw new MyDirectoryNotFoundException(inner.Message, inner);
            }
            catch (DriveNotFoundException inner)
            {
                throw new MyDriveNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new UnauthorizedAccessException(inner.Message, inner);
            }
        }
        public static Encoding GetEncoding(this FileInfo file)
        {
            try
            {
                // Read the BOM
                var bom = new byte[4];
                using (FileStream fileStream = file.Open(FileMode.Open))
                {
                    fileStream.Read(bom, 0, 4);
                }
                // Analyze the BOM
                if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
                if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
                if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
                if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
                return Encoding.ASCII;
            }
            catch (FileNotFoundException inner)
            {
                throw new MyFileNotFoundException(inner.Message, inner);
            }
            catch (DirectoryNotFoundException inner)
            {
                throw new MyDirectoryNotFoundException(inner.Message, inner);
            }
            catch (DriveNotFoundException inner)
            {
                throw new MyDriveNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new UnauthorizedAccessException(inner.Message, inner);
            }
        }
        public static string GetPermissions(this FileInfo file)
        {
            try
            {
                var inf = file.GetAccessControl();
                string result = string.Empty;
                FileSecurity fileSec = file.GetAccessControl();
                AuthorizationRuleCollection acl = fileSec.GetAccessRules(true, true, typeof(NTAccount));
                foreach (FileSystemAccessRule rule in acl)
                {
                    result += String.Format("IdentityReference: {0} \n Access control type: {1} \n Rights: {2} \n Inherited: {3} \n \n", rule.IdentityReference, rule.AccessControlType, rule.FileSystemRights, rule.IsInherited);
                }
                return result;
            }
            catch (FileNotFoundException inner)
            {
                throw new MyFileNotFoundException(inner.Message, inner);
            }
            catch (DirectoryNotFoundException inner)
            {
                throw new MyDirectoryNotFoundException(inner.Message, inner);
            }
            catch (DriveNotFoundException inner)
            {
                throw new MyDriveNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new UnauthorizedAccessException(inner.Message, inner);
            }
        }
    }
}
