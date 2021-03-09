using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace T2BootstrapServer.API
{
    public class SustainableTaskManager
    {
        /// <summary>
        /// Salem SustainableTaskManager Calss  2017 
        /// </summary>
        /// <param name="action">the action you need to execute </param>
        /// <param name="cancellationToken"></param>
        /// <param name="consumerDalyTime">consumer Daly Time in  Minutes </param>
        /// <returns></returns>
        public static ActionBlock<DateTimeOffset> SustainableTask(Action<DateTimeOffset> action, CancellationToken cancellationToken, int consumerDalyTime = 10)
        {
            // Validate parameters.
            if (action == null)
                throw new ArgumentNullException("action is null ");

            // Declare the block variable needs to be captured.
            ActionBlock<DateTimeOffset> block = null;


            block = new ActionBlock<DateTimeOffset>(async now => {
                // Perform the consumer action.
                action(now);

                // Wait.
                await Task.Delay(TimeSpan.FromMinutes(consumerDalyTime), cancellationToken).
                    // we dont need awaiter  "read about it may be will create performence issue in this location @{salem}"
                    ConfigureAwait(false);

                // Post the action back to the block  salem note : its like recursion will call it self agian  
                block.Post(DateTimeOffset.Now);
            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            // Return the block.
            return block;
        }
    }
}
