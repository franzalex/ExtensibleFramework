using ExtensibleFramework.Core;
using System;
using System.Collections.Generic;

namespace SalesManager
{
    public class SalesManager : ExtensibleFramework.Core.Plugin
    {
        private List<ActivityLauncher> _mainActivities;
        private Dictionary<string, Func<ActivityControl>> activityCreator;

        /// <summary>Initializes a new instance of the <see cref="SalesManager" /> class.</summary>
        public SalesManager()
        {
            // create an index of activities in this plug-in and their launchers
            activityCreator = new Dictionary<string, Func<ActivityControl>>();
            _mainActivities = new List<ActivityLauncher>();

            foreach (var ctor in new Func<ActivityControl>[] { () => new ClientList() })
                using (var ahc = ctor.Invoke())
                {
                    activityCreator.Add(ahc.ID, ctor);
                    if (ahc.Launcher != null)
                        _mainActivities.Add(ahc.Launcher);
                }
        }

        /// <summary>Gets the launchers for launching activities directly for this plug-in.</summary>
        public override IEnumerable<ActivityLauncher> ActivityLaunchers
        {
            get { return _mainActivities; }
        }

        /// <summary>Gets the description of the plug-in.</summary>
        public override string Description
        {
            get { return "Sales Manager Plug-in"; }
        }

        /// <summary>Gets the name of the plug-in.</summary>
        public override string Name
        {
            get { return "Sales Manager"; }
        }

        /// <summary>
        /// Creates the activity associated with the specified <paramref name="activityID" />.
        /// </summary>
        /// <param name="activityID">The activity ID of the activity to be created.</param>
        /// <returns>
        /// An instance of <seealso cref="T:ExtensibleFramework.Core.ActivityControl" />.
        /// </returns>
        public override ActivityControl CreateActivity(string activityID)
        {
            Func<ActivityControl> ctor;

            // get the creator for the specified ID and invoke it else return null.
            if (activityCreator.TryGetValue(activityID, out ctor))
                return ctor.Invoke();
            else
                return null;
        }

        /// <summary>
        /// Gets the ID of the <see cref="T:ExtensibleFramework.Core.ActivityControl" /> to use for
        /// executing the specified command.
        /// </summary>
        /// <param name="command">The command to be evaluated.</param>
        /// <returns>
        /// The ID of the <see cref="T:ExtensibleFramework.Core.ActivityControl" /> to use for
        /// executing <paramref name="command" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// If a non-empty string is returned and 
        /// <seealso cref="M:ExtensibleFramework.Core.Plugin.SupportsCommand(System.String)" /> is
        /// <c>true</c>, the activity whose ID is returned will be initialized with <paramref
        /// name="command" />. On the other hand, if an empty string is returned, <paramref
        /// name="command" /> will be passed to 
        /// <see cref="M:ExtensibleFramework.Core.Plugin.RunCommand(System.String)" />.
        /// </remarks>
        public override string GetActivityForCommand(string command)
        {
            // return the ID of the activity which can launch the command passed
            return "";
        }

        /// <summary>Runs the specified command.</summary>
        /// <param name="command">The command to be run.</param>
        /// <returns>The result produced from running the command.</returns>
        public override object RunCommand(string command)
        {
            // we don't run any commands so return null irrespective of what the command is
            return null;
        }

        /// <summary>Gets a value indicating whether the plug-in supports the specified command.</summary>
        /// <param name="command">The command to be evaluated.</param>
        /// <returns>
        ///   <c>true</c> if the plug-in supports the specified command; else <c>false</c>.
        /// </returns>
        public override bool SupportsCommand(string command)
        {
            // we don't support any command yet
            return false;
        }
    }
}