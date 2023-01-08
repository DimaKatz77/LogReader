using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogReader.Services
{
    public abstract class FileWatcher
    {
        int m_LastStreamPosition;

        volatile bool m_IsReading;

        FileInfo m_FileInfo;
        // file system watcher 
        FileSystemWatcher m_Watcher;

        public FileWatcher(string filePath)
        {

            m_FileInfo = new FileInfo(filePath);

            m_Watcher = new FileSystemWatcher(m_FileInfo.DirectoryName, m_FileInfo.Extension);
            m_Watcher.EnableRaisingEvents = true;
            m_Watcher.Changed += WhenFileChanged;

            m_LastStreamPosition = 0;
            ReadFile();
        }

        void WhenFileChanged(object source, FileSystemEventArgs e)
        {

            if (e.FullPath != m_FileInfo.FullName) return;

            m_FileInfo.Refresh();
            if (m_FileInfo.Length <= m_LastStreamPosition)
                m_LastStreamPosition = 0;

            // read the file
            ReadFile();
        }

        void ReadFile()
        {
            if (m_IsReading) return;

            m_IsReading = true;

            using (StreamReader reader = m_FileInfo.OpenText())
            {
                reader.BaseStream.Position = m_LastStreamPosition;
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    ProcessLine(line);
                }
                m_LastStreamPosition = (int)reader.BaseStream.Position;
            }
            m_IsReading = false;
        }

        protected abstract void ProcessLine(string line);
    }
}
