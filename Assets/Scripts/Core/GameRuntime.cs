using BusinessLife.Models;

namespace BusinessLife.Core
{
    /// <summary>
    /// Shared state between scenes for the current run.
    /// </summary>
    public static class GameRuntime
    {
        public static GameState PendingNewGameState;
        public static JournalService PendingJournal;
    }
}
