using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class Size
    {
        public int Width { get; set; }
		public int Height { get; set; }
		
		public Size()
		{
			this.Width = 0;
			this.Height = 0;
		}
		
		public Size(int Width, int Height)
		{
			this.Width = Width;
			this.Height = Height;
		}
    }
}
