using Alesp.Worker;

var legislatureWorker = new LegislaturesWorker();
await legislatureWorker.GetLesgilatures();


var congressPersonWorker = new CongressPersonWorker();


