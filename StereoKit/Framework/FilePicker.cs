﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StereoKit.Framework
{
	public struct FileFilter
	{
		public string name;
		public string filter;
		public FileFilter(string name, string filter)
		{
			this.name = name;
			this.filter = filter;
		}
	}
	public enum FilePickerMode
	{
		Open,
		Save
	}

	/// <summary>Lets you pick files from the file system! Use FilePicker.Show
	/// to begin a file picker, and FilePicker.Hide to close it. You can safely 
	/// call FilePicker.Hide even if it's already closed.</summary>
	public class FilePicker : IStepper
	{
		#region Support types
		enum FileType
		{
			File,
			Folder
		}
		struct File
		{
			public FileType type;
			public string   name;
			public string   extension;
		}
		#endregion

		#region Singleton
		static FilePicker _inst;

		/// <summary>Show a file picker to the user! If one is already up, it'll be cancelled out,
		/// and this one will replace it.</summary>
		/// <param name="mode">For opening files, or for saving them?</param>
		/// <param name="onSelectFile">The function to call when the user has selected a file.</param>
		/// <param name="filters">What file types should show up in the picker?</param>
		public static void Show(FilePickerMode mode, Action<string> onSelectFile, params FileFilter[] filters)
			=> Show(mode, null, onSelectFile, null, filters);
		/// <summary>Show a file picker to the user! If one is already up, it'll be cancelled out,
		/// and this one will replace it.</summary>
		/// <param name="mode">For opening files, or for saving them?</param>
		/// <param name="onSelectFile">The function to call when the user has selected a file.</param>
		/// <param name="onCancel">If the file selection has been cancelled, this'll get called!</param>
		/// <param name="filters">What file types should show up in the picker?</param>
		public static void Show(FilePickerMode mode, Action<string> onSelectFile, Action onCancel = null, params FileFilter[] filters)
			=> Show(mode, null, onSelectFile, onCancel, filters);
		/// <summary>Show a file picker to the user! If one is already up, it'll be cancelled out,
		/// and this one will replace it.</summary>
		/// <param name="mode">For opening files, or for saving them?</param>
		/// <param name="initialFolder">The starting folder. By default (or null), this'll just be
		/// the working directory.</param>
		/// <param name="onSelectFile">The function to call when the user has selected a file.</param>
		/// <param name="filters">What file types should show up in the picker?</param>
		public static void Show(FilePickerMode mode, string initialFolder, Action<string> onSelectFile, params FileFilter[] filters)
			=> Show(mode, initialFolder, onSelectFile, null, filters);
		/// <summary>Show a file picker to the user! If one is already up, it'll be cancelled out,
		/// and this one will replace it.</summary>
		/// <param name="mode">For opening files, or for saving them?</param>
		/// <param name="initialFolder">The starting folder. By default (or null), this'll just be
		/// the working directory.</param>
		/// <param name="onSelectFile">The function to call when the user has selected a file.</param>
		/// <param name="onCancel">If the file selection has been cancelled, this'll get called!</param>
		/// <param name="filters">What file types should show up in the picker?</param>
		public static void Show(FilePickerMode mode, string initialFolder, Action<string> onSelectFile, Action onCancel, params FileFilter[] filters)
		{
			if (_inst != null) _inst._onCancel?.Invoke();
			if (_inst == null) { 
				_inst = SK.AddStepper(new FilePicker());
				Vec3 pos = Input.Head.position + Input.Head.Forward * .5f + Input.Head.Up * 0.2f;
				_inst._windowPose = new Pose(pos, Quat.LookAt(pos, Input.Head.position));
			}
			_inst.Setup(mode, initialFolder, onSelectFile, onCancel, filters);
		}
		/// <summary>If the file picker is `Active`, this will cancel it out and remove it from
		/// the stepper list. If it's already inactive, this does nothing!</summary>
		public static void Hide()
		{
			if (_inst != null)
				SK.RemoveStepper(_inst);
			_inst = null;
		}
		/// <summary>Is the file picker visible and doing things?</summary>
		public static bool Active => _inst != null;
		#endregion

		#region Fields
		FilePickerMode _mode;
		FileFilter[] _filters;
		List<File> _activeFiles = new List<File>();
		string     _path;
		string     _title;
		string[]   _separatedPath;
		Pose       _windowPose = new Pose(new Vec3(-10,0,0)*U.cm, Quat.LookDir(-Vec3.Forward));
		string     _resultName;
		string     _resultFolder;

		Action<string> _onFileSelect;
		Action         _onCancel;

		public bool Enabled => true;
		#endregion

		void Setup(FilePickerMode mode, string initialFolder, Action<string> onSelectFile, Action onCancel, FileFilter[] filters)
		{
			_mode         = mode;
			_title        = string.Join(" | ", filters.Select(filter => filter.name));
			_filters      = filters;
			_onFileSelect = onSelectFile;
			_onCancel     = onCancel;

			_resultFolder = null;
			_resultName   = null;
			UpdateFolder(initialFolder == null ? Directory.GetCurrentDirectory() : initialFolder);
		}

		bool Select(int index)
		{
			if (_activeFiles[index].type == FileType.Folder)
			{
				string newPath = Path.Combine(_path, _activeFiles[index].name);
				UpdateFolder(newPath);
			} 
			else
			{
				return true;
			}
			return false;
		}

		void UpdateFolder(string newPath)
		{
			_path = newPath;

			// Find the files and folders at the current path!
			List<string> files = new List<string>( _filters
				.SelectMany(f => Directory.GetFiles(_path, f.filter))
				.OrderBy   (f => f));
			string[] folders = Directory.GetDirectories(_path);

			// Parse files and folders into data we can draw!
			_activeFiles.Clear();
			_activeFiles.AddRange(files.Select(f => new File { 
				type      = FileType.File, 
				extension = Path.GetExtension(f),
				name      = Path.GetFileNameWithoutExtension(f)}));
			_activeFiles.AddRange(folders.Select(f => new File{ 
				type      = FileType.Folder, 
				extension = "",
				name      = Path.GetFileName(f)}));

			// Parse the path for display
			_separatedPath = _path.Split(new char[]{Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < _separatedPath.Length; i++) {
				_separatedPath[i] += Path.DirectorySeparatorChar;
			}
		}

		public bool Initialize() => true;
		public void Shutdown() { }

		public void Step()
		{
			UI.WindowBegin(_title, ref _windowPose, new Vec2(40, 0) * U.cm);

			ShowPath();

			if (_mode == FilePickerMode.Open)
				ShowOpenFile();
			else if (_mode == FilePickerMode.Save)
				ShowSaveFile();

			ShowFolderItems();

			UI.WindowEnd();
		}

		void ShowPath()
		{
			UI.Label("...\\");
			UI.SameLine();
			float width = UI.AreaRemaining.x*0.8f;
			for (int i = Math.Max(0, _separatedPath.Length - 3); i < _separatedPath.Length; i++)
			{
				if (UI.Button(_separatedPath[i], new Vec2(width/3, UI.LineHeight)))
				{
					string newPath = "";
					for (int t = 0; t <= i; t++)
						newPath += _separatedPath[t] + Path.DirectorySeparatorChar;
					UpdateFolder(newPath);
				}
				UI.SameLine();
			}
			UI.NextLine();
		}

		void ShowOpenFile()
		{
			if (_resultName != null)
			{
				if (UI.Button("Open"))
				{
					_onFileSelect?.Invoke(Path.Combine(_resultFolder, _resultName));
					Hide();
				}
				UI.SameLine();
				UI.Label(_resultName, new Vec2(UI.AreaRemaining.x, UI.LineHeight) );
			}
			else
			{
				UI.Label(" ");
			}
			UI.HSeparator();
		}

		void ShowSaveFile()
		{
			if (string.IsNullOrEmpty( _resultName ))
			{
				UI.Label("Save");
			}
			else if (UI.Button("Save"))
			{
				_resultFolder = _path;
				_onFileSelect?.Invoke(Path.Combine(_resultFolder, _resultName + Path.GetExtension(_filters[0].filter)));
				Hide();
			}
			UI.SameLine();
			UI.Input("saveFile", ref _resultName, new Vec2(25.5f*U.cm, UI.LineHeight));
			UI.SameLine();
			if (UI.Button("Mic"))
			{
				_resultName = "test";
			}
		}

		void ShowFolderItems()
		{
			Vec2 size = new Vec2(.085f, UI.LineHeight*1.5f);

			for (int i = 0; i < _activeFiles.Count; i++)
			{
				if (UI.Button(_activeFiles[i].name + "\n" + _activeFiles[i].extension, size) && Select(i))
				{
					_resultName   = _activeFiles[i].name + _activeFiles[i].extension;
					_resultFolder = _path;
				}
				UI.SameLine();
			}
		}
	}
}
