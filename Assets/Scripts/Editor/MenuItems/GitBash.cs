using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;

namespace Editor.MenuItems
{
    public static class MenuItems
    {
        private static readonly string GitBashPath = @"C:\Program Files\Git\git-bash.exe";
        private static readonly FileInfo GitBashFile = new(GitBashPath);

        [MenuItem("Git/Open Git Bash")] 
        public static void OpenGitBash()
        {
            Process.Start(GitBashFile.FullName, $"--cd=\"{Environment.CurrentDirectory}\"");
        }

        [MenuItem("Git/Open Git Bash", isValidateFunction: true)]
        public static bool HasGitBash()
        {
            return GitBashFile.Exists;
        }
    }
}