# BootstrapServer

this is an API project working as alyer on front of several  servers and check wheter this server is running or can handle new request or no  then it will give a list of working 
servers to the client  so the client can know the working servers and send request to ; 



in many cases in high trafic application with ahuge transaction a big request payload  may be even the load balancer stop 
so if you have more that LB or service server you need you will call this API to get the running servers


<H2><b>Technologies  </H2>
  <ol>
    <li>.net core </li>
    <li> API</li>
    <li>System.Threading.Tasks.Dataflow</li>
    </ol>
  we use System.Threading.Tasks.Dataflow to create coninuese running job on the IIS  like below  
  
  <code>
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
  </code>
