using System;
using System.Collections.Generic;
using System.Text;

namespace Projet.Infrastructure
{
    public interface IPathProvider
    {
        string GetLogDir();
        string GetStatusDir();
    }
}
