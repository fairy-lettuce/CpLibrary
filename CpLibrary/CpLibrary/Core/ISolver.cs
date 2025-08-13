using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary;

public interface ISolver
{
	public void Run(StreamReader sr, StreamWriter sw);
}
