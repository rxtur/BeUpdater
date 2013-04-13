using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace BeUpdater
{
    public class Upgrade
    {
        public static string Old { get; set; }
        public static string New { get; set; }

        public static void Run(int step)
        {
            switch (step)
            {
                case 1:
                    CopyCustom();
                    break;
                case 2:
                    MergeCommon();
                    break;
                case 3:
                    MergeAppData();
                    break;
                case 4:
                    FixCompatibility();
                    break;
                default:
                    break;
            }
        }

        static void MergeAppData()
        {
            var info = new DirectoryInfo(Path.Combine(New, "App_Data"));

            foreach (var d in info.GetDirectories())
            {
                if (!d.Name.ToLower().Contains("blogs"))
                {
                    FileSystem.ForceDeleteDirectory(d.FullName);
                }
            }

            foreach (var f in info.GetFiles())
            {
                if (!f.Name.ToLower().Contains("blogs.xml"))
                {
                    File.Delete(f.FullName);
                }
            }

            FileSystem.CopyFromTo(Path.Combine(Old, "App_Data"), Path.Combine(New, "App_Data"));
        }

        static void MergeCommon()
        {
            FileSystem.CopyFromTo(Path.Combine(Old, "App_Code"), Path.Combine(New, "App_Code"));
            FileSystem.CopyFromTo(Path.Combine(Old, "Bin"), Path.Combine(New, "Bin"));
            FileSystem.CopyFromTo(Path.Combine(Old, "themes"), Path.Combine(New, "themes"));
            FileSystem.CopyFromTo(Path.Combine(Old, "User controls"), Path.Combine(New, "User controls"));
            FileSystem.CopyFromTo(Path.Combine(Old, "widgets"), Path.Combine(New, "widgets"));

            // copy custom scripts
            var oldScripts = new DirectoryInfo(Path.Combine(Old, "Scripts"));
            var oldHdrScripts = new DirectoryInfo(Path.Combine(Old, "Scripts", "Header"));
            var newScripts = Path.Combine(New, "Scripts");

            // don't copy these files
            var deprecated = new List<string> { "01-jquery.js", "02-jquery.cookie.js", "04-jquery-jtemplates.js", "05-json2.js" };
            var newFile = "";

            // in BE 2.7 scripts from /scripts and /scripts/header 
            // auto-loaded; in BE 2.8 all auto-loaded scripts
            // moved to /Scripts/Auto directory
            foreach (var f in oldHdrScripts.GetFiles())
            {
                newFile = Path.Combine(newScripts, "Auto", f.Name);
                if (!File.Exists(newFile) && !deprecated.Contains(f.Name))
                {
                    File.Copy(f.FullName, newFile);
                }
            }
            foreach (var f in oldScripts.GetFiles())
            {
                newFile = Path.Combine(newScripts, "Auto", f.Name);
                if (!File.Exists(newFile) && !deprecated.Contains(f.Name))
                {
                    File.Copy(f.FullName, newFile);
                }
            }
            foreach (var d in oldScripts.GetDirectories())
            {
                var newDir = Path.Combine(New, "Scripts", d.Name);
                if (!d.Name.Contains("Header") && !Directory.Exists(newDir))
                {
                    FileSystem.CopyFromTo(d.FullName, newDir);
                }
            }

            // copy custom styles : css files go to "Auto" 
            // for auto-execution and direcrories copied as is
            var oldStyles = new DirectoryInfo(Path.Combine(Old, "Styles"));
            var newStyles = Path.Combine(New, "Content", "Auto");
            foreach (var f in oldStyles.GetFiles())
            {
                if (!File.Exists(Path.Combine(newStyles, f.Name)))
                {
                    File.Copy(f.FullName, Path.Combine(newStyles, f.Name));
                }
            }
            foreach (var d in oldStyles.GetDirectories())
            {
                FileSystem.CopyFromTo(d.FullName, Path.Combine(New, "Content"));
            }

            //// remove scripts added by default in 2.0.x.x
            //string[] srcs = { "blog.js", "jquery.cookie.js", "jquery.js", "jquery.validate.min.js", "jquery-1.4.3-vsdoc.js", "jquery-jtemplates.js", "json2.js" };

            //foreach (var src in srcs)
            //{
            //    string srcPath = Path.Combine(New, @"Scripts\" + src);
            //    if (File.Exists(srcPath))
            //    {
            //        File.Delete(srcPath);
            //    }
            //}
        }

        static void CopyCustom()
        {
            // copy probably customized robots.txt
            File.Delete(Path.Combine(New, "robots.txt"));
            File.Copy(Path.Combine(Old, "robots.txt"), Path.Combine(New, "robots.txt"));

            // copy any custom files and directories from the root
            var info = new DirectoryInfo(Old);

            foreach (var d in info.GetDirectories())
            {
                if (!Directory.Exists(Path.Combine(New, d.Name)))
                {
                    // in 2.8 styles go to "Content", don't copy
                    if (d.Name.ToLower() != "styles")
                    {
                        FileSystem.CopyFromTo(d.FullName, Path.Combine(New, d.Name));
                    }
                }
            }

            foreach (var f in info.GetFiles())
            {
                if (!File.Exists(Path.Combine(New, f.Name)))
                {
                    File.Copy(f.FullName, Path.Combine(New, f.Name));
                }
            }
        }

        static void FixCompatibility()
        {
            FixKnownIssues(Path.Combine(New, "App_Code"));
            FixKnownIssues(Path.Combine(New, "themes"));
            FixKnownIssues(Path.Combine(New, "User controls"));
        }

        static void FixKnownIssues(string sourceDir)
        {
            foreach (string fileName in Directory.GetFiles(sourceDir))
            {
                if (fileName.ToLower().EndsWith(".cs") ||
                    fileName.ToLower().EndsWith(".aspx") ||
                    fileName.ToLower().EndsWith(".ascx") ||
                    fileName.ToLower().EndsWith(".master"))
                {
                    ReplaceInFile(fileName, "BlogSettings.Instance.StorageLocation", "Blog.CurrentInstance.StorageLocation");
                    ReplaceInFile(fileName, "BlogSettings.Instance.FileExtension", "BlogConfig.FileExtension");
                    ReplaceInFile(fileName, "\"login.aspx", "\"account/login.aspx");
                }
            }

            foreach (string subdir in Directory.GetDirectories(sourceDir))
                if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                    FixKnownIssues(subdir);
        }

        static void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            StreamReader reader = new StreamReader(filePath);
            string content1 = reader.ReadToEnd();
            reader.Close();

            string content2 = Regex.Replace(content1, searchText, replaceText);

            if (content1.Length != content2.Length)
            {
                StreamWriter writer = new StreamWriter(filePath);
                writer.Write(content2);
                writer.Close();
            }
        }
    }
}
