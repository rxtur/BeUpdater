using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BeUpdater
{
    public class FileSystem
    {
        public static void CopyFromTo(string from, string to)
        {
            var src = new DirectoryInfo(from);
            var trg = new DirectoryInfo(to);

            CopyDirectory(src, trg);
        }

        public static void ClearDirectory(string path)
        {
            var dir = new DirectoryInfo(path);

            foreach (var d in dir.GetDirectories())
            {
                ForceDeleteDirectory(d.FullName);
            }

            foreach (var f in dir.GetFiles())
            {
                File.Delete(f.FullName);
            }
        }

        public static void ForceDeleteDirectory(string path)
        {
            var fols = new Stack<DirectoryInfo>();
            var root = new DirectoryInfo(path);
            fols.Push(root);
            while (fols.Count > 0)
            {
                var fol = fols.Pop();
                fol.Attributes = fol.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                foreach (var d in fol.GetDirectories())
                {
                    fols.Push(d);
                }
                foreach (var f in fol.GetFiles())
                {
                    f.Attributes = f.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                    f.Delete();
                }
            }
            root.Delete(true);
        }

        static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            if (!Directory.Exists(target.FullName))
                Directory.CreateDirectory(target.FullName);

            foreach (var dir in source.GetDirectories())
            {
                DirectoryInfo targetDir;
                if (!Directory.Exists(Path.Combine(target.FullName, dir.Name)))
                {
                    targetDir = target.CreateSubdirectory(dir.Name);
                }
                else
                {
                    targetDir = new DirectoryInfo(Path.Combine(target.FullName, dir.Name));
                }
                CopyDirectory(dir, targetDir);
            }

            foreach (var file in source.GetFiles())
            {
                if (!File.Exists(Path.Combine(target.FullName, file.Name)))
                {
                    File.Copy(file.FullName, Path.Combine(target.FullName, file.Name));
                }
            }
        }
    }
}
