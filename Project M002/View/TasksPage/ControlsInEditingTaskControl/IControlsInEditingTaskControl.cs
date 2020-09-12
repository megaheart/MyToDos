using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.View.TasksPage.ControlsInEditingTaskControl
{
    public interface IControlsInEditingTaskControl
    {
        /// <summary>
        /// Initialize new value of result property of IControlsInEditingTaskControl
        /// </summary>
        /// <returns>
        /// null:
        ///     user has no mistakes. New value is initialized.
        /// not null:
        ///     user has some mistakes enumerated in Exception list. New value cannot be initialized.
        /// </returns>
        Exception[] Save();
    }
}
