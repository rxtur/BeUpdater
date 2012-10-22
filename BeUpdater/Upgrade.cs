using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BeUpdater
{
    public class Upgrade
    {
        public static string Old { get; set; }
        public static string New { get; set; }

        public static string Run()
        {
            try
            {
                MergeAppData();

                MergeCommon();

                CopyCustom();

                FixCompatibility();

                return "Upgrade completed";
            }
            catch (Exception ex)
            {
                return ex.Message;
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
            var newScripts = Path.Combine(New, "Scripts");
            foreach (var f in oldScripts.GetFiles())
            {
                if (!File.Exists(Path.Combine(newScripts, f.Name)))
                {
                    File.Copy(f.FullName, Path.Combine(newScripts, f.Name));
                }
            }

            // copy custom styles
            var oldStyles = new DirectoryInfo(Path.Combine(Old, "Styles"));
            var newStyles = Path.Combine(New, "Styles");
            foreach (var f in oldStyles.GetFiles())
            {
                if (!File.Exists(Path.Combine(newStyles, f.Name)))
                {
                    File.Copy(f.FullName, Path.Combine(newStyles, f.Name));
                }
            }

            // remove scripts added by default in 2.0.x.x
            string[] srcs = { "blog.js", "jquery.cookie.js", "jquery.js", "jquery.validate.min.js", "jquery-1.4.3-vsdoc.js", "jquery-jtemplates.js", "json2.js" };

            foreach (var src in srcs)
            {
                string srcPath = Path.Combine(New, @"Scripts\" + src);
                if (File.Exists(srcPath))
                {
                    File.Delete(srcPath);
                }
            }
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
                    FileSystem.CopyFromTo(d.FullName, Path.Combine(New, d.Name));
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
