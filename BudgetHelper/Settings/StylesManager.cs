using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace BudgetHelper.Settings
{
	public class StylesManager
	{
		private void a()
		{
			
		}
	}

	public class AccentTile : Tile
	{
		public Accent Accent { get; protected set; }

		public AccentTile(Accent accent)
		{
			Accent = accent;
			Background = (System.Windows.Media.Brush)accent.Resources["WindowTitleColorBrush"];
			Width = 30;
			Height = 30;
			Margin = new System.Windows.Thickness(0);
			TitleFontSize = 14;
		}

		public static List<AccentTile> GetAccentTiles()
		{
			List<AccentTile> ret = new List<AccentTile>(ThemeManager.Accents.Count());
			foreach (Accent accent in ThemeManager.Accents)
			{
				ret.Add(new AccentTile(accent));
			}
			return ret;
		}
	}
}
