﻿/*
Copyright (c) 2018, Lars Brubaker, John Lewin
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using System.Collections.Generic;
using System.Linq;

namespace MatterHackers.Agg.UI
{
	public class BitmapWinformsWindowProvider : WinformsSystemWindowProvider<BitmapSystemWindow>
	{
	}

	public class OpenGLWinformsWindowProvider : WinformsSystemWindowProvider<OpenGLSystemWindow>
	{
	}

	public class WinformsSystemWindowProvider<T> : ISystemWindowProvider
		where T : WinformsSystemWindow, new()
	{
		private List<SystemWindow> _openWindows = new List<SystemWindow>();

		public IReadOnlyList<SystemWindow> OpenWindows => _openWindows;

		public SystemWindow TopWindow => _openWindows.LastOrDefault();

		/// <summary>
		/// Creates or connects a PlatformWindow to the given SystemWindow
		public void ShowSystemWindow(SystemWindow systemWindow)
		{
			IPlatformWindow platformWindow;

			if (systemWindow.PlatformWindow == null)
			{
				platformWindow = new T();
				platformWindow.Caption = systemWindow.Title;
				platformWindow.MinimumSize = systemWindow.MinimumSize;
			}
			else
			{
				platformWindow = systemWindow.PlatformWindow;
			}

			if (platformWindow is WinformsSystemWindow winforms)
			{
				winforms.WindowProvider = this;
			}

			_openWindows.Add(systemWindow);

			platformWindow.ShowSystemWindow(systemWindow);
		}

		public void CloseSystemWindow(SystemWindow systemWindow)
		{
			systemWindow.PlatformWindow.CloseSystemWindow(systemWindow);
			_openWindows.Remove(systemWindow);
		}
	}
}
