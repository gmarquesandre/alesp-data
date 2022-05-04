using Alesp.Worker;

//var legislatureWorker = new LegislaturesWorker();
//await legislatureWorker.GetLesgilatures();

var congressPersonWorker = new CongressPersonWorker();
await congressPersonWorker.GetCongressPersonInfo();



var spendingsWorker = new SpendingWorker();
await spendingsWorker.GetAllSpendings();

var presenceWorker = new PresenceWorker();
await presenceWorker.GetAllCongressPeoplePresence();


//https://www.al.sp.gov.br/alesp/presenca-plenario/