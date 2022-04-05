using Alesp.Worker;

var legislatureWorker = new LegislaturesWorker();
await legislatureWorker.GetLesgilatures();


var congressPersonWorker = new CongressPersonWorker();
await congressPersonWorker.GetCongressPeople();

var spendingsWorker = new SpendingWorker();
await spendingsWorker.GetSpendings();



