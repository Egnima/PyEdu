using ExplorerControl.Interfaces;
using ExplorerControl.ViewModels;

namespace ExplorerControl
{
    public sealed class ExplorerFactory
    {

        private ExplorerFactory()
        { }

        /// <summary>
        /// Creates a <see cref="IListControllerViewModel"/> instance and returns it.
        /// </summary>
        /// <returns></returns>
        public static IListControllerViewModel CreateList()
        {
            return new ListControllerViewModel();
        }

        public static ITreeListControllerViewModel CreateTreeList()
        {
            return new TreeListControllerViewModel();
        }
    }
}
