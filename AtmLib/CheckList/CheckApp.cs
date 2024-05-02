using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AtmLib.CheckList
{
    public class CheckApp
    {
        Dictionary< string, AppInfo > _appInfos = new Dictionary< string, AppInfo >();
        Dictionary<string, AppInfo> AppInfos
        {
            get => _appInfos;
            set => _appInfos = value;
        }

        public CheckApp()
        {
            _appInfos.Add("AtmLoader", new AppInfo
            {
                Name = "AtmLoader",
                Directory = "./",
                Path = "./AtmLoader.exe",
                Description = "AtmLoader works on .net 4.7.2.",
                TargetVersion = new Version(1, 0, 0),
                TargetDate = DateTime.Now,
                AppFileInfos = new List<AppFileInfo>
                {
                    new AppFileInfo { Name = "AtmApi.dll", Path = "AtmApi.dll", TargetSize = 16896, TargetDate = new DateTime(2023, 7, 3) },
                    new AppFileInfo { Name = "AtmLib.dll", Path = "AtmLib.dll", TargetSize = 53248, TargetDate = new DateTime(2023, 7, 3) }
                }
            });

            _appInfos.Add("KioskWatcher", new AppInfo
            {
                Name = "KioskWatcher",
                Directory = "./",
                Path = "./KioskWatcher.exe",
                Description = "KioskWatcher monitors the apps are running properly.",
                TargetVersion = new Version(1, 0, 0),
                TargetDate = DateTime.Now
            });

            _appInfos.Add("AtmUi", new AppInfo
            {
                Name = "AtmUi",
                Directory = "./",
                Path = "./AtmUi.exe",
                Description = "AtmUi is the main app works on .net 7.0.5.",
                TargetVersion = new Version(1, 0, 0),
                TargetDate = DateTime.Now
            });
        }
        
        public CheckAppItem Check( string name )
        {
            if ( ! _appInfos.ContainsKey( name ) )
            {
                return null;
            }

            AppInfo appInfo = _appInfos[ name ];
            CheckAppItem result = new CheckAppItem
            {
                Name = appInfo.Name,
                Description = appInfo.Description,
                Path = appInfo.Path,
                TargetVersion = appInfo.TargetVersion,
                TargetDate = appInfo.TargetDate
            };

            FileInfo fileInfo = new FileInfo( appInfo.Path );
            if ( !fileInfo.Exists )
            {
                result.Status = CheckAppItem.AppStatus.Missing;
                result.IsChecked = false;
                result.IconPath = "/res/uncheck.jpg";
                result.Result = $"{appInfo.Name} does not exist. Please install or download the app.";
            }
            else
            {
                var versionInfo = FileVersionInfo.GetVersionInfo( appInfo.Path );
                result.CurrentVersion = new Version( versionInfo.FileVersion );
                result.CurrentDate = fileInfo.CreationTime;

                int updated = 0, updates = 0, missing = 0;

                // Check required files
                foreach ( AppFileInfo appFileInfo in appInfo.AppFileInfos)
                {
                    CheckAppFileItem fileItem = new CheckAppFileItem
                    {
                        Name = appFileInfo.Name,
                        Path = appFileInfo.Path,
                        TargetDate= appFileInfo.TargetDate,
                        TargetSize = appFileInfo.TargetSize,
                        TargetSizeString = Requirement.SizeToString(appFileInfo.TargetSize)
                    };

                    FileInfo fileInfo1 = new FileInfo( appFileInfo.Path );
                    if ( fileInfo1.Exists )
                    {
                        fileItem.CurrentDate = fileInfo1.CreationTime;
                        fileItem.CurrentSize = fileInfo1.Length;
                        fileItem.CurrentSizeString = Requirement.SizeToString(fileInfo1.Length);
                        fileItem.Path = fileInfo1.FullName;
                        if (fileInfo1.CreationTime > appFileInfo.TargetDate )
                        {
                            fileItem.Status = CheckAppFileItem.FileStatus.Updated;
                            updated++;
                        }
                        else
                        {
                            fileItem.Status = CheckAppFileItem.FileStatus.NeedUpdate;
                            updates++;
                        }
                    }
                    else
                    {
                        fileItem.Path = "NA";
                        fileItem.CurrentSizeString = "NA";
                        fileItem.Status = CheckAppFileItem.FileStatus.Missing;
                        missing++;
                    }

                    result.Files.Add( fileItem );
                }

                if (result.CurrentVersion >= appInfo.TargetVersion)
                {
                    result.Result += $"Current version is {result.CurrentVersion} and it is up to date. ";
                    result.Status = CheckAppItem.AppStatus.Updated;
                }
                else
                {
                    result.Result += $"Current version is {result.CurrentVersion}. Need to update to {result.TargetVersion}. ";
                    result.Status = CheckAppItem.AppStatus.NeedUpdate;
                }

                if (updates == 0 && missing == 0)
                    result.Result += "All files are up to date. ";
                if (updates > 0)
                    result.Result += $"{updates} file(s) need to update. ";
                if (missing > 0)
                    result.Result += $"{missing} file(s) are missing. ";

                result.IsChecked = result.Status == CheckAppItem.AppStatus.Updated && missing == 0 && updates == 0;
                if ( result.IsChecked)
                    result.IconPath = "/res/check.png";
                else
                    result.IconPath = "/res/uncheck.jpg";
            }

            return result;
        }
    }

    public class AppInfo
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Directory { get; set; }
        public Version TargetVersion { get; set; }
        public DateTime TargetDate { get; set; }
        public List< AppFileInfo > AppFileInfos { get; set; } = new List< AppFileInfo >();
    }

    public class AppFileInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime TargetDate { get; set; }
        public long TargetSize { get; set; }
    }

    public class CheckAppItem
    {
        public enum AppStatus { None, Missing, NeedUpdate, Updated };
        public string Name { get; set; }
        public string Description { get; set; }
        public string Result { get; set; }
        public string Path { get; set; }
        public Version CurrentVersion { get; set; }
        public Version TargetVersion { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime TargetDate { get; set; }
        public Boolean IsChecked { get; set; } = false;
        public AppStatus Status { get; set; }
        public string IconPath { get; set; }
        public List<CheckAppFileItem> Files { get; set; } = new List<CheckAppFileItem>();
    }

    public class CheckAppFileItem
    {
        public enum FileStatus { None, Missing, Dismatch, NeedUpdate, Updated };
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime TargetDate { get; set; }
        public long CurrentSize { get; set; }
        public string CurrentSizeString { get; set; }
        public long TargetSize { get; set; }
        public string TargetSizeString { get; set; }
        public FileStatus Status { get; set; } = FileStatus.None;
    }
}
