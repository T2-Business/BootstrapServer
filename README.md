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
  
  ![alt text](https://github.com/T2-Business/BootstrapServer/blob/main/TBL.PNG)

   
  </code>
  
  <H2>  To run the above code   </H2>
  <p> task = SustainableTaskManager.SustainableTask(async now => await CheckHostStatusAsync(), consumertoken.Token, consumerDalyTime);
            task.Post(DateTimeOffset.Now);
    return Task.CompletedTask;
  </p>
  <H2><b>How to change the Servers URL   </H2>
           <p>its easy  to change the servers url you want to monitore  by going to the  appsetting.json file  then change the url to your servers url like the image below </p>
               
![alt text](https://github.com/T2-Business/BootstrapServer/blob/main/hostURl.PNG)


 <H2> How to request the Servers list  </H2>
<p>to request the server list all what you need to do is to get the authintication code first from BootstrapServer
By calling   http://BootstrapServer/Token/GeneratToken?yourclientId</p>

![alt text](https://github.com/T2-Business/BootstrapServer/blob/main/Token.PNG)
 
